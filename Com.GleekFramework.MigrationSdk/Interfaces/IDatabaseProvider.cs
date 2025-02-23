using System.Collections.Generic;
using System.Data;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本迁移数据库实现接口
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// 连接字符串对象
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        string GetDatabaseName();

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        IDbConnection GetConnection();

        /// <summary>
        /// 初始化数据库
        /// </summary>
        void InitializeDatabase();

        /// <summary>
        /// 获取已存在的版本号集合
        /// </summary>
        /// <returns></returns>
        IEnumerable<long> GetExistsVersionList();

        /// <summary>
        /// 保存版本信息
        /// </summary>
        /// <param name="versionList">版本列表</param>
        void SaveVersion(IEnumerable<VersionInfo> versionList);

        /// <summary>
        /// 执行数据库脚本（不带事务处理）
        /// </summary>
        /// <param name="scripts">数据库脚本集合</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        bool ExecuteScripts(IEnumerable<string> scripts, int pageSize = 2000);

        /// <summary>
        /// 获取数据库索引摘要信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        IEnumerable<IndexSchemaModel> GetIndexSchemaList(string databaseName);

        /// <summary>
        /// 获取表的摘要信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        IEnumerable<TableSchemaModel> GetTableSchemaList(string databaseName);
    }
}