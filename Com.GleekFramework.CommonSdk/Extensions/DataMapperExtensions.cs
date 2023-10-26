using System.Collections.Generic;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 数据模型映射拓展类
    /// </summary>
    public static partial class DataMapperExtensions
    {
        /// <summary>
        /// 单模型映射方法
        /// </summary>
        /// <typeparam name="TSource">原数据类型</typeparam>
        /// <typeparam name="TTarget">目标数据类型</typeparam>
        /// <param name="source">原数据实例</param>
        /// <returns></returns>
        public static TTarget Map<TSource, TTarget>(this TSource source)
            where TSource : class
            where TTarget : class
        {
            return DataMapper<TSource, TTarget>.Map(source);
        }

        /// <summary>
        /// 单模型映射方法
        /// </summary>
        /// <typeparam name="TSource">原数据类型</typeparam>
        /// <typeparam name="TTarget">目标数据类型</typeparam>
        /// <param name="sources">原数据实例集合</param>
        /// <returns></returns>
        public static IEnumerable<TTarget> Map<TSource, TTarget>(this IEnumerable<TSource> sources)
            where TSource : class
            where TTarget : class
        {
            return DataMapper<TSource, TTarget>.MapList(sources);
        }
    }
}