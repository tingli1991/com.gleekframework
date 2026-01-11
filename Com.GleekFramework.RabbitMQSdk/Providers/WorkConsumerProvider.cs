using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConsumerSdk;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 工作模式消费者
    /// </summary>
    public static partial class WorkConsumerProvider
    {
        /// <summary>
        /// 订阅消费者
        /// </summary>
        /// <param name="options"></param>
        public static void Subscribe(RabbitConsumerOptions options)
        {
            if (options.HostOptions == null)
            {
                throw new ArgumentNullException(nameof(options.HostOptions));
            }

            if (options.HostOptions.Any(e => e.Host == null))
            {
                throw new ArgumentNullException("Host");
            }


            var handlerServiceList = HandlerFactory.GetHandlerServiceList<RabbitWorkHandler>();
            if (handlerServiceList.IsNullOrEmpty())
            {
                return;
            }

            options.HostOptions.ForEach(host =>
            {
                var handlerGroupServiceList = handlerServiceList
                .GroupBy(e => new { e.AutoAck, e.QueueName })
                .Select(e => new { e.Key.AutoAck, e.Key.QueueName });

                var hostOptions = host.Map<RabbitHostOptions, RabbitConnectionOptions>();
                hostOptions.VirtualHost = options.VirtualHost;

                handlerGroupServiceList.ForEach(service
                 => Task.Run(() => PullMessageBodyAsync(options.AwaitTask, service, hostOptions)));
            });
        }

        /// <summary>
        /// 拉取并处理消息
        /// </summary>
        /// <param name="awaitTask">是否同步等待任务结果</param>
        /// <param name="options">处理服务</param>
        /// <param name="hostOptions">配置选项</param>
        /// <returns></returns>
        private static async Task PullMessageBodyAsync(bool awaitTask, dynamic options, RabbitConnectionOptions hostOptions)
        {
            bool autoAck = options.AutoAck;//自动应答
            string queueName = options.QueueName;//队列名称

            var connectionStrings = hostOptions.ToConnectionStrings();//转换成连接字符串
            var channel = await ConnectionProvider.GetChannelAsync(connectionStrings);//获取通道

            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ShutdownAsync += async (sender, eventArgs) => await eventArgs.ConsumerShutdownEventAsync(sender);
            consumer.ReceivedAsync += async (sender, eventArgs) => await eventArgs.ReceivedMessageAsync<RabbitWorkHandler>(channel, awaitTask, autoAck);
            await channel.BasicConsumeAsync(queue: queueName, autoAck: autoAck, consumer: consumer);
            await new TaskCompletionSource<string>().Task;//阻塞当前线程
        }
    }
}