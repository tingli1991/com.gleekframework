using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Dapper;
using DapperExtensions;
using DapperExtensions.Predicate;
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
        /// 流水号生成服务
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

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
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public T GetFirstOrDefault<T>(dynamic primaryId, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        => Open(db => DapperExtensions.DapperExtensions.Get<T>(db, primaryId, null, timeoutSeconds));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public T GetFirstOrDefault<T>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
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
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<T> GetFirstOrDefaultAsync<T>(dynamic primaryId, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
            => await OpenAsync(async db => await Task.FromResult<T>(DapperExtensions.DapperExtensions.Get<T>(db, primaryId, null, timeoutSeconds)));

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
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="param">参数对象</param>
        /// <param name="sortList">排序字段</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public PageDataResult<T> GetPageList<T>(int pageIndex = 1, int pageSize = 20, object param = null, List<Sort> sortList = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            var conn = GetConnection();
            return new PageDataResult<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = conn.Count<T>(param, null, timeoutSeconds),
                PageList = conn.GetPage<T>(param, (IList<ISort>)sortList, pageIndex, pageSize, null, timeoutSeconds)
            };
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="param">参数对象</param>
        /// <param name="sortList">排序字段</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<PageDataResult<T>> GetPageListAsync<T>(int pageIndex = 1, int pageSize = 20, object param = null, List<Sort> sortList = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            var conn = GetConnection();
            return new PageDataResult<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = await conn.CountAsync<T>(param, null, timeoutSeconds),
                PageList = await conn.GetPageAsync<T>(param, (IList<ISort>)sortList, pageIndex, pageSize, null, timeoutSeconds)
            };
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public dynamic InsertOne<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return null;
            }

            if (entity is IVersionTable versionInfo)
            {
                //赋值版本号
                versionInfo.Version = SnowflakeService.GetVersionNo();
            }
            return Open(db => db.Insert(entity, null, timeoutSeconds));
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间</param>
        /// <returns></returns>
        public void InsertMany<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            if (entitys is IEnumerable<IVersionTable> versionInfoList)
            {
                foreach (var versionInfo in versionInfoList)
                {
                    //赋值版本号
                    versionInfo.Version = SnowflakeService.GetVersionNo();
                }
            }

            entitys.ForEach(pageSize, (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                conn.Insert(pageList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<dynamic> InsertOneAsync<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return null;
            }

            if (entity is IVersionTable versionInfo)
            {
                //赋值版本号
                versionInfo.Version = SnowflakeService.GetVersionNo();
            }

            return await OpenAsync(db => db.InsertAsync(entity, null, timeoutSeconds));
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task InsertManyAsync<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            if (entitys is IEnumerable<IVersionTable> versionInfoList)
            {
                foreach (var versionInfo in versionInfoList)
                {
                    //赋值版本号
                    versionInfo.Version = SnowflakeService.GetVersionNo();
                }
            }

            await entitys.ForEachAsync(pageSize, async (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                await conn.InsertAsync(pageList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        public bool UpdateOne<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return false;
            }

            if (entity is IVersionTable versionInfo)
            {
                //赋值版本号
                versionInfo.Version = SnowflakeService.GetVersionNo();
            }

            return Open(db => db.Update(entity, null, timeoutSeconds));
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        public void UpdateMany<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            if (entitys is IEnumerable<IVersionTable> versionInfoList)
            {
                foreach (var versionInfo in versionInfoList)
                {
                    //赋值版本号
                    versionInfo.Version = SnowflakeService.GetVersionNo();
                }
            }

            entitys.ForEach(pageSize, (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                conn.Update(pageList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        public async Task<bool> UpdateOneAsync<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return false;
            }

            if (entity is IVersionTable versionInfo)
            {
                //赋值版本号
                versionInfo.Version = SnowflakeService.GetVersionNo();
            }

            return await OpenAsync(db => db.UpdateAsync(entity, null, timeoutSeconds));
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        public async Task UpdateManyAsync<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            if (entitys is IEnumerable<IVersionTable> versionInfoList)
            {
                foreach (var versionInfo in versionInfoList)
                {
                    //赋值版本号
                    versionInfo.Version = SnowflakeService.GetVersionNo();
                }
            }

            await entitys.ForEachAsync(pageSize, async (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                await conn.UpdateAsync(pageList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public bool DeleteOne<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return false;
            }
            return Open(db => db.Delete(entity, null, timeoutSeconds));
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public bool DeleteOne(object param, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (param == null)
            {
                return false;
            }
            return Open(db => db.Delete<object>(predicate: param, transaction: null, commandTimeout: timeoutSeconds));
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public void DeleteMany<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            entitys.ForEach(pageSize, (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                conn.Delete(entitys, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">主键</param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public bool DeleteById<T>(long primaryKey, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : ITable
        {
            if (primaryKey <= 0)
            {
                return false;
            }

            var type = typeof(T);
            var tableName = type.GetTableName<T>();//获取表名称
            var primaryColumnName = type.GetPrimaryColumnName<T>();//主键字段名称

            // 数据库脚本
            var sql = @$"delete from {tableName} where {primaryColumnName}=@PrimaryKey";
            return Open(db => db.Execute(sql, new { PrimaryKey = primaryKey }, null, timeoutSeconds) > 0);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKeys">主键Id集合</param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public void DeleteByIds<T>(IEnumerable<long> primaryKeys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : ITable
        {
            if (primaryKeys.IsNullOrEmpty())
            {
                return;
            }

            var type = typeof(T);
            var tableName = type.GetTableName<T>();//获取表名称
            var primaryColumnName = type.GetPrimaryColumnName<T>();//主键字段名称
            var sql = @$"delete from {tableName} where {primaryColumnName} in @PrimaryKeys";
            primaryKeys.ForEach(pageSize, (pageIndex, pageList) => Open(db => db.ExecuteAsync(sql, new { PrimaryKeys = pageList }, null, timeoutSeconds)));
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOneAsync<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return false;
            }
            return await OpenAsync(db => db.DeleteAsync(entity, null, timeoutSeconds));
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="param"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<bool> DeleteOneAsync(object param, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (param == null)
            {
                return false;
            }
            return await OpenAsync(db => db.DeleteAsync<object>(predicate: param, transaction: null, commandTimeout: timeoutSeconds));
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task DeleteManyAsync<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            await entitys.ForEachAsync(pageSize, async (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                await conn.DeleteAsync(entitys, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKey">主键</param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync<T>(long primaryKey, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : ITable
        {
            if (primaryKey <= 0)
            {
                return false;
            }

            var type = typeof(T);
            var tableName = type.GetTableName<T>();//获取表名称
            var primaryColumnName = type.GetPrimaryColumnName<T>();//主键字段名称

            // 数据库脚本
            var sql = @$"delete from {tableName} where {primaryColumnName}=@PrimaryKey";
            return await OpenAsync(async db => await db.ExecuteAsync(sql, new { PrimaryKey = primaryKey }, null, timeoutSeconds) > 0);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryKeys">主键Id集合</param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task DeleteByIdsAsync<T>(IEnumerable<long> primaryKeys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : ITable
        {
            if (primaryKeys.IsNullOrEmpty())
            {
                return;
            }

            var type = typeof(T);
            var tableName = type.GetTableName<T>();//获取表名称
            var primaryColumnName = type.GetPrimaryColumnName<T>();//主键字段名称
            var sql = @$"delete from {tableName} where {primaryColumnName} in @PrimaryKeys";
            await primaryKeys.ForEachAsync(pageSize, async (pageIndex, pageList) => await OpenAsync(db => db.ExecuteAsync(sql, new { PrimaryKeys = pageList }, null, timeoutSeconds)));
        }

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
            if (paramters.IsNullOrEmpty())
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
            if (paramters.IsNullOrEmpty())
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