using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// Kafka客户端服务
    /// </summary>
    public partial class KafkaClientService : IBaseAutofac
    {
        /// <summary>
        /// 环境变量服务
        /// </summary>
        public EnvironmentService EnvironmentService { get; set; }

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
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部信息</param>
        /// <param name="key">分区键</param>
        /// <returns></returns>
        public Task<ContractResult> PublishAsync(string host, string topic, Enum type, string serialNo = null, Dictionary<string, string> headers = null, string key = null)
        {
            var messageBody = new MessageBody()
            {
                ActionKey = type.GetActionKey(),
                SerialNo = HttpContextAccessor.GetSerialNo(serialNo),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds()
            };
            return PublishAsync(host, topic, messageBody, messageBody.SerialNo, key);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">主机地址</param>
        /// <param name="topic">主题</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="data">数据包</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部信息</param>
        /// <param name="key">分区键</param>
        /// <returns></returns>
        public Task<ContractResult> PublishAsync<T>(string host, string topic, Enum type, T data, string serialNo = null, Dictionary<string, string> headers = null, string key = null) where T : class
        {
            var messageBody = new MessageBody<T>()
            {
                Data = data,
                ActionKey = type.GetActionKey(),
                SerialNo = HttpContextAccessor.GetSerialNo(serialNo),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds()
            };
            return PublishAsync(host, topic, messageBody, messageBody.SerialNo, key);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">主机地址</param>
        /// <param name="topic">主题</param>
        /// <param name="type">消息类型(方法名称)</param>
        /// <param name="data">数据包</param>
        /// <param name="serialNo">流水编号</param>
        /// <param name="headers">头部信息</param>
        /// <param name="key">分区键</param>
        /// <returns></returns>
        public Task<ContractResult> PublishManyAsync<T>(string host, string topic, Enum type, IEnumerable<T> data, string serialNo = null, Dictionary<string, string> headers = null, string key = null) where T : class
        {
            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            headers = HttpContextAccessor.ToHeaders().AddHeaders(headers);//转换头部信息
            var dataList = data.Select(e => new MessageBody<T>()
            {
                Data = e,
                Headers = headers,
                SerialNo = serialNo,
                ActionKey = type.GetActionKey(),
                TimeStamp = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds()
            });
            return PublishManyAsync(host, topic, dataList, serialNo, key);
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host">主机</param>
        /// <param name="topic">主题</param>
        /// <param name="key">分区键</param>
        /// <param name="data">消息内容</param>
        /// <param name="serialNo">业务流水号</param>
        /// <returns></returns>
        public Task<ContractResult> PublishAsync<T>(string host, string topic, T data, string serialNo = null, string key = null)
        {
            var result = new ContractResult();
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentNullException(nameof(topic));
            }

            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            var producer = ProducerProvider.GetProducerSingle(host);
            producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = JsonConvert.SerializeObject(data) });
            return Task.FromResult(result.SetSuceccful(serialNo));//设置为成功
        }

        /// <summary>
        /// 批量发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="host"></param>
        /// <param name="topic"></param>
        /// <param name="data"></param>
        /// <param name="serialNo">业务流水号</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<ContractResult> PublishManyAsync<T>(string host, string topic, IEnumerable<T> data, string serialNo = null, string key = null)
        {
            var result = new ContractResult();
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentNullException(nameof(topic));
            }

            if (data.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(data));
            }

            data.ForEach(e => PublishAsync(host, topic, e, serialNo, key));
            return Task.FromResult(result.SetSuceccful(serialNo));//设置为成功
        }
    }
}