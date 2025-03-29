using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 分页数据请求参数
    /// </summary>
    public class PageDataParam
    {
        /// <summary>
        /// 页码
        /// </summary>
        [FromQuery(Name = "page_index")]
        [JsonProperty("page_index"), JsonPropertyName("page_index")]
        public long PageIndex { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        [FromQuery(Name = "page_size")]
        [JsonProperty("page_size"), JsonPropertyName("page_size")]
        [MaxLength(200,ErrorMessage =nameof(GlobalMessageCode.EXCEED_PPER_LIMIT_200))]
        public long PageSize { get; set; } = 20;
    }
}