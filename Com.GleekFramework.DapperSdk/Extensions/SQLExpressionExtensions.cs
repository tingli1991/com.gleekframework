using Com.GleekFramework.CommonSdk;
using System;
using System.Collections;
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
        public static string HandlerSelectValues(this Type type)
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