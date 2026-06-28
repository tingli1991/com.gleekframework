using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 实例主机节点
    /// </summary>
    [Serializable]
    public class ListInstancesHostResponse
    {
        /// <summary>
        /// 实例Id
        /// </summary>
        [JsonProperty("instanceId"), JsonPropertyName("instanceId")]
        public string InstanceId { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [JsonProperty("ip"), JsonPropertyName("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// 服务实例端口
        /// </summary>
        [JsonProperty("port"), JsonPropertyName("port")]
        public int Port { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        [JsonProperty("weight"), JsonPropertyName("weight")]
        public double Weight { get; set; }

        /// <summary>
        /// 是否健康
        /// </summary>
        [JsonProperty("healthy"), JsonPropertyName("healthy")]
        public bool Healthy { get; set; }

        /// <summary>
        /// 是否上线
        /// </summary>
        [JsonProperty("enabled"), JsonPropertyName("enabled")]
        public bool Enable { get; set; }

        /// <summary>
        /// 是否临时实例
        /// </summary>
        [JsonProperty("ephemeral"), JsonPropertyName("ephemeral")]
        public bool Ephemeral { get; set; }

        /// <summary>
        /// 集群名
        /// </summary>
        [JsonProperty("clusterName"), JsonPropertyName("clusterName")]
        public string ClusterName { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        [JsonProperty("serviceName"), JsonPropertyName("serviceName")]
        public string ServiceName { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        [JsonProperty("metadata"), JsonPropertyName("metadata")]
        public string Metadata { get; set; }

        /// <summary>
        /// 实例心跳间隔(单位：毫秒)
        /// </summary>
        [JsonProperty("instanceHeartBeatInterval"), JsonPropertyName("instanceHeartBeatInterval")]
        public int HeartBeatInterval { get; set; }

        /// <summary>
        /// 实例心跳超时时间(单位：毫秒)
        /// </summary>
        [JsonProperty("instanceHeartBeatTimeOut"), JsonPropertyName("instanceHeartBeatTimeOut")]
        public int HeartBeatTimeOut { get; set; }
    }
}