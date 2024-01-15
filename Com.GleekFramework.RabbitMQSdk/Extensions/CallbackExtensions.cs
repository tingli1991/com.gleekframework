using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.NLogSdk;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 回调处理类
    /// </summary>
    public static partial class CallbackExtensions
    {
        /// <summary>
        /// 关机事件
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <param name="sender"></param>
        public static Task ConsumerShutdownEventAsync(this ShutdownEventArgs eventArgs, object sender)
        {
            NLogProvider.Warn($"【Rabbit发布订阅模式消费者】收到停机信息，事件参数：{eventArgs}，Sender：{sender}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 消息执行函数
        /// </summary>
        /// <param name="eventArgs">消息内容</param>
        /// <returns></returns>
        public static Task<ContractResult> MessageExecuteAsync<T>(this BasicDeliverEventArgs eventArgs) where T : RabbitHandler
        {
            var beginTime = DateTime.Now.ToCstTime();
            var messageBody = eventArgs.ToMessageBody();
            try
            {
                if (messageBody != null)
                {
                    var handler = HandlerFactory.GetInstance<T>(messageBody.ActionKey);
                    return CoustomExecuteExtensions.ExecuteAsync(handler, messageBody);//执行调用方法
                }
            }
            catch (Exception ex)
            {
                var totalMilliseconds = (long)(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
                NLogProvider.Error($"【RabbitMQ消费】{ex}", messageBody?.SerialNo ?? "", messageBody?.Headers?.GetUrl(), totalMilliseconds);
            }
            return Task.FromResult(new ContractResult());
        }
    }
}