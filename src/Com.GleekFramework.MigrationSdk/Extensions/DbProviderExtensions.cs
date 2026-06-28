using Com.GleekFramework.ContractSdk;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 数据库实现拓展
    /// </summary>
    public static partial class DbProviderExtensions
    {
        /// <summary>
        /// 注入迁移运行时的数据库类型
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public static IMigrationRunnerBuilder AddIMigrationRunner(this IMigrationRunnerBuilder builder, DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.MsSQL:
                    builder.AddSqlServer();
                    break;
                case DatabaseType.MySQL:
                    builder.AddMySql5();
                    break;
                case DatabaseType.PgSQL:
                    builder.AddPostgres();
                    break;
            }
            return builder;
        }

        /// <summary>
        /// 添加数据库实现注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseProviderSingle(this IServiceCollection services, MigrationOptions options)
        {
            switch (options.DatabaseType)
            {
                case DatabaseType.MsSQL:
                    services.AddSingleton<IDatabaseProvider>(provider => new MsSQLDatabaseProvider(options.ConnectionString));
                    break;
                case DatabaseType.MySQL:
                    services.AddSingleton<IDatabaseProvider>(provider => new MySQLDatabaseProvider(options.ConnectionString));
                    break;
                case DatabaseType.PgSQL:
                    services.AddSingleton<IDatabaseProvider>(provider => new PgSQLDatabaseProvider(options.ConnectionString));
                    break;
                case DatabaseType.SQLite:
                    services.AddSingleton<IDatabaseProvider>(provider => new SQLiteDatabaseProvider(options.ConnectionString));
                    break;
            }
            return services;
        }
    }
}