using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.ContractSdk;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// Rocket队列处理类
    /// </summary>
    public abstract class RabbitHandler : IHandler
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public abstract Enum ActionKey { get; }

        /// <summary>
        /// 是否自动应答
        /// </summary>
        public virtual bool AutoAck { get; set; } = true;

        /// <summary>
        /// 处理方法
        /// </summary>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public abstract Task<ContractResult> ExecuteAsync(MessageBody messageBody);
    }
}