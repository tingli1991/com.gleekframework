using Com.GleekFramework.ConfigSdk;
using System;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Nacos配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NacosConfigAttribute : ConfigAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        public NacosConfigAttribute(string key = "") : base(NacosConfig.Configuration, key)
        {

        }
    }
}