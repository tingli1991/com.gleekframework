using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 服务中心工具类
    /// </summary>
    internal static partial class NacosServiceExtensions
    {
        /// <summary>
        /// 注册服务实例
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static async Task<bool> RegisterInstanceAsync(this string baseUrl, RegisterInstanceRequestParam param)
        {
            var paramters = new Dictionary<string, string>()
            {
                { "port", $"{param.Port}" },
                { "ip", $"{param.Ip ?? ""}".Trim() },
                { "weight", $"{param.Weight ?? 1}".Trim() },
                { "enabled", $"{param.Enable ?? false}".Trim() },
                { "metadata", $"{param.Metadata ?? ""}".Trim() },
                { "healthy", $"{param.Healthy ?? false}".Trim() },
                { "groupName", $"{param.GroupName ?? ""}".Trim() },
                { "ephemeral", $"{param.Ephemeral ?? true}".Trim() },
                { "namespaceId", $"{param.NamespaceId ?? ""}".Trim() },
                { "clusterName", $"{param.ClusterName ?? ""}".Trim() },
                { "serviceName", $"{param.ServiceName ?? ""}".Trim() }
            };

            if (!string.IsNullOrEmpty(param.Token))
            {
                //绑定token字段
                paramters.Add("accessToken", param.Token);
            }
            var responseContent = await paramters.PostAsync(RestConstant.GetRegisterServiceInstanceUrl(baseUrl));
            return !string.IsNullOrEmpty(responseContent) && responseContent.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 注销服务实例
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static async Task<bool> RemoveServiceInstanceAsync(this string baseUrl, RemoveInstanceRequestParam param)
        {
            var paramters = new Dictionary<string, string>()
            {
                { "port", $"{param.Port}" },
                { "ip", $"{param.Ip ?? ""}".Trim() },
                { "groupName", $"{param.GroupName ?? ""}".Trim() },
                { "ephemeral", $"{param.Ephemeral ?? true}".Trim() },
                { "serviceName", $"{param.ServiceName ?? ""}".Trim() },
                { "namespaceId", $"{param.NamespaceId ?? ""}".Trim() },
                { "clusterName", $"{param.ClusterName ?? ""}".Trim() }
            };

            if (!string.IsNullOrEmpty(param.Token))
            {
                //绑定token字段
                paramters.Add("accessToken", param.Token);
            }
            var responseContent = await paramters.DeleteAsync(RestConstant.GetRemoveServiceInstanceUrl(baseUrl));
            return !string.IsNullOrEmpty(responseContent) && responseContent.Equals("ok", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 发送心跳
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static async Task<bool> SendHeartbeatAsync(this string baseUrl, SendHeartbeatRequestParam param)
        {
            var beatJsonValue = JsonConvert.SerializeObject(new SendHeartbeatParam()
            {
                Ip = param.Ip,
                Scheduled = false,
                Port = param.Port,
                Cluster = param.ClusterName ?? "",
                ServiceName = param.ServiceName ?? "",
                Metadata = new Dictionary<string, string>(),
                Weight = param.Weight <= 0 ? 1 : param.Weight,
            });

            var paramters = new Dictionary<string, string>()
            {
                { "port", $"{param.Port}" },
                { "ip", $"{param.Ip ?? ""}".Trim() },
                { "groupName", $"{param.GroupName ?? ""}".Trim() },
                { "ephemeral", $"{param.Ephemeral ?? true}".Trim() },
                { "namespaceId", $"{param.NamespaceId ?? ""}".Trim() },
                { "serviceName", $"{param.ServiceName ?? ""}".Trim() },
                { "beat", $"{HttpUtility.UrlEncode(beatJsonValue) ?? ""}" }
            };

            if (!string.IsNullOrEmpty(param.Token))
            {
                //绑定token字段
                paramters.Add("accessToken", param.Token);
            }
            var responseContent = await paramters.PutAsync(RestConstant.GetSendServiceInstanceBeatUrl(baseUrl));
            return !string.IsNullOrEmpty(responseContent) && responseContent.Contains("10200", StringComparison.InvariantCulture);
        }
    }
}