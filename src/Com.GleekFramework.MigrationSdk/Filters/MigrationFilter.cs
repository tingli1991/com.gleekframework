using Com.GleekFramework.NLogSdk;
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
                    try
                    {
                        var serviceProvider = MigrationOptions.CreateMigrationProvider();
                        using var scope = serviceProvider.CreateScope();
                        {
                            await scope.MigrationUpAsync(MigrationOptions);//执行升级脚本迁移
                            await scope.UpgrationAsync(MigrationOptions);//执行程序升级脚本
                        }
                    }
                    catch (Exception ex)
                    {
                        NLogProvider.Error($"【Migration】迁移执行失败：{ex}");
                    }
                    finally
                    {
                        // 释放阻塞，无论成功或失败都允许应用程序继续启动
                        _tcs.TrySetResult(true);
                    }
                });
                _tcs.Task.Wait();// 阻塞，直到 _tcs 完成（即上面的 Task.Run 完成）
                next(app); //调用下一个中间件
            };
        }
    }
}