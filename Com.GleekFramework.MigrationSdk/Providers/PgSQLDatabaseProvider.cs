using Com.GleekFramework.CommonSdk;
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
    public class PgSQLDatabaseProvider : IDatabaseProvider
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        public PgSQLDatabaseProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public void InitializeDatabase()
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
        /// 获取最大的版本号
        /// </summary>
        /// <returns></returns>
        public long GetMaxVersion()
        {
            using var connection = GetConnection();
            var sql = "select max(version) from versioninfo;";
            return connection.ExecuteScalar<long>(sql);
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            return ConnectionString.ExtractDatabaseName();
        }

        /// <summary>
        /// 获取数据库索引摘要信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public IEnumerable<IndexSchemaModel> GetIndexSchemaList(string databaseName)
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
        public IEnumerable<TableSchemaModel> GetTableSchemaList(string databaseName)
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

        /// <summary>
        /// 保存版本信息
        /// </summary>
        /// <param name="versionList">版本列表</param>
        public void SaveVersion(IEnumerable<VersionModel> versionList)
        {
            if (versionList.IsNullOrEmpty())
            {
                return;
            }
            var sql = @"insert into versioninfo(version,appliedon,description) values(@Version,@AppliedOn,@Description);";
            Execute(sql, versionList);
        }

        /// <summary>
        /// 数据库操作执行方法（不带事务处理）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public bool Execute<T>(string sql, IEnumerable<T> paramters, int pageSize = 2000)
        {
            if (paramters.IsNullOrEmpty())
            {
                return false;
            }

            using var db = GetConnection();
            var pageList = paramters.ToPageDictionary(pageSize);
            pageList.ForEach(e => db.Execute(sql, e.Value, null));
            return true;
        }
    }
}