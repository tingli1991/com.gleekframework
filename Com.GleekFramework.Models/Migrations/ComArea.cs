using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 地区信息表
    /// </summary>
    [Table("com_area")]
    [Comment("地区信息表")]
    public class ComArea : VersionTable
    {
        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(255)]
        [Column("code")]
        [Comment("编码")]
        [JsonProperty("code"), JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        [MaxLength(255)]
        [Column("name")]
        [Comment("名称")]
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 地区级别
        /// </summary>
        [Column("level")]
        [Comment("级别")]
        [JsonProperty("level"), JsonPropertyName("level")]
        public AreaLevel Level { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [MaxLength(50)]
        [Column("lng")]
        [Comment("经度")]
        [JsonProperty("lng"), JsonPropertyName("lng")]
        public string Lng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [MaxLength(50)]
        [Column("lat")]
        [Comment("纬度")]
        [JsonProperty("lat"), JsonPropertyName("lat")]
        public string Lat { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        [Comment("父级")]
        [Column("parent_id")]
        [JsonProperty("parent_id"), JsonPropertyName("parent_id")]
        public long ParentId { get; set; }
    }
}