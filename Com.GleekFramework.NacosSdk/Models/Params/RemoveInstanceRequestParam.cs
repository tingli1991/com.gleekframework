using System;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 从服务中删除实例请求参数
    /// </summary>
    [Serializable]
    internal class RemoveInstanceRequestParam
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
        /// 集群名称
        /// </summary>
        public string ClusterName { get; set; }

        /// <summary>
        /// 分组名
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