using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 基础表
    /// </summary>
    public class BasicTable : ITable
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Column("id")]
        [Comment("主键")]
        [JsonProperty("id"), JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// 死否删除
        /// </summary>
        [Comment("是否删除")]
        [Column("id_deleted")]
        [JsonProperty("id_deleted"), JsonPropertyName("id_deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 透传字段
        /// </summary>
        [MaxLength(500)]
        [Column("extend")]
        [Comment("透传字段")]
        [JsonProperty("extend"), JsonPropertyName("extend")]
        public string Extend { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Comment("更新时间")]
        [Column("update_time")]
        [JsonProperty("update_time"), JsonPropertyName("update_time")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Comment("创建时间")]
        [Column("create_time")]
        [JsonProperty("create_time"), JsonPropertyName("create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        [Comment("备注")]
        [Column("remark")]
        [JsonProperty("remark"), JsonPropertyName("remark")]
        public string Remark { get; set; }
    }
}