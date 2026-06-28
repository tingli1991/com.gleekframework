using Com.GleekFramework.CommonSdk;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// SQL表达式拓展类
    /// </summary>
    public static class SQLExpressionExtensions
    {
        /// <summary>
        /// 判断子表达式是否需要括号包裹
        /// </summary>
        /// <param name="parent">父级表达式</param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static bool IsNeedParenthesis(this BinaryExpression parent, Expression child)
        {
            if (child is BinaryExpression childBinary)
            {
                var parentPrecedence = parent.NodeType.GetPrecedence();
                var childPrecedence = childBinary.NodeType.GetPrecedence();
                return childPrecedence < parentPrecedence;// 子表达式优先级低于父表达式时需要括号
            }
            return false;//非二元表达式不需要括号
        }

        /// <summary>
        /// 获取表达式类型的运算符优先级（数值越大优先级越高）
        /// <para>优先级规则参考标准SQL运算符优先级设计：</para>
        /// <list type="bullet">
        /// <item>1. 算术运算符 > 比较运算符 > 逻辑运算符</item>
        /// <item>2. 同级运算符按标准优先级处理（如 AND 高于 OR）</item>
        /// <item>3. 未明确列出的运算符默认优先级最低</item>
        /// </list>
        /// </summary>
        /// <param name="nodeType">表达式节点类型</param>
        /// <returns>优先级数值（0表示最低优先级）</returns>
        private static int GetPrecedence(this ExpressionType nodeType)
        {
            return nodeType switch
            {
                //逻辑运算符
                ExpressionType.OrElse => 1,//SQL OR
                ExpressionType.AndAlso => 2,//SQL AND

                //比较运算符 =
                ExpressionType.Equal or ExpressionType.NotEqual or ExpressionType.GreaterThan or ExpressionType.GreaterThanOrEqual or ExpressionType.LessThan or ExpressionType.LessThanOrEqual or ExpressionType.Not => 3,

                //算术运算符
                ExpressionType.Add or ExpressionType.Subtract => 4, // + -
                ExpressionType.Multiply or ExpressionType.Divide or ExpressionType.Modulo => 5,// * / %

                // 位运算符（需根据数据库支持情况处理）
                ExpressionType.ExclusiveOr => 6, // ^
                ExpressionType.And or ExpressionType.Or => 7,// &（按位与）

                // 未处理的运算符类型默认最低优先级
                _ => 0,
            };
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