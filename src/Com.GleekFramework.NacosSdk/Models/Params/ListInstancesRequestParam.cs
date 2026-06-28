using System;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 查询服务下的实例列表请求参数
    /// </summary>
    [Serializable]
    internal class ListInstancesRequestParam
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string NamespaceId { get; set; }

        /// <summary>
        /// 集群名称
        /// </summary>
        public string Clusters { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 是否只返回健康实例
        /// </summary>
        public bool HealthyOnly { get; set; } = true;
    }
}