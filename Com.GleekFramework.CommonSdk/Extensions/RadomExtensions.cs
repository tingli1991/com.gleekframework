using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 随机数基础拓展
    /// </summary>
    public static partial class RadomExtensions
    {
        /// <summary>
        /// 随机因子
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// LONG类型随机
        /// </summary>
        /// <param name="random">扩展的对象</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns></returns>
        public static long NextLong(this Random random, long minValue, long maxValue)
        {
            if (minValue > maxValue)
            {
                var tmpValue = minValue;
                maxValue = minValue;
                minValue = tmpValue;
            }
            var key = Random.NextDouble();
            return minValue + (long)((maxValue - minValue) * key);
        }

        /// <summary>
        /// 从指定的列表随机获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList">列表对象</param>
        /// <returns></returns>
        public static T Next<T>(this IEnumerable<T> sourceList)
        {
            return Random.Next(sourceList);
        }

        /// <summary>
        /// 从指定的列表随机获取一个值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_random">扩展源</param>
        /// <param name="sourceList">列表对象</param>
        /// <returns></returns>
        public static T Next<T>(this Random _random, IEnumerable<T> sourceList)
        {
            if (sourceList.IsNullOrEmpty())
                return default;

            var index = _random.Next(0, sourceList.Count());
            var dataList = sourceList.ToList();
            return dataList[index];
        }
    }
}