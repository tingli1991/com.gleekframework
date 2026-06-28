using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.NLogSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;

namespace Com.GleekFramework.QueueSdk
{
    //BlockingCollection<T> 为实现 IProducerConsumerCollection<T> 的线程安全集合提供阻塞和限制功能
    //ConcurrentBag<T> 表示对象的线程安全的无序集合
    //ConcurrentDictionary<TKey, TValue> 表示可由多个线程同时访问的键值对的线程安全集合

    //ConcurrentQueue<T> 表示线程安全的先进先出 (FIFO) 集合
    //ConcurrentStack<T> 表示线程安全的后进先出 (LIFO) 集合

    //Partitioner 提供针对数组、列表和可枚举项的常见分区策略
    //Partitioner<TSource> 表示将一个数据源拆分成多个分区的特定方式
    //OrderablePartitioner<TSource>  表示将一个可排序数据源拆分成多个分区的特定方式

    /// <summary>
    /// 队列主机拓展
    /// </summary>
    public static partial class HostingQueueExtensions
    {
        /// <summary>
        /// 随机因子
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// 订阅Queue
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IHost SubscribeQueue(this IHost host)
        {
            return host.SubscribeQueue((config) => Environment.ProcessorCount * 2);
        }

        /// <summary>
        /// 订阅Queue
        /// </summary>
        /// <param name="host"></param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IHost SubscribeQueue(this IHost host, Func<IConfiguration, int> callback)
        {
            host.RegisterApplicationStarted(() =>
            {
                //设置分区队列的分区数量(必须在发起订阅之前，不能调整顺序)
                PartitionedQueueProvider.PartitionCount = callback(AppConfig.Configuration);
                SubscribeQueue();//发起订阅
                NLogProvider.Info($"【Queue订阅】分区数量：{PartitionedQueueProvider.PartitionCount}");
            }).RegisterApplicationStopped(async () =>
            {
                while (true)
                {
                    var surplusMessageCount = await PartitionedQueueProvider.GetSurplusMessageCountAsync();
                    if (surplusMessageCount > 0)
                    {
                        var delayMilliseconds = Random.Next(0, 5000);
                        NLogProvider.Warn($"【Queue订阅】停机延迟{delayMilliseconds}毫秒，剩余消息数量：{surplusMessageCount}");
                        Thread.Sleep(delayMilliseconds);//阻塞主线程
                    }
                    else
                    {
                        NLogProvider.Info($"【Queue订阅】分区数量：{PartitionedQueueProvider.PartitionCount}，停机完毕！！！");
                        break;
                    }
                }
            });
            return host;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="cancellationToken"></param>
        private static void SubscribeQueue(CancellationToken cancellationToken = default)
        {
            var topicGroupServiceList = HandlerFactory.GetTopicServiceList<QueueHandler>();
            if (topicGroupServiceList.IsNullOrEmpty())
            {
                return;
            }

            foreach (var topicGroupService in topicGroupServiceList)
            {
                PartitionedQueueProvider.Subscribe(topicGroupService.Topic, async (topic, partitionIndex, messageBody) =>
                {
                    if (messageBody == null)
                    {
                        return;
                    }

                    var serviceList = topicGroupService.ServiceList.Where(e => e.ActionKey.EqualsActionKey(messageBody.ActionKey));
                    if (serviceList.IsNullOrEmpty())
                    {
                        return;
                    }

                    await serviceList.ForEachAsync(e => SemaphoreProvider.ExecuteAsync((surplusCount) => CoustomExecuteExtensions.ExecuteAsync(e, messageBody)));
                }, cancellationToken);
            }
        }
    }
}