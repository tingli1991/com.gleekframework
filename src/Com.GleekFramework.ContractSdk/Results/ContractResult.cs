using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 协议返回结果
    /// </summary>
    [Serializable]
    public partial class ContractResult
    {
        /// <summary>
        /// 业务处理结果(true 表示成功  false 表示失败)
        /// </summary>
        [JsonProperty("success"), JsonPropertyName("success")]
        public bool Success { get; set; }

        /// <summary>
        /// 错误状态码
        /// </summary>
        [JsonProperty("code"), JsonPropertyName("code")]
        public string Code { get; set; } = $"{GlobalMessageCode.FAIL.GetHashCode()}";

        /// <summary>
        /// 业务流水号
        /// </summary>
        [JsonProperty("serial_no"), JsonPropertyName("serial_no")]
        public string SerialNo { get; set; } = SnowflakeProvider.GetSerialNo();

        /// <summary>
        /// 错误信息，成功将返回空
        /// </summary>
        [JsonProperty("message"), JsonPropertyName("message")]
        public string Message { get; set; } = GlobalMessageCode.FAIL.GetDescription();

        /// <summary>
        /// 时间戳
        /// </summary>
        [JsonProperty("timestamp"), JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; } = DateTime.Now.ToCstTime().ToUnixTimeForMilliseconds();
    }

    /// <summary>
    /// 返回结果契约
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ContractResult<T> : ContractResult
    {
        /// <summary>
        /// 数据详情
        /// </summary>
        [JsonProperty("data"), JsonPropertyName("data")]
        public T Data { get; set; }
    }
}