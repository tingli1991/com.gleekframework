using Com.GleekFramework.ContractSdk;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ComAreaPageParam : PageDataParam
    {
        /// <summary>
        /// 搜索关键词
        /// </summary>
        [JsonProperty("keywords")]
        [FromQuery(Name = "keywords")]
        public string Keywords { get; set; }

        /// <summary>
        /// 创建开始时间
        /// </summary>
        [JsonProperty("create_begin_time")]
        [FromQuery(Name = "create_begin_time")]
        public DateTime? CreateBeginTime { get; set; }

        /// <summary>
        /// 创建结束时间
        /// </summary>
        [JsonProperty("create_end_time")]
        [FromQuery(Name = "create_end_time")]
        public DateTime? CreateEndTime { get; set; }
    }
}