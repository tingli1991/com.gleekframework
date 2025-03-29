using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 分页请求参数
    /// </summary>
    public class PageParam
    {
        /// <summary>
        /// 最大Id
        /// </summary>
        [FromQuery(Name = "next_id")]
        [JsonProperty("next_id"), JsonPropertyName("next_id")]
        public long? NextId { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [FromQuery(Name = "page_size")]
        [JsonProperty("page_size"), JsonPropertyName("page_size")]
        [MaxLength(200, ErrorMessage = nameof(GlobalMessageCode.EXCEED_PPER_LIMIT_200))]
        public long PageSize { get; set; } = 20;
    }
}