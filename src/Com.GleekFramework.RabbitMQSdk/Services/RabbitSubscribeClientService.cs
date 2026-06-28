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
    /// 发布订阅客户端服务
    /// </summary>
    public partial class RabbitSubscribeClientService : IBaseAutofac
    {
        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部喜喜</param>
        /// <returns></returns>
        public async Task<ContractResult> PublishAsync(string host, string exchangeName, Enum type, string serialNo = null, Dictionary<string, string> headers = null)
        {
            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            headers = HttpContextAccessor.ToHeaders().AddHeaders(headers);//转换头部信息
            var messageBody = new MessageBody()
            {
                SerialNo = serialNo,
                ActionKey = $"{type}",
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            return await PublishAsync(host, exchangeName, messageBody, serialNo);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="data">参数</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部喜喜</param>
        /// <returns></returns>
        public async Task<ContractResult> PublishAsync<T>(string host, string exchangeName, Enum type, T data, string serialNo = null, Dictionary<string, string> headers = null)
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
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            return await PublishAsync(host, exchangeName, messageBody, serialNo);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">主机</param>
        /// <param name="exchangeName">交换机名称</param>
        /// <param name="data">消息内容</param>
        /// <param name="serialNo">业务流水号</param>
        /// <returns></returns>
        public async Task<ContractResult> PublishAsync<T>(string host, string exchangeName, T data, string serialNo = null)
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

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Fanout);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

            var jsonValue = JsonConvert.SerializeObject(data);
            var messageBytes = Encoding.UTF8.GetBytes(jsonValue);

            var properties = new BasicProperties();
            properties.Persistent = true;
            await channel.BasicPublishAsync(exchange: exchangeName, routingKey: string.Empty, mandatory: false, basicProperties: properties, body: messageBytes);
            return await Task.FromResult(new ContractResult().SetSuceccful(serialNo));
        }
    }
}