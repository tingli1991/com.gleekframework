using Com.GleekFramework.MongodbSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 自定义仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoOnlyRepository<T> : BaseMongoRepository<T> where T : class, IMEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public override string ConnectionName => "MongoConnectionStrings:DefaultClientOnlyHosts";
    }
}