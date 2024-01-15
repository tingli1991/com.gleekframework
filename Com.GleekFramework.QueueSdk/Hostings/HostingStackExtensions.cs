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
    /// <summary>
    /// 队列主机拓展
    /// </summary>
    public static partial class HostingStackExtensions
    {
        /// <summary>
        /// 随机因子
        /// </summary>
        private static Random Random => new Random((int)DateTime.Now.ToCstTime().Ticks);

        /// <summary>
        /// 订阅Stack
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static IHost SubscribeStack(this IHost host)
        {
            return host.SubscribeStack((config) => Environment.ProcessorCount * 2);
        }
        /// <summary>
        /// 订阅Stack
        /// </summary>
        /// <param name="host"></param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IHost SubscribeStack(this IHost host, Func<IConfiguration, int> callback)
        {
            host.RegisterApplicationStarted(() =>
            {
                //设置分区队列的分区数量(必须在发起订阅之前，不能调整顺序)
                PartitionedStackProvider.PartitionCount = callback(AppConfig.Configuration);

                SubscribeStack();//发起订阅
                NLogProvider.Info($"【Stack订阅】分区数量：{PartitionedQueueProvider.PartitionCount}");
            }).RegisterApplicationStopped(async () =>
            {
                while (true)
                {
                    var surplusMessageCount = await PartitionedQueueProvider.GetSurplusMessageCountAsync();
                    if (surplusMessageCount > 0)
                    {
                        var delayMilliseconds = Random.Next(0, 5000);
                        NLogProvider.Warn($"【Stack订阅】停机延迟{delayMilliseconds}毫秒，剩余消息数量：{surplusMessageCount}");
                        Thread.Sleep(delayMilliseconds);//阻塞主线程
                    }
                    else
                    {
                        NLogProvider.Info($"【Stack订阅】分区数量：{PartitionedQueueProvider.PartitionCount}，停机完毕！！！");
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
        private static void SubscribeStack(CancellationToken cancellationToken = default)
        {
            var topicGroupServiceList = HandlerFactory.GetTopicServiceList<StackHandler>();
            if (topicGroupServiceList.IsNullOrEmpty())
            {
                return;
            }

            foreach (var topicGroupService in topicGroupServiceList)
            {
                PartitionedStackProvider.Subscribe(topicGroupService.Topic, async (topic, partitionIndex, messageBody) =>
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