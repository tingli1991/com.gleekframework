namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 常量配置
    /// </summary>
    public static class RabbitConstant
    {
        /// <summary>
        /// RPC模式默认的队列名称
        /// </summary>
        public const string DEFAULT_RPC_QUEUE_NAME = "com.gleekframework.default.queue.rpc";

        /// <summary>
        /// 默认的主题交换机
        /// </summary>
        public const string DEFAULT_TOPIC_EXCHANGE_NAME = "com.gleekframework.default.exchange.topic";

        /// <summary>
        /// 默认的路由交换机
        /// </summary>
        public const string DEFAULT_ROUTING_EXCHANGE_NAME = "com.gleekframework.default.exchange.routing";

        /// <summary>
        /// 默认的发布订阅模式交换机
        /// </summary>
        public const string DEFAULT_SUBSCRIBE_EXCHANGE_NAME = "com.gleekframework.default.exchange.subscribe";
    }
}