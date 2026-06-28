using Com.GleekFramework.ConsumerSdk;
using Com.GleekFramework.ContractSdk;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.QueueSdk
{
    /// <summary>
    /// 栈消息处理接口
    /// </summary>
    public abstract class StackHandler : ITopicHandler
    {
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Order { get; } = 0;

        /// <summary>
        /// 方法名称
        /// </summary>
        public abstract Enum ActionKey { get;}

        /// <summary>
        /// 主题
        /// </summary>
        public virtual string Topic { get; } = StackConstant.DEFAULT_STACK_TOPIC;

        /// <summary>
        /// 实现方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public abstract Task<ContractResult> ExecuteAsync(MessageBody messageBody);
    }
}