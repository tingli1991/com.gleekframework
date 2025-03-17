using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public partial class PageDataResult<T>
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        [JsonProperty("results"), JsonPropertyName("results")]
        public IEnumerable<T> Results { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        [JsonProperty("page_index"), JsonPropertyName("page_index")]
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [JsonProperty("page_size"), JsonPropertyName("page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        [JsonProperty("total_count"), JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// 是否包含下一页
        /// </summary>
        [JsonProperty("has_next"), JsonPropertyName("has_next")]
        public bool HasNext { get { return PageIndex < PageCount; } }

        /// <summary>
        /// 是否包含上一页
        /// </summary>
        [JsonProperty("has_previous"), JsonPropertyName("has_previous")]
        public bool HasPrevious { get { return PageIndex > 1; } }

        /// <summary>
        /// 分页总页数
        /// </summary>
        [JsonProperty("page_count"), JsonPropertyName("page_count")]
        public int PageCount { get { return (int)Math.Ceiling((decimal)TotalCount / (PageSize <= 0 ? 1 : PageSize)); } }
    }
}