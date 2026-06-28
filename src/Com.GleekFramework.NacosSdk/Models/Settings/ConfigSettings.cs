using System.Collections.Generic;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Nacos配置选项
    /// </summary>
    public class ConfigSettings
    {
        /// <summary>
        /// 配置文件存放路径
        /// </summary>
        public string ConfigPath { get; set; }

        /// <summary>
        /// 是否使用相对路径
        /// </summary>
        public bool RelativePath { get; set; } = true;

        /// <summary>
        /// 配置的选项节点
        /// </summary>
        public List<ConfigOptionSettings> ConfitOptions { get; set; }
    }

    /// <summary>
    /// 配置选项
    /// </summary>
    public class ConfigOptionSettings
    {
        /// <summary>
        /// 配置文件Id
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// 是否是私有路径
        /// true：是
        /// false：不是
        /// </summary>
        public bool PrivatePath { get; set; }

        /// <summary>
        /// 配置文件名称
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 私有的配置文件路径
        /// </summary>
        public string ConfigPath { get; set; } = "";

        /// <summary>
        /// 私有的名命空间
        /// </summary>
        public string NamespaceId { get; set; } = "";

        /// <summary>
        /// 私有配置分组
        /// </summary>
        public string GroupName { get; set; }
    }
}