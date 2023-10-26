using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// Dapper基础仓库
    /// </summary>
    public abstract class DapperRepository : IBaseAutofac
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public abstract string ConnectionName { get; }

        /// <summary>
        /// 获取并打开连接字符串对象
        /// </summary>
        /// <returns></returns>
        protected abstract IDbConnection GetConnection();

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        protected string ConnectionString => RepositoryProvider.GetConnectionString(ConnectionName);

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T Open<T>(Func<IDbConnection, T> func)
        {
            using var conn = GetConnection();
            return func(conn);
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected async Task<T> OpenAsync<T>(Func<IDbConnection, Task<T>> func)
        {
            using var conn = GetConnection();
            return await func(conn);
        }

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public T GetFirstOrDefault<T>(string sql, object param = null, int? timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => Open(db => db.QueryFirstOrDefault<T>(sql, param, null, timeoutSeconds));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<T> GetFirstOrDefaultAsync<T>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => await OpenAsync(db => db.QueryFirstOrDefaultAsync<T>(sql, param, null, timeoutSeconds));

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => Open(db => db.Query<T>(sql, param, null, true, timeoutSeconds));

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetListAsync<T>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => await OpenAsync(db => db.QueryAsync<T>(sql, param, null, timeoutSeconds));

        /// <summary>
        /// 数据库操作执行方法（不带事务处理）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public bool Execute(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => Open(db => db.Execute(sql, param, null, timeoutSeconds)) > 0;

        /// <summary>
        /// 数据库操作执行方法（不带事务处理）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> ExecuteAsync(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => await OpenAsync(db => db.ExecuteAsync(sql, param, null, timeoutSeconds)) > 0;

        /// <summary>
        /// 数据库操作执行方法（不带事务处理）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public bool Execute<T>(string sql, IEnumerable<T> paramters, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (paramters == null || !paramters.Any())
            {
                return false;
            }

            using var db = GetConnection();
            var pageList = paramters.ToPageDictionary(pageSize);
            pageList.ForEach(e => db.Execute(sql, e.Value, null, timeoutSeconds));
            return true;
        }

        /// <summary>
        /// 数据库操作执行方法（不带事务处理）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> ExecuteAsync<T>(string sql, IEnumerable<T> paramters, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (paramters == null || !paramters.Any())
            {
                return false;
            }

            using var db = GetConnection();
            var pageList = paramters.ToPageDictionary(pageSize);
            await pageList.ForEachAsync(e => db.ExecuteAsync(sql, e.Value, null, timeoutSeconds));
            return true;
        }

        /// <summary>
        /// 数据库操作执行方法（不带事务处理）
        /// </summary>
        /// <typeparam name="T">指定返回的对象</typeparam>
        /// <param name="sql">sql脚本</param>
        /// <param name="param">sql脚本对应的参数</param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => Open(db => db.ExecuteScalar<T>(sql, param, null, timeoutSeconds));

        /// <summary>
        /// 数据库操作执行方法（不带事务处理）
        /// </summary>
        /// <typeparam name="T">指定返回的对象</typeparam>
        /// <param name="sql">sql脚本</param>
        /// <param name="param">sql脚本对应的参数</param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => await OpenAsync(db => db.ExecuteScalarAsync<T>(sql, param, null, timeoutSeconds));
    }
}