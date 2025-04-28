using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 编辑模型
    /// </summary>
    public class EditParam : IVersionTable
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonIgnore()]
        [Column("version")]
        [JsonProperty("version")]
        public decimal Version { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonIgnore]
        [Column("update_time")]
        [JsonProperty("update_time")]
        public DateTime UpdateTime { get; set; } = DateTime.Now.ToCstTime();

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remark")]
        [JsonProperty("remark")]
        public string Remark { get; set; }
    }
}