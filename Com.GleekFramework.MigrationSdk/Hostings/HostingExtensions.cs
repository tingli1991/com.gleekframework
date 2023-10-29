using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 主机拓展类
    /// </summary>
    public static partial class HostingExtensions
    {
        /// <summary>
        /// 使用版本迁移
        /// </summary>
        /// <param name="host">主机信息</param>
        /// <param name="callback">回调配置</param>
        /// <returns></returns>
        public static IHost UseMigrations(this IHost host, Func<IConfiguration, MigrationOptions> callback)
        {
            var options = callback(AppConfig.Configuration);
            host.RegisterApplicationStarted(async () => await CreateServices(options.ConnectionString, options.Assemblys));
            return host;
        }

        /// <summary>
        /// 创建升级服务
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="assemblies">待扫描的程序集列表</param>
        /// <returns></returns>
        private static async Task<IServiceProvider> CreateServices(string connectionString, IEnumerable<Assembly> assemblies)
        {
            var serviceProvider = assemblies.CreateMigrationProvider(connectionString);
            using var scope = serviceProvider.CreateScope();
            await scope.MigrateUpAsync();//执行升级脚本迁移
            await scope.UpgrationAsync();//执行程序升级脚本
            return serviceProvider;
        }
    }
}