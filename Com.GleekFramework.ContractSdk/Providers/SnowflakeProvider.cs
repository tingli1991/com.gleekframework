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
        /// 自增序号(默认从最小序号开始)
        /// </summary>
        private static long Sequence = 0L;

        /// <summary>
        /// 最大序号(上限位置)
        /// </summary>
        private const long MaxSequence = 999999999L;

        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new();

        /// <summary>
        /// 随机因子
        /// </summary>
        private static readonly Random Random = new((int)DateTime.Now.Ticks);

        /// <summary>
        /// 重新计数的时间点
        /// </summary>
        private static DateTime RefDateTime = DateTime.Now.Date.ToCstToday().AddDays(1);

        /// <summary>
        /// 机器码
        /// </summary>
        private static readonly string PersonCode = $"{Random.NextLong(1, 999)}".PadLeft(3, '0');

        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <returns></returns>
        public static string GetSerialNo()
        {
            var serialNo = NextSnowflakeNo();
            if (string.IsNullOrWhiteSpace(serialNo))
            {
                throw new ArgumentNullException($"获取序列号失败!!!");
            }
            return serialNo;
        }

        /// <summary>
        /// 生成雪花编号
        /// </summary>
        /// <returns></returns>
        private static string NextSnowflakeNo()
        {
            if (Sequence >= MaxSequence)
            {
                lock (@lock)
                {
                    if (Sequence >= MaxSequence)
                    {
                        //从开始位置进行计算
                        Sequence = 0L;
                    }
                }
            }

            var today = DateTime.Now.Date.ToCstToday();
            if (today >= RefDateTime)
            {
                lock (@lock)
                {
                    if (today >= RefDateTime)
                    {
                        Sequence = 0L;
                        RefDateTime = DateTime.Now.Date.ToCstToday().AddDays(1);
                    }
                }
            }
            Interlocked.Increment(ref Sequence);//原子递增
            return $"{DateTime.Now.ToCstTime():yyMMddHHmmssffff}{PersonCode}{Sequence.ToString().PadLeft(9, '0')}";
        }
    }
}