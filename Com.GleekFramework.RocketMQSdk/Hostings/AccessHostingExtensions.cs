using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
        /// <param name="options">账号配置选项</param>
        /// <returns></returns>
        public static IHostBuilder AddAccessOptions(this IHostBuilder builder, RocketAccessOptions options)
        {
            builder.ConfigureAppConfiguration((context, configuration) => configuration.AddAccessOptions(options));
            return builder;
        }

        /// <summary>
        /// 添加账号配置信息
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="options">账号配置选项</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddAccessOptions(this IConfigurationBuilder configuration, RocketAccessOptions options)
        {
            RocketAccessProvider.SetAccessOptions(options);
            return configuration;
        }
    }
}