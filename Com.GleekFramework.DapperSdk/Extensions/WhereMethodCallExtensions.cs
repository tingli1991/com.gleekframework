using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// WHERE方法条件表达式拓展类
    /// 提供对LINQ表达式中Where相关方法的解析和SQL转换支持
    /// </summary>
    public static class WhereMethodCallExtensions
    {
        /// <summary>
        /// 获取成员表达式的值
        /// </summary>
        /// <param name="node">成员表达式节点</param>
        /// <returns>成员值，若引用参数则返回null</returns>
        public static object GetMemberValue(this MemberExpression node)
        {
            if (node == null)
            {
                return null;
            }

            // 若成员引用的是参数（如 e.X），视为列，不进行解析
            if (node.Expression is ParameterExpression)
            {
                return null;
            }

            // 递归解析闭包实例
            if (!TryEvaluate(node.Expression, out object parentValue))
            {
                return null;
            }

            // 根据成员类型获取值
            if (node.Member is PropertyInfo propertyInfo)
            {
                return propertyInfo.GetValue(parentValue);
            }

            if (node.Member is FieldInfo fieldInfo)
            {
                return fieldInfo.GetValue(parentValue);
            }

            return null;
        }

        /// <summary>
        /// 处理Where方法调用表达式
        /// </summary>
        /// <param name="expression">方法调用表达式</param>
        /// <param name="parameters">参数字典</param>
        /// <param name="paramCounter">参数计数器引用</param>
        /// <param name="isUnary">是否为一元操作（取反）</param>
        /// <returns>SQL条件字符串</returns>
        public static string HandlerWhereValues(this MethodCallExpression expression, Dictionary<string, object> parameters, ref long paramCounter, bool isUnary = false)
        {
            var builder = new StringBuilder();
            parameters ??= new Dictionary<string, object>();
            var methodName = expression.Method.Name;
            var declaringType = expression.Method.DeclaringType;
            var lastArgumentInfo = expression.Arguments.LastOrDefault();

            // 处理字符串静态方法（如string.IsNullOrEmpty）
            if (declaringType == typeof(string) && expression.Object == null && lastArgumentInfo is MemberExpression memberExpressionStr)
            {
                var columnName = memberExpressionStr.GetColumnName();
                switch (methodName)
                {
                    case "IsNullOrEmpty":
                        {
                            builder.Append($"({columnName} is {(isUnary ? "not null and " + columnName + "!=''" : "null or " + columnName + "=''")})");
                            break;
                        }
                }
                return builder.ToString();
            }

            // 处理字符串实例方法（如Contains, StartsWith, EndsWith）
            if (declaringType == typeof(string) && expression.Object is MemberExpression columnNameExpression)
            {
                var columnName = columnNameExpression.GetColumnName();
                if (!TryEvaluate(lastArgumentInfo, out object columnValue))
                {
                    throw new NotSupportedException($"无法在构建时解析字符串方法参数：{methodName}");
                }

                var propertyName = $"@P{paramCounter++}";
                var likeColumnValue = methodName switch
                {
                    "Contains" => $"%{columnValue}%",
                    "StartsWith" => $"{columnValue}%",
                    "EndsWith" => $"%{columnValue}",
                    _ => throw new NotSupportedException($"字符串方法 {methodName} 不支持")
                };
                parameters.Add(propertyName, likeColumnValue);
                builder.Append($"{columnName} {(isUnary ? "not " : "")}like {propertyName}");
                return builder.ToString();
            }

            // 处理Contains方法（集合包含查询）
            if (methodName == "Contains")
            {
                return HandleContainsMethod(expression, parameters, ref paramCounter, isUnary);
            }

            throw new NotSupportedException($"方法 {expression.Method} 不被支持用于条件构建");
        }

        /// <summary>
        /// 处理Contains方法调用
        /// </summary>
        /// <param name="expression">方法调用表达式</param>
        /// <param name="parameters">参数字典</param>
        /// <param name="paramCounter">参数计数器引用</param>
        /// <param name="isUnary">是否为一元操作</param>
        /// <returns>SQL条件字符串</returns>
        private static string HandleContainsMethod(MethodCallExpression expression, Dictionary<string, object> parameters, ref long paramCounter, bool isUnary)
        {
            var builder = new StringBuilder();
            var declaringType = expression.Method.DeclaringType;

            // 处理实例Contains方法（如 list.Contains(x.Prop)）
            MemberExpression columnMember = null;
            if (expression.Object is MemberExpression objMe && !IsEnumerableType(objMe.Type))
            {
                columnMember = objMe;
            }
            else
            {
                // 优化：避免遍历所有参数，直接查找MemberExpression
                columnMember = expression.Arguments.FirstOrDefault(arg => arg is MemberExpression) as MemberExpression;
            }

            if (columnMember != null)
            {
                return HandleInstanceContains(columnMember, expression, parameters, ref paramCounter, isUnary, builder);
            }

            // 处理静态Enumerable.Contains方法（如 Enumerable.Contains(collection, column)）
            if (declaringType == typeof(Enumerable) && expression.Arguments.Count >= 2 && expression.Arguments[0] != null && expression.Arguments[1] is MemberExpression memberExpression)
            {
                return HandleEnumerableContains(memberExpression, expression.Arguments[0], parameters, ref paramCounter, isUnary, builder);
            }

            throw new NotSupportedException("无法解析 Contains 方法的类型");
        }

        /// <summary>
        /// 处理实例Contains方法
        /// </summary>
        private static string HandleInstanceContains(MemberExpression columnMember, MethodCallExpression expression, Dictionary<string, object> parameters, ref long paramCounter, bool isUnary, StringBuilder builder)
        {
            Expression collectionExpr = null;
            if (expression.Object != null && !ReferenceEquals(expression.Object, columnMember))
            {
                collectionExpr = expression.Object;
            }
            else
            {
                // 优化：使用FirstOrDefault而非遍历所有参数
                collectionExpr = expression.Arguments.FirstOrDefault(a => !ReferenceEquals(a, columnMember));
            }

            if (collectionExpr == null)
            {
                throw new NotSupportedException("无法解析 Contains 的集合来源");
            }

            if (!TryEvaluate(collectionExpr, out object instancesObj))
            {
                throw new NotSupportedException("无法在构建时解析 Contains 的集合，请确保集合为闭包常量或可求值表达式");
            }

            var columnName = columnMember.GetColumnName();
            return BuildContainsCondition(instancesObj, columnName, parameters, ref paramCounter, isUnary, builder);
        }

        /// <summary>
        /// 处理Enumerable.Contains方法
        /// </summary>
        private static string HandleEnumerableContains(MemberExpression memberExpression, Expression collectionExpr, Dictionary<string, object> parameters, ref long paramCounter, bool isUnary, StringBuilder builder)
        {
            var columnName = memberExpression.GetColumnName();
            if (!TryEvaluate(collectionExpr, out object instancesObj))
            {
                throw new NotSupportedException("无法在构建时解析 Enumerable.Contains 的集合来源");
            }

            return BuildContainsCondition(instancesObj, columnName, parameters, ref paramCounter, isUnary, builder);
        }

        /// <summary>
        /// 构建Contains条件的SQL
        /// </summary>
        private static string BuildContainsCondition(object instancesObj, string columnName, Dictionary<string, object> parameters, ref long paramCounter, bool isUnary, StringBuilder builder)
        {
            if (instancesObj == null)
            {
                builder.Append(isUnary ? "(1=1)" : "(1=0)");
                return builder.ToString();
            }

            // 优化：先判断是否为IEnumerable，减少不必要的类型检查
            if (instancesObj is IEnumerable enumerable)
            {
                builder.Append($"{columnName} {(isUnary ? "not " : "")}in (");
                HandleDefaultCollectionValues(parameters, builder, enumerable, ref paramCounter);

                // 如果集合为空，返回永远真或永远假
                if (builder.Length >= 4 && builder.ToString().EndsWith("in ("))
                {
                    builder.Length = 0;
                    builder.Append(isUnary ? "(1=1)" : "(1=0)");
                    return builder.ToString();
                }

                builder.Append(")");
                return builder.ToString();
            }
            else
            {
                // 单个值的情况
                var parameterName = $"@P{paramCounter++}";
                parameters.Add(parameterName, instancesObj);
                builder.Append($"{columnName} {(isUnary ? "<>" : "=")} {parameterName}");
                return builder.ToString();
            }
        }

        /// <summary>
        /// 尝试评估表达式，成功返回 true（value 可以为 null），失败返回 false。
        /// 针对 ReadOnlySpan<T>/ref struct 做特殊处理，转换为数组后返回。
        /// </summary>
        /// <param name="expr">要评估的表达式</param>
        /// <param name="value">输出评估结果</param>
        /// <returns>评估是否成功</returns>
        private static bool TryEvaluate(Expression expr, out object value)
        {
            value = null;
            if (expr == null)
            {
                return true;
            }

            // 优化：使用switch语句进行类型判断，提高性能
            return expr switch
            {
                ConstantExpression constantExpression => EvaluateConstantExpression(constantExpression, out value),
                MemberExpression memberExpression => TryEvaluateMemberExpression(memberExpression, out value),
                UnaryExpression unaryExpression => EvaluateUnaryExpression(unaryExpression, out value),
                NewArrayExpression newArrayExpression => TryEvaluateNewArrayExpression(newArrayExpression, out value),
                ListInitExpression listInitExpression => TryEvaluateListInitExpression(listInitExpression, out value),
                NewExpression newExpression => TryEvaluateNewExpression(newExpression, out value),
                MethodCallExpression methodCallExpression => TryEvaluateMethodCallExpression(methodCallExpression, out value),
                _ => TryCompileAndEvaluate(expr, out value)
            };
        }

        /// <summary>
        /// 评估常量表达式
        /// </summary>
        private static bool EvaluateConstantExpression(ConstantExpression constantExpression, out object value)
        {
            value = constantExpression.Value;
            return true;
        }

        /// <summary>
        /// 评估一元表达式
        /// </summary>
        private static bool EvaluateUnaryExpression(UnaryExpression unaryExpression, out object value)
        {
            // 处理类型转换表达式
            if (unaryExpression.NodeType == ExpressionType.Convert || unaryExpression.NodeType == ExpressionType.ConvertChecked)
            {
                return TryEvaluate(unaryExpression.Operand, out value);
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 尝试评估成员表达式
        /// </summary>
        private static bool TryEvaluateMemberExpression(MemberExpression memberExpression, out object value)
        {
            value = null;

            // 如果是参数表达式，则视为列，不进行评估
            if (memberExpression.Expression is ParameterExpression)
            {
                return false;
            }

            // 评估父对象
            if (!TryEvaluate(memberExpression.Expression, out object parent))
            {
                return false;
            }

            if (parent == null)
            {
                // 优化：简化null检查逻辑
                if (memberExpression.Member is PropertyInfo || memberExpression.Member is FieldInfo)
                {
                    value = null;
                    return true;
                }
                return false;
            }

            // 获取属性值
            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                value = propertyInfo.GetValue(parent);
                return true;
            }

            // 获取字段值
            if (memberExpression.Member is FieldInfo fieldInfo)
            {
                value = fieldInfo.GetValue(parent);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 尝试评估数组创建表达式
        /// </summary>
        private static bool TryEvaluateNewArrayExpression(NewArrayExpression newArrayExpression, out object value)
        {
            var list = new List<object>(newArrayExpression.Expressions.Count);
            foreach (var element in newArrayExpression.Expressions)
            {
                if (!TryEvaluate(element, out object elementValue))
                {
                    value = null;
                    return false;
                }
                list.Add(elementValue);
            }
            value = list.ToArray();
            return true;
        }

        /// <summary>
        /// 尝试评估列表初始化表达式
        /// </summary>
        private static bool TryEvaluateListInitExpression(ListInitExpression listInitExpression, out object value)
        {
            if (!TryEvaluate(listInitExpression.NewExpression, out object created))
            {
                value = null;
                return false;
            }

            if (!(created is IList list))
            {
                value = null;
                return false;
            }

            foreach (var init in listInitExpression.Initializers)
            {
                foreach (var argument in init.Arguments)
                {
                    if (!TryEvaluate(argument, out object argumentValue))
                    {
                        value = null;
                        return false;
                    }
                    list.Add(argumentValue);
                }
            }
            value = list;
            return true;
        }

        /// <summary>
        /// 尝试评估对象创建表达式
        /// </summary>
        private static bool TryEvaluateNewExpression(NewExpression newExpression, out object value)
        {
            value = null;

            // 无参构造函数
            if (newExpression.Arguments == null || newExpression.Arguments.Count == 0)
            {
                try
                {
                    value = Activator.CreateInstance(newExpression.Type);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            // 有参构造函数需要特殊处理，这里简化处理
            return false;
        }

        /// <summary>
        /// 尝试评估方法调用表达式
        /// </summary>
        private static bool TryEvaluateMethodCallExpression(MethodCallExpression methodCallExpression, out object value)
        {
            value = null;

            try
            {
                // 处理 ReadOnlySpan<T> 的隐式转换
                if (methodCallExpression.Method.IsSpecialName &&
                    methodCallExpression.Method.ReturnType.IsGenericType &&
                    methodCallExpression.Method.ReturnType.GetGenericTypeDefinition() == typeof(ReadOnlySpan<>))
                {
                    return HandleReadOnlySpanConversion(methodCallExpression, out value);
                }

                // 处理 Enumerable.Range 方法
                if (methodCallExpression.Method.DeclaringType == typeof(Enumerable) &&
                    methodCallExpression.Method.Name == "Range" &&
                    methodCallExpression.Arguments.Count == 2)
                {
                    if (!TryEvaluate(methodCallExpression.Arguments[0], out object start) ||
                        !TryEvaluate(methodCallExpression.Arguments[1], out object count))
                    {
                        return false;
                    }
                    value = Enumerable.Range(Convert.ToInt32(start), Convert.ToInt32(count)).ToArray();
                    return true;
                }

                // 处理 Enumerable.Repeat 方法
                if (methodCallExpression.Method.DeclaringType == typeof(Enumerable) &&
                    methodCallExpression.Method.Name == "Repeat" &&
                    methodCallExpression.Arguments.Count == 2)
                {
                    if (!TryEvaluate(methodCallExpression.Arguments[0], out object item) ||
                        !TryEvaluate(methodCallExpression.Arguments[1], out object repeatCount))
                    {
                        return false;
                    }
                    value = Enumerable.Repeat(item, Convert.ToInt32(repeatCount)).ToArray();
                    return true;
                }

                // 处理集合转换方法
                if (TryEvaluate(methodCallExpression.Object, out object sourceObj) && sourceObj is IEnumerable sourceEnumerable)
                {
                    return HandleEnumerableConversion(methodCallExpression, sourceEnumerable, out value);
                }

                // 通用方法调用评估
                return EvaluateGenericMethodCall(methodCallExpression, out value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 处理 ReadOnlySpan<T> 的隐式转换
        /// </summary>
        private static bool HandleReadOnlySpanConversion(MethodCallExpression methodCallExpression, out object value)
        {
            value = null;

            if (methodCallExpression.Arguments.Count == 1 &&
                TryEvaluate(methodCallExpression.Arguments[0], out object argumentValue))
            {
                if (argumentValue is IEnumerable enumerable)
                {
                    // 字符串不进行处理
                    if (argumentValue is string)
                    {
                        value = argumentValue;
                        return true;
                    }

                    // 尝试转换为数值集合
                    if (TryConvertEnumCollection(enumerable, out object numericCollection))
                    {
                        value = numericCollection;
                    }
                    else
                    {
                        value = argumentValue;
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 处理集合转换方法
        /// </summary>
        private static bool HandleEnumerableConversion(MethodCallExpression methodCallExpression, IEnumerable sourceEnumerable, out object value)
        {
            value = null;

            switch (methodCallExpression.Method.Name)
            {
                case "ToArray":
                    {
                        value = sourceEnumerable.Cast<object>().ToArray();
                        return true;
                    }
                case "ToList":
                    {
                        value = sourceEnumerable.Cast<object>().ToList();
                        return true;
                    }
                case "AsEnumerable":
                    {
                        value = sourceEnumerable;
                        return true;
                    }
                case "Cast":
                    {
                        value = sourceEnumerable.Cast<object>().ToArray();
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        /// <summary>
        /// 通用方法调用评估
        /// </summary>
        private static bool EvaluateGenericMethodCall(MethodCallExpression methodCallExpression, out object value)
        {
            value = null;

            // 优化：预分配数组，避免多次分配
            var argumentCount = methodCallExpression.Arguments.Count;
            var argumentValues = argumentCount > 0 ? new object[argumentCount] : Array.Empty<object>();

            for (int i = 0; i < argumentCount; i++)
            {
                if (!TryEvaluate(methodCallExpression.Arguments[i], out object argumentValue))
                {
                    return false;
                }
                argumentValues[i] = argumentValue;
            }

            // 评估方法实例
            object instance = null;
            if (methodCallExpression.Object != null)
            {
                if (!TryEvaluate(methodCallExpression.Object, out instance))
                {
                    return false;
                }
            }

            try
            {
                value = methodCallExpression.Method.Invoke(instance, argumentValues);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 尝试通过编译表达式获取值
        /// </summary>
        private static bool TryCompileAndEvaluate(Expression expr, out object value)
        {
            value = null;

            var type = expr.Type;
            // 特殊处理：ReadOnlySpan<T> (ref struct) -> 尝试通过MemoryExtensions.ToArray<T>转换为数组
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ReadOnlySpan<>))
            {
                var elementType = type.GetGenericArguments()[0];
                var memoryExtensionsType = typeof(MemoryExtensions);
                var toArrayMethod = memoryExtensionsType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .FirstOrDefault(m => m.Name == "ToArray" &&
                                        m.IsGenericMethodDefinition &&
                                        m.GetParameters().Length == 1);

                if (toArrayMethod != null)
                {
                    try
                    {
                        var genericMethod = toArrayMethod.MakeGenericMethod(elementType);
                        var call = Expression.Call(genericMethod, expr);
                        var convert = Expression.Convert(call, typeof(object));
                        var lambda = Expression.Lambda<Func<object>>(convert);
                        var func = lambda.Compile();
                        value = func();
                        return true;
                    }
                    catch
                    {
                        // 忽略异常，回退到普通编译
                    }
                }
            }

            // 最后回退到强类型编译（仅当表达式不依赖参数时才会成功）
            try
            {
                var convert = Expression.Convert(expr, typeof(object));
                var lambda = Expression.Lambda<Func<object>>(convert);
                var func = lambda.Compile();
                value = func();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 处理默认集合值，构建SQL IN子句参数
        /// </summary>
        /// <param name="parameters">参数字典</param>
        /// <param name="builder">SQL构建器</param>
        /// <param name="values">集合值</param>
        /// <param name="paramCounter">参数计数器引用</param>
        private static void HandleDefaultCollectionValues(Dictionary<string, object> parameters, StringBuilder builder, IEnumerable values, ref long paramCounter)
        {
            var list = values.Cast<object>().ToList();
            AppendCollectionParameters(parameters, builder, list, ref paramCounter);
        }

        /// <summary>
        /// 添加集合参数到SQL构建器
        /// </summary>
        /// <param name="parameters">参数字典</param>
        /// <param name="builder">SQL构建器</param>
        /// <param name="values">参数值集合</param>
        /// <param name="paramCounter">参数计数器引用</param>
        private static void AppendCollectionParameters<T>(Dictionary<string, object> parameters, StringBuilder builder, IEnumerable<T> values, ref long paramCounter)
        {
            var hasAdded = false;
            foreach (var value in values)
            {
                if (value == null)
                {
                    continue;
                }

                var parameterName = $"@P{paramCounter++}";
                parameters.Add(parameterName, value);
                builder.Append($"{parameterName},");
                hasAdded = true;
            }

            if (hasAdded && builder.Length > 0 && builder[^1] == ',')
            {
                builder.Length--;
            }
        }

        /// <summary>
        /// 判断类型是否为集合类型（排除字符串）
        /// </summary>
        /// <param name="type">要检查的类型</param>
        /// <returns>是否为集合类型</returns>
        private static bool IsEnumerableType(Type type)
        {
            if (type == typeof(string))
            {
                return false;
            }

            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// 尝试将枚举集合转换为数值集合
        /// </summary>
        /// <param name="enumerable">枚举集合</param>
        /// <param name="numericCollection">输出的数值集合</param>
        /// <returns>转换是否成功</returns>
        private static bool TryConvertEnumCollection(IEnumerable enumerable, out object numericCollection)
        {
            numericCollection = null;

            if (enumerable == null)
            {
                return false;
            }

            // 快速路径：检查常见的枚举集合类型
            if (enumerable is IEnumerable<Enum> enumEnumerable)
            {
                numericCollection = ConvertEnumEnumerable(enumEnumerable);
                return true;
            }

            // 通用路径：通过迭代器处理
            var enumerator = enumerable.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return false; // 空集合
            }

            object firstItem = enumerator.Current;
            if (firstItem == null)
            {
                return false;
            }

            Type firstItemType = firstItem.GetType();
            if (!firstItemType.IsEnum)
            {
                return false;
            }

            // 确定枚举的底层类型并创建合适的集合
            Type underlyingType = Enum.GetUnderlyingType(firstItemType);
            Type listType = typeof(List<>).MakeGenericType(underlyingType);
            IList resultList = (IList)Activator.CreateInstance(listType);

            // 添加第一个元素
            resultList.Add(Convert.ChangeType(firstItem, underlyingType));

            // 处理剩余元素
            while (enumerator.MoveNext())
            {
                object item = enumerator.Current;
                if (item == null || item.GetType() != firstItemType)
                {
                    return false;
                }
                resultList.Add(Convert.ChangeType(item, underlyingType));
            }

            numericCollection = resultList;
            return true;
        }

        /// <summary>
        /// 转换枚举可枚举对象为长整型列表
        /// </summary>
        /// <param name="enumEnumerable">枚举可枚举对象</param>
        /// <returns>长整型列表</returns>
        private static List<long> ConvertEnumEnumerable(IEnumerable<Enum> enumEnumerable)
        {
            // 优化：预分配列表容量
            var result = new List<long>();
            foreach (var enumValue in enumEnumerable)
            {
                result.Add(Convert.ToInt64(enumValue));
            }
            return result;
        }
    }
}