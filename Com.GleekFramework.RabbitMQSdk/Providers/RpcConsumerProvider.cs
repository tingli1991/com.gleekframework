using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.ContractSdk;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// RPC模式消费者
    /// </summary>
    public static partial class RpcConsumerProvider
    {
        /// <summary>
        /// 等待回调的任务列表
        /// </summary>
        private static readonly ConcurrentDictionary<IConnection, TaskCompletionSource<string>> AwaitCallbackTask = new ConcurrentDictionary<IConnection, TaskCompletionSource<string>>();

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

            if (options.Hosts == null)
            {
                throw new ArgumentNullException(nameof(options.Hosts));
            }

            if (options.Hosts.Any(e => e.Host == null))
            {
                throw new ArgumentNullException("Host");
            }

            var handlerServiceList = HandlerFactory.GetHandlerServiceList<RabbitRpcHandler>();
            if (handlerServiceList.IsNullOrEmpty())
            {
                return;
            }

            options.Hosts.ForEach(host =>
            {
                var handlerGroupServiceList = handlerServiceList
                .GroupBy(e => new { e.AutoAck, e.QueueName })
                .Select(e => new { e.Key.AutoAck, e.Key.QueueName });

                var hostOptions = host.Map<RabbitHostOptions, RabbitConnectionOptions>();
                hostOptions.VirtualHost = options.VirtualHost;
                handlerGroupServiceList.ForEach(service => PullMessageBodyAsync(options.AwaitTask, service, hostOptions));
            });
        }

        /// <summary>
        /// 拉取并处理消息
        /// </summary>
        /// <param name="awaitTask">是否同步等待任务结果</param>
        /// <param name="options">处理服务</param>
        /// <param name="hostOptions">配置选项</param>
        /// <returns></returns>
        private static Task PullMessageBodyAsync(bool awaitTask, dynamic options, RabbitConnectionOptions hostOptions)
        {
            bool autoAck = options.AutoAck;//自动应答
            string queueName = options.QueueName;//队列名称

            var connectionStrings = hostOptions.ToConnectionStrings();//转换成连接字符串
            var channel = ConnectionProvider.GetChannel(connectionStrings);//获取通道

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Shutdown += (sender, eventArgs) => eventArgs.ConsumerShutdownEventAsync(sender);
            consumer.Received += (sender, eventArgs) => eventArgs.ReceivedMessageAsync<RabbitRpcHandler>(channel, awaitTask, autoAck, (response) => ClientCallbackAsync(eventArgs, channel, response));
            channel.BasicConsume(queue: queueName, autoAck: autoAck, consumer: consumer);
            return new TaskCompletionSource<string>().Task;//阻塞当前线程
        }

        /// <summary>
        /// 回调客户端
        /// </summary>
        /// <param name="eventArgs">消息内容</param>
        /// <param name="channel">通道</param>
        /// <param name="response">返回结果</param>
        /// <returns></returns>
        private static Task ClientCallbackAsync(BasicDeliverEventArgs eventArgs, IModel channel, Task<ContractResult> response)
        {
            var result = response;//返回结果
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = eventArgs.BasicProperties.CorrelationId;
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result));
            channel.BasicPublish(exchange: string.Empty, routingKey: eventArgs.BasicProperties.ReplyTo, basicProperties: replyProps, body: bytes);
            return Task.CompletedTask;
        }
    }
}