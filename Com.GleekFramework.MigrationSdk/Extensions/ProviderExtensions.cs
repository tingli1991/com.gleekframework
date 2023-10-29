using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 服务实现类
    /// </summary>
    public static partial class ProviderExtensions
    {
        /// <summary>
        /// 创建默认的服务集合
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static IServiceProvider CreateMigrationProvider(this IEnumerable<Assembly> assemblies, string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()//添加FluentMigrator基础服务
                .EnsureDatabase(connectionString)
                .ConfigureRunner(builder =>
                {
                    builder
                    .AddMySql5()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(assemblies.ToArray())
                    .For.Migrations();
                })
              .AddLogging(lb => lb.AddFluentMigratorConsole())//启用控制台日志
              .BuildServiceProvider(false);//构建服务提供器
        }
    }
}