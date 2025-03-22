using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// WHERE方法条件表达式拓展类
    /// </summary>
    public static class WhereMethodCallExtensions
    {
        /// <summary>
        /// 处理方法条件查询条件
        /// </summary>
        /// <param name="expression">方法访问器</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="paramCounter">参数计数器</param>
        /// <param name="isUnary">是否来源于一元运算符的计算入口</param>
        /// <returns></returns>
        public static string HandlerWhereValues(this MethodCallExpression expression, Dictionary<string, object> parameters, ref long paramCounter, bool isUnary = false)
        {
            var builder = new StringBuilder();
            parameters = parameters ?? [];//非空的时候重新初始化
            var methodName = expression.Method.Name;//方法名称
            var declaringType = expression.Method.DeclaringType;//声明类型
            var lastArgumentInfo = expression.Arguments.LastOrDefault(); //最后一个元素参数
            if (declaringType == typeof(string) && expression.Object is MemberExpression columnNameExpression)
            {
                object columnValue = "";//字段名称
                var columnName = columnNameExpression.GetColumnName();//字段名称
                switch (lastArgumentInfo)
                {
                    case ConstantExpression constantExpr:
                        columnValue = constantExpr.Value?.ToString();
                        break;
                    case MemberExpression memberExpression1:
                        columnValue = Expression.Lambda(memberExpression1).Compile().DynamicInvoke();
                        break;
                }

                //生成属性的参数名称
                var propertyName = $"@P{paramCounter++}";

                // 根据方法名生成 LIKE 模式
                var likeColumnValue = methodName switch
                {
                    "Contains" => $"%{columnValue}%",
                    "StartsWith" => $"{columnValue}%",
                    "EndsWith" => $"%{columnValue}",
                    _ => throw new NotSupportedException()
                };
                parameters.Add(propertyName, likeColumnValue);
                builder.Append($"{columnName} {(isUnary ? "not " : "")}like {propertyName}");//拼接查询条件
                return builder.ToString();
            }

            if (declaringType == typeof(Enumerable) && lastArgumentInfo is MemberExpression memberExpression)
            {
                var columnName = memberExpression.GetColumnName();//字段名称
                switch (methodName)
                {
                    case "IsNullOrEmpty"://非空判断
                        builder.Append($"({columnName} is {(isUnary ? $"not null and {columnName}!=''" : $" null or {columnName}=''")})");
                        break;
                    case "Contains"://IN 和 NOT IN查询
                        builder.Append($"{columnName} {(isUnary ? "not " : "")}in (");//拼接查询条件
                        var collection = expression.Object ?? expression.Arguments[0]; //集合对象(如 list)
                        var instances = Expression.Lambda(collection).Compile().DynamicInvoke();//编译表达式获取实际对象实例
                        switch (instances)
                        {
                            case IEnumerable<Enum> enumValues:
                                HandleEnumCollectionValues(parameters, builder, enumValues, ref paramCounter);
                                break;
                            case IEnumerable collectionValues:
                                HandleDefaultCollectionValues(parameters, builder, collectionValues, ref paramCounter);
                                break;
                        }
                        builder.Append(")");
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 处理枚举类型参数(非字符串类型)
        /// </summary>
        /// <param name="parameters">查询参数</param>
        /// <param name="paramCounter">参数计数器</param>
        /// <param name="builder">查询条件</param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static void HandleEnumCollectionValues(Dictionary<string, object> parameters, StringBuilder builder, IEnumerable<Enum> values, ref long paramCounter)
        {
            var intValues = values.Cast<Enum>().Select(e => e.GetHashCode());
            AppendCollectionParameters(parameters, builder, intValues, ref paramCounter);
        }

        /// <summary>
        /// 处理枚集合型参数
        /// </summary>
        /// <param name="parameters">查询参数</param>
        /// <param name="paramCounter">参数计数器</param>
        /// <param name="builder">查询条件</param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static void HandleDefaultCollectionValues(Dictionary<string, object> parameters, StringBuilder builder, IEnumerable values, ref long paramCounter)
        {
            dynamic typedCollection = values;
            AppendCollectionParameters(parameters, builder, typedCollection, ref paramCounter);
        }

        /// <summary>
        /// 追加参数(非字符串类型)
        /// </summary>
        /// <param name="parameters">查询参数</param>
        /// <param name="paramCounter">参数计数器</param>
        /// <param name="builder">查询条件</param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static void AppendCollectionParameters<T>(Dictionary<string, object> parameters, StringBuilder builder, IEnumerable<T> values, ref long paramCounter)
        {
            foreach (var value in values)
            {
                var propertyName = $"@P{paramCounter++}";
                parameters.Add(propertyName, value);
                builder.Append($"{propertyName},");
            }

            if (builder.Length > 0 && builder[^1] == ',')
            {
                builder.Length--;
            }
        }
    }
}