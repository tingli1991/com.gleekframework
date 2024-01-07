using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Com.GleekFramework.CommonSdk.Extensions
{
    /// <summary>
    /// 查询器拓展
    /// </summary>
    public static partial class QueryableExtensions
    {
        /// <summary>
        /// 条件过滤
        /// </summary>
        /// <param name="query">可查询以应用筛选</param>
        /// <param name="condition">判断条件</param>
        /// <param name="predicate">用于筛选查询的表达式</param>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        /// <summary>
        /// 条件过滤
        /// </summary>
        /// <param name="query">可查询以应用筛选</param>
        /// <param name="condition">判断条件</param>
        /// <param name="predicate">用于筛选查询的表达式</param>
        public static TQueryable WhereIf<T, TQueryable>(this TQueryable query, bool condition, Expression<Func<T, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            return condition ? (TQueryable)query.Where(predicate) : query;
        }

        /// <summary>
        /// 条件过滤
        /// </summary>
        /// <param name="query">可查询以应用筛选</param>
        /// <param name="condition">判断条件</param>
        /// <param name="predicate">用于筛选查询的表达式</param>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        /// <summary>
        /// 条件过滤
        /// </summary>
        /// <param name="query">可查询以应用筛选</param>
        /// <param name="condition">判断条件</param>
        /// <param name="predicate">用于筛选查询的表达式</param>
        public static TQueryable WhereIf<T, TQueryable>(this TQueryable query, bool condition, Expression<Func<T, int, bool>> predicate)
            where TQueryable : IQueryable<T>
        {
            return condition ? (TQueryable)query.Where(predicate) : query;
        }

        /// <summary>
        /// 条件排序
        /// </summary>
        /// <param name="query">可查询以应用筛选</param>
        /// <param name="condition">判断条件</param>
        /// <param name="sorting">排序值</param>
        public static TQueryable OrderByIf<T, TQueryable>(this TQueryable query, bool condition, string sorting)
            where TQueryable : IQueryable<T>
        {
            return condition ? (TQueryable)DynamicQueryableExtensions.OrderBy(query, sorting) : query;
        }
    }
}