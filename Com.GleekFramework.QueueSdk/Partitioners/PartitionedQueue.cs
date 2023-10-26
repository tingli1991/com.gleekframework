using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.NLogSdk;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.QueueSdk
{
    /// <summary>
    /// 分区队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PartitionedQueue<T>
    {
        /// <summary>
        /// 分区数量
        /// </summary>
        public int PartitionCount = 0;

        /// <summary>
        /// 剩余的消息数量
        /// </summary>
        public long SurplusMessageCount = 0;

        /// <summary>
        /// 分区队列集合
        /// </summary>
        private readonly Dictionary<int, ConcurrentQueue<T>> PartitionerQueues = new Dictionary<int, ConcurrentQueue<T>>();

        /// <summary>
        /// 默认构造函数
        ///  分区数量按照计算机的核心数量*2
        /// </summary>
        public PartitionedQueue() : this(Environment.ProcessorCount * 2)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="partitionCount">分区数量</param>
        public PartitionedQueue(int partitionCount)
        {
            PartitionCount = partitionCount;//设置分区数量
            for (int index = 0; index < partitionCount; index++)
            {
                var concurrentQueue = new ConcurrentQueue<T>();
                PartitionerQueues.Add(index, concurrentQueue);
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="messageBody">消息</param>
        /// <param name="partitionKey">分区键</param>
        public Task PublishAsync(T messageBody, object partitionKey = null)
        {
            var partitionIndex = messageBody.GetPartitionIndex(PartitionCount, partitionKey);
            var concurrentQueue = PartitionerQueues[partitionIndex];
            concurrentQueue.Enqueue(messageBody);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="messageBodys">消息</param>
        /// <param name="partitionKey">分区键</param>
        public Task PublishAsync(IEnumerable<T> messageBodys, object partitionKey = null)
        {
            if (messageBodys == null || !messageBodys.Any())
            {
                return Task.CompletedTask;
            }
            var partitionIndex = messageBodys.GetPartitionIndex(PartitionCount, partitionKey);
            return messageBodys.ForEachAsync(messageBody => PublishAsync(messageBody, partitionKey));
        }

        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="partitionIndex">分区索引</param>
        /// <returns></returns>
        public Task<T> ConsumerAsync(int partitionIndex)
        {
            var concurrentQueue = PartitionerQueues[partitionIndex];
            concurrentQueue.TryDequeue(out T message);
            SurplusMessageCount = PartitionerQueues.Values.Sum(e => e.Count);
            return Task.FromResult(message);
        }

        /// <summary>
        /// 订阅消费
        /// </summary>
        /// <param name="callback">消息回调函数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public void Subscribe(Action<int, T> callback, CancellationToken cancellationToken = default)
        {
            if (PartitionerQueues == null || !PartitionerQueues.Any())
            {
                callback(0, default);
            }

            foreach (var partitionerQueue in PartitionerQueues)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var partitionIndex = partitionerQueue.Key;
                        while (cancellationToken == default || !cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                var messageBody = await ConsumerAsync(partitionIndex);
                                if (messageBody == null)
                                {
                                    await Task.Delay(1000);
                                    continue;
                                }

                                callback(partitionIndex, messageBody);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        NLogProvider.Error($"{ex}");
                    }
                });
            }
        }
    }
}