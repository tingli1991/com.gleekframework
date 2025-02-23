using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk.Providers;
using Dapper;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Collections.Generic;
using System.Data;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// PgSQL数据库实现类
    /// </summary>
    public class PgSQLDatabaseProvider : BaseDatabaseProvider
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库链接字符串</param>
        public PgSQLDatabaseProvider(string connectionString) : base(connectionString) { }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public override IDbConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public override void InitializeDatabase()
        {
            var databaseName = ConnectionString.ExtractDatabaseName();
            var connectionString = ConnectionString.ClearDatabaseName();
            using var connection = new MySqlConnection(connectionString);
            var query = $@"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT FROM pg_catalog.pg_database WHERE datname = @Name)
                THEN
                    CREATE DATABASE ""{databaseName}"";
                END IF;
            END $$;";
            connection.Execute(query, new { Name = databaseName });
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        public override string GetDatabaseName()
        {
            return ConnectionString.ExtractDatabaseName();
        }

        /// <summary>
        /// 获取数据库索引摘要信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public override IEnumerable<IndexSchemaModel> GetIndexSchemaList(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                return new List<IndexSchemaModel>();
            }

            var sql = @"SELECT tablename AS ""TableName"", indexname AS ""IndexName""
            FROM pg_indexes
            WHERE schemaname = @DatabaseName;";
            using var db = GetConnection();
            return db.Query<IndexSchemaModel>(sql, new { DatabaseName = databaseName });
        }

        /// <summary>
        /// 获取表的摘要信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public override IEnumerable<TableSchemaModel> GetTableSchemaList(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
            {
                return new List<TableSchemaModel>();
            }

            var sql = @"SELECT table_name AS ""TableName"", column_name AS ""ColumnName""
            FROM information_schema.columns
            WHERE table_schema = @DatabaseName;";
            using var db = GetConnection();
            return db.Query<TableSchemaModel>(sql, new { DatabaseName = databaseName });
        }
    }
}