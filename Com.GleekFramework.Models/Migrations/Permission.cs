using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 权限表
    /// </summary>
    [Comment("权限表")]
    [Table("permission")]
    public class Permission : BasicTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(50)]
        [Column("name")]
        [Comment("角色名称")]
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Comment("状态")]
        [Column("status")]
        [DefaultValue(10)]
        [JsonProperty("status"), JsonPropertyName("status")]
        public EnableStatus Status { get; set; }
    }
}