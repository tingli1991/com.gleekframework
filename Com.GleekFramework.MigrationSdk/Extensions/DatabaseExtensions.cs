using Com.GleekFramework.CommonSdk;
using Dapper;
using FluentMigrator.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 数据库拓展
    /// </summary>
    public static partial class DatabaseExtensions
    {
        /// <summary>
        /// 执行数据库初始化
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static IServiceCollection EnsureDatabase(this IServiceCollection services, string connectionString)
        {
            connectionString = connectionString.ClearDatabaseName();
            var databaseName = connectionString.ExtractDatabaseName();
            using var connection = new MySqlConnection(connectionString);
            var sql = @"select schema_name from information_schema.schemata where schema_name=@Name limit 1;";
            var existsDatabaseName = connection.ExecuteScalar<string>(sql, new { Name = databaseName });
            if (string.IsNullOrEmpty(existsDatabaseName))
            {
                connection.Execute($"create database {databaseName} character set utf8 collate utf8_general_ci;");
            }
            return services;
        }

        /// <summary>
        /// 获取最大的版本号
        /// </summary>
        /// <param name="migrationContext"></param>
        /// <returns></returns>
        public static long GetMaxVersion(this IMigrationContext migrationContext)
        {
            var sql = "select max(version) from versioninfo;";
            using var connection = new MySqlConnection(migrationContext.Connection);
            return connection.ExecuteScalar<long>(sql);
        }

        /// <summary>
        /// 保存版本信息
        /// </summary>
        /// <param name="migrationContext"></param>
        /// <param name="versionList">版本列表</param>
        public static async Task SaveVersion(this IMigrationContext migrationContext, IEnumerable<VersionModel> versionList)
        {
            if (versionList == null || !versionList.Any())
            {
                return;
            }
            using var connection = new MySqlConnection(migrationContext.Connection);
            var sql = @"insert into versioninfo(version,appliedon,description) values(@Version,@AppliedOn,@Description);";
            connection.Execute(sql, versionList);
            await Task.CompletedTask;
        }
    }
}