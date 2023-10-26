using MongoDB.Driver;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// 自定义客户端
    /// </summary>
    public class MongodbClient : MongoClient
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
        /// 构造函数
        /// </summary>
        public MongodbClient() : base()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">链接字符串</param>
        public MongodbClient(string connectionString) : base(connectionString)
        {
            var mongoUrl = new MongoUrl(connectionString);
            DataBaseName = mongoUrl.DatabaseName;
            ConnectionString = connectionString;
        }
    }
}