using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk.Providers;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// MySQL数据库实现类
    /// </summary>
    public class MySQLDatabaseProvider : BaseDatabaseProvider
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库链接字符串</param>
        public MySQLDatabaseProvider(string connectionString) : base(connectionString) { }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public override IDbConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public override void InitializeDatabase()
        {
            var databaseName = ConnectionString.ExtractDatabaseName();
            var connectionString = ConnectionString.ClearDatabaseName();
            using var connection = new MySqlConnection(connectionString);
            var query = $@"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8 COLLATE utf8_general_ci;";
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

            var sql = @"select TABLE_NAME as 'TableName',INDEX_NAME as 'IndexName' 
            from information_schema.statistics 
            where TABLE_SCHEMA = @DatabaseName;";
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

            var sql = @"select TABLE_NAME as 'TableName',COLUMN_NAME as 'ColumnName'
            from information_schema.columns
            where TABLE_SCHEMA=@DatabaseName;";
            using var db = GetConnection();
            return db.Query<TableSchemaModel>(sql, new { DatabaseName = databaseName });
        }
    }
}