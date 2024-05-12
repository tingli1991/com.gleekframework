using Aliyun.MQ.Model;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// RocketMQ客户端服务
    /// </summary>
    public partial class RocketClientService : IBaseAutofac
    {
        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="topic">主题</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="deliverTimeMillis">指定一个时刻，在这个时刻之后消息才能被消费（单位：毫秒时间戳）</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="key">分区键</param>
        /// <param name="headers">头部喜喜</param>
        /// <returns></returns>
        public async Task<ContractResult<string>> PublishMessageBodyAsync(string host, string topic, Enum type, long deliverTimeMillis = 0,
            string serialNo = null, string key = null, Dictionary<string, string> headers = null)
        {
            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            var messageBody = new MessageBody()
            {
                SerialNo = serialNo,
                ActionKey = type.GetActionKey(),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds()
            };
            return await PublishAsync(host, topic, messageBody, deliverTimeMillis, serialNo, key);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">主机地址</param>
        /// <param name="topic">主题</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="data">数据包</param>
        /// <param name="deliverTimeMillis">指定一个时刻，在这个时刻之后消息才能被消费（单位：毫秒时间戳）</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="key">分区键</param>
        /// <param name="headers">头部喜喜</param>
        /// <returns></returns>
        public async Task<ContractResult<string>> PublishMessageBodyAsync<T>(string host, string topic, Enum type, T data, long deliverTimeMillis = 0,
            string serialNo = null, string key = null, Dictionary<string, string> headers = null) where T : class
        {
            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            var messageBody = new MessageBody<T>()
            {
                Data = data,
                SerialNo = serialNo,
                ActionKey = type.GetActionKey(),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds()
            };
            return await PublishAsync(host, topic, messageBody, deliverTimeMillis, serialNo, key);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">主机</param>
        /// <param name="topic">主题</param>
        /// <param name="data">消息内容</param>
        /// <param name="serialNo">业务流水号</param>
        /// <param name="key">设置代表消息的业务关键属性，请尽可能全局唯一
        ///     以方便您在无法正常收到消息情况下，可通过消息队列RocketMQ版控制台查询消息并补发
        ///     注意：不设置也不会影响消息正常收发。
        /// </param>
        /// <returns></returns>
        public async Task<ContractResult<string>> PublishAsync<T>(string host, string topic, T data, string serialNo = null, string key = null)
        {
            return await PublishAsync(host, topic, data, 0, serialNo, key);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">主机</param>
        /// <param name="topic">主题</param>
        /// <param name="data">消息内容</param>
        /// <param name="deliverTimeMillis">指定一个时刻，在这个时刻之后消息才能被消费（单位：毫秒时间戳）</param>
        /// <param name="serialNo">业务流水号</param>
        /// <param name="key">设置代表消息的业务关键属性，请尽可能全局唯一
        ///     以方便您在无法正常收到消息情况下，可通过消息队列RocketMQ版控制台查询消息并补发
        ///     注意：不设置也不会影响消息正常收发。
        /// </param>
        /// <returns></returns>
        public async Task<ContractResult<string>> PublishAsync<T>(string host, string topic, T data, long deliverTimeMillis = 0, string serialNo = null, string key = null)
        {
            var result = new ContractResult<string>();
            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentException("topic不能为空");
            }

            if (data == null)
            {
                throw new ArgumentException("消息内容不能为空");
            }

            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            var jsonValue = JsonConvert.SerializeObject(data);
            var producer = ProducerProvider.GetProducerSingle(host, topic);//创建生产者
            var message = new TopicMessage(jsonValue, RocketMQConstant.DEFAULT_TAGES);//创建消息对象
            if (!string.IsNullOrEmpty(key))
            {
                // 设置代表消息的业务关键属性，请尽可能全局唯一。
                // 以方便您在无法正常收到消息情况下，可通过消息队列RocketMQ版控制台查询消息并补发。
                // 注意：不设置也不会影响消息正常收发。
                message.MessageKey = key;
            }

            if (deliverTimeMillis > 0)
            {
                //指定一个时刻，在这个时刻之后消息才能被消费。
                message.StartDeliverTime = deliverTimeMillis;
            }
            TopicMessage response = producer.PublishMessage(message);//发送消息
            return await Task.FromResult(result.SetSuceccful(response.Id, serialNo));
        }
    }
}