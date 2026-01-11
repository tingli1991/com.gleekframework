using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ZstdSharp;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// WHERE条件表达式访问器，用于将Lambda表达式转换为SQL条件
    /// </summary>
    public class WhereExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// 参数计数器
        /// </summary>
        private long ParamCounter;

        /// <summary>
        /// WHERE子句
        /// </summary>
        private readonly StringBuilder WhereClause = new();

        /// <summary>
        /// SQL参数
        /// </summary>
        private readonly Dictionary<string, object> Parameters;

        /// <summary>
        /// 获取生成的WHERE子句
        /// </summary>
        public string GetWhereClause() => WhereClause.ToString();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameters">SQL参数</param>
        public WhereExpressionVisitor(Dictionary<string, object> parameters) => Parameters = parameters;

        /// <summary>
        /// 处理一元运算符
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression expression)
        {
            if (expression.NodeType != ExpressionType.Not)
            {
                return base.VisitUnary(expression);
            }

            switch (expression.Operand)
            {
                case MemberExpression memberExpression:
                    if (memberExpression.Type == typeof(bool))
                    {
                        var propertyName = $"@P{ParamCounter++}";//参数名称
                        Parameters.Add(propertyName, false);//追加参数名称
                        WhereClause.Append($"{memberExpression.GetColumnName()}={propertyName}");
                        return expression;
                    }
                    break;
                case MethodCallExpression methodCallExpression:
                    var methodCallWhereValues = methodCallExpression.HandlerWhereValues(Parameters, ref ParamCounter, true);
                    WhereClause.Append(methodCallWhereValues);
                    return expression;
            }
            return base.VisitUnary(expression);
        }

        /// <summary>
        /// 访问方法调用表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            var methodCallWhereValues = expression.HandlerWhereValues(Parameters, ref ParamCounter);
            WhereClause.Append(methodCallWhereValues);
            return expression;
        }

        /// <summary>
        /// 访问二元表达式
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression expression)
        {
            //处理左子表达式
            var leftNeedParen = expression.IsNeedParenthesis(expression.Left);//判定左子表达式是否需要添加括号
            if (leftNeedParen) WhereClause.Append("(");
            Visit(expression.Left);
            if (leftNeedParen) WhereClause.Append(")");

            //添加当前的运算符
            WhereClause.Append(expression.ToOperator());

            //处理右子表达式
            var rightNeedParen = expression.IsNeedParenthesis(expression.Right);//判定左子表达式是否需要添加括号
            if (rightNeedParen) WhereClause.Append("(");
            Visit(expression.Right);
            if (rightNeedParen) WhereClause.Append(")");

            //返回表达式
            return expression;
        }

        /// <summary>
        /// 访问成员表达式
        /// </summary>
        protected override Expression VisitMember(MemberExpression expression)
        {
            var memberValue = expression.GetMemberValue();
            if (memberValue != null)
            {
                var propertyName = $"@P{ParamCounter++}";//参数名称
                Parameters.Add(propertyName, memberValue);
                WhereClause.Append(propertyName);
            }
            else
            {
                WhereClause.Append(expression.GetColumnName());
            }
            return expression;
        }

        /// <summary>
        /// 访问常量表达式
        /// </summary>
        protected override Expression VisitConstant(ConstantExpression expression)
        {
            var propertyName = $"@P{ParamCounter++}";//参数名称
            WhereClause.Append(propertyName);
            Parameters.Add(propertyName, expression.Value);
            return expression;
        }
    }
}