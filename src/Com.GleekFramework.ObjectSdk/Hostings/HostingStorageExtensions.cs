using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 对象存储主机
    /// </summary>
    public static class HostingStorageExtensions
    {
        /// <summary>
        /// 使用对象存储配置
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="options">账号配置选项</param>
        /// <returns></returns>
        public static IConfigurationBuilder UseObjectStorage(this IConfigurationBuilder configuration, ObjectStorageOptions options)
        {
            ObjectStorageOptions.Initialize(options);
            return configuration;
        }

        /// <summary>
        /// 使用对象存储配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="callback">账号配置选项</param>
        /// <returns></returns>
        public static IHostBuilder UseObjectStorage(this IHostBuilder builder, Func<IConfiguration, ObjectStorageOptions> callback)
        {
            builder.ConfigureAppConfiguration((context, configuration) => configuration.UseObjectStorage(callback(AppConfig.Configuration)));
            return builder;
        }
    }
}