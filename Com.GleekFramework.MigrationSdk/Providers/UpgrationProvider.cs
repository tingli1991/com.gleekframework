using Com.GleekFramework.CommonSdk;
using FluentMigrator;
using FluentMigrator.Builders.Execute;
using FluentMigrator.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 升级实现类
    /// </summary>
    public static partial class UpgrationProvider
    {
        /// <summary>
        /// 执行升级脚本
        /// </summary>
        /// <param name="serviceProvider">容器实现类</param>
        /// <returns></returns>
        public static async Task ExecuteAsync(IServiceProvider serviceProvider)
        {
            var serviceList = MigrationFactory.GetServiceList<Upgration>();
            if (serviceList == null || !serviceList.Any())
            {
                return;
            }

            var migrationContext = serviceProvider.GetService<IMigrationContext>();
            var migrationExecute = new ExecuteExpressionRoot(migrationContext);
            var versionServiceList = serviceList.Select(service =>
            {
                var attribute = service.GetCustomAttribute<UpgrationAttribute>();
                if (attribute == null)
                {
                    throw new InvalidOperationException(nameof(UpgrationAttribute));
                }

                service.Context = migrationContext;
                service.Execute = migrationExecute;
                return new
                {
                    Service = service,
                    attribute.Version,
                    Description = attribute.Description ?? service.GetType().Name,
                    TransactionBehavior = attribute?.TransactionBehavior ?? TransactionBehavior.Default
                };
            });

            var maxVersion = migrationContext.GetMaxVersion();
            var upgrateVersionList = new List<VersionModel>();
            foreach (var upgrate in versionServiceList.OrderBy(e => e.Version))
            {
                if (upgrate.Version <= maxVersion)
                {
                    continue;
                }

                await upgrate.TransactionBehavior.ExecuteAsync(async () =>
                {
                    //分批执行数据库脚本语句
                    var executeScripts = await upgrate.Service.ExecuteScriptsAsync();
                    if (executeScripts != null && executeScripts.Any())
                    {
                        var pageScripts = executeScripts.ToPageDictionary();
                        pageScripts.Values.ForEach(scriptList =>
                        {
                            var executeSql = string.Join(';', scriptList = scriptList.Select(e => e.TrimEnd(';')));
                            upgrate.Service.Execute.Sql(executeSql);//执行数据库脚本
                        });
                    }

                    var executeSqlFiles = await upgrate.Service.ExecuteSqlFilesAsync();
                    if (executeSqlFiles != null && executeSqlFiles.Any())
                    {
                        var filePaths = executeSqlFiles.Select(e => Path.Combine(AppContext.BaseDirectory, "Scripts", e)).Where(e => File.Exists(e));
                        filePaths.ForEach(path => upgrate.Service.Execute.Script(path));//运行SQL文件
                    }

                    await upgrate.Service.ExecuteAsync();
                    upgrateVersionList.Add(new VersionModel()
                    {
                        Version = upgrate.Version,
                        Description = upgrate.Description,
                        AppliedOn = DateTime.Now.ToCstTime()
                    });
                });
            }
            await migrationContext.SaveVersion(upgrateVersionList);
        }
    }
}