namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// Kafka常量配置
    /// </summary>
    public static partial class KafkaConstant
    {
        /// <summary>
        /// 消费端拉取消息数据的大小
        /// </summary>
        public static readonly int FetchMaxBytes = 30 * 1024 * 1024;

        /// <summary>
        /// 最大消息字节数
        /// </summary>
        public static readonly int MessageMaxBytes = 30 * 1024 * 1024;
    }
}