using Aliyun.MQ.Model;
using Com.GleekFramework.ContractSdk;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// 消息拓展类
    /// </summary>
    public static partial class MessageExtensions
    {
        /// <summary>
        /// 转换成消息内容
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<MessageBody> ToMessageBodyAsync(this Message message)
        {
            return await Task.FromResult(message.ToMessageBody());
        }

        /// <summary>
        /// 转换成消息内容
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MessageBody ToMessageBody(this Message message)
        {
            MessageBody messageBody = null;
            try
            {
                if (string.IsNullOrEmpty(message?.Body))
                {
                    return messageBody;
                }

                messageBody = JsonConvert.DeserializeObject<MessageBody<object>>(message.Body);
            }
            catch (Exception)
            {

            }
            return messageBody;
        }
    }
}