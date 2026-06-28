using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// Mongo基础实体类
    /// </summary>
    public class MEntity : IMEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = $"{Guid.NewGuid()}";
    }
}