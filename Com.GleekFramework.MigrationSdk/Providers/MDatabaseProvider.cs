using Com.GleekFramework.CommonSdk;
using Dapper;
using System.Collections.Generic;
using System.Data;

namespace Com.GleekFramework.MigrationSdk.Providers
{
    /// <summary>
    /// 基础数据库实现类
    /// </summary>
    public abstract class MDatabaseProvider : IDatabaseProvider
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        public MDatabaseProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        public abstract string GetDatabaseName();

        /// <summary>
        /// 初始化数据库
        /// </summary>
        public abstract void InitializeDatabase();

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public abstract IDbConnection GetConnection();

        /// <summary>
        /// 获取数据库索引摘要信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public abstract IEnumerable<IndexSchemaModel> GetIndexSchemaList(string databaseName);

        /// <summary>
        /// 获取表的摘要信息
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public abstract IEnumerable<TableSchemaModel> GetTableSchemaList(string databaseName);

        /// <summary>
        /// 获取已存在的版本号集合
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<long> GetExistsVersionList()
        {
            using var connection = GetConnection();
            var sql = "select distinct version from versioninfo where Description<>'SchemaMigration';";
            return connection.Query<long>(sql);
        }

        /// <summary>
        /// 保存版本信息
        /// </summary>
        /// <param name="versionList">版本信息列表</param>
        public virtual void SaveVersion(IEnumerable<VersionInfo> versionList)
        {
            if (versionList.IsNullOrEmpty())
            {
                return;
            }
            Execute(@"insert into versioninfo(version,appliedon,description) values(@Version,@AppliedOn,@Description);", versionList);
        }

        /// <summary>
        /// 执行数据库脚本（不带事务处理）
        /// </summary>
        /// <param name="scripts">数据库脚本集合</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public virtual bool ExecuteScripts(IEnumerable<string> scripts, int pageSize = 2000)
        {
            if (scripts.IsNullOrEmpty())
            {
                return false;
            }

            using var db = GetConnection();
            var pageScripts = scripts.ToPageDictionary(pageSize);
            foreach (var scriptDic in pageScripts)
            {
                scriptDic.Value.ForEach(script => db.Execute(script, null, null));
            }
            return true;
        }

        /// <summary>
        /// 数据库操作执行方法（不带事务处理）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        protected virtual bool Execute<T>(string sql, IEnumerable<T> paramters, int pageSize = 2000)
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