using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System;

namespace Com.GleekFramework.NLogSdk
{
    /// <summary>
    /// 日志输出模型
    /// </summary>
    public class NLogModel
    {
        /// <summary>
        /// 地址
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        /// <summary>
        /// 操作编号(全局唯一)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SerialNo { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }

        /// <summary>
        /// 内容长度
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long ContentLength { get => $"{Content ?? ""}".Length; }

        /// <summary>
        /// 程序执行时间(单位：毫秒)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long TotalMilliseconds { get; set; }

        /// <summary>
        /// 日志打印时间(在配置文件中配置)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ServiceTime { get => DateTime.Now.ToCstTime().Format(true); }

        /// <summary>
        /// 程序执行时间(单位：秒)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long TotalSeconds { get => TotalMilliseconds > 0 ? TotalMilliseconds / 1000 : 0; }
    }
}