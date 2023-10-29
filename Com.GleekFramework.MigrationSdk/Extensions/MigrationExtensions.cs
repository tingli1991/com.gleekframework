using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本升级拓展类
    /// </summary>
    public static partial class MigrationExtensions
    {
        /// <summary>
        /// 本本升级
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static async Task MigrateUpAsync(this IServiceScope scope)
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>(); //初始化进程内迁移构建器
            runner.MigrateUp();//执行数据库升级
            await Task.CompletedTask;
        }
    }
}