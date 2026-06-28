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

            if (options.HostOptions == null)
            {
                throw new ArgumentNullException(nameof(options.HostOptions));
            }

            if (options.HostOptions.Any(e => e.Host == null))
            {
                throw new ArgumentNullException("Host");
            }

            var handlerServiceList = HandlerFactory.GetHandlerServiceList<RabbitRpcHandler>();
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
                handlerGroupServiceList.ForEach(async service => await PullMessageBodyAsync(options.AwaitTask, service, hostOptions));
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
            consumer.ShutdownAsync += (sender, eventArgs) => eventArgs.ConsumerShutdownEventAsync(sender);
            consumer.ReceivedAsync += (sender, eventArgs) => eventArgs.ReceivedMessageAsync<RabbitRpcHandler>(channel, awaitTask, autoAck, (response) => ClientCallbackAsync(eventArgs, channel, response));
            await channel.BasicConsumeAsync(queue: queueName, autoAck: autoAck, consumer: consumer);
            await new TaskCompletionSource<string>().Task;//阻塞当前线程
        }

        /// <summary>
        /// 回调客户端
        /// </summary>
        /// <param name="eventArgs">消息内容</param>
        /// <param name="channel">通道</param>
        /// <param name="response">返回结果</param>
        /// <returns></returns>
        private static async Task ClientCallbackAsync(BasicDeliverEventArgs eventArgs, IChannel channel, Task<ContractResult> response)
        {
            var result =await response;//返回结果
            var replyProps = new BasicProperties();
            replyProps.CorrelationId = eventArgs.BasicProperties.CorrelationId;
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result));
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: eventArgs.BasicProperties.ReplyTo, mandatory: false, basicProperties: replyProps, body: bytes);
        }
    }
}