using System.Collections.Generic;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 主题分组服务模型
    /// </summary>
    public class TopicServiceModel<T>
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 消息处理服务列表
        /// </summary>
        public IEnumerable<T> ServiceList { get; set; }
    }
}
