using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk.Providers;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// SQLite数据库实现类
    /// </summary>
    public class SQLiteDatabaseProvider : MDatabaseProvider
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">数据库链接字符串</param>
        public SQLiteDatabaseProvider(string connectionString) : base(connectionString) { }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public override IDbConnection GetConnection()
        {
            return new SqliteConnection(ConnectionString);
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public override void InitializeDatabase()
        {
            //不需要单独创建，GetConnection的时候会自动创建数据库
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

            var sql = @"select m.name as TableName,i.name as IndexName
            from sqlite_master AS m
            join pragma_index_list(m.name) AS i
            where m.type = 'table' and m.name not like 'sqlite_%' and i.origin != 'pk';";
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

            var sql = @"select m.name as TableName,p.name as ColumnName
            from sqlite_master as m
            join pragma_table_info(m.name) as p
            where m.type = 'table' and m.name not like 'sqlite_%';";
            using var db = GetConnection();
            return db.Query<TableSchemaModel>(sql, new { DatabaseName = databaseName });
        }
    }
}