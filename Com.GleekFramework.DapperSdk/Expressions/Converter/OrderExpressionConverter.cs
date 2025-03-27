using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// 排序表达式转换类
    /// </summary>
    /// <typeparam name="TEntity">查询的实体类型</typeparam>
    public class OrderExpressionConverter<TEntity>
    {
        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="orderDic"></param>
        /// <returns></returns>
        public List<(Expression expression, bool isAscending)> Convert(Dictionary<string, string> orderDic)
        {
            var orderExpressionList = new List<(Expression, bool)>();
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            foreach (var kvp in orderDic)
            {
                var propertyExpression = Expression.Property(parameter, kvp.Key);//构建属性访问表达式
                var isAscending = ParseSortDirection(kvp.Value);//解析排序方向
                orderExpressionList.Add((propertyExpression, isAscending));
            }
            return orderExpressionList;
        }

        /// <summary>
        /// 解析排序的方向
        /// </summary>
        /// <param name="direction"></param>
        /// <returns>是否为升序，true：升序，false：降序</returns>
        private static bool ParseSortDirection(string direction)
        {
            if (string.IsNullOrEmpty(direction))
            {
                // 默认升序
                return true;
            }
            return direction.StartsWith("asc", StringComparison.OrdinalIgnoreCase);
        }
    }
}