using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 更新时间
    /// </summary>
    public interface IUpdateTime
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        [Comment("更新时间")]
        [Column("update_time")]
        [JsonProperty("update_time"), JsonPropertyName("update_time")]
        DateTime UpdateTime { get; set; }
    }
}