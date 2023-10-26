using Com.GleekFramework.ContractSdk;
using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

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
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<MessageBody> ToMessageBodyAsync(this ConsumeResult<string, string> message)
        {
            return await Task.FromResult(message.ToMessageBody());
        }

        /// <summary>
        /// 转换成消息内容
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MessageBody ToMessageBody(this ConsumeResult<string, string> message)
        {
            MessageBody messageBody = null;
            try
            {
                if (string.IsNullOrEmpty(message?.Message?.Value))
                {
                    return messageBody;
                }

                messageBody = JsonConvert.DeserializeObject<MessageBody<object>>(message.Message.Value);
            }
            catch (Exception)
            {

            }
            return messageBody;
        }
    }
}