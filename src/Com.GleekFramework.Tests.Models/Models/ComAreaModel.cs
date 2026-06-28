using Com.GleekFramework.ContractSdk;
using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 区域模型
    /// </summary>
    [Serializable, ProtoContract]
    public class ComAreaModel : IVersion
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonProperty("id"), JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [JsonProperty("code"), JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 地区级别
        /// </summary>
        [JsonProperty("level"), JsonPropertyName("level")]
        public AreaLevel Level { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [JsonProperty("lng"), JsonPropertyName("lng")]
        public string Lng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [JsonProperty("lat"), JsonPropertyName("lat")]
        public string Lat { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        [JsonProperty("parent_id"), JsonPropertyName("parent_id")]
        public long ParentId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonProperty("version"), JsonPropertyName("version")]
        public decimal Version { get; set; }

        /// <summary>
        /// 死否删除
        /// </summary>
        [JsonProperty("is_deleted"), JsonPropertyName("is_deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 透传字段
        /// </summary>
        [JsonProperty("extend"), JsonPropertyName("extend")]
        public string Extend { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("update_time"), JsonPropertyName("update_time")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("create_time"), JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("remark"), JsonPropertyName("remark")]
        public string Remark { get; set; }
    }
}