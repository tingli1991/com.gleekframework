using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 版本号
    /// </summary>
    public interface IVersion
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [Column("version")]
        [Comment("版本号")]
        [JsonProperty("version"), JsonPropertyName("version")]
        decimal Version { get; set; }
    }
}