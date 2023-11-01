using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
            host.RegisterApplicationStarted(async () => await CreateServices(options));
            return host;
        }

        /// <summary>
        /// 创建升级服务
        /// </summary>
        /// <param name="options">配置选项</param>
        /// <returns></returns>
        private static async Task<IServiceProvider> CreateServices(MigrationOptions options)
        {
            var serviceProvider = options.CreateMigrationProvider();
            using var scope = serviceProvider.CreateScope();
            await scope.MigrationUpAsync(options);//执行升级脚本迁移
            await scope.UpgrationAsync(options);//执行程序升级脚本
            return serviceProvider;
        }
    }
}