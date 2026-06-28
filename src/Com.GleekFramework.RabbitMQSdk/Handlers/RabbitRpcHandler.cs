namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// RPC模式处理类
    /// </summary>
    public abstract class RabbitRpcHandler : RabbitHandler
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public virtual string QueueName { get; set; } = RabbitConstant.DEFAULT_RPC_QUEUE_NAME;
    }
}