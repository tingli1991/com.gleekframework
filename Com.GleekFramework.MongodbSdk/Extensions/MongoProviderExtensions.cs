using Com.GleekFramework.CommonSdk;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// Mongo实现类拓展
    /// </summary>
    public static class MongoProviderExtensions
    {
        /// <summary>
        /// 获取Mongo类型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client">客户端</param>
        /// <returns></returns>
        public static IMongoCollection<T> GetCollection<T>(this MongodbClient client) where T : IMEntity
        {
            var tableName = GetTableName<T>();
            var dataBase = MongoProvider.GetDatabaseSingle(client);
            return dataBase.GetCollection<T>(tableName);
        }

        /// <summary>
        /// 获取表名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetTableName<T>() where T : IMEntity
        {
            var type = typeof(T);
            var tableName = type.GetAttributeValue((TableAttribute tab) => tab.Name);
            if (string.IsNullOrEmpty(tableName))
            {
                //如果没有标注特性头的情况下使用类名
                tableName = type.Name;
            }
            return tableName;
        }
    }
}