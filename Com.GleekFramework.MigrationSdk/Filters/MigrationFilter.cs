using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.MigrationSdk.Filters
{
    /// <summary>
    /// 版本升级过滤器
    /// </summary>
    public class MigrationFilter : IStartupFilter
    {
        /// <summary>
        /// 迁移配置信息
        /// </summary>
        private readonly MigrationOptions MigrationOptions;

        /// <summary>
        /// 使用 TaskCompletionSource 创建一个阻塞点
        /// </summary>
        private readonly TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

        /// <summary>
        /// 通过构造函数注入 MigrationOptions
        /// </summary>
        /// <param name="migrationOptions">迁移配置信息</param>
        public MigrationFilter(MigrationOptions migrationOptions)
        {
            MigrationOptions = migrationOptions;
        }

        /// <summary>
        /// 执行配置
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                // 在后台运行阻塞逻辑
                Task.Run(async () =>
                {
                    var serviceProvider = MigrationOptions.CreateMigrationProvider();
                    using var scope = serviceProvider.CreateScope();
                    {
                        await scope.MigrationUpAsync(MigrationOptions);//执行升级脚本迁移
                        await scope.UpgrationAsync(MigrationOptions);//执行程序升级脚本
                    }

                    // 释放阻塞，允许应用程序继续启动
                    _tcs.SetResult(true);
                });

                // 阻塞，直到 _tcs 完成（即上面的 Task.Run 完成）
                _tcs.Task.Wait();

                next(app); //调用下一个中间件
            };
        }
    }
}