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
        /// 构造函数
        /// </summary>
        /// <param name="parameters">SQL参数</param>
        public WhereExpressionVisitor(Dictionary<string, object> parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// 获取生成的WHERE子句
        /// </summary>
        public string GetWhereClause()
        {
            var whereClauseStr = WhereClause.ToString();
            if (whereClauseStr.IsNotNull() && whereClauseStr.StartsWith("(") && whereClauseStr.EndsWith(")"))
            {
                whereClauseStr = whereClauseStr.TrimStart("(").TrimEnd(")");
            }
            return whereClauseStr;
        }

        /// <summary>
        /// 处理一元运算符
        /// </summary>
        /// <param name="unaryExpression"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression unaryExpression)
        {
            if (unaryExpression.NodeType != ExpressionType.Not)
            {
                return base.VisitUnary(unaryExpression);
            }

            switch (unaryExpression.Operand)
            {
                case MemberExpression memberExpression:
                    if (memberExpression.Type == typeof(bool))
                    {
                        var propertyName = $"@P{ParamCounter++}";//参数名称
                        Parameters.Add(propertyName, false);//追加参数名称
                        WhereClause.Append($"{memberExpression.GetColumnName()}={propertyName}");
                        return unaryExpression;
                    }
                    break;
                case MethodCallExpression methodCallExpression:
                    var methodCallWhereValues = methodCallExpression.HandlerWhereValues(Parameters, ref ParamCounter, true);
                    WhereClause.Append(methodCallWhereValues);
                    return unaryExpression;
            }
            return base.VisitUnary(unaryExpression);
        }

        /// <summary>
        /// 访问方法调用表达式
        /// </summary>
        /// <param name="methodCallExpression"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
        {
            var methodCallWhereValues = methodCallExpression.HandlerWhereValues(Parameters, ref ParamCounter);
            WhereClause.Append(methodCallWhereValues);
            return methodCallExpression;
        }

        /// <summary>
        /// 访问二元表达式
        /// </summary>
        protected override Expression VisitBinary(BinaryExpression binaryExpression)
        {
            WhereClause.Append("(");
            Visit(binaryExpression.Left);
            WhereClause.Append(binaryExpression.ToOperator());
            Visit(binaryExpression.Right);
            WhereClause.Append(")");
            return binaryExpression;
        }

        /// <summary>
        /// 访问成员表达式
        /// </summary>
        protected override Expression VisitMember(MemberExpression node)
        {
            WhereClause.Append(node.GetColumnName());
            return node;
        }

        /// <summary>
        /// 访问常量表达式
        /// </summary>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            var propertyName = $"@P{ParamCounter++}";//参数名称
            WhereClause.Append(propertyName);
            Parameters.Add(propertyName, node.Value);
            return node;
        }
    }
}