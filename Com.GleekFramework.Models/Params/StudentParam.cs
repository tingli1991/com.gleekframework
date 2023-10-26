using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 学员信息
    /// </summary>
    public class StudentParam
    {
        /// <summary>
        /// 学员Id
        /// </summary>
        [JsonProperty("id"), JsonPropertyName("id")]
        public int Id { get; set; }

        /// <summary>
        /// 学员名称
        /// </summary>
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }
    }
}