using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
        private int _paramCounter;

        /// <summary>
        /// SQL参数
        /// </summary>
        private readonly Dictionary<string, object> _parameters;

        /// <summary>
        /// WHERE子句
        /// </summary>
        private readonly StringBuilder _whereClause = new StringBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="parameters">SQL参数</param>
        public WhereExpressionVisitor(Dictionary<string, object> parameters)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// 获取生成的WHERE子句
        /// </summary>
        public string GetWhereClause()
        {
            var whereClauseStr = _whereClause.ToString();
            if (whereClauseStr.IsNotNull() && whereClauseStr.StartsWith("(") && whereClauseStr.EndsWith(")"))
            {
                whereClauseStr = whereClauseStr.TrimStart("(").TrimEnd(")");
            }
            return whereClauseStr;
        }

        /// <summary>
        /// 处理一元运算符
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                var operand = node.Operand;
                switch (operand)
                {
                    case MemberExpression memberExpr:
                        if (memberExpr.Type == typeof(bool))
                        {
                            _whereClause.Append($"{memberExpr.GetColumnName()}=0");
                            return node;
                        }
                        break;
                    case MethodCallExpression methodCallExpression:
                        var containsValueString = methodCallExpression.GetContainsSQL(true);
                        if (!containsValueString.IsNullOrEmpty())
                        {
                            _whereClause.Append(containsValueString);
                            return node;
                        }
                        break;
                }
            }
            return base.VisitUnary(node);
        }

        /// <summary>
        /// 访问方法调用表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var containsValueString = node.GetContainsSQL();
            if (!containsValueString.IsNullOrEmpty())
            {
                _whereClause.Append(containsValueString);
                return node;
            }
            return base.VisitMethodCall(node);
        }

        /// <summary>
        /// 访问二元表达式
        /// </summary>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            _whereClause.Append("(");
            Visit(node.Left);
            _whereClause.Append(node.ToOperator());
            Visit(node.Right);
            _whereClause.Append(")");
            return node;
        }

        /// <summary>
        /// 访问成员表达式
        /// </summary>
        protected override Expression VisitMember(MemberExpression node)
        {
            _whereClause.Append(node.GetColumnName());
            return node;
        }

        /// <summary>
        /// 访问常量表达式
        /// </summary>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            var paramName = $"@p{_paramCounter++}";
            _whereClause.Append(paramName);
            _parameters.Add(paramName, node.Value);
            return node;
        }
    }
}