using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 角色权限对应表
    /// </summary>
    [Table("role_permission")]
    [Comment("角色权限对应表")]
    public class RolePermission : BasicTable
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Column("role_id")]
        [Comment("角色Id")]
        [JsonProperty("role_id"), JsonPropertyName("role_id")]
        public long RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        [Comment("权限Id")]
        [Column("permission_id")]
        [JsonProperty("permission_id"), JsonPropertyName("permission_id")]
        public long PermissionId { get; set; }
    }
}