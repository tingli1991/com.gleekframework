using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 获取服务列表返回结果
    /// </summary>
    [Serializable]
    internal class ListInstancesResponse
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        [JsonProperty("valid"), JsonPropertyName("valid")]
        public bool Valid { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name"), JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        [JsonProperty("groupName"), JsonPropertyName("groupName")]
        public string GroupName { get; set; }

        /// <summary>
        /// 集群名称
        /// </summary>
        [JsonProperty("clusters"), JsonPropertyName("clusters")]
        public string Clusters { get; set; }

        /// <summary>
        /// 缓存时间(单位：毫秒)
        /// </summary>
        [JsonProperty("cacheMillis"), JsonPropertyName("cacheMillis")]
        public string CacheMillis { get; set; }

        /// <summary>
        /// 是否触发保护阈值
        /// </summary>
        [JsonProperty("reachProtectionThreshold"), JsonPropertyName("reachProtectionThreshold")]
        public bool ReachProtectionThreshold { get; set; }

        /// <summary>
        /// 实例主机节点
        /// </summary>
        [JsonProperty("hosts"), JsonPropertyName("hosts")]
        public IEnumerable<ListInstancesHostResponse> Hosts { get; set; }
    }
}