using Confluent.Kafka;
using System.Collections.Generic;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// Kafka消费配置选项
    /// </summary>
    public class KafkaConsumerOptions
    {
        /// <summary>
        /// 分组Id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 自动应答开关(默认开启)
        /// </summary>
        public bool AutoAck { get; set; } = true;

        /// <summary>
        /// 是否需要等待Task任务完成
        /// </summary>
        public bool AwaitTask { get; set; } = false;

        /// <summary>
        /// 主机配置选项集
        /// </summary>
        public IEnumerable<KafkaConsumerHostOptions> HostOptions { get; set; }

        /// <summary>
        /// 便宜方向（默认：最新的）
        /// </summary>
        public AutoOffsetReset AutoOffset { get; set; } = AutoOffsetReset.Latest;
    }

    /// <summary>
    /// Kafka消费订阅的主机配置信息
    /// </summary>
    public class KafkaConsumerHostOptions
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 主题列表
        /// </summary>
        public IEnumerable<string> Topics { get; set; }
    }
}