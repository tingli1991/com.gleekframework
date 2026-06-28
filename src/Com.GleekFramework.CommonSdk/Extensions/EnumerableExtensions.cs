using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 集合基础拓展
    /// </summary>
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// 确定序列中的任何元素是否满足条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool AnyOf<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        /// <summary>
        /// 确定序列中的任何元素是否满足条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool AnyOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source != null && source.Any(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsOf<T>(this IEnumerable<T> source, T value)
        {
            return source.IsNotNull() && source.Contains(value);
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> Add<T>(this IEnumerable<T> source, T value)
        {
            source ??= new List<T>();
            foreach (var cur in source)
            {
                yield return cur;
            }

            if (value == null)
            {
                yield break;
            }
            yield return value;
        }

        /// <summary>
        /// 添加项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<T> AddRange<T>(this IEnumerable<T> source, IEnumerable<T> values)
        {
            source ??= new List<T>();
            values ??= new List<T>();
            foreach (var cur in source)
            {
                yield return cur;
            }

            foreach (var value in values)
            {
                if (value != null)
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// ForEach扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source.IsNullOrEmpty())
            {
                return;
            }

            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// ForEach扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageSize"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, int pageSize, Action<int, IEnumerable<T>> action)
        {
            if (source.IsNullOrEmpty())
            {
                return;
            }

            var pageDataDic = source.ToPageDictionary(pageSize);
            foreach (var pageDataInfo in pageDataDic)
            {
                var pageIndex = pageDataInfo.Key;//分页页码
                var pageDataList = pageDataInfo.Value;//分页后的数据列表
                action(pageIndex, pageDataList);//执行回调函数
            }
        }

        /// <summary>
        /// ForEach扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> func)
        {
            if (source.IsNullOrEmpty())
            {
                return;
            }

            foreach (T item in source)
            {
                await func(item);
            }
        }

        /// <summary>
        /// ForEach分页扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="func"></param>
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, int pageSize, Func<int, IEnumerable<T>, Task> func)
        {
            if (source.IsNullOrEmpty())
            {
                return;
            }

            var pageDataDic = source.ToPageDictionary(pageSize);
            foreach (var pageDataInfo in pageDataDic)
            {
                var pageIndex = pageDataInfo.Key;//分页页码
                var pageDataList = pageDataInfo.Value;//分页后的数据列表
                await func(pageIndex, pageDataList);//执行回调函数
            }
        }

        /// <summary>
        /// 转换分页的大小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList"></param>
        /// <param name="pageSize">分页大小(默认：2000条)</param>
        /// <returns></returns>
        public static int ToPageCount<T>(this IEnumerable<T> sourceList, int pageSize = 2000)
        {
            if (sourceList.IsNullOrEmpty() || pageSize <= 0)
                return 0;

            return (int)Math.Ceiling((decimal)sourceList.Count() / pageSize);
        }

        /// <summary>
        /// 转换分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList">数据源</param>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static IEnumerable<T> ToPageList<T>(this IEnumerable<T> sourceList, int pageIndex = 0, int pageSize = 2000)
        {
            if (sourceList.IsNullOrEmpty())
                return new List<T>();

            if (pageIndex <= 0)
                pageIndex = 1;

            return sourceList.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// 转换分页字典列表(Key存页码,Value存分页数据)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList">数据源</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static Dictionary<int, IEnumerable<T>> ToPageDictionary<T>(this IEnumerable<T> sourceList, int pageSize = 2000)
        {
            var dic = new Dictionary<int, IEnumerable<T>>();
            if (sourceList.IsNullOrEmpty())
                return dic;

            for (int pageIndex = 1; pageIndex <= sourceList.ToPageCount(pageSize); pageIndex++)
            {
                dic.Add(pageIndex, sourceList.ToPageList(pageIndex, pageSize));
            }
            ;
            return dic;
        }

        /// <summary>
        /// 转换分页字典列表(Key存页码,Value存分页数据)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList">数据源</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static Dictionary<int, IEnumerable<byte[]>> ToPageBinaryDictionary<T>(this IEnumerable<T> sourceList, int pageSize = 2000)
        {
            var dic = new Dictionary<int, IEnumerable<byte[]>>();
            if (sourceList.IsNullOrEmpty())
                return dic;

            for (int pageIndex = 1; pageIndex <= sourceList.ToPageCount(pageSize); pageIndex++)
            {
                var pageList = sourceList.ToPageList(pageIndex, pageSize);
                dic.Add(pageIndex, pageList.Select(value => value.SerializeBinary()));
            }
            ;
            return dic;
        }
    }
}