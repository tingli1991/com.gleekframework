using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 分页返回结果
    /// </summary>
    public partial class PageResult<T>
    {
        /// <summary>
        /// 最大ID
        /// </summary>
        [JsonProperty("max_id"), JsonPropertyName("max_id")]
        public int MaxId { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [JsonProperty("page_size"), JsonPropertyName("page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// 分页列表
        /// </summary>
        [JsonProperty("page_list"), JsonPropertyName("page_list")]
        public IEnumerable<T> PageList { get; set; }
    }
}