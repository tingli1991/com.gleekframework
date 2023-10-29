using Aliyun.MQ;
using Aliyun.MQ.Model;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.NLogSdk;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// RocketMQ消费者主机拓展
    /// </summary>
    public static partial class ConsumerHostingExtensions
    {
        /// <summary>
        /// 消息数量
        /// </summary>
        public static long MessageCount = 0;

        /// <summary>
        /// 随机因子
        /// </summary>
        private static Random Random => new Random((int)DateTime.Now.ToCstTime().Ticks);

        /// <summary>
        /// 多线程控制器
        /// </summary>
        private static readonly CancellationTokenSource @Cts = new CancellationTokenSource();

        /// <summary>
        /// 订阅RocketMQ
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="options">配置选项</param>
        /// <returns></returns>
        public static IHost SubscribeRocketMQ(this IHost host, RocketConsumerOptions options)
        {
            host.RegisterApplicationStarted(() => Subscribe(options))
                .RegisterApplicationStopping(() => @Cts.Cancel())
                .RegisterApplicationStopped(() =>
                {
                    while (true)
                    {
                        if (MessageCount > 0)
                        {
                            var delayMilliseconds = Random.Next(0, 5000);
                            NLogProvider.Warn($"【RocketMQ订阅】停机延迟{delayMilliseconds}毫秒，消息数量：{MessageCount}");
                            Thread.Sleep(delayMilliseconds);//阻塞主线程   
                        }

                        NLogProvider.Warn($"【RocketMQ订阅】订阅配置：{options.JsonCompressAndEscape()}，取消订阅成功！");
                        break;
                    }
                });
            return host;
        }

        /// <summary>
        /// 订阅消费者
        /// </summary>
        /// <param name="options">配置选项</param>
        private static void Subscribe(RocketConsumerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.GroupId))
            {
                throw new ArgumentNullException(nameof(options.GroupId));
            }

            if (options.HostOptions == null || !options.HostOptions.Any())
            {
                throw new ArgumentNullException(nameof(options.HostOptions));
            }

            if (RocketAccessProvider.AccessOptions == null)
            {
                throw new ArgumentNullException(nameof(RocketAccessProvider.AccessOptions));
            }

            foreach (var hostOptions in options.HostOptions)
            {
                if (hostOptions.Topics == null || !hostOptions.Topics.Any())
                {
                    continue;
                }

                hostOptions.Topics.ForEach(topic => Task.Run(() => PullMessageBodyAsync(hostOptions.Host, options.GroupId,
                    topic, options.BatchSize, options.WaitSeconds, options.AwaitTask)));
            }
        }

        /// <summary>
        /// 拉去消息内容并消费
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="topic">分组Id</param>
        /// <param name="batchSize">批量消费的消息条数（上限：16条）</param>
        /// <param name="waitSeconds">长轮询时间时间秒（最多可设置为30秒）</param>
        /// <param name="awaitTask">是否需要等待Task任务完成</param>
        private static async Task PullMessageBodyAsync(string host, string groupId, string topic, uint batchSize, uint waitSeconds, bool awaitTask)
        {
            topic = EnvironmentProvider.GetVersion(topic);//转化环境参数
            var consumer = ConsumerProvider.GetConsumerSingle(host, topic, groupId);//获取消费者实例
            while (@Cts.Token == default || !@Cts.Token.IsCancellationRequested)
            {
                var messageInfoList = ConsumeMessageList(consumer, awaitTask, batchSize, waitSeconds);
                if (messageInfoList == null || !messageInfoList.Any())
                {
                    //延迟1秒继续(有可能是报错，也有可能是没有消息)
                    await Task.Delay(1000);
                    continue;
                }
                await CallbackMessageAsync(awaitTask, consumer, messageInfoList);
            }
        }

        /// <summary>
        /// 消费消息数据
        /// </summary>
        /// <param name="consumer">消费者对象</param>
        /// <param name="awaitTask">是否需要等待Task任务完成</param>
        /// <param name="batchSize">批量消费的消息条数（上限：16条）</param>
        /// <param name="waitSeconds">长轮询时间时间秒（最多可设置为30秒）</param>
        /// <returns></returns>
        private static List<Message> ConsumeMessageList(MQConsumer consumer, bool awaitTask, uint batchSize, uint waitSeconds)
        {
            List<Message> messageInfoList = null;
            try
            {
                if (awaitTask)
                {
                    //按顺序消费
                    messageInfoList = consumer.ConsumeMessageOrderly(batchSize, waitSeconds);
                }
                else
                {
                    //无序消费
                    messageInfoList = consumer.ConsumeMessage(batchSize, waitSeconds);
                }
            }
            catch (Exception ex)
            {
                NLogProvider.Error($"【RocketMQ消费者】拉取消息错误： {ex}");
            }
            return messageInfoList ?? new List<Message>();
        }

        /// <summary>
        /// 消息处理方法
        /// </summary>
        /// <param name="awaitTask">是否需要等待Task任务完成</param>
        /// <param name="consumer">消费者对象</param>
        /// <param name="messageInfoList">消息列表</param>
        /// <returns></returns>
        private static async Task CallbackMessageAsync(bool awaitTask, MQConsumer consumer, List<Message> messageInfoList)
        {
            try
            {
                foreach (var messageInfo in messageInfoList)
                {
                    Interlocked.Increment(ref MessageCount);//递增消息数量
                    var task = SemaphoreProvider.ExecuteAsync((surplusCount) => messageInfo.MessageCallbackAsync()).ContinueWith(e =>
                    {
                        Interlocked.Decrement(ref MessageCount);//递减数量
                        consumer.AckMessage(new List<string> { messageInfo.ReceiptHandle });//应答
                    });

                    if (awaitTask)
                    {
                        //等待消息处理结果
                        await task;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogProvider.Error($"【RocketMQ消费者】回调消息错误： {ex}");
            }
        }
    }
}