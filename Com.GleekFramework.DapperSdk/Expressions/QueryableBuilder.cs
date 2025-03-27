using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    ///  查询构建器，支持条件过滤、排序和分页功能
    /// </summary>
    /// <typeparam name="TEntity">查询的实体类型</typeparam>
    public class QueryableBuilder<TEntity> : QueryableBuilder<TEntity, TEntity>
    {

    }

    /// <summary>
    ///  查询构建器，支持条件过滤、排序和分页功能
    /// </summary>
    /// <typeparam name="TEntity">查询的实体类型</typeparam>
    /// <typeparam name="TResult">返回的实体类型</typeparam>
    public class QueryableBuilder<TEntity, TResult>
    {
        /// <summary>
        /// Count执行脚本
        /// </summary>
        public StringBuilder CountSQL;

        /// <summary>
        /// SQL执行脚本
        /// </summary>
        public StringBuilder ExecuteSQL;

        /// <summary>
        /// 查询的实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 要取的元素数量
        /// </summary>
        private long TakeCount { get; set; }

        /// <summary>
        /// 要跳过的元素数量
        /// </summary>
        private long SkipCount { get; set; }

        /// <summary>
        /// SQL参数列表
        /// </summary>
        public Dictionary<string, object> Parameters;

        /// <summary>
        /// 存储过滤条件表达式
        /// </summary>
        private Expression<Func<TEntity, bool>> FilterExpression;

        /// <summary>
        /// 存储排序表达式和排序方向（升序或降序）
        /// </summary>
        private readonly List<(Expression expression, bool isAscending)> OrderExpressions = [];

        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryableBuilder() => EntityType = typeof(TEntity);

        /// <summary>
        /// 跳过序列中指定数量的元素
        /// </summary>
        /// <param name="count">要跳过的元素数量</param>
        /// <returns></returns>
        public QueryableBuilder<TEntity, TResult> Skip(long count = 1)
        {
            SkipCount = count;
            return this;
        }

        /// <summary>
        /// 从序列中取指定数量的连续元素
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public QueryableBuilder<TEntity, TResult> Take(long count = 1)
        {
            TakeCount = count;
            return this;
        }

        /// <summary>
        /// 设置分页参数
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public QueryableBuilder<TEntity, TResult> Page(long pageIndex = 1, long pageSize = 20)
        {
            return Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="predicate">过滤条件的Lambda表达式</param>
        /// <returns>当前查询构建器实例</returns>
        public QueryableBuilder<TEntity, TResult> Where(Expression<Func<TEntity, bool>> predicate)
        {
            // 如果已有过滤条件，则使用AND逻辑组合新条件
            FilterExpression = FilterExpression == null ? predicate : FilterExpression.And(predicate);
            return this;
        }

        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="predicate">过滤条件的Lambda表达式</param>
        /// <returns>当前查询构建器实例</returns>
        public QueryableBuilder<TEntity, TResult> WhereIf(bool condition, Expression<Func<TEntity, bool>> predicate)
        {
            if (!condition)
            {
                return this;
            }
            return Where(predicate);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="orderDic">排序规则字典</param>
        /// <returns>当前查询构建器实例。</returns>
        public QueryableBuilder<TEntity, TResult> Order(Dictionary<string, string> orderDic)
        {
            if (orderDic.IsNullOrEmpty())
            {
                return this;
            }
            var orderExpression = new OrderExpressionConverter<TEntity>();
            OrderExpressions.AddRange(orderExpression.Convert(orderDic));
            return this;
        }

        /// <summary>
        /// 添加升序条件
        /// </summary>
        /// <typeparam name="TKey">排序字段的类型</typeparam>
        /// <param name="orderExpression">排序字段的Lambda表达式</param>
        /// <returns>当前查询构建器实例。</returns>
        public QueryableBuilder<TEntity, TResult> OrderBy<TKey>(Expression<Func<TEntity, TKey>> orderExpression)
        {
            // 将排序表达式和排序方向存储到列表中
            OrderExpressions.Add((orderExpression.Body, true));
            return this;
        }

        /// <summary>
        /// 添加降序条件
        /// </summary>
        /// <typeparam name="TKey">排序字段的类型</typeparam>
        /// <param name="keySelector">排序字段的Lambda表达式</param>
        /// <returns>当前查询构建器实例。</returns>
        public QueryableBuilder<TEntity, TResult> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
        {
            // 将排序表达式和排序方向存储到列表中
            OrderExpressions.Add((keySelector.Body, false));
            return this;
        }

        /// <summary>
        /// 构建SQL脚本
        /// </summary>
        /// <param name="databaseType">数据库类型</param>
        public void Build(DatabaseType databaseType)
        {
            //重新计算参数
            Parameters = [];
            CountSQL = new StringBuilder();
            ExecuteSQL = new StringBuilder();

            //SELECT
            var selectExpression = new SelectExpressionVisitor<TEntity, TResult>();
            selectExpression.Visit();
            ExecuteSQL.Append($"select {selectExpression.GetSelectColumns()}");


            //FROM
            ExecuteSQL.Append($" from {EntityType.GetTableName()}");

            //COUNT
            CountSQL.Append($"select count(1) from {EntityType.GetTableName()}");

            //WHERE
            if (FilterExpression != null)
            {
                var whereVisitor = new WhereExpressionVisitor(Parameters);
                whereVisitor.Visit(FilterExpression);
                ExecuteSQL.Append($" where {whereVisitor.GetWhereClause()}");
                CountSQL.Append($" where {whereVisitor.GetWhereClause()}");
            }

            //ORDER BY
            if (OrderExpressions.Any())
            {
                var orderBuilder = new StringBuilder();
                foreach (var (expression, isAscending) in OrderExpressions)
                {
                    var orderVisitor = new OrderExpressionVisitor();
                    orderVisitor.Visit(expression);
                    orderBuilder.Append($"{orderVisitor.GetOrderClause()} {(isAscending ? "asc" : "desc")},");
                }
                ExecuteSQL.Append($" order by {orderBuilder.ToString().TrimEnd(',', ' ').TrimEnd()}");
            }


            if (TakeCount > 0 && SkipCount <= 0)
            {
                //TAKE(读取的元素数量)
                switch (databaseType)
                {
                    case DatabaseType.MySQL:
                    case DatabaseType.PgSQL:
                    case DatabaseType.SQLite:
                        ExecuteSQL.Append($" limit {TakeCount}");
                        break;
                    case DatabaseType.MsSQL:
                        ExecuteSQL.Replace("select ", $"select top {TakeCount} ");
                        break;
                    default:
                        throw new Exception($"{databaseType} 暂不支持读取的元素数量");
                }
            }
            else
            {
                if (TakeCount > 0 || SkipCount > 0)
                {
                    //PAGING|SKIP
                    switch (databaseType)
                    {
                        case DatabaseType.MySQL:
                        case DatabaseType.PgSQL:
                        case DatabaseType.SQLite:
                            ExecuteSQL.Append($" limit {TakeCount} offset {SkipCount}");
                            break;
                        case DatabaseType.MsSQL:
                            ExecuteSQL.Append($"offset {SkipCount} rows fetch next {TakeCount} rows only");
                            break;
                        default:
                            throw new Exception($"{databaseType} 暂不支持分页查询！");
                    }
                }
            }
            TakeCount = 0;//清空要取的元素数量
            SkipCount = 0;//清空要跳过的元素数量
        }
    }
}