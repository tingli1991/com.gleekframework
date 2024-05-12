using Com.GleekFramework.ConfigSdk;
using System.Collections.Generic;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 服务配置选项
    /// </summary>
    public class ServiceSettings
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = int.Parse(EnvironmentProvider.GetPort());

        /// <summary>
        /// 协议头
        /// </summary>
        public string Scheme { get; set; } = "http";

        /// <summary>
        /// 是否使用私有服务
        /// </summary>
        public bool PrivateService { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string NamespaceId { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 集群名称
        /// </summary>
        public string ClusterName { get; set; }

        /// <summary>
        /// 当前实例的权重
        /// </summary>
        public double Weight { get; set; } = 10;

        /// <summary>
        /// 服务缓存时间
        /// </summary>
        public int ExpireSeconds { get; set; } = 10;

        /// <summary>
        /// Nacos服务客户端配置列表
        /// </summary>
        public List<ServiceClientOptions> ClientOptions { get; set; }
    }
}