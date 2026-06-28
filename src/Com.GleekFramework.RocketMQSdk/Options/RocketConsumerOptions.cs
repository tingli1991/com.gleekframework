using System.Collections.Generic;

namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// Rocket消费者配置
    /// </summary>
    public class RocketConsumerOptions
    {
        /// <summary>
        /// 分组Id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 批量消费的消息条数（上限：16条）
        /// </summary>
        public uint BatchSize { get; set; } = 16;

        /// <summary>
        /// 长轮询时间时间秒（最多可设置为30秒）
        /// </summary>
        public uint WaitSeconds { get; set; } = 30;

        /// <summary>
        /// 是否需要等待Task任务完成
        /// </summary>
        public bool AwaitTask { get; set; } = false;

        /// <summary>
        /// 主机配置选项集
        /// </summary>
        public IEnumerable<RocketConsumerHostOptions> HostOptions { get; set; }
    }
}