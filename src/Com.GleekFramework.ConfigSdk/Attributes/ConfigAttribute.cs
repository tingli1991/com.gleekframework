using Microsoft.Extensions.Configuration;
using System;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 配置文件特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 配置文件信息
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">配置键</param>
        public ConfigAttribute(string key = "") : this(null, key)
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration">配置文件</param>
        /// <param name="key">配置键</param>
        public ConfigAttribute(IConfiguration configuration, string key)
        {
            Key = key;
            Configuration = configuration ?? JsonConfigProvider.GetConfiguration();
        }
    }
}