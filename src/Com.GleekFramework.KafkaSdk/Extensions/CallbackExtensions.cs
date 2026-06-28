using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.NLogSdk;
using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// 回调处理类
    /// </summary>
    public static partial class CallbackExtensions
    {
        /// <summary>
        /// 消息订阅事件
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public static async Task MessageCallbackAsync(this ConsumeResult<string, string> message)
        {
            var beginTime = DateTime.Now.ToCstTime();
            var messageBody = await message.ToMessageBodyAsync();
            try
            {
                if (messageBody == null)
                {
                    //位置的方法，直接不处理即可
                    return;
                }

                var handler = HandlerFactory.GetInstance<IKafkaHandler>(messageBody.ActionKey);
                await CoustomExecuteExtensions.ExecuteAsync(handler, messageBody);//执行调用方法
            }
            catch (Exception ex)
            {
                var totalMilliseconds = (long)(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
                NLogProvider.Error($"【Kafka消费者】{ex}", messageBody.SerialNo, messageBody.Headers.GetUrl(), totalMilliseconds);
            }
        }
    }
}