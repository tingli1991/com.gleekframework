using System.Collections.Generic;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Nacos添加的时候必要的选项
    /// </summary>
    public class NacosSettings
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 开关(默认)
        /// </summary>
        public bool Switch { get; set; } = true;

        /// <summary>
        /// 默认的命名空间Id
        /// </summary>
        public string NamespaceId { get; set; } = "";

        /// <summary>
        /// 心跳间隔时长（单位：毫秒）
        /// </summary>
        public int ListenInterval { get; set; } = 3000;

        /// <summary>
        /// 监听的配置选项
        /// </summary>
        public ConfigSettings ConfigSettings { get; set; }

        /// <summary>
        /// 监听的服务选项
        /// </summary>
        public ServiceSettings ServiceSettings { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; } = "DEFAULT_GROUP";

        /// <summary>
        /// Nacos 服务器地址列表
        /// </summary>
        /// <example>
        /// 10.1.12.123:8848,10.1.12.124:8848
        /// </example>
        public IEnumerable<string> ServerAddresses { get; set; }
    }
}