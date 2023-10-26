using Microsoft.Extensions.Hosting;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 主机拓展
    /// </summary>
    public static partial class HostingExtensions
    {
        /// <summary>
        /// 添加Nacos
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseNacosConf(this IHostBuilder builder)
        {
            NacosConfig.UseNacosConfig(builder);
            NacosProvider.UseNacosConf(builder);
            return builder;
        }

        /// <summary>
        /// 使用阿里巴巴的Nacos配置功能
        /// </summary>
        /// <param name="host"></param>
        /// <param name="serverName">服务名称</param>
        public static IHost UseNacosService(this IHost host, string serverName)
        {
            NacosProvider.UseNacosService(host, serverName);
            return host;
        }
    }
}