using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.NLogSdk;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本升级拓展类
    /// </summary>
    public static partial class MigrationExtensions
    {
        /// <summary>
        /// 创建默认的服务集合
        /// </summary>
        /// <param name="options">配置选项</param>
        /// <returns></returns>
        public static IServiceProvider CreateMigrationProvider(this MigrationOptions options)
        {
            return MigrationProvider.CreateMigrationProvider(options);
        }

        /// <summary>
        /// 版本升级
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="options">配置信息</param>
        /// <returns></returns>
        public static async Task MigrationUpAsync(this IServiceScope scope, MigrationOptions options)
        {
            if (scope == null || !options.MigrationSwitch)
            {
                return;
            }
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>(); //初始化进程内迁移构建器
            runner.MigrateUp();//执行数据库升级
            await Task.CompletedTask;
        }

        /// <summary>
        /// 执行数据库初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider EnsureDatabase(this IServiceProvider serviceProvider)
        {
            var databaseProvider = serviceProvider.GetService<IDatabaseProvider>();
            databaseProvider.InitializeDatabase();//初始化数据库
            return serviceProvider;
        }
    }
}