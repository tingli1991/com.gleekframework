using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// SELECT表达式访问器
    /// </summary>
    public class SelectExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// 源对象类型
        /// </summary>
        private Type SourceType { get; set; }

        /// <summary>
        /// 最终显示的列集合
        /// </summary>
        private readonly List<string> Columns = new List<string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        public SelectExpressionVisitor(Type type) => SourceType = type;

        /// <summary>
        /// 获取解析后的字段列表（例如 "t0.Name, t0.Age"）
        /// </summary>
        public string GetSelectValues()
        {
            return Columns.Any() ? string.Join(",", Columns) : SourceType.HandlerSelectValues(); // 无字段时返回全表
        }

        /// <summary>
        /// 访问成员
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression expression)
        {
            var columnName = expression.GetColumnName();//列名称
            Columns.Add($"{columnName ?? expression.Member.Name}");
            return base.VisitMember(expression);
        }
    }
}
