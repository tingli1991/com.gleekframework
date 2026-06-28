using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// 查询拓展类
    /// </summary>
    public static class MongoFindExtensions
    {
        /// <summary>
        /// 排序(升序)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="find"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static IOrderedFindFluent<T, TP> OrderBy<T, TP>(this IFindFluent<T, TP> find, Expression<Func<T, object>> field)
            => find.SortBy(field);

        /// <summary>
        /// 排序(降序)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="find"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static IOrderedFindFluent<T, TP> OrderByDescending<T, TP>(this IFindFluent<T, TP> find, Expression<Func<T, object>> field)
            => find.SortByDescending(field);

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="find"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        public static IFindFluent<T, BsonDocument> Select<T, TP>(this IFindFluent<T, TP> find, ProjectionDefinition<T, BsonDocument> projection)
            => find.Project(projection);

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <typeparam name="TNP"></typeparam>
        /// <param name="find"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        public static IFindFluent<T, TNP> Select<T, TProjection, TNP>(this IFindFluent<T, TProjection> find, Expression<Func<T, TNP>> projection)
            => find.Project(projection);

    }
}