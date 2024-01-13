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
    /// 分区栈实现类
    /// </summary>
    public static class PartitionedStackProvider
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
        /// 主题分区消息栈
        /// </summary>
        private static readonly Dictionary<string, PartitionedStack<MessageBody>> PartitionedCacheList = new Dictionary<string, PartitionedStack<MessageBody>>();

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
            var partitionedQueueSigle = GetPartitionedStackSigle(topic);//获取分区栈单例对象
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
            var partitionedQueueSigle = GetPartitionedStackSigle(topic);//获取分区栈单例对象
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
            var partitionedQueueSigle = GetPartitionedStackSigle(topic);//获取分区栈单例对象
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
            var partitionedQueueSigle = GetPartitionedStackSigle(topic);//获取分区栈单例对象
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
            var partitionedStackSigle = GetPartitionedStackSigle(topic);
            partitionedStackSigle.Subscribe((partitionIndex, messageBody) => callback(topic, partitionIndex, messageBody), cancellationToken);
        }

        /// <summary>
        /// 获取分区栈单列
        /// </summary>
        /// <param name="topic">主题名称</param>
        /// <returns></returns>
        private static PartitionedStack<MessageBody> GetPartitionedStackSigle(string topic)
        {
            if (!PartitionedCacheList.ContainsKey(topic))
            {
                lock (@lock)
                {
                    if (!PartitionedCacheList.ContainsKey(topic))
                    {
                        PartitionedCacheList.Add(topic, new PartitionedStack<MessageBody>(PartitionCount));
                    }
                }
            }
            return PartitionedCacheList[topic];
        }
    }
}