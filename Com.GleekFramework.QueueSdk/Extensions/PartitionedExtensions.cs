using System;
using System.Threading;

namespace Com.GleekFramework.QueueSdk
{
    /// <summary>
    /// 分区拓展类
    /// </summary>
    public static class PartitionedExtensions
    {
        /// <summary>
        /// 分区递增Id位置
        /// </summary>
        private static long PartitionIncrementKey = 0;

        /// <summary>
        /// 获取分区键
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long GetPartitionIncrementKey<T>(this T source)
        {
            Interlocked.Increment(ref PartitionIncrementKey);//递增分区键数量
            return PartitionIncrementKey;
        }

        /// <summary>
        /// 获取分区索引位置
        /// </summary>
        /// <param name="source"></param>
        /// <param name="partitionCount"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int GetPartitionIndex<T>(this T source, int partitionCount, object partitionKey = null)
        {
            if (partitionCount <= 0)
            {
                throw new ArgumentNullException(nameof(partitionCount));
            }

            if (source == null && partitionKey == null)
            {
                throw new ArgumentNullException("source and partitionKey");
            }

            int hashCode;
            if (partitionKey != null)
            {
                //只用分区键的Hash值
                hashCode = partitionKey.GetHashCode();
            }
            else
            {
                //使用当前消息对象的Hhash值
                hashCode = source.GetHashCode();

            }
            return Math.Abs(hashCode % partitionCount);
        }
    }
}