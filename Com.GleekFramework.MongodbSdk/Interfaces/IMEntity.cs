using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// Mongo基础实体
    /// </summary>
    public interface IMEntity
    {
        //[BsonId] 将此属性指定为文档的主键
        //[BsonElement("id")]  属性使用特性进行批注
        //[BsonIgnoreExtraElements] 忽略不匹配的字段
        //[BsonRepresentation(BsonType.ObjectId)] 以允许将参数作为类型 string 而非 ObjectId 结构传递

        /// <summary>
        /// 主键Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        string Id { get; set; }
    }
}