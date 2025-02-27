namespace Com.GleekFramework.Models
{
    /// <summary>
    /// Rabbit队列常量
    /// </summary>
    public static partial class RabbitQueueConstant
    {
        /// <summary>
        /// 自定义RPC队列名称
        /// </summary>
        public const string RpcCustomerQueue = "com.gleekframework.customer.queue.rpc";

        /// <summary>
        /// 自定义Work队列名称
        /// </summary>
        public const string WorkCustomerQueue = "com.gleekframework.customer.queue.work";

        /// <summary>
        /// 自定义Subscribe交换名称
        /// </summary>
        public const string WorkCustomerExchangeName = "com.gleekframework.customer.exchange.test";
    }
}