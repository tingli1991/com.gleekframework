namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// Mongo仓储接口
    /// </summary>
    public interface IMongoRepository
    {

    }

    /// <summary>
    /// Mongo仓储接口
    /// </summary>
    public interface IMongoRepository<T> : IMongoRepository where T : class
    {

    }
}