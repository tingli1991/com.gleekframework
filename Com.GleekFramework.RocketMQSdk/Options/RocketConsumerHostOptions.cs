using System.Collections.Generic;

namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// Rocket订阅的主机信息
    /// </summary>
    public class RocketConsumerHostOptions
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