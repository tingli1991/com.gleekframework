using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// SQL表达式拓展类
    /// </summary>
    public static class SQLExpressionExtensions
    {
        /// <summary>
        /// 获取查询
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetColumnsSQL(this Type type)
        {
            if (type == null)
            {
                throw new NullReferenceException(nameof(type));
            }

            var builder = new StringBuilder();
            var propertyInfoList = type.GetPropertyInfoList();
            var columnList = propertyInfoList.Select(e => e.GetCustomAttribute<ColumnAttribute>()?.Name ?? e.Name);
            builder.AppendJoin(",", columnList);
            return builder.ToString();
        }

        /// <summary>
        /// 获取In和Not In查询的拼接字符
        /// </summary>
        /// <param name="methodCallExpression">方法访问器</param>
        /// <param name="isNotIn">是否是not in查询入口</param>
        /// <returns></returns>
        public static string GetContainsSQL(this MethodCallExpression methodCallExpression, bool isNotIn = false)
        {
            var builder = new StringBuilder();
            if (methodCallExpression.Method.Name.Equals("Contains"))
            {
                var collection = methodCallExpression.Object ?? methodCallExpression.Arguments[0]; //集合对象(如 list)
                var elementInfo = methodCallExpression.Arguments.Count > 1 ? methodCallExpression.Arguments[1] : methodCallExpression.Arguments[0]; //元素参数(如 x)
                var instances = Expression.Lambda(collection).Compile().DynamicInvoke();//编译表达式获取实际对象实例
                if (elementInfo is MemberExpression memberExpr)
                {
                    var columnName = memberExpr.GetColumnName();//字段名称
                    if (columnName.IsNullOrEmpty())
                    {
                        throw new ArgumentNullException(nameof(memberExpr));
                    }

                    builder.Append($"{columnName} {(isNotIn ? "not " : "")}in (");
                    switch (instances)
                    {
                        case IEnumerable<int> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<uint> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<long> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<ulong> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<double> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<float> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<decimal> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<byte> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<sbyte> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<short> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<ushort> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<char> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<bool> values:
                            builder.AppendJoin(",", values);
                            break;
                        case IEnumerable<string> values:
                            builder.Append("'").AppendJoin("','", values).Append("'");
                            break;
                        case IEnumerable<Enum> values:
                            builder.AppendJoin(",", values.Select(e => e.GetHashCode()));
                            break;
                    }
                    builder.Append(")");
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取列名称
        /// </summary>
        /// <param name="memberExpression"></param>
        /// <returns></returns>
        public static string GetColumnName(this MemberExpression memberExpression)
        {
            var columnAttribute = memberExpression.Member.GetCustomAttribute<ColumnAttribute>();
            return $"{columnAttribute?.Name ?? memberExpression.Member.Name}";
        }

        /// <summary>
        /// 实际应根据自定义属性获取表名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetTableName(this Type type)
        {
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            return tableAttribute?.Name ?? type.Name;
        }

        /// <summary>
        /// 转换成操作类型
        /// </summary>
        /// <param name="binaryExpression"></param>
        /// <returns></returns>
        public static string ToOperator(this BinaryExpression binaryExpression)
        {
            switch (binaryExpression.NodeType)
            {
                case ExpressionType.Equal:
                    return " = ";
                case ExpressionType.NotEqual:
                    return " != ";
                case ExpressionType.GreaterThan:
                    return " > ";
                case ExpressionType.GreaterThanOrEqual:
                    return " >= ";
                case ExpressionType.LessThan:
                    return " < ";
                case ExpressionType.LessThanOrEqual:
                    return " <= ";
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " and ";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " or ";
                default:
                    throw new NotSupportedException($"运算符 {binaryExpression.NodeType} 不支持");
            }
        }
    }
}