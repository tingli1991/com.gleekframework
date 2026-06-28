using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Redis仓储拓展类
    /// </summary>
    public static partial class RedisHostingExtensions
    {
        /// <summary>
        /// 使用CsRedis
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionNames">连接名称列表</param>
        /// <returns></returns>
        public static IHostBuilder UseCsRedis(this IHostBuilder builder, params string[] connectionNames)
        {
            builder.ConfigureServices((context, services) => services.UseCsRedis(AppConfig.Configuration, connectionNames));
            return builder;
        }

        /// <summary>
        /// 使用CsRedis
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration">配置对象</param>
        /// <param name="connectionNames">连接名称列表</param>
        /// <returns></returns>
        public static IHostBuilder UseCsRedis(this IHostBuilder builder, IConfiguration configuration, params string[] connectionNames)
        {
            builder.ConfigureServices((context, services) => services.UseCsRedis(configuration, connectionNames));
            return builder;
        }

        /// <summary>
        /// 使用CsRedis
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置对象</param>
        /// <param name="connectionNames">连接名称列表</param>
        /// <returns></returns>
        public static IServiceCollection UseCsRedis(this IServiceCollection services, IConfiguration configuration, params string[] connectionNames)
        {
            RedisProvider.RegisterClientPool(configuration, connectionNames);
            return services;
        }
    }
}