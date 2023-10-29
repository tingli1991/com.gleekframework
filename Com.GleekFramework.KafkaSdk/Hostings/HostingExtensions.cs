using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.NLogSdk;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// 消费者拓展类
    /// </summary>
    public static partial class HostingExtensions
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
        /// 订阅Kafka
        /// </summary>
        /// <param name="host"></param>
        /// <param name="callback">配置选项</param>
        /// <returns></returns>
        public static IHost SubscribeKafka(this IHost host, Func<IConfiguration, KafkaConsumerOptions> callback)
        {
            var options = callback(AppConfig.Configuration);
            host.RegisterApplicationStarted(() => Subscribe(options))
                .RegisterApplicationStopping(() => Cts.Cancel())
                .RegisterApplicationStopped(() =>
                {
                    while (true)
                    {
                        if (MessageCount <= 0)
                        {
                            ConsumerProvider.UnSubscribe();//取消订阅
                            NLogProvider.Warn($"【Kafka订阅】订阅配置：{options.JsonCompressAndEscape()}，取消订阅成功！");
                            break;
                        }
                        else
                        {
                            var delayMilliseconds = Random.Next(0, 5000);
                            NLogProvider.Warn($"【Kafka订阅】停机延迟{delayMilliseconds}毫秒，剩余消息数量：{MessageCount}");
                            Thread.Sleep(delayMilliseconds);//阻塞主线程
                        }
                    }
                });
            return host;
        }

        /// <summary>
        /// 订阅消费者
        /// </summary>
        /// <param name="options"></param>
        private static void Subscribe(KafkaConsumerOptions options)
        {
            try
            {
                if (options == null)
                {
                    throw new ArgumentNullException(nameof(options));
                }

                if (options.HostOptions == null || !options.HostOptions.Any())
                {
                    throw new ArgumentNullException(nameof(options.HostOptions));
                }

                if (string.IsNullOrEmpty(options.GroupId))
                {
                    throw new ArgumentNullException(nameof(options.GroupId));
                }

                foreach (var hostOptions in options.HostOptions)
                {
                    if (hostOptions.Topics == null || !hostOptions.Topics.Any())
                    {
                        continue;
                    }
                    hostOptions.Topics.ForEach(topic => Task.Run(() => PullMessageBodyAsync(hostOptions.Host, options.GroupId, topic, options.AwaitTask, options.AutoAck, options.AutoOffset)));
                }
            }
            catch (Exception ex)
            {
                NLogProvider.Error($"【Kafka消费者】订阅配置：{options.JsonCompressAndEscape()}，发生异常：{ex}");
            }
        }

        /// <summary>
        /// 拉取消息
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="topic">主题</param>
        /// <param name="awaitTask">是否需要等待Task任务完成</param>
        /// <param name="autoAck">是否启用自动提交偏移量</param>
        /// <param name="autoOffset">偏移方式</param>
        private static async Task PullMessageBodyAsync(string host, string groupId, string topic, bool awaitTask, bool autoAck, AutoOffsetReset autoOffset)
        {
            var consumer = ConsumerProvider.GetConsumerSingle(host, groupId, EnvironmentProvider.GetVersion(topic), autoAck, autoOffset);
            consumer.Subscribe(topic);//订阅消息
            while (@Cts.Token == default || !@Cts.Token.IsCancellationRequested)
            {
                var messageBody = ConsumeMessage(consumer);
                if (messageBody == null || messageBody.IsPartitionEOF)
                {
                    //消费到最后一个位置
                    continue;
                }
                await CallbackMessageAsync(consumer, messageBody, awaitTask, autoAck);
            }
        }

        /// <summary>
        /// 消费消息
        /// </summary>
        /// <param name="consumer">消费者对象</param>
        /// <returns></returns>
        private static ConsumeResult<string, string> ConsumeMessage(IConsumer<string, string> consumer)
        {
            ConsumeResult<string, string> messageBody = null;
            try
            {
                messageBody = consumer.Consume();
            }
            catch (Exception ex)
            {
                NLogProvider.Error($"【Kafka消费者】拉取消息错误： {ex}");
            }
            return messageBody;
        }

        /// <summary>
        /// 回调消息处理方法
        /// </summary>
        /// <param name="consumer">消费者对象</param>
        /// <param name="messageBody">消息内容</param>
        /// <param name="awaitTask">是否需要等待Task任务完成</param>
        /// <param name="autoAck">是否开启自动应答模式</param>
        /// <returns></returns>
        private static async Task CallbackMessageAsync(IConsumer<string, string> consumer, ConsumeResult<string, string> messageBody, bool awaitTask, bool autoAck)
        {
            try
            {
                Interlocked.Increment(ref MessageCount);//递增消息数量
                var task = SemaphoreProvider.ExecuteAsync((surplusCount) => messageBody.MessageCallbackAsync()).ContinueWith(e =>
                {
                    Interlocked.Decrement(ref MessageCount);//递减消息数量
                    if (!autoAck && messageBody.Offset % 10 == 0)
                    {
                        //手动提交偏移(每消费10条消息应答一次)
                        consumer.StoreOffset(messageBody);
                    }

                    if (messageBody.Message != null)
                    {
                        //防止大对象(手动清理数据)
                        messageBody.Message.Value = null;
                        messageBody.Message = null;
                        messageBody = null;
                    }
                });

                if (awaitTask)
                {
                    //等待消息处理结果
                    await task;
                }
            }
            catch (Exception ex)
            {
                NLogProvider.Error($"【Kafka消费者】回调消息错误： {ex}");
            }
        }
    }
}