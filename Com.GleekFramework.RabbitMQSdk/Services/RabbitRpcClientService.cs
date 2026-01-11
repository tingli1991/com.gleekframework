using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// Rpc客户端服务
    /// </summary>
    public partial class RabbitRpcClientService : IBaseAutofac
    {
        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 等待回调的任务列表
        /// </summary>
        private static readonly ConcurrentDictionary<string, TaskCompletionSource<ContractResult>> CallbackTask = new ConcurrentDictionary<string, TaskCompletionSource<ContractResult>>();

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部喜喜</param>
        /// <returns></returns>
        public Task<ContractResult> PublishAsync(string host, Enum type, string serialNo = null, Dictionary<string, string> headers = null)
        {
            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            headers = HttpContextAccessor.ToHeaders().AddHeaders(headers);//转换头部信息
            var messageBody = new MessageBody()
            {
                SerialNo = serialNo,
                ActionKey = type.GetActionKey(),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds()
            };
            return PublishAsync(host, messageBody, serialNo);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="data">参数</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部喜喜</param>
        /// <returns></returns>
        public Task<ContractResult> PublishAsync<T>(string host, Enum type, T data, string serialNo = null, Dictionary<string, string> headers = null)
            where T : class
        {
            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            headers = HttpContextAccessor.ToHeaders().AddHeaders(headers);//转换头部信息
            var messageBody = new MessageBody<T>()
            {
                Data = data,
                SerialNo = serialNo,
                ActionKey = type.GetActionKey(),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds()
            };
            return PublishAsync(host, messageBody, serialNo);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部喜喜</param>
        /// <returns></returns>
        public Task<ContractResult> PublishAsync(string host, string queueName, Enum type, string serialNo = null, Dictionary<string, string> headers = null)
        {
            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            headers = HttpContextAccessor.ToHeaders().AddHeaders(headers);//转换头部信息
            var messageBody = new MessageBody()
            {
                SerialNo = serialNo,
                ActionKey = type.GetActionKey(),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds()
            };
            return PublishAsync(host, queueName, messageBody, serialNo);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="data">参数</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部喜喜</param>
        /// <returns></returns>
        public Task<ContractResult> PublishAsync<T>(string host, string queueName, Enum type, T data, string serialNo = null, Dictionary<string, string> headers = null)
            where T : class
        {
            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            headers = HttpContextAccessor.ToHeaders().AddHeaders(headers);//转换头部信息
            var messageBody = new MessageBody<T>()
            {
                Data = data,
                SerialNo = serialNo,
                ActionKey = type.GetActionKey(),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds()
            };
            return PublishAsync(host, queueName, messageBody, serialNo);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">主机</param>
        /// <param name="data">消息内容</param>
        /// <param name="serialNo">业务流水号</param>
        /// <returns></returns>
        public Task<ContractResult> PublishAsync<T>(string host, T data, string serialNo = null)
        {
            return PublishAsync(host, RabbitConstant.DEFAULT_RPC_QUEUE_NAME, data, serialNo);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">主机</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="data">消息内容</param>
        /// <param name="serialNo">业务流水号</param>
        /// <returns></returns>
        public async Task<ContractResult> PublishAsync<T>(string host, string queueName, T data, string serialNo = null)
        {
            var channel = await ConnectionProvider.GetChannelAsync(host);
            var consumer = new AsyncEventingBasicConsumer(channel);

            var replyQueueName = await ReplyQueueProvider.QueueDeclareAsync(channel, queueName);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            consumer.ReceivedAsync += (model, eventArgs) => ReceivedMessage(eventArgs);
            await channel.BasicConsumeAsync(consumer: consumer, queue: replyQueueName, autoAck: true);
            return await PublishAsync(channel, queueName, replyQueueName, data, serialNo);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="queueName"></param>
        /// <param name="replyQueueName">回调的队列名称</param>
        /// <param name="data">数据</param>
        /// <param name="serialNo">流水号</param>
        /// <returns></returns>
        private async Task<ContractResult> PublishAsync<T>(IChannel channel, string queueName, string replyQueueName, T data, string serialNo = null)
        {
            var basicProperties = new BasicProperties
            {
                ReplyTo = replyQueueName,
                CorrelationId = Guid.NewGuid().ToString()
            };
            var jsonValue = JsonConvert.SerializeObject(data);
            var messageBytes = Encoding.UTF8.GetBytes(jsonValue);

            var cts = new TaskCompletionSource<ContractResult>();
            CallbackTask.TryAdd(basicProperties.CorrelationId, cts);//这个必须在前
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, mandatory: false, basicProperties: basicProperties, body: messageBytes);
            return await cts.Task;
        }

        /// <summary>
        /// 接受消息回调处理方法
        /// </summary>
        /// <param name="eventArgs">回调事件参数</param>
        /// <returns></returns>
        private Task ReceivedMessage(BasicDeliverEventArgs eventArgs)
        {
            var correlationId = eventArgs.BasicProperties.CorrelationId;
            if (CallbackTask.TryRemove(correlationId, out var tcs))
            {
                var body = eventArgs.Body.ToArray();
                var jsonValue = Encoding.UTF8.GetString(body);
                var response = jsonValue.DeserializeObject<ContractResult>();
                tcs.TrySetResult(response);
            }
            return Task.CompletedTask;
        }
    }
}