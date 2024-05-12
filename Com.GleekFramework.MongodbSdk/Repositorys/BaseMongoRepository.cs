using Com.GleekFramework.CommonSdk;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// 基础仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseMongoRepository<T> : IMongoRepository<T>
        where T : class, IMEntity
    {
        /// <summary>
        /// 单模型映射方法
        /// </summary>
        /// <typeparam name="R">目标数据类型</typeparam>
        /// <param name="source">原数据实例</param>
        /// <returns></returns>
        protected R Map<R>(T source)
            where R : class => source.Map<T, R>();

        /// <summary>
        /// 单模型映射方法
        /// </summary>
        /// <typeparam name="R">目标数据类型</typeparam>
        /// <param name="sourceList">原数据实例</param>
        /// <returns></returns>
        protected IEnumerable<R> Map<R>(IEnumerable<T> sourceList)
            where R : class => sourceList.Map<T, R>();

        /// <summary>
        /// 单模型映射方法
        /// </summary>
        /// <typeparam name="R">目标数据类型</typeparam>
        /// <param name="source">原数据实例</param>
        /// <returns></returns>
        protected async Task<R> MapAsync<R>(Task<T> source)
            where R : class => (await source).Map<T, R>();

        /// <summary>
        /// 单模型映射方法
        /// </summary>
        /// <typeparam name="R">目标数据类型</typeparam>
        /// <param name="sourceList">原数据实例</param>
        /// <returns></returns>
        protected async Task<IEnumerable<R>> MapAsync<R>(Task<IEnumerable<T>> sourceList)
            where R : class => (await sourceList).Map<T, R>();
        
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public abstract string ConnectionName { get; }

        /// <summary>
        /// Mongo的类型集合
        /// </summary>
        protected IMongoCollection<T> Collection => Client.GetCollection<T>();

        /// <summary>
        /// 客户端
        /// </summary>
        protected MongodbClient Client => MongoClientProvider.GetClientSingle(ConnectionName);

        /// <summary>
        /// 获取文档的数量
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public long Count(Expression<Func<T, bool>> filter)
            => Collection.CountDocuments(filter);

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        public IFindFluent<T, T> Where(Expression<Func<T, bool>> filter)
            => Collection.Find(filter);

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="docment">文档对象</param>
        public void InsertOne(T docment)
            => Collection.InsertOne(docment);

        /// <summary>
        /// 插入单条数据
        /// </summary>
        /// <param name="docment">文档对象</param>
        public async Task InsertOneAsync(T docment)
            => await Collection.InsertOneAsync(docment);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="docments">文档对象列表</param>
        /// <param name="isOrdered">是否有序的插入(默认：false)</param>
        public void InsertMany(IEnumerable<T> docments, bool isOrdered = false)
            => Collection.InsertMany(docments, MongoConstant.GetInsertManyOptions(isOrdered));

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="docments">文档对象列表</param>
        /// <param name="isOrdered">是否有序的插入(默认：false)</param>
        public async Task InsertManyAsync(IEnumerable<T> docments, bool isOrdered = false)
            => await Collection.InsertManyAsync(docments, MongoConstant.GetInsertManyOptions(isOrdered));

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="update">更新对象</param>
        /// <returns></returns>
        public void UpdateOne(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
            => Collection.UpdateOne(filter, update);

        /// <summary>
        /// 更新单条数据
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="update">更新对象</param>
        /// <returns></returns>
        public async Task UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
            => await Collection.UpdateOneAsync(filter, update);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="update">更新对象</param>
        public void UpdateMany(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
            => Collection.UpdateMany(filter, update);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="update">更新对象</param>
        public async Task UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
            => await Collection.UpdateManyAsync(filter, update);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="filter">过滤条件</param>
        public void DeleteMany(Expression<Func<T, bool>> filter)
            => Collection.DeleteMany(filter);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="filter">过滤条件</param>
        public async Task DeleteManyAsync(Expression<Func<T, bool>> filter)
            => await Collection.DeleteManyAsync(filter);

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
            => Collection.Find(filter).Limit(1).SingleOrDefault();

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter)
            => await Collection.Find(filter).Limit(1).SingleOrDefaultAsync();

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">返回模型</typeparam>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public R GetFirstOrDefault<R>(Expression<Func<T, bool>> filter)
            where R : class => Map<R>(GetFirstOrDefault(filter));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">返回模型</typeparam>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public async Task<R> GetFirstOrDefaultAsync<R>(Expression<Func<T, bool>> filter)
            where R : class => await MapAsync<R>(GetFirstOrDefaultAsync(filter));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public IEnumerable<T> GetList(Expression<Func<T, bool>> filter)
            => Collection.Find(filter).ToList();

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public IEnumerable<T> GetList(Expression<Func<T, bool>> filter, int pageSize)
            => Collection.Find(filter).Limit(pageSize).ToList();

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter)
            => await Collection.Find(filter).ToListAsync();

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> filter, int pageSize)
            => await Collection.Find(filter).Limit(pageSize).ToListAsync();

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">返回模型</typeparam>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public IEnumerable<R> GetList<R>(Expression<Func<T, bool>> filter)
            where R : class => Map<R>(GetList(filter));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">返回模型</typeparam>
        /// <param name="filter">过滤条件</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public IEnumerable<R> GetList<R>(Expression<Func<T, bool>> filter, int pageSize)
            where R : class => Map<R>(GetList(filter, pageSize));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">返回模型</typeparam>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public async Task<IEnumerable<R>> GetListAsync<R>(Expression<Func<T, bool>> filter)
            where R : class => await MapAsync<R>(GetListAsync(filter));

        /// <summary>
        /// 获取单条记录信息
        /// </summary>
        /// <typeparam name="R">返回模型</typeparam>
        /// <param name="filter">过滤条件</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public Task<IEnumerable<R>> GetListAsync<R>(Expression<Func<T, bool>> filter, int pageSize)
            where R : class => MapAsync<R>(GetListAsync(filter, pageSize));
    }
}