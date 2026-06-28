using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 集合数据缓存模型
    /// </summary>
    [Serializable, ProtoContract]
    public partial class CacheModel<T> where T : class
    {
        /// <summary>
        /// 是否有值
        /// </summary>
        [ProtoMember(1)]
        [JsonProperty("has_value"), JsonPropertyName("has_value")]
        public bool HasValue { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        [ProtoMember(2)]
        [JsonProperty("data"), JsonPropertyName("data")]
        public T Data { get; set; }
    }
}