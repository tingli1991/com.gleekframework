using Com.GleekFramework.CommonSdk;
using FluentMigrator;
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
    public static class UpgrationProvider
    {
        /// <summary>
        /// 执行升级脚本
        /// </summary>
        /// <param name="serviceProvider">容器实现类</param>
        /// <returns></returns>
        public static async Task ExecuteAsync(IServiceProvider serviceProvider)
        {
            var serviceList = UpgrationFactory.GetServiceList<Upgration>();
            if (serviceList.IsNullOrEmpty())
            {
                return;
            }

            var databaseProvider = serviceProvider.GetService<IDatabaseProvider>();
            var upgrationAttributeList = serviceList.Select(service =>
            {
                var type = service.GetType();
                service.ServiceProvider = serviceProvider;
                service.DatabaseProvider = databaseProvider;
                var attribute = type.GetCustomAttribute<UpgrationAttribute>() ?? throw new InvalidOperationException(nameof(UpgrationAttribute));
                return new
                {
                    Service = service,
                    attribute.Version,
                    Description = attribute.Description ?? type.Name,
                    TransactionBehavior = attribute?.TransactionBehavior ?? TransactionBehavior.Default
                };
            });

            var upgrateVersionList = new List<VersionInfo>();
            var existsVersionList = databaseProvider.GetExistsVersionList();//已经存在的版本号列表
            foreach (var upgrationAttribute in upgrationAttributeList.OrderBy(e => e.Version))
            {
                if (existsVersionList.IsNotNull() && existsVersionList.Any(e => e == upgrationAttribute.Version))
                {
                    continue;
                }

                await upgrationAttribute.TransactionBehavior.ExecuteAsync(async () =>
                {
                    var executeScripts = await upgrationAttribute.Service.ExecuteScriptsAsync();
                    databaseProvider.ExecuteScripts(executeScripts);//分批执行数据库脚本语句

                    var executeSqlFiles = await upgrationAttribute.Service.ExecuteSqlFilesAsync();
                    if (executeSqlFiles.IsNotEmpty())
                    {
                        var filePaths = executeSqlFiles.Select(e => Path.Combine(AppContext.BaseDirectory, "Scripts", e));
                        var executeSqlFileScripts = filePaths.SelectMany(e => File.ReadAllLines(e));//需要执行的Sql文件存放的所有脚本
                        databaseProvider.ExecuteScripts(executeSqlFileScripts);//分批执行数据库脚本语句
                    }

                    await upgrationAttribute.Service.ExecuteAsync();
                    upgrateVersionList.Add(new VersionInfo()
                    {
                        Version = upgrationAttribute.Version,
                        Description = upgrationAttribute.Description,
                        AppliedOn = DateTime.Now.ToCstTime()
                    });
                });
            }
            databaseProvider.SaveVersion(upgrateVersionList);
        }
    }
}