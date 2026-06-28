using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    ///  天气模型
    /// </summary>
    [Serializable, ProtoContract]
    public class WeatherForecastModel
    {
        /// <summary>
        /// 时间
        /// </summary>
        [BsonElement("data")]
        [ProtoMember(1)]
        [JsonProperty("date"), JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 摄氏度
        /// </summary>
        [ProtoMember(2)]
        [BsonElement("temperature_c")]
        [JsonProperty("temperature_c"), JsonPropertyName("temperature_c")]
        public int TemperatureC { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [ProtoMember(3)]
        [BsonElement("summary")]
        [JsonProperty("summary"), JsonPropertyName("summary")]
        public string Summary { get; set; }

        /// <summary>
        /// 华氏度
        /// </summary>
        [BsonElement("temperature_f")]
        [JsonProperty("temperature_f"), JsonPropertyName("temperature_f")]
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}