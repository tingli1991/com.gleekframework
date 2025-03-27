using System.Linq.Expressions;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// 排序表达式访问器
    /// </summary>
    public class OrderExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// 排序
        /// </summary>
        private string _orderClause;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetOrderClause() => _orderClause;

        /// <summary>
        /// 访问成员表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            _orderClause = node.GetColumnName();
            return node;
        }
    }
}