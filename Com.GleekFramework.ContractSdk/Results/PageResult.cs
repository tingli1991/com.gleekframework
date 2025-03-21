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
        [JsonProperty("next_id"), JsonPropertyName("next_id")]
        public long NextId { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [JsonProperty("page_size"), JsonPropertyName("page_size")]
        public long PageSize { get; set; }

        /// <summary>
        /// 分页列表
        /// </summary>
        [JsonProperty("results"), JsonPropertyName("results")]
        public IEnumerable<T> Results { get; set; }
    }
}