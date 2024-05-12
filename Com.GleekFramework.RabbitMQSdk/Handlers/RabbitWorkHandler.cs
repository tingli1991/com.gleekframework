namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 工作模式处理类
    /// </summary>
    public abstract class RabbitWorkHandler : RabbitHandler
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public abstract string QueueName { get; }
    }
}