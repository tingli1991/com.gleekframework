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
    /// 发布订阅模式实现类
    /// </summary>
    public static partial class SubscribeConsumerProvider
    {
        /// <summary>
        /// 订阅消费者
        /// </summary>
        /// <param name="options"></param>
        public static void Subscribe(RabbitConsumerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.HostOptions == null)
            {
                throw new ArgumentNullException(nameof(options.HostOptions));
            }

            if (options.HostOptions.Any(e => e.Host == null))
            {
                throw new ArgumentNullException("Host");
            }

            var handlerServiceList = HandlerFactory.GetHandlerServiceList<RabbitSubscribeHandler>();
            if (handlerServiceList.IsNullOrEmpty())
            {
                return;
            }

            options.HostOptions.ForEach(host =>
            {
                var handlerGroupServiceList = handlerServiceList
                .GroupBy(e => new { e.AutoAck, e.ExchangeName })
                .Select(e => new { e.Key.AutoAck, e.Key.ExchangeName });

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
            string exchangeName = options.ExchangeName;//交换机名称

            var connectionStrings = hostOptions.ToConnectionStrings();//转换成连接字符串
            var channel = ConnectionProvider.GetChannel(connectionStrings);//获取通道

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;//队列名称
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Shutdown += async (sender, eventArgs) => await eventArgs.ConsumerShutdownEventAsync(sender);
            consumer.Received += async (sender, eventArgs) => await eventArgs.ReceivedMessageAsync<RabbitSubscribeHandler>(channel, awaitTask, autoAck);
            channel.BasicConsume(queue: queueName, autoAck: autoAck, consumer: consumer);
            await new TaskCompletionSource<string>().Task;//阻塞当前线程
        }
    }
}