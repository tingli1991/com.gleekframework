using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// Mongo拓展类
    /// </summary>
    public static partial class HostingExtensions
    {
        /// <summary>
        /// 使用Mongodb
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionNames">连接名称列表</param>
        /// <returns></returns>
        public static IHostBuilder UseMongodb(this IHostBuilder builder, params string[] connectionNames)
        {
            builder.ConfigureServices((context, services) => services.UseMongodb(AppConfig.Configuration, connectionNames));
            return builder;
        }

        /// <summary>
        /// 使用Mongodb
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration">配置对象</param>
        /// <param name="connectionNames">连接名称列表</param>
        /// <returns></returns>
        public static IHostBuilder UseMongodb(this IHostBuilder builder, IConfiguration configuration, params string[] connectionNames)
        {
            builder.ConfigureServices((context, services) => services.UseMongodb(configuration, connectionNames));
            return builder;
        }

        /// <summary>
        /// 使用Mongodb
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置对象</param>
        /// <param name="connectionNames">连接名称列表</param>
        /// <returns></returns>
        public static IServiceCollection UseMongodb(this IServiceCollection services, IConfiguration configuration, params string[] connectionNames)
        {
            MongoClientProvider.RegisterClientPool(configuration, connectionNames);
            return services;
        }
    }
}