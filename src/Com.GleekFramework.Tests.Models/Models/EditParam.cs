using Newtonsoft.Json;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 编辑模型
    /// </summary>
    public class EditParam
    {
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }
    }
}