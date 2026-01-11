using MongoDB.Driver;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// 自定义客户端
    /// </summary>
    public class MongodbClient
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName { get; set; }

        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 内部的 MongoClient 实例
        /// </summary>
        private readonly MongoClient _mongoClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MongodbClient() : base()
        {
            _mongoClient = new MongoClient();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">链接字符串</param>
        public MongodbClient(string connectionString)
        {
            ConnectionString = connectionString;
            _mongoClient = new MongoClient(ConnectionString);

            var mongoUrl = new MongoUrl(connectionString);
            DataBaseName = mongoUrl.DatabaseName;
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="settings">数据库设置</param>
        /// <returns></returns>
        public IMongoDatabase GetDatabase(string databaseName = null, MongoDatabaseSettings settings = null)
        {
            var dbName = databaseName ?? DataBaseName;
            return _mongoClient.GetDatabase(dbName, settings);
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T">文档类型</typeparam>
        /// <param name="collectionName">集合名称</param>
        /// <param name="databaseName">数据库名称</param>
        /// <param name="settings">数据库设置</param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string collectionName = null, string databaseName = null, MongoDatabaseSettings settings = null)
        {
            var db = GetDatabase(databaseName, settings);
            var collName = collectionName ?? typeof(T).Name.ToLower();
            return db.GetCollection<T>(collName);
        }
    }
}