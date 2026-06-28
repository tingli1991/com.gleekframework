using Com.GleekFramework.CommonSdk;
using FluentMigrator;
using FluentMigrator.Builders.Execute;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本迁移实现类
    /// </summary>
    public static partial class MigrationProvider
    {
        /// <summary>
        /// 程序集列表
        /// </summary>
        public static Assembly[] AssemblyList { get; set; }

        /// <summary>
        /// 版本迁移配置项
        /// </summary>
        public static MigrationOptions MigrationOptions { get; set; }

        /// <summary>
        /// 创建默认的服务集合
        /// </summary>
        /// <param name="options">配置选项</param>
        /// <returns></returns>
        public static IServiceProvider CreateMigrationProvider(MigrationOptions options)
        {
            MigrationOptions = options;
            AssemblyList = GetAssemblyList();
            var serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()//添加FluentMigrator基础服务
                .AddDatabaseProviderSingle(options)//注入数据库实现类
                .AddLogging(e => e.AddFluentMigratorConsole())//启用控制台日志;
                .ConfigureRunner(builder =>
                {
                    builder
                    .AddIMigrationRunner(options.DatabaseType)
                    .WithGlobalConnectionString(options.ConnectionString)
                    .ScanIn(AssemblyList)
                    .For.Migrations();
                });

            return serviceProvider
                .BuildServiceProvider(false)
                .EnsureDatabase();
        }

        /// <summary>
        /// 获取数据库实现类
        /// </summary>
        /// <param name="execute"></param>
        /// <returns></returns>
        public static IDatabaseProvider GetDatabaseProvider(this IExecuteExpressionRoot execute)
        {
            var type = execute.GetType();
            var fieldInfo = type.GetField("_context", BindingFlags.Instance | BindingFlags.NonPublic);
            var migrationContext = (IMigrationContext)fieldInfo.GetValue(execute);//迁移上下文
            return migrationContext.ServiceProvider.GetRequiredService<IDatabaseProvider>();
        }

        /// <summary>
        /// 获取程序集列表
        /// </summary>
        /// <returns></returns>
        private static Assembly[] GetAssemblyList()
        {
            var types = new List<Type>() { typeof(IUpgration), typeof(IMigration) };
            return types.SelectMany(e => AssemblyProvider.GetAssemblyList(e)).ToArray();
        }
    }
}