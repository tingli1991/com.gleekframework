namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 配置常量
    /// </summary>
    public static partial class ConfigConstant
    {
        /// <summary>
        /// 静态数组配置键
        /// </summary>
        public const string SummariesKey = "Summaries";

        /// <summary>
        /// 静态测试配置模型键
        /// </summary>
        public const string ConfigOptionsKey = "ConfigOptions";

        /// <summary>
        /// Kafka连接订阅配置
        /// </summary>
        public const string KafkaConnectionOptionsKey = "KafkaConnectionOptions";

        /// <summary>
        /// Rabbit连接订阅配置
        /// </summary>
        public const string RabbitConnectionOptionsKey = "RabbitConnectionOptions";

        /// <summary>
        /// 默认客户端配置名称
        /// </summary>
        public const string KafkaDefaultClientHostsKey = "KafkaConnectionStrings:DefaultClientHosts";

        /// <summary>
        /// 默认客户端配置名称
        /// </summary>
        public const string RabbitDefaultClientHostsKey = "RabbitConnectionStrings:DefaultClientHosts";
    }
}