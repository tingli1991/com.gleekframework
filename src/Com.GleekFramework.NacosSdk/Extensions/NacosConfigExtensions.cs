using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 配置中心工具类
    /// </summary>
    internal static partial class NacosConfigExtensions
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="baseUrl">接口基础地址</param>
        /// <param name="namespaceId">命名空间</param>
        /// <param name="group">分组名称</param>
        /// <param name="dataId">配置Id</param>
        /// <param name="token">token字段</param>
        /// <returns></returns>
        public static async Task<string> GetConfigAsync(this string baseUrl, string namespaceId, string group, string dataId, string token)
        {
            var paramters = new Dictionary<string, string>()
            {
                { "dataId",dataId.Trim() },
                { "tenant",namespaceId.Trim() },
                { "group",string.IsNullOrEmpty(group) ? "DEFAULT_GROUP" : group.Trim() }
            };

            if (!string.IsNullOrEmpty(token))
            {
                //绑定token字段
                paramters.Add("accessToken", token);
            }
            return await paramters.GetAsync(RestConstant.GetConfigsUrl(baseUrl));
        }

        /// <summary>
        /// 添加监听配置
        /// </summary>
        /// <param name="baseUrl">接口基础地址</param>
        /// <param name="namespaceId">命名空间</param>
        /// <param name="group">分组名称</param>
        /// <param name="dataId">配置Id</param>
        /// <param name="configValue">配置内容</param>
        /// <param name="pulllistenInterval">长轮训等待时间(单位：毫秒)</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static async Task<string> AddListenerAsync(this string baseUrl, string namespaceId, string group, string dataId, string configValue, int pulllistenInterval, string token)
        {
            if (string.IsNullOrEmpty(group))
            {
                group = "DEFAULT_GROUP";
            }

            var headers = new Dictionary<string, string>() { { "Long-Pulling-Timeout", $"{pulllistenInterval}" } };
            var paramterStr = $"{dataId}{char.ConvertFromUtf32(2)}{group}{char.ConvertFromUtf32(2)}{configValue.EncryptMd5()}";
            if (!string.IsNullOrEmpty(namespaceId))
            {
                //绑定命名空间
                paramterStr += $"{char.ConvertFromUtf32(2)}{namespaceId}";
            }

            var paramters = new Dictionary<string, string>() { { "Listening-Configs", $"{paramterStr}{char.ConvertFromUtf32(1)}" } };
            if (!string.IsNullOrEmpty(token))
            {
                //绑定token字段
                paramters.Add("accessToken", token);
            }
            return await paramters.PostAsync(RestConstant.GetConfigListenerUrl(baseUrl), headers);
        }
    }
}