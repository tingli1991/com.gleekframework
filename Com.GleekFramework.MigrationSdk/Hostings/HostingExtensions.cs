using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.MigrationSdk.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 主机拓展类
    /// </summary>
    public static partial class MigrationHostingExtensions
    {
        /// <summary>
        /// 使用版本迁移
        /// </summary>
        /// <param name="builder">主机信息</param>
        /// <param name="callback">回调配置</param>
        /// <returns></returns>
        public static IHostBuilder UseMigrations(this IHostBuilder builder, Func<IConfiguration, MigrationOptions> callback)
        {
            var options = callback(AppConfig.Configuration);
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(provider => callback(AppConfig.Configuration));// 将配置以单例的形式注入到容器中
                services.AddTransient<IStartupFilter, MigrationFilter>();// 注册自定义的 StartupFilter

                //var serviceProvider = options.CreateMigrationProvider();
                //using var scope = serviceProvider.CreateScope();
                //{
                //    await scope.MigrationUpAsync(options);//执行升级脚本迁移
                //    await scope.UpgrationAsync(options);//执行程序升级脚本
                //}
            });
            return builder;
        }
    }
}