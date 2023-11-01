using System;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public void InitializeDatabase()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取最大的版本号
        /// </summary>
        /// <returns></returns>
        public long GetMaxVersion()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 保存版本信息
        /// </summary>
        /// <param name="versionList">版本列表</param>
        public void SaveVersion(IEnumerable<VersionModel> versionList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取数据库索引摘要信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public IEnumerable<IndexSchemaModel> GetIndexSchemaList(string databaseName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取表的摘要信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public IEnumerable<TableSchemaModel> GetTableSchemaList(string databaseName)
        {
            throw new NotImplementedException();
        }
    }
}