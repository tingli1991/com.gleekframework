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
    /// 角色表
    /// </summary>
    [Table("role")]
    [Comment("角色表")]
    public class Role : MigrationTable
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