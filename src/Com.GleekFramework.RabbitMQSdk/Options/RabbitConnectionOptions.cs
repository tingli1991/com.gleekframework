namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// RabbitMQ客户端配置
    /// </summary>
    public class RabbitConnectionOptions : RabbitHostOptions
    {
        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; } = "/";
    }
}