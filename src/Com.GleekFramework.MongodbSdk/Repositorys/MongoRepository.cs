namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// Mongo仓储类
    /// </summary>
    public class MongoRepository<T> : BaseMongoRepository<T>
        where T : class, IMEntity
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        public override string ConnectionName => MongoConstant.DEFAULT_CONNECTION_NAME;
    }
}