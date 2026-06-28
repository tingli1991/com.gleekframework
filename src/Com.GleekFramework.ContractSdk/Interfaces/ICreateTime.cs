using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 创建时间
    /// </summary>
    public interface ICreateTime
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [Comment("创建时间")]
        [Column("create_time")]
        [JsonProperty("create_time"), JsonPropertyName("create_time")]
        DateTime CreateTime { get; set; }
    }
}