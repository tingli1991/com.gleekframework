using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 用户角色对应表
    /// </summary>
    [Table("user_role")]
    [Comment("用户角色对应表")]
    public class UserRole : MigrationTable
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column("user_id")]
        [Comment("用户Id")]
        [JsonProperty("user_id"), JsonPropertyName("user_id")]
        public long UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Column("role_id")]
        [Comment("角色Id")]
        [JsonProperty("role_id"), JsonPropertyName("role_id")]
        public long RoleId { get; set; }
    }
}