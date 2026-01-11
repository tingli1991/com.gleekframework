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
    /// 分区栈
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PartitionedStack<T>
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
        /// 分区栈集合
        /// </summary>
        private readonly Dictionary<int, ConcurrentStack<T>> PartitionerStacks = new Dictionary<int, ConcurrentStack<T>>();

        /// <summary>
        /// 默认构造函数
        ///  分区数量按照计算机的核心数量*2
        /// </summary>
        public PartitionedStack() : this(Environment.ProcessorCount * 2)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="partitionCount">分区数量</param>
        public PartitionedStack(int partitionCount)
        {
            PartitionCount = partitionCount;//设置分区数量
            for (int index = 0; index < partitionCount; index++)
            {
                var concurrentStack = new ConcurrentStack<T>();
                PartitionerStacks.Add(index, concurrentStack);
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="messageBody">消息</param>
        /// <param name="partitionKey">分区键</param>
        public async Task PublishAsync(T messageBody, object partitionKey = null)
        {
            var partitionIndex = messageBody.GetPartitionIndex(PartitionCount, partitionKey);
            var concurrentQueue = PartitionerStacks[partitionIndex];
            concurrentQueue.Push(messageBody);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="messageBodys">消息</param>
        /// <param name="partitionKey">分区键</param>
        public async Task PublishAsync(IEnumerable<T> messageBodys, object partitionKey = null)
        {
            if (messageBodys.IsNullOrEmpty())
            {
                return;
            }
            var partitionIndex = messageBodys.GetPartitionIndex(PartitionCount, partitionKey);
            await messageBodys.ForEachAsync(async messageBody => await PublishAsync(messageBody, partitionKey));
        }

        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="partitionIndex">分区索引</param>
        /// <returns></returns>
        private T ConsumerAsync(int partitionIndex)
        {
            var concurrentQueue = PartitionerStacks[partitionIndex];
            concurrentQueue.TryPop(out T message);
            SurplusMessageCount = PartitionerStacks.Values.Sum(e => e.Count);
            return message;
        }

        /// <summary>
        /// 订阅消费
        /// </summary>
        /// <param name="callback">消息回调函数</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public void Subscribe(Action<int, T> callback, CancellationToken cancellationToken = default)
        {
            if (PartitionerStacks.IsNullOrEmpty())
            {
                callback(0, default);
            }

            foreach (var partitionerStack in PartitionerStacks)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var partitionIndex = partitionerStack.Key;
                        while (cancellationToken == default || !cancellationToken.IsCancellationRequested)
                        {
                            try
                            {
                                var messageBody = ConsumerAsync(partitionIndex);
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