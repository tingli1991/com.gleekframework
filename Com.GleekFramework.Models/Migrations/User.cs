using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models.Enums;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("user")]
    [Comment("用户表")]
    [Index("idx_user_email", nameof(Email))]
    [Index("idx_user_user_name", nameof(UserName))]
    public class User : BasicTable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(50)]
        [Comment("用户名")]
        [Column("user_name")]
        [JsonProperty("user_name"), JsonPropertyName("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(50)]
        [Comment("密码")]
        [Column("password")]
        [JsonProperty("password"), JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(100)]
        [Comment("邮箱")]
        [Column("email")]
        [JsonProperty("email"), JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(50)]
        [Comment("昵称")]
        [Column("nick_name")]
        [JsonProperty("nick_name"), JsonPropertyName("nick_name")]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(500)]
        [Comment("头像")]
        [Column("avatar")]
        [JsonProperty("avatar"), JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Comment("性别")]
        [Column("gender")]
        [DefaultValue(30)]
        [JsonProperty("gender"), JsonPropertyName("gender")]
        public Gender Gender { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Comment("状态")]
        [Column("status")]
        [DefaultValue(10)]
        [JsonProperty("status"), JsonPropertyName("status")]
        public EnableStatus Status { get; set; }

        /// <summary>
        /// 个人名片
        /// </summary>
        [MaxLength(500)]
        [Comment("个人名片")]
        [Column("business_card")]
        [JsonProperty("business_card"), JsonPropertyName("business_card")]
        public string BusinessCard { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [MaxLength(1000)]
        [Comment("个性签名")]
        [Column("signature")]
        [JsonProperty("signature"), JsonPropertyName("signature")]
        public string Signature { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Comment("注册时间")]
        [Column("register_time", TypeName = "datetime")]
        [JsonProperty("register_time"), JsonPropertyName("register_time")]
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Comment("生日")]
        [Column("birthday_time", TypeName = "datetime")]
        [JsonProperty("birthday_time"), JsonPropertyName("birthday_time")]
        public DateTime BirthdayTime { get; set; }

        /// <summary>
        /// 最近登录时间
        /// </summary>
        [Comment("最近登录时间")]
        [Column("last_login_time", TypeName = "datetime")]
        [JsonProperty("last_login_time"), JsonPropertyName("last_login_time")]
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 最近登出时间
        /// </summary>
        [Comment("最近登出时间")]
        [Column("last_logout_time", TypeName = "datetime")]
        [JsonProperty("last_logout_time"), JsonPropertyName("last_logout_time")]
        public DateTime LastLogoutTime { get; set; }
    }
}