using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ContractSdk;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 消费者消费消息
    /// </summary>
    public interface IHandler : IBaseAutofac
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        Enum ActionKey { get; }

        /// <summary>
        /// 执行业务动作的方法
        /// </summary>
        /// <param name="messageBody">消息内容</param>
        Task<ContractResult> ExecuteAsync(MessageBody messageBody);
    }
}