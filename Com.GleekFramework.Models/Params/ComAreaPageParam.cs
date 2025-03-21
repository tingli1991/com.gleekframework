using Com.GleekFramework.ContractSdk;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    }
}