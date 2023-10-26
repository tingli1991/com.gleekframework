using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// Rpc客户端服务
    /// </summary>
    public partial class RabbitWorkClientService : IBaseAutofac
    {
        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="queueName">队列名称</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部喜喜</param>
        /// <returns></returns>
        public async Task<ContractResult> PublishAsync(string host, string queueName, Enum type, string serialNo = null, Dictionary<string, string> headers = null)
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
            return await PublishAsync(host, queueName, messageBody, serialNo);
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
        public async Task<ContractResult> PublishAsync<T>(string host, string queueName, Enum type, T data, string serialNo = null, Dictionary<string, string> headers = null)
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
            return await PublishAsync(host, queueName, messageBody, serialNo);
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
            var options = host.ToConnectionObject<RabbitConnectionOptions>();
            if (options == null || options.Host == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var factory = new ConnectionFactory()
            {
                Port = options.Port,
                HostName = options.Host,
                Password = options.Password,
                UserName = options.UserName,
                VirtualHost = options.VirtualHost,
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var jsonValue = JsonConvert.SerializeObject(data);
            var messageBytes = Encoding.UTF8.GetBytes(jsonValue);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: string.Empty, routingKey: queueName, basicProperties: properties, body: messageBytes);
            return await Task.FromResult(new ContractResult().SetSuceccful(serialNo));
        }
    }
}