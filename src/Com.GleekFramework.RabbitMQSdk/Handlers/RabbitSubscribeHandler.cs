namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 发布订阅模式处理类
    /// </summary>
    public abstract class RabbitSubscribeHandler : RabbitHandler
    {
        /// <summary>
        /// 交换机名称
        /// </summary>
        public virtual string ExchangeName { get; set; } = RabbitConstant.DEFAULT_SUBSCRIBE_EXCHANGE_NAME;
    }
}