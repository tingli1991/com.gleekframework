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
        ///迭代遍历选项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> Add<T>(this IEnumerable<T> enumerable, T value)
        {
            foreach (var cur in enumerable)
            {
                yield return cur;
            }
            yield return value;
        }

        /// <summary>
        /// ForEach扩展方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || !source.Any())
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
        /// <param name="func"></param>
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> func)
        {
            if (source == null || !source.Any())
            {
                return;
            }

            foreach (T item in source)
            {
                await func(item);
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
            if (sourceList == null || !sourceList.Any() || pageSize <= 0)
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
            if (sourceList == null || !sourceList.Any())
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
            if (sourceList == null || !sourceList.Any())
                return dic;

            for (int pageIndex = 1; pageIndex <= sourceList.ToPageCount(pageSize); pageIndex++)
            {
                dic.Add(pageIndex, sourceList.ToPageList(pageIndex, pageSize));
            };
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
            if (sourceList == null || !sourceList.Any())
                return dic;

            for (int pageIndex = 1; pageIndex <= sourceList.ToPageCount(pageSize); pageIndex++)
            {
                var pageList = sourceList.ToPageList(pageIndex, pageSize);
                dic.Add(pageIndex, pageList.Select(value => value.SerializeBinary()));
            };
            return dic;
        }
    }
}