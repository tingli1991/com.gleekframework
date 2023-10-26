using System.Collections.Generic;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// RabbitMQ消费者配置
    /// </summary>
    public class RabbitConsumerOptions
    {
        /// <summary>
        /// 是否需要等待Task任务完成
        /// </summary>
        public bool AwaitTask { get; set; } = false;

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; } = "/";

        /// <summary>
        /// 主机配置
        /// </summary>
        public IEnumerable<RabbitHostOptions> Hosts { get; set; }
    }
}