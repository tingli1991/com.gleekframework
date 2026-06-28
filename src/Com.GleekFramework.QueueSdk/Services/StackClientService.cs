using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.QueueSdk
{
    /// <summary>
    /// 队列客户端服务
    /// </summary>
    public partial class StackClientService : IBaseAutofac
    {
        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">方法名称</param>
        /// <param name="serialNo">流水号</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task PublishAsync<T>(Enum type, string serialNo = null, Dictionary<string, string> headers = null) where T : class
        {
            await PublishAsync(type, StackConstant.DEFAULT_STACK_TOPIC, serialNo, headers);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">方法名称</param>
        /// <param name="topic">主题</param>
        /// <param name="serialNo">流水号</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task PublishAsync<T>(Enum type, string topic, string serialNo = null, Dictionary<string, string> headers = null) where T : class
        {
            var messageBody = new MessageBody()
            {
                ActionKey = type.GetActionKey(),
                SerialNo = HttpContextAccessor.GetSerialNo(serialNo),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            await PartitionedStackProvider.PublishAsync(topic, messageBody);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">方法名称</param>
        /// <param name="data">消息内容</param>
        /// <param name="serialNo">流水号</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task PublishAsync<T>(Enum type, T data, string serialNo = null, Dictionary<string, string> headers = null) where T : class
        {
            await PublishAsync(type, StackConstant.DEFAULT_STACK_TOPIC, data, serialNo, headers);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">方法名称</param>
        /// <param name="topic">主题</param>
        /// <param name="data">消息内容</param>
        /// <param name="serialNo">流水号</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task PublishAsync<T>(Enum type, string topic, T data, string serialNo = null, Dictionary<string, string> headers = null) where T : class
        {
            var messageBody = new MessageBody<T>()
            {
                Data = data,
                ActionKey = type.GetActionKey(),
                SerialNo = HttpContextAccessor.GetSerialNo(serialNo),
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Headers = HttpContextAccessor.ToHeaders().AddHeaders(headers),
            };
            await PartitionedStackProvider.PublishAsync(topic, messageBody);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">方法名称</param>
        /// <param name="data">消息内容</param>
        /// <param name="serialNo">流水号</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task PublishManyAsync<T>(Enum type, IEnumerable<T> data, string serialNo = null, Dictionary<string, string> headers = null) where T : class
        {
            await PublishManyAsync(type, StackConstant.DEFAULT_STACK_TOPIC, data, serialNo, headers);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">方法名称</param>
        /// <param name="topic">主题</param>
        /// <param name="data">消息内容</param>
        /// <param name="serialNo">流水号</param>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public async Task PublishManyAsync<T>(Enum type, string topic, IEnumerable<T> data, string serialNo = null, Dictionary<string, string> headers = null) where T : class
        {
            serialNo = HttpContextAccessor.GetSerialNo(serialNo);//转换流水号
            headers = HttpContextAccessor.ToHeaders().AddHeaders(headers);//转换头部信息
            var messageBodys = data.Select(e => new MessageBody<T>()
            {
                Data = e,
                Headers = headers,
                SerialNo = serialNo,
                ActionKey = type.GetActionKey(),
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
            await PartitionedStackProvider.PublishAsync(topic, messageBodys);
        }
    }
}