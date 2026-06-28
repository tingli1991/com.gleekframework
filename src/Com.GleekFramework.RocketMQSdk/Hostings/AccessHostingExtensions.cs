using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// 账号主题拓展类
    /// </summary>
    public static partial class AccessHostingExtensions
    {
        /// <summary>
        /// 添加账号配置信息
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="callback">账号配置选项</param>
        /// <returns></returns>
        public static IHostBuilder AddRocketMQOptions(this IHostBuilder builder, Func<IConfiguration, RocketAccessOptions> callback)
        {
            builder.ConfigureAppConfiguration((context, configuration) => configuration.AddRocketMQOptions(callback(AppConfig.Configuration)));
            return builder;
        }

        /// <summary>
        /// 添加账号配置信息
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="options">账号配置选项</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddRocketMQOptions(this IConfigurationBuilder configuration, RocketAccessOptions options)
        {
            RocketAccessProvider.SetAccessOptions(options);
            return configuration;
        }
    }
}