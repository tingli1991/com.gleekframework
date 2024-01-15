using Com.GleekFramework.CommonSdk;
using System;
using System.Threading;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 雪花算法实现类
    /// </summary>
    public static class SnowflakeProvider
    {
        /// <summary>
        /// 自增开始位置
        /// </summary>
        private static long Sequence = 100000000L;

        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 随机因子
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.ToCstTime().Ticks);

        /// <summary>
        /// 机器码
        /// </summary>
        private static readonly string PersonCode = $"{Random.NextLong(1, 999)}".PadLeft(3, '0');

        /// <summary>
        /// 重新计数的时间点
        /// </summary>
        private static long RefSequence = long.Parse(DateTime.Now.Date.ToCstToday().AddDays(1).ToString("yyyyMMddHHmmssffff"));

        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="suffix">流水号前缀</param>
        /// <returns></returns>
        public static string GetSerialNo(int suffix = 1)
        {
            var serialNo = NextSnowflakeNo(suffix);
            if (string.IsNullOrWhiteSpace(serialNo))
            {
                throw new ArgumentNullException($"GetSerialNo({suffix}): 获取序列号失败!!!");
            }
            return serialNo;
        }

        /// <summary>
        /// 生成雪花编号
        /// </summary>
        /// <param name="suffix">流水号前缀</param>
        /// <returns></returns>
        private static string NextSnowflakeNo(int suffix)
        {
            lock (@lock)
            {
                var currentTimeSpan = GetCurrentTimeSpan();
                if (currentTimeSpan >= RefSequence)
                {
                    Sequence = 100000000L;
                    RefSequence = long.Parse(DateTime.Now.Date.ToCstToday().AddDays(1).ToString("yyyyMMddHHmmssffff"));
                }
                Interlocked.Increment(ref Sequence);//原子递增
                return $"{currentTimeSpan}{PersonCode}{Sequence}{suffix.ToString().PadLeft(2, '0').Substring(0, 2)}";
            }
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns></returns>
        private static long GetCurrentTimeSpan()
        {
            return long.Parse(DateTime.Now.ToCstTime().ToString("yyyyMMddHHmmssffff"));
        }
    }
}