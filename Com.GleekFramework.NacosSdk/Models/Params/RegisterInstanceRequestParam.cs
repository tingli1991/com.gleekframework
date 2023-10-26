using System;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 注册实例的请求参数
    /// </summary>
    [Serializable]
    internal class RegisterInstanceRequestParam
    {
        /// <summary>
        /// 服务实例IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 服务实例端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 命名空间ID
        /// </summary>
        public string NamespaceId { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// 是否上线
        /// </summary>
        public bool? Enable { get; set; }

        /// <summary>
        /// 是否健康
        /// </summary>
        public bool? Healthy { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// 集群名
        /// </summary>
        public string ClusterName { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 是否临时实例
        /// </summary>
        public bool? Ephemeral { get; set; }

        /// <summary>
        /// Token字段
        /// </summary>
        public string Token { get; set; }
    }
}