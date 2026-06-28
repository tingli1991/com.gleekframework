using MongoDB.Driver;
using System.Collections.Generic;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// Mongo数据库实现类
    /// </summary>
    public static class MongoProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 客户字典缓存
        /// </summary>
        private static readonly Dictionary<string, IMongoDatabase> DatabaseCache = new Dictionary<string, IMongoDatabase>();

        /// <summary>
        /// 获取数据库单例对象
        /// </summary>
        /// <returns></returns>
        public static IMongoDatabase GetDatabaseSingle(MongodbClient client)
        {
            if (!DatabaseCache.ContainsKey(client.ConnectionString))
            {
                lock (@lock)
                {
                    if (!DatabaseCache.ContainsKey(client.ConnectionString))
                    {
                        var mongoUrl = new MongoUrl(client.ConnectionString);
                        var dataBase = client.GetDatabase(mongoUrl.DatabaseName);
                        DatabaseCache.Add(client.ConnectionString, dataBase);
                    }
                }
            }
            return DatabaseCache[client.ConnectionString];
        }
    }
}