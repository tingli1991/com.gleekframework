using Microsoft.Extensions.Hosting;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 配置主机
    /// </summary>
    public static partial class HostingExtensions
    {
        /// <summary>
        /// 使用配置
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseConfig(this IHostBuilder builder)
        {
            AppConfig.UseAppConfig(builder);
            return builder;
        }

        /// <summary>
        /// 添加Nacos
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseConfigAttribute(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                DependencyProvider.Switch = true;//打开开关
                DependencyProvider.RefreshConfigAttribute();
            });
            return builder;
        }
    }
}