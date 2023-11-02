using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 接受消息的内容
    /// </summary>
    [Serializable]
    public partial class MessageBody
    {
        /// <summary>
        /// 消息类型(方法名称)
        /// </summary>
        [JsonProperty("action_key"), JsonPropertyName("action_key")]
        public string ActionKey { get; set; }

        /// <summary>
        /// 业务流水号
        /// </summary>
        [JsonProperty("serial_no"), JsonPropertyName("serial_no")]
        public string SerialNo { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonProperty("timestamp"), JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; }

        /// <summary>
        /// 请求头信息
        /// </summary>
        [JsonProperty("headers"), JsonPropertyName("headers")]
        public Dictionary<string, string> Headers { get; set; }
    }

    /// <summary>
    /// 消息内容
    /// </summary>
    [Serializable]
    public partial class MessageBody<T> : MessageBody where T : class
    {
        /// <summary>
        /// 数据详情
        /// </summary>
        [JsonProperty("data"), JsonPropertyName("data")]
        public T Data { get; set; }
    }
}