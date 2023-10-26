using Com.GleekFramework.ContractSdk;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// 消息拓展类
    /// </summary>
    public static partial class MessageExtensions
    {
        /// <summary>
        /// 转换成消息内容
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public static MessageBody ToMessageBody(this BasicDeliverEventArgs eventArgs)
        {
            MessageBody messageBody = null;
            try
            {
                var body = eventArgs.Body.ToArray();
                var messageStr = Encoding.UTF8.GetString(body);
                if (string.IsNullOrEmpty(messageStr))
                {
                    return messageBody;
                }

                messageBody = JsonConvert.DeserializeObject<MessageBody<object>>(messageStr);
            }
            catch (Exception ex)
            {

            }
            return messageBody;
        }
    }
}