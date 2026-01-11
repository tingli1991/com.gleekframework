using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.NLogSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 工作模式消费拓展主机
    /// </summary>
    public static partial class WorkHostingExtensions
    {
        /// <summary>
        /// 消息数量
        /// </summary>
        public static long MessageCount = 0;

        /// <summary>
        /// 随机因子
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// 订阅RabbitMQ
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="callback">主机配置列表</param>
        /// <returns></returns>
        public static IHost SubscribeRabbitMQ(this IHost host, Func<IConfiguration, RabbitConsumerOptions> callback)
        {
            var options = callback(AppConfig.Configuration);
            host.RegisterApplicationStarted(() =>
            {
                RpcConsumerProvider.Subscribe(options);
                WorkConsumerProvider.Subscribe(options);
                SubscribeConsumerProvider.Subscribe(options);
            }).RegisterApplicationStopped(() =>
            {
                while (true)
                {
                    if (MessageCount <= 0)
                    {
                        NLogProvider.Warn($"【Rabbit工作模式消费者】订阅配置：{options.JsonCompressAndEscape()}，取消订阅成功！");
                        break;
                    }
                    else
                    {
                        var delayMilliseconds = Random.Next(0, 5000);
                        NLogProvider.Warn($"【Rabbit工作模式消费者】停机延迟{delayMilliseconds}毫秒，剩余消息数量：{MessageCount}");
                        Thread.Sleep(delayMilliseconds);//阻塞主线程
                    }
                }
            });
            return host;
        }

        /// <summary>
        /// 接受消息回调处理方法
        /// </summary>
        /// <param name="eventArgs">回调事件参数</param>
        /// <param name="channel">通道回</param>
        /// <param name="awaitTask">是否同步等待任务结果</param>
        /// <param name="autoAck">是否自动应答</param>
        /// <param name="callback">业务回调处理</param>
        /// <returns></returns>
        public static Task ReceivedMessageAsync<T>(this BasicDeliverEventArgs eventArgs, IChannel channel, bool awaitTask, bool autoAck, Func<Task<ContractResult>, Task> callback = null) where T : RabbitHandler
        {
            try
            {
                Interlocked.Increment(ref MessageCount);//递增消息数量
                var task = SemaphoreProvider.ExecuteAsync((surplusCount) => eventArgs.MessageExecuteAsync<T>()).ContinueWith(async response =>
                {
                    Interlocked.Decrement(ref MessageCount);//递减消息数量
                    if (callback != null)
                    {
                        //调用回调函数
                        await callback(response);
                    }

                    if (!autoAck)
                    {
                        //手动应答
                        await channel.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false);
                    }
                });

                if (awaitTask)
                {
                    //等待任务结果
                    task.Wait();
                }
            }
            catch (Exception ex)
            {
                NLogProvider.Error($"【Rabbit工作模式消费者】回调消息错误： {ex}");
            }
            return Task.CompletedTask;
        }
    }
}