using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.QueueSdk
{
    /// <summary>
    /// 分区队列实现类
    /// </summary>
    public static class PartitionedQueueProvider
    {
        /// <summary>
        /// 分布式锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 当前系统设置的分区数量
        /// </summary>
        public static int PartitionCount { get; set; } = Environment.ProcessorCount * 2;

        /// <summary>
        /// 主题分区消息队列
        /// </summary>
        private static readonly Dictionary<string, PartitionedQueue<MessageBody>> PartitionedCacheList = new Dictionary<string, PartitionedQueue<MessageBody>>();

        /// <summary>
        /// 获取所有的剩余消息数量
        /// </summary>
        /// <returns></returns>
        public static Task<long> GetSurplusMessageCountAsync()
        {
            if (PartitionedCacheList.IsNullOrEmpty())
            {
                return Task.FromResult(0L);
            }
            else
            {
                return Task.FromResult(PartitionedCacheList.Sum(e => e.Value.SurplusMessageCount));
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public static Task PublishAsync(string topic, MessageBody messageBody)
        {
            var partitionedQueueSigle = GetPartitionedQueueSigle(topic);//获取分区队列单例对象
            var partitionIncrementKey = partitionedQueueSigle.GetPartitionIncrementKey();//自增的分区键(本机有效)
            return partitionedQueueSigle.PublishAsync(messageBody, partitionIncrementKey);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="messageBodys">消息内容</param>
        /// <returns></returns>
        public static Task PublishAsync(string topic, IEnumerable<MessageBody> messageBodys)
        {
            var partitionedQueueSigle = GetPartitionedQueueSigle(topic);//获取分区队列单例对象
            var partitionIncrementKey = partitionedQueueSigle.GetPartitionIncrementKey();//自增的分区键(本机有效)
            return partitionedQueueSigle.PublishAsync(messageBodys, partitionIncrementKey);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public static Task PublishAsync<T>(string topic, MessageBody<T> messageBody) where T : class
        {
            var partitionedQueueSigle = GetPartitionedQueueSigle(topic);//获取分区队列单例对象
            var partitionIncrementKey = partitionedQueueSigle.GetPartitionIncrementKey();//自增的分区键(本机有效)
            return partitionedQueueSigle.PublishAsync(messageBody, partitionIncrementKey);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="topic">主题</param>
        /// <param name="messageBodys">消息内容</param>
        /// <returns></returns>
        public static Task PublishAsync<T>(string topic, IEnumerable<MessageBody<T>> messageBodys) where T : class
        {
            var partitionedQueueSigle = GetPartitionedQueueSigle(topic);//获取分区队列单例对象
            var partitionIncrementKey = partitionedQueueSigle.GetPartitionIncrementKey();//自增的分区键(本机有效)
            return partitionedQueueSigle.PublishAsync(messageBodys, partitionIncrementKey);
        }

        /// <summary>
        /// 订阅消费
        /// </summary>
        /// <param name="topic">主题名称</param>
        /// <param name="callback">消息回调函数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static void Subscribe(string topic, Action<string, int, MessageBody> callback, CancellationToken cancellationToken = default)
        {
            var partitionedQueueSigle = GetPartitionedQueueSigle(topic);
            partitionedQueueSigle.Subscribe((partitionIndex, messageBody) => callback(topic, partitionIndex, messageBody), cancellationToken);
        }

        /// <summary>
        /// 获取分区队列单列
        /// </summary>
        /// <param name="topic">主题名称</param>
        /// <returns></returns>
        private static PartitionedQueue<MessageBody> GetPartitionedQueueSigle(string topic)
        {
            if (!PartitionedCacheList.ContainsKey(topic))
            {
                lock (@lock)
                {
                    if (!PartitionedCacheList.ContainsKey(topic))
                    {
                        PartitionedCacheList.Add(topic, new PartitionedQueue<MessageBody>(PartitionCount));
                    }
                }
            }
            return PartitionedCacheList[topic];
        }
    }
}