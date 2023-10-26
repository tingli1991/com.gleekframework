using Microsoft.Extensions.Hosting;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 配置文件依赖注入拓展
    /// </summary>
    public static partial class DependencyExtensions
    {
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