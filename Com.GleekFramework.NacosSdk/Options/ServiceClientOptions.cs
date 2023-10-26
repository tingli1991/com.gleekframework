namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Nacos客户端服务
    /// </summary>
    public class ServiceClientOptions
    {
        /// <summary>
        /// 集群名称(字符串，多个集群用逗号分隔)
        /// </summary>
        public string Clusters { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string NamespaceId { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 当前服务名称
        /// </summary>
        public string ServiceName { get; set; }
    }
}