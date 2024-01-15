using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// Dapper仓储拓展类
    /// </summary>
    public static partial class DapperHostingExtensions
    {
        /// <summary>
        /// 使用Dapper
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionNames">连接名称列表</param>
        /// <returns></returns>
        public static IHostBuilder UseDapper(this IHostBuilder builder, params string[] connectionNames)
        {
            builder.ConfigureServices((context, services) => services.UseDapper(AppConfig.Configuration, connectionNames));
            return builder;
        }

        /// <summary>
        /// 使用Dapper
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration">配置对象</param>
        /// <param name="connectionNames">连接名称列表</param>
        /// <returns></returns>
        public static IHostBuilder UseDapper(this IHostBuilder builder, IConfiguration configuration, params string[] connectionNames)
        {
            builder.ConfigureServices((context, services) => services.UseDapper(configuration, connectionNames));
            return builder;
        }

        /// <summary>
        /// 使用Dapper
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">配置对象</param>
        /// <param name="connectionNames">连接名称列表</param>
        /// <returns></returns>
        public static IServiceCollection UseDapper(this IServiceCollection services, IConfiguration configuration, params string[] connectionNames)
        {
            RepositoryProvider.RegisterConnectionStrings(configuration, connectionNames);
            return services;
        }
    }
}