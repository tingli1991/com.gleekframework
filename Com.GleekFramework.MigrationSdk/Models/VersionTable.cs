using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 带版本号的表名称
    /// </summary>
    [Index("idx_version",nameof(Version))]
    public class VersionTable : BasicTable
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("version")]
        [Comment("版本号")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [JsonProperty("version"), JsonPropertyName("version")]
        public long Version { get; set; }
    }
}