using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
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
        public T GetFirstOrDefault<T>(string sql, object param = null, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
            => Open(db => db.QueryFirstOrDefault<T>(sql, param, null, timeoutSeconds));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public T GetFirstOrDefault<T>(QueryableBuilder<T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            query.Take(1).Build(DatabaseType);
            using var db = GetConnection();
            return db.QueryFirstOrDefault<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public T GetFirstOrDefault<E, T>(QueryableBuilder<E, T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            query.Take(1).Build(DatabaseType);
            using var db = GetConnection();
            return db.QueryFirstOrDefault<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

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
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<T> GetFirstOrDefaultAsync<T>(QueryableBuilder<T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            query.Take(1).Build(DatabaseType);
            using var db = GetConnection();
            return await db.QueryFirstOrDefaultAsync<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="E">实体模型</typeparam>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<T> GetFirstOrDefaultAsync<E, T>(QueryableBuilder<E, T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            query.Take(1).Build(DatabaseType);
            using var db = GetConnection();
            return await db.QueryFirstOrDefaultAsync<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

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
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<T>(QueryableBuilder<T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            query.Build(DatabaseType);
            using var db = GetConnection();
            return db.Query<T>(query.ExecuteSQL.ToString(), query.Parameters, null, true, timeoutSeconds);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="E">实体模型</typeparam>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public IEnumerable<T> GetList<E, T>(QueryableBuilder<E, T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            query.Build(DatabaseType);
            using var db = GetConnection();
            return db.Query<T>(query.ExecuteSQL.ToString(), query.Parameters, null, true, timeoutSeconds);
        }

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
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetListAsync<T>(QueryableBuilder<T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            query.Build(DatabaseType);
            using var db = GetConnection();
            return await db.QueryAsync<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="E">实体模型</typeparam>
        /// <typeparam name="T">返回结果</typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetListAsync<E, T>(QueryableBuilder<E, T> query, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            query.Build(DatabaseType);
            using var db = GetConnection();
            return await db.QueryAsync<T>(query.ExecuteSQL.ToString(), query.Parameters, null, timeoutSeconds);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="query">查询构造器</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public PageDataResult<T> GetPageList<T>(QueryableBuilder<T> query, long pageIndex = 1, long pageSize = 20, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
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
        /// <typeparam name="E"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public PageDataResult<T> GetPageList<E, T>(QueryableBuilder<E, T> query, long pageIndex = 1, long pageSize = 20, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
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
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<PageDataResult<T>> GetPageListAsync<T>(QueryableBuilder<T> query, long pageIndex = 1, long pageSize = 20, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
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
        /// <typeparam name="E"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">查询构造器</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<PageDataResult<T>> GetPageListAsync<E, T>(QueryableBuilder<E, T> query, long pageIndex = 1, long pageSize = 20, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS)
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
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public long Insert<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return 0L;
            }

            if (entity is IVersionTable versionInfo && versionInfo.Version <= 0)
            {
                //赋值版本号
                versionInfo.Version = SnowflakeService.GetVersionNo();
            }
            var builder = new SqlBuilder<T>();
            var sql = builder.GenInsertSQL();
            return Open(db => db.ExecuteScalar<long>($"{sql}{builder.GetIdentitySQL(DatabaseType)}", entity, null, timeoutSeconds));
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="timeoutSeconds">超时时间</param>
        /// <returns></returns>
        public void Insert<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            if (entitys is IEnumerable<IVersionTable> versionInfoList)
            {
                foreach (var versionInfo in versionInfoList)
                {
                    if (versionInfo.Version <= 0)
                    {
                        continue;
                    }

                    //赋值版本号
                    versionInfo.Version = SnowflakeService.GetVersionNo();
                }
            }

            var sql = new SqlBuilder<T>().GenInsertSQL();
            entitys.ForEach(pageSize, (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                conn.Execute(sql, pageList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<long> InsertAsync<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return 0L;
            }

            if (entity is IVersionTable versionInfo && versionInfo.Version <= 0)
            {
                //赋值版本号
                versionInfo.Version = SnowflakeService.GetVersionNo();
            }

            var builder = new SqlBuilder<T>();
            var sql = builder.GenInsertSQL();
            return await OpenAsync(db => db.ExecuteScalarAsync<long>($"{sql}{builder.GetIdentitySQL(DatabaseType)}", entity, null, timeoutSeconds));
        }

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task InsertAsync<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            if (entitys is IEnumerable<IVersionTable> versionInfoList)
            {
                foreach (var versionInfo in versionInfoList)
                {
                    if (versionInfo.Version <= 0)
                    {
                        continue;
                    }

                    //赋值版本号
                    versionInfo.Version = SnowflakeService.GetVersionNo();
                }
            }

            var sql = new SqlBuilder<T>().GenInsertSQL();
            await entitys.ForEachAsync(pageSize, async (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                await conn.ExecuteAsync(sql, pageList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        public bool Update<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return false;
            }

            if (entity is IVersionTable versionInfo && versionInfo.Version <= 0)
            {
                //赋值版本号
                versionInfo.Version = SnowflakeService.GetVersionNo();
            }

            var sql = new SqlBuilder<T>().GenUpdateSQL();
            return Open(db => db.Execute(sql, entity, null, timeoutSeconds) > 0);
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        public void Update<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            if (entitys is IEnumerable<IVersionTable> versionInfoList)
            {
                foreach (var versionInfo in versionInfoList)
                {
                    if (versionInfo.Version <= 0)
                    {
                        continue;
                    }

                    //赋值版本号
                    versionInfo.Version = SnowflakeService.GetVersionNo();
                }
            }

            var sql = new SqlBuilder<T>().GenUpdateSQL();
            entitys.ForEach(pageSize, (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                conn.Execute(sql, pageList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        public async Task<bool> UpdateAsync<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entity == null)
            {
                return false;
            }

            if (entity is IVersionTable versionInfo && versionInfo.Version <= 0)
            {
                //赋值版本号
                versionInfo.Version = SnowflakeService.GetVersionNo();
            }

            var sql = new SqlBuilder<T>().GenUpdateSQL();
            return await OpenAsync(db => db.ExecuteAsync(sql, entity, null, timeoutSeconds)) > 0;
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="pageSize"></param>
        /// <param name="timeoutSeconds"></param>
        public async Task UpdateAsync<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
        {
            if (entitys.IsNullOrEmpty())
            {
                return;
            }

            if (entitys is IEnumerable<IVersionTable> versionInfoList)
            {
                foreach (var versionInfo in versionInfoList)
                {
                    if (versionInfo.Version <= 0)
                    {
                        continue;
                    }

                    //赋值版本号
                    versionInfo.Version = SnowflakeService.GetVersionNo();
                }
            }

            var sql = new SqlBuilder<T>().GenUpdateSQL();
            await entitys.ForEachAsync(pageSize, async (pageIndex, pageList) =>
            {
                using var conn = GetConnection();
                await conn.ExecuteAsync(sql, pageList, null, timeoutSeconds);
            });
        }

        /// <summary>
        /// 删除单条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public bool Delete<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
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
        public void Delete<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
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
        public async Task<bool> DeleteAsync<T>(T entity, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
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
        public async Task DeleteAsync<T>(IEnumerable<T> entitys, int pageSize = 2000, int timeoutSeconds = DapperConstant.DEFAULT_TIMEOUT_SECONDS) where T : class
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