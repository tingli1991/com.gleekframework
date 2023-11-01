using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Com.GleekFramework.ObjectSdk
{
    /// <summary>
    /// 阿里云主机拓展类
    /// </summary>
    public static partial class AliyunHostingExtensions
    {
        /// <summary>
        /// 添加账号配置信息
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="callback">账号配置选项</param>
        /// <returns></returns>
        public static IHostBuilder UseAliyunOSS(this IHostBuilder builder, Func<IConfiguration, AliyunOSSOptions> callback)
        {
            builder.ConfigureAppConfiguration((context, configuration) => configuration.AddAccessOptions(callback(AppConfig.Configuration)));
            return builder;
        }

        /// <summary>
        /// 添加账号配置信息
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="options">账号配置选项</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddAccessOptions(this IConfigurationBuilder configuration, AliyunOSSOptions options)
        {
            AliyunOSSProvider.Options = options;
            return configuration;
        }
    }
}