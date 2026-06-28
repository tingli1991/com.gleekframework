using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
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
    public abstract class DapperRepository<T> : IBaseAutofac<T> where T : ITable
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public abstract string ConnectionName { get; }

        /// <summary>
        /// 数据库类型枚举
        /// </summary>
        public abstract DatabaseType DatabaseType { get; }

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
        /// <typeparam name="R"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected R Open<R>(Func<IDbConnection, R> func)
        {
            using var conn = GetConnection();
            return func(conn);
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected async Task<R> OpenAsync<R>(Func<IDbConnection, Task<R>> func)
        {
            using var conn = GetConnection();
            return await func(conn);
        }

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">返回结果</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public R GetFirstOrDefault<R>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => Open(db => db.QueryFirstOrDefault<R>(sql, param, null, timeoutSeconds));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public T GetFirstOrDefault(QueryableBuilder<T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            query.Take(1).Build(DatabaseType);
            using var db = GetConnection();
            return db.QueryFirstOrDefault<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">返回结果</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public R GetFirstOrDefault<R>(QueryableBuilder<T, R> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            query.Take(1).Build(DatabaseType);
            using var db = GetConnection();
            return db.QueryFirstOrDefault<R>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">返回结果</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<R> GetFirstOrDefaultAsync<R>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        => await OpenAsync(db => db.QueryFirstOrDefaultAsync<R>(sql, param, null, timeoutSeconds));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<T> GetFirstOrDefaultAsync(QueryableBuilder<T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            query.Take(1).Build(DatabaseType);
            using var db = GetConnection();
            return await db.QueryFirstOrDefaultAsync<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">实体模型</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<R> GetFirstOrDefaultAsync<R>(QueryableBuilder<T, R> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            query.Take(1).Build(DatabaseType);
            using var db = GetConnection();
            return await db.QueryFirstOrDefaultAsync<R>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="R">返回结果</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public IEnumerable<R> GetList<R>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => Open(db => db.Query<R>(sql, param, null, true, timeoutSeconds));

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public IEnumerable<T> GetList(QueryableBuilder<T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            query.Build(DatabaseType);
            using var db = GetConnection();
            return db.Query<T>(query.ExecuteSQL.ToString(), query.Parameters, null, true, timeoutSeconds);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="R">返回结果</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public IEnumerable<R> GetList<R>(QueryableBuilder<T, R> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            query.Build(DatabaseType);
            using var db = GetConnection();
            return db.Query<R>(query.ExecuteSQL.ToString(), query.Parameters, null, true, timeoutSeconds);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="R">返回结果</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数对象</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<IEnumerable<R>> GetListAsync<R>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => await OpenAsync(db => db.QueryAsync<R>(sql, param, null, timeoutSeconds));

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetListAsync(QueryableBuilder<T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            query.Build(DatabaseType);
            using var db = GetConnection();
            return await db.QueryAsync<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="R">返回结果</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<IEnumerable<R>> GetListAsync<R>(QueryableBuilder<T, R> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            query.Build(DatabaseType);
            using var db = GetConnection();
            return await db.QueryAsync<R>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="query">查询构造器</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public PageDataResult<T> GetPageList(QueryableBuilder<T> query, long pageIndex = 1, long pageSize = 20, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            //编译查询构造器
            query.Page(pageIndex, pageSize).Build(DatabaseType);

            //执行查询
            using var connection = GetConnection();
            var results = connection.Query<T>(query.ExecuteSQL.ToString(), query.Parameters, null, true, timeoutSeconds);
            var totalCount = connection.ExecuteScalar<int>(query.CountSQL.ToString(), query.Parameters, null, timeoutSeconds);
            return new PageDataResult<T>()
            {
                Results = results,
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalCount = totalCount,
            };
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public PageDataResult<R> GetPageList<R>(QueryableBuilder<T, R> query, long pageIndex = 1, long pageSize = 20, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            //编译查询构造器
            query.Page(pageIndex, pageSize).Build(DatabaseType);

            //执行查询
            using var connection = GetConnection();
            var results = connection.Query<R>(query.ExecuteSQL.ToString(), query.Parameters, null, true, timeoutSeconds);
            var totalCount = connection.ExecuteScalar<int>(query.CountSQL.ToString(), query.Parameters, null, timeoutSeconds);
            return new PageDataResult<R>()
            {
                Results = results,
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalCount = totalCount,
            };
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="query">查询构造器</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<PageDataResult<T>> GetPageListAsync(QueryableBuilder<T> query, long pageIndex = 1, long pageSize = 20, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            //编译查询构造器
            query.Page(pageIndex, pageSize).Build(DatabaseType);

            //执行查询
            using var connection = GetConnection();
            var results = await connection.QueryAsync<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
            var totalCount = await connection.ExecuteScalarAsync<int>(query.CountSQL.ToString(), query.Parameters, null, timeoutSeconds);
            return new PageDataResult<T>()
            {
                Results = results,
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalCount = totalCount,
            };
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<PageDataResult<R>> GetPageListAsync<R>(QueryableBuilder<T, R> query, long pageIndex = 1, long pageSize = 20, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            //编译查询构造器
            query.Page(pageIndex, pageSize).Build(DatabaseType);

            //执行查询
            using var connection = GetConnection();
            var results = await connection.QueryAsync<R>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
            var totalCount = await connection.ExecuteScalarAsync<int>(query.CountSQL.ToString(), query.Parameters, null, timeoutSeconds);
            return new PageDataResult<R>()
            {
                Results = results,
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalCount = totalCount,
            };
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public T Insert(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entity == null)
            {
                return default;
            }

            entity.ConversionInterfaceField(true);
            var builder = new SqlBuilder<T>();
            var sql = builder.GenInsertSQL();
            var responseId = Open(db => db.ExecuteScalar<long>($"{sql}{builder.GetIdentitySQL(DatabaseType)}", entity, null, timeoutSeconds));
            if (responseId > 0)
            {
                entity.SetPropertyValue(builder.KeyPropertyInfo.Name, responseId);
                return entity;
            }
            return entity;
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间</param>
        /// <returns></returns>
        public void InsertMany(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            var sql = new SqlBuilder<T>().GenInsertSQL();
            entitys.ForEach(pageSize, (pageIndex, batchList) =>
            {
                batchList = batchList.ConversionInterfaceFields(true);
                using var conn = GetConnection();
                conn.Execute(sql, batchList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<T> InsertAsync(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entity == null)
            {
                return default;
            }

            entity.ConversionInterfaceField(true);
            var builder = new SqlBuilder<T>();
            var sql = builder.GenInsertSQL();
            var responseId = await OpenAsync(db => db.ExecuteScalarAsync<long>($"{sql}{builder.GetIdentitySQL(DatabaseType)}", entity, null, timeoutSeconds));
            if (responseId > 0)
            {
                entity.SetPropertyValue(builder.KeyPropertyInfo.Name, responseId);
                return entity;
            }
            return entity;
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task InsertManyAsync(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            var sql = new SqlBuilder<T>().GenInsertSQL();
            await entitys.ForEachAsync(pageSize, async (pageIndex, batchList) =>
            {
                batchList = batchList.ConversionInterfaceFields(true);
                using var conn = GetConnection();
                await conn.ExecuteAsync(sql, batchList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        public bool Update(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entity == null)
            {
                return false;
            }

            entity.ConversionInterfaceField();
            var sql = new SqlBuilder<T>().GenUpdateSQL();
            return Open(db => db.Execute(sql, entity, null, timeoutSeconds) > 0);
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="updateFieldValue">需要更新的字典</param>
        /// <param name="primaryValue">主键值</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        public bool Update(object updateFieldValue, object primaryValue, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            return Update(updateFieldValue.ToDictionary(), primaryValue, timeoutSeconds);
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="updateFieldValue">需要更新的字典</param>
        /// <param name="primaryValue">主键值</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        public bool Update(Dictionary<string, object> updateFieldValue, object primaryValue, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (updateFieldValue.IsNullOrEmpty())
            {
                return false;
            }

            var (UpdateSQL, UpdateFieldValues) = new SqlBuilder<T>().GenUpdateSQL(updateFieldValue, primaryValue);
            return Open(db => db.Execute(UpdateSQL, UpdateFieldValues, null, timeoutSeconds) > 0);
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="updateFieldValues">需要更新的字典集合</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        public bool UpdateMany(Dictionary<object, object> updateFieldValues, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (updateFieldValues.IsNullOrEmpty())
            {
                return false;
            }
            return UpdateMany(updateFieldValues.ToDictionary(k => k.Key, v => v.Value.ToDictionary()), timeoutSeconds);
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="updateFieldValues">需要更新的字典集合</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        public bool UpdateMany(Dictionary<object, Dictionary<string, object>> updateFieldValues, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (updateFieldValues.IsNullOrEmpty())
            {
                return false;
            }

            var (UpdateSQL, UpdateFieldValues) = new SqlBuilder<T>().GenUpdateSQL(updateFieldValues);
            return Open(db => db.Execute(UpdateSQL, UpdateFieldValues, null, timeoutSeconds) > 0);
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        public void UpdateMany(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            var sql = new SqlBuilder<T>().GenUpdateSQL();
            entitys.ForEach(pageSize, (pageIndex, batchList) =>
            {
                batchList = batchList.ConversionInterfaceFields();
                using var conn = GetConnection();
                conn.Execute(sql, batchList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="entity">更新的模型实例</param>
        /// <param name="timeoutSeconds"></param>
        public async Task<bool> UpdateAsync(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entity == null)
            {
                return false;
            }

            entity.ConversionInterfaceField();
            var sql = new SqlBuilder<T>().GenUpdateSQL();
            return await OpenAsync(db => db.ExecuteAsync(sql, entity, null, timeoutSeconds)) > 0;
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="updateFieldValue">需要更新的字典</param>
        /// <param name="primaryValue">主键值</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        public async Task<bool> UpdateAsync(object updateFieldValue, object primaryValue, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            return await UpdateAsync(updateFieldValue.ToDictionary(), primaryValue, timeoutSeconds);
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="updateFieldValue">需要更新的字典</param>
        /// <param name="primaryValue">主键值</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        public async Task<bool> UpdateAsync(Dictionary<string, object> updateFieldValue, object primaryValue, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (updateFieldValue.IsNullOrEmpty())
            {
                return false;
            }

            var (UpdateSQL, UpdateFieldValues) = new SqlBuilder<T>().GenUpdateSQL(updateFieldValue, primaryValue);
            return await OpenAsync(db => db.ExecuteAsync(UpdateSQL, UpdateFieldValues, null, timeoutSeconds)) > 0;
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="updateFieldValues">需要更新的字典集合</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        public async Task<bool> UpdateManyAsync(Dictionary<object, object> updateFieldValues, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (updateFieldValues.IsNullOrEmpty())
            {
                return false;
            }
            return await UpdateManyAsync(updateFieldValues.ToDictionary(k => k.Key, v => v.Value.ToDictionary()), timeoutSeconds);
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="updateFieldValues">需要更新的字典集合</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        public async Task<bool> UpdateManyAsync(Dictionary<object, Dictionary<string, object>> updateFieldValues, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (updateFieldValues.IsNullOrEmpty())
            {
                return false;
            }

            var (UpdateSQL, UpdateFieldValues) = new SqlBuilder<T>().GenUpdateSQL(updateFieldValues);
            return await OpenAsync(db => db.ExecuteAsync(UpdateSQL, UpdateFieldValues, null, timeoutSeconds)) > 0;
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        public async Task UpdateManyAsync(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            var sql = new SqlBuilder<T>().GenUpdateSQL();
            await entitys.ForEachAsync(pageSize, async (pageIndex, batchList) =>
            {
                batchList = batchList.ConversionInterfaceFields();
                using var conn = GetConnection();
                await conn.ExecuteAsync(sql, batchList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public bool Delete(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entity == null)
            {
                return false;
            }
            var sql = new SqlBuilder<T>().GenDeleteSQL();
            return Open(db => db.Execute(sql, entity, null, timeoutSeconds) > 0);
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public void DeleteMany(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            var sql = new SqlBuilder<T>().GenDeleteSQL();
            entitys.ForEach(pageSize, (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                conn.Execute(sql, pageList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entity == null)
            {
                return false;
            }
            var sql = new SqlBuilder<T>().GenDeleteSQL();
            return await OpenAsync(db => db.ExecuteAsync(sql, entity, null, timeoutSeconds)) > 0;
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task DeleteManyAsync(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            var sql = new SqlBuilder<T>().GenDeleteSQL();
            await entitys.ForEachAsync(pageSize, async (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                await conn.ExecuteAsync(sql, pageList, null, timeoutSeconds);
            });
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
        /// <typeparam name="R">指定返回的对象</typeparam>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public bool Execute<R>(string sql, IEnumerable<R> paramters, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
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
        /// <typeparam name="R">指定返回的对象</typeparam>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> ExecuteAsync<R>(string sql, IEnumerable<R> paramters, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
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
        /// <typeparam name="R">指定返回的对象</typeparam>
        /// <param name="sql">sql脚本</param>
        /// <param name="param">sql脚本对应的参数</param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public R ExecuteScalar<R>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => Open(db => db.ExecuteScalar<R>(sql, param, null, timeoutSeconds));

        /// <summary>
        /// 数据库操作执行方法（不带事务处理）
        /// </summary>
        /// <typeparam name="R">指定返回的对象</typeparam>
        /// <param name="sql">sql脚本</param>
        /// <param name="param">sql脚本对应的参数</param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<R> ExecuteScalarAsync<R>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => await OpenAsync(db => db.ExecuteScalarAsync<R>(sql, param, null, timeoutSeconds));
    }
}