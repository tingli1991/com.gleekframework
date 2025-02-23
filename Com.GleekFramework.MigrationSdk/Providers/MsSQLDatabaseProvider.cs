using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk.Providers;
using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// MsSQL数据库实现类
    /// </summary>
    public class MsSQLDatabaseProvider : BaseDatabaseProvider
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库链接字符串</param>
        public MsSQLDatabaseProvider(string connectionString) : base(connectionString) { }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public override IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public override void InitializeDatabase()
        {
            var databaseName = ConnectionString.ExtractDatabaseName();
            var connectionString = ConnectionString.ClearDatabaseName();
            using var connection = new MySqlConnection(connectionString);
            var query = $@"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name=@Name)
            CREATE DATABASE {databaseName}";
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

            var sql = @"SELECT t.name AS 'TableName', ind.name AS 'IndexName'
            FROM sys.indexes ind
            INNER JOIN sys.tables t ON ind.object_id = t.object_id
            WHERE t.name = @DatabaseName;";
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

            var sql = @"SELECT TABLE_NAME AS 'TableName', COLUMN_NAME AS 'ColumnName'
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_CATALOG = @DatabaseName;";
            using var db = GetConnection();
            return db.Query<TableSchemaModel>(sql, new { DatabaseName = databaseName });
        }
    }
}