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
    /// </summary>
    public static class WhereMethodCallExtensions
    {
        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static object GetMemberValue(this MemberExpression node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.Expression is ParameterExpression)
            {
                //若成员引用的是参数（如 e.X），视为列，不去尝试解析
                return null;
            }

            // 递归解析闭包实例
            var parent = TryEvaluate(node.Expression, out var parentValue);
            if (!parent)
            {
                return null;
            }

            if (node.Member is PropertyInfo prop)
            {
                return prop.GetValue(parentValue);
            }
            else if (node.Member is FieldInfo field)
            {
                return field.GetValue(parentValue);
            }
            return null;
        }

        /// <summary>
        /// 处理Where方法调用表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="parameters"></param>
        /// <param name="paramCounter"></param>
        /// <param name="isUnary"></param>
        /// <returns></returns>
        public static string HandlerWhereValues(this MethodCallExpression expression, Dictionary<string, object> parameters, ref long paramCounter, bool isUnary = false)
        {
            var builder = new StringBuilder();
            parameters ??= new Dictionary<string, object>();
            var methodName = expression.Method.Name;
            var declaringType = expression.Method.DeclaringType;
            var lastArgumentInfo = expression.Arguments.LastOrDefault();
            if (declaringType == typeof(string) && expression.Object == null && lastArgumentInfo is MemberExpression memberExpressionStr)
            {
                var columnName = memberExpressionStr.GetColumnName();
                switch (methodName)
                {
                    case "IsNullOrEmpty":
                        builder.Append($"({columnName} is {(isUnary ? $"not null and {columnName}!=''" : $"null or {columnName}=''")})");
                        break;
                }
                return builder.ToString();
            }

            // x.Prop.Contains/StartsWith/EndsWith(...)
            if (declaringType == typeof(string) && expression.Object is MemberExpression columnNameExpression)
            {
                var columnName = columnNameExpression.GetColumnName();
                if (!TryEvaluate(lastArgumentInfo, out var columnValue))
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

            // 集合 Contains -> IN / NOT IN
            if (methodName == "Contains")
            {
                MemberExpression columnMember = null;
                if (expression.Object is MemberExpression objMe && !IsEnumerableType(objMe.Type))
                {
                    columnMember = objMe;
                }
                else
                {
                    columnMember = expression.Arguments.OfType<MemberExpression>().FirstOrDefault();
                }

                if (columnMember != null)
                {
                    Expression collectionExpr = null;
                    if (expression.Object != null && !ReferenceEquals(expression.Object, columnMember))
                    {
                        collectionExpr = expression.Object;
                    }
                    else
                    {
                        collectionExpr = expression.Arguments.FirstOrDefault(a => !ReferenceEquals(a, columnMember));
                    }

                    if (collectionExpr == null)
                    {
                        throw new NotSupportedException("无法解析 Contains 的集合来源");
                    }

                    if (!TryEvaluate(collectionExpr, out var instancesObj))
                    {
                        throw new NotSupportedException("无法在构建时解析 Contains 的集合，请确保集合为闭包常量或可求值表达式");
                    }

                    var columnName = columnMember.GetColumnName();
                    if (instancesObj == null)
                    {
                        builder.Append(isUnary ? "(1=1)" : "(1=0)");
                        return builder.ToString();
                    }

                    if (instancesObj is IEnumerable instEnum)
                    {
                        builder.Append($"{columnName} {(isUnary ? "not " : "")}in (");
                        HandleDefaultCollectionValues(parameters, builder, instEnum, ref paramCounter);
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
                        var p = $"@P{paramCounter++}";
                        parameters.Add(p, instancesObj);
                        builder.Append($"{columnName} {(isUnary ? "<>" : "=")} {p}");
                        return builder.ToString();
                    }
                }

                //static Enumerable.Contains(collection, column)
                if (declaringType == typeof(Enumerable) && expression.Arguments.Count >= 2 && expression.Arguments[0] != null && expression.Arguments[1] is MemberExpression memberExpression)
                {
                    var columnName = memberExpression.GetColumnName();
                    var collectionExpr = expression.Arguments[0];
                    if (!TryEvaluate(collectionExpr, out var instancesObj))
                    {
                        throw new NotSupportedException("无法在构建时解析 Enumerable.Contains 的集合来源");
                    }

                    if (instancesObj is IEnumerable instEnum)
                    {
                        builder.Append($"{columnName} {(isUnary ? "not " : "")}in (");
                        HandleDefaultCollectionValues(parameters, builder, instEnum, ref paramCounter);
                        builder.Append(")");
                        return builder.ToString();
                    }
                    else
                    {
                        var p = $"@P{paramCounter++}";
                        parameters.Add(p, instancesObj);
                        builder.Append($"{columnName} {(isUnary ? "<>" : "=")} {p}");
                        return builder.ToString();
                    }
                }
            }
            throw new NotSupportedException($"方法 {expression.Method} 不被支持用于条件构建");
        }

        /// <summary>
        /// 尝试评估表达式，成功返回 true（value 可以为 null），失败返回 false。
        /// 不会以异常作为常规控制流。
        /// 针对 ReadOnlySpan<T>/ref struct 做特殊处理，转换为数组后返回。
        /// </summary>
        private static bool TryEvaluate(Expression expr, out object value)
        {
            value = null;
            if (expr == null)
            {
                return true;
            }

            switch (expr)
            {
                case ConstantExpression c:
                    value = c.Value;
                    return true;
                case MemberExpression m:
                    if (m.Expression is ParameterExpression) return false;
                    if (!TryEvaluate(m.Expression, out var parent)) return false;
                    if (parent == null)
                    {
                        if (m.Member is PropertyInfo || m.Member is FieldInfo)
                        {
                            value = null;
                            return true;
                        }
                        return false;
                    }
                    if (m.Member is PropertyInfo pi)
                    {
                        value = pi.GetValue(parent);
                        return true;
                    }
                    if (m.Member is FieldInfo fi)
                    {
                        value = fi.GetValue(parent);
                        return true;
                    }
                    return false;

                case UnaryExpression u when (u.NodeType == ExpressionType.Convert || u.NodeType == ExpressionType.ConvertChecked):
                    return TryEvaluate(u.Operand, out value);
                case NewArrayExpression na:
                    {
                        var list = new List<object>();
                        foreach (var e in na.Expressions)
                        {
                            if (!TryEvaluate(e, out var ev)) return false;
                            list.Add(ev);
                        }
                        value = list.ToArray();
                        return true;
                    }
                case ListInitExpression li:
                    {
                        if (!TryEvaluate(li.NewExpression, out var created)) return false;
                        if (!(created is IList list)) return false;
                        foreach (var init in li.Initializers)
                        {
                            foreach (var a in init.Arguments)
                            {
                                if (!TryEvaluate(a, out var av)) return false;
                                list.Add(av);
                            }
                        }
                        value = list;
                        return true;
                    }
                case NewExpression ne:
                    {
                        if (ne.Arguments == null || ne.Arguments.Count == 0)
                        {
                            try
                            {
                                value = Activator.CreateInstance(ne.Type);
                                return true;
                            }
                            catch { return false; }
                        }
                        break;
                    }
                case MethodCallExpression mc:
                    {
                        try
                        {
                            // --- 新增：处理 op_Implicit 从数组/集合到 ReadOnlySpan<T> 的情况 ---
                            if (mc.Method.IsSpecialName && mc.Method.ReturnType.IsGenericType && mc.Method.ReturnType.GetGenericTypeDefinition() == typeof(ReadOnlySpan<>))
                            {
                                // 通常形态： op_Implicit(array) -> ReadOnlySpan<T>
                                if (mc.Arguments.Count == 1 && TryEvaluate(mc.Arguments[0], out var argVal))
                                {
                                    if (argVal is IEnumerable enumerable)
                                    {
                                        // 排除字符串
                                        if (argVal is string)
                                        {
                                            value = argVal;
                                            return true;
                                        }

                                        // 尝试作为枚举集合处理
                                        if (TryConvertEnumCollection(enumerable, out var numericCollection))
                                        {
                                            // 成功转换为数值集合
                                            value = numericCollection;
                                        }
                                        else
                                        {
                                            // 保持原始集合
                                            value = argVal;
                                        }
                                        return true;
                                    }
                                }
                            }

                            if (mc.Method.DeclaringType == typeof(Enumerable) && mc.Method.Name == "Range" && mc.Arguments.Count == 2)
                            {
                                if (!TryEvaluate(mc.Arguments[0], out var s) || !TryEvaluate(mc.Arguments[1], out var c)) return false;
                                value = Enumerable.Range(Convert.ToInt32(s), Convert.ToInt32(c)).ToArray();
                                return true;
                            }

                            if (mc.Method.DeclaringType == typeof(Enumerable) && mc.Method.Name == "Repeat" && mc.Arguments.Count == 2)
                            {
                                if (!TryEvaluate(mc.Arguments[0], out var item) || !TryEvaluate(mc.Arguments[1], out var cnt)) return false;
                                value = Enumerable.Repeat(item, Convert.ToInt32(cnt)).ToArray();
                                return true;
                            }

                            if (TryEvaluate(mc.Object, out var srcObj) && srcObj is IEnumerable srcEnum)
                            {
                                switch (mc.Method.Name)
                                {
                                    case "ToArray":
                                        value = srcEnum.Cast<object>().ToArray();
                                        return true;
                                    case "ToList":
                                        value = srcEnum.Cast<object>().ToList();
                                        return true;
                                    case "AsEnumerable":
                                        value = srcEnum;
                                        return true;
                                    case "Cast":
                                        value = srcEnum.Cast<object>().ToArray();
                                        return true;
                                }
                            }

                            var argsOk = true;
                            var argValues = new object[mc.Arguments.Count];
                            for (int i = 0; i < mc.Arguments.Count; i++)
                            {
                                if (!TryEvaluate(mc.Arguments[i], out var av)) { argsOk = false; break; }
                                argValues[i] = av;
                            }
                            object instance = null;
                            if (mc.Object != null)
                            {
                                if (!TryEvaluate(mc.Object, out instance)) argsOk = false;
                            }

                            if (argsOk)
                            {
                                try
                                {
                                    value = mc.Method.Invoke(instance, argValues);
                                    return true;
                                }
                                catch
                                {
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    break;
            }

            try
            {
                // special-case: ReadOnlySpan<T> (ref struct) -> try convert to array via MemoryExtensions.ToArray<T>
                var t = expr.Type;
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ReadOnlySpan<>))
                {
                    var elemType = t.GetGenericArguments()[0];
                    var memExtType = typeof(MemoryExtensions);
                    var toArrayMethod = memExtType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .FirstOrDefault(m => m.Name == "ToArray" && m.IsGenericMethodDefinition && m.GetParameters().Length == 1);
                    if (toArrayMethod != null)
                    {
                        var gen = toArrayMethod.MakeGenericMethod(elemType);
                        var call = Expression.Call(gen, expr);
                        var convert = Expression.Convert(call, typeof(object));
                        var lambda = Expression.Lambda<Func<object>>(convert);
                        var func = lambda.Compile();
                        value = func();
                        return true;
                    }
                }
            }
            catch
            {
                // 忽略并回退到普通编译回退
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
                value = null;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="builder"></param>
        /// <param name="values"></param>
        /// <param name="paramCounter"></param>
        private static void HandleDefaultCollectionValues(Dictionary<string, object> parameters, StringBuilder builder, IEnumerable values, ref long paramCounter)
        {
            var list = values.Cast<object>().ToList();
            AppendCollectionParameters(parameters, builder, list, ref paramCounter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="builder"></param>
        /// <param name="values"></param>
        /// <param name="paramCounter"></param>
        private static void AppendCollectionParameters<T>(Dictionary<string, object> parameters, StringBuilder builder, IEnumerable<T> values, ref long paramCounter)
        {
            var added = false;
            foreach (var value in values)
            {
                if (value == null) continue;
                var name = $"@P{paramCounter++}";
                parameters.Add(name, value);
                builder.Append($"{name},");
                added = true;
            }
            if (added && builder.Length > 0 && builder[^1] == ',') builder.Length--;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsEnumerableType(Type type)
        {
            if (type == typeof(string)) return false;
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="numericCollection"></param>
        /// <returns></returns>
        private static bool TryConvertEnumCollection(IEnumerable enumerable, out object numericCollection)
        {
            numericCollection = null;

            if (enumerable == null)
                return false;

            // 快速路径：检查常见集合类型
            switch (enumerable)
            {
                case IEnumerable<Enum> enumEnumerable:
                    // 这是明确的枚举集合
                    numericCollection = ConvertEnumEnumerable(enumEnumerable);
                    return true;
            }

            // 通用路径
            var enumerator = enumerable.GetEnumerator();
            if (!enumerator.MoveNext())
                return false; // 空集合

            object firstItem = enumerator.Current;
            if (firstItem == null)
                return false;

            Type firstItemType = firstItem.GetType();
            if (!firstItemType.IsEnum)
                return false;

            // 确定枚举的底层类型并创建合适的集合
            Type underlyingType = Enum.GetUnderlyingType(firstItemType);

            // 使用动态方法创建对应类型的列表
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
        /// 
        /// </summary>
        /// <param name="enumEnumerable"></param>
        /// <returns></returns>
        private static List<long> ConvertEnumEnumerable(IEnumerable<Enum> enumEnumerable)
        {
            var result = new List<long>();
            foreach (var enumValue in enumEnumerable)
            {
                result.Add(Convert.ToInt64(enumValue));
            }
            return result;
        }
    }
}