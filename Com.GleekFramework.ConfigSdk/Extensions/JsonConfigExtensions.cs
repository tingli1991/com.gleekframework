using Microsoft.Extensions.Configuration;
using System;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// JSON配置文件拓展
    /// </summary>
    public static partial class JsonConfigExtensions
    {
        /// <summary>
        /// 获取Json配置
        /// </summary>
        /// <param name="configuration">配置文件</param>
        /// <param name="key">配置文件的Key(格式：xxx:yyy，注意中间使用':'分割)</param>
        /// <returns></returns>
        public static string GetValue(this IConfiguration configuration, string key)
        {
            return configuration[key];
        }

        /// <summary>
        /// 获取Json配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration">配置文件</param>
        /// <param name="key">配置文件的Key(格式：xxx:yyy，注意中间使用':'分割)</param>
        /// <returns></returns>
        public static T Get<T>(this IConfiguration configuration, string key)
        {
            return configuration.GetSection(key).Get<T>();
        }

        /// <summary>
        /// 获取Json配置
        /// </summary>
        /// <param name="configuration">配置文件</param>
        /// <param name="type">对象类型</param>
        /// <param name="key">配置文件的Key(格式：xxx:yyy，注意中间使用':'分割)</param>
        /// <returns></returns>
        public static object GetValue(this IConfiguration configuration, Type type, string key)
        {
            return configuration.GetSection(key).Get(type);
        }

        /// <summary>
        /// 获取整个JSON配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static T GetConfiguration<T>(this IConfiguration configuration)
        {
            return ConfigurationBinder.Get<T>(configuration);
        }

        /// <summary>
        /// 获取整个JSON配置
        /// </summary>
        /// <param name="configuration">配置文件</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static object GetConfiguration(this IConfiguration configuration, Type type)
        {
            return ConfigurationBinder.Get(configuration, type);
        }
    }
}