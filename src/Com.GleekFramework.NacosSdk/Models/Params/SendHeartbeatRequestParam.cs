using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 发送心跳请求参数
    /// </summary>
    [Serializable]
    internal class SendHeartbeatRequestParam
    {
        /// <summary>
        /// 服务实例IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 服务实例端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// 集群名
        /// </summary>
        public string ClusterName { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 分组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 是否临时实例
        /// </summary>
        public bool? Ephemeral { get; set; }

        /// <summary>
        /// 命名空间ID
        /// </summary>
        public string NamespaceId { get; set; }

        /// <summary>
        /// Token字段
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// 心跳参数
    /// </summary>
    [Serializable]
    public class SendHeartbeatParam
    {
        /// <summary>
        /// 端口
        /// </summary>
        [JsonProperty("port"), JsonPropertyName("port")]
        public int Port { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [JsonProperty("ip"), JsonPropertyName("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        [JsonProperty("weight"), JsonPropertyName("weight")]
        public double Weight { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        [JsonProperty("serviceName"), JsonPropertyName("serviceName")]
        public string ServiceName { get; set; }

        /// <summary>
        /// 集群名称
        /// </summary>
        [JsonProperty("cluster"), JsonPropertyName("cluster")]
        public string Cluster { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("scheduled"), JsonPropertyName("scheduled")]
        public bool Scheduled { get; set; }

        /// <summary>
        /// 元数据
        /// </summary>
        [JsonProperty("metadata"), JsonPropertyName("metadata")]
        public Dictionary<string, string> Metadata { get; set; }
    }
}