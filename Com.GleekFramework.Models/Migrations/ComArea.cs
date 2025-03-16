using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Newtonsoft.Json;
using ProtoBuf;
using System;
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
    public class ComArea : ITable
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Column("id")]
        [Comment("主键")]
        [ProtoMember(1)]
        [JsonProperty("id"), JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(255)]
        [Column("code")]
        [Comment("编码")]
        [ProtoMember(2)]
        [JsonProperty("code"), JsonPropertyName("code")]
        public string Code { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        [MaxLength(255)]
        [Column("name")]
        [Comment("名称")]
        [ProtoMember(3)]
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 地区级别
        /// </summary>
        [Column("level")]
        [Comment("级别")]
        [ProtoMember(4)]
        [JsonProperty("level"), JsonPropertyName("level")]
        public AreaLevel Level { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [MaxLength(50)]
        [Column("lng")]
        [Comment("经度")]
        [ProtoMember(5)]
        [JsonProperty("lng"), JsonPropertyName("lng")]
        public string Lng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [MaxLength(50)]
        [Column("lat")]
        [Comment("纬度")]
        [ProtoMember(6)]
        [JsonProperty("lat"), JsonPropertyName("lat")]
        public string Lat { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        [Comment("父级")]
        [ProtoMember(7)]
        [Column("parent_id")]
        [JsonProperty("parent_id"), JsonPropertyName("parent_id")]
        public long ParentId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [ProtoMember(8)]
        [Column("version")]
        [Comment("版本号")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonProperty("version"), JsonPropertyName("version")]
        public long Version { get; set; }

        /// <summary>
        /// 死否删除
        /// </summary>
        [ProtoMember(9)]
        [Comment("是否删除")]
        [Column("is_deleted")]
        [JsonProperty("is_deleted"), JsonPropertyName("is_deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 透传字段
        /// </summary>
        [ProtoMember(10)]
        [MaxLength(500)]
        [Column("extend")]
        [Comment("透传字段")]
        [JsonProperty("extend"), JsonPropertyName("extend")]
        public string Extend { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [ProtoMember(11)]
        [Comment("更新时间")]
        [Column("update_time")]
        [JsonProperty("update_time"), JsonPropertyName("update_time")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [ProtoMember(12)]
        [Comment("创建时间")]
        [Column("create_time")]
        [JsonProperty("create_time"), JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [ProtoMember(13)]
        [MaxLength(500)]
        [Comment("备注")]
        [Column("remark")]
        [JsonProperty("remark"), JsonPropertyName("remark")]
        public string Remark { get; set; }
    }
}