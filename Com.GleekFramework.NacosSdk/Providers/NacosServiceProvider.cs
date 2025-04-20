using Com.GleekFramework.CommonSdk;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Nacos服务实现类
    /// </summary>
    internal static class NacosServiceProvider
    {
        /// <summary>
        /// 内存缓存对象
        /// </summary>
        private static readonly IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="action">服务名称</param>
        /// <param name="scheme">契约</param>
        /// <returns></returns>
        public static async Task<string> GetHostUrlAsync(string serviceName, string action, string scheme)
        {
            var baseServiceUri = await GetServerHostAsync(serviceName);
            return $"{scheme ?? ""}{baseServiceUri.TrimEnd('/', '\\')}/{action.TrimStart('/', '\\')}";
        }

        /// <summary>
        /// 获取服务主机信息
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        private static async Task<string> GetServerHostAsync(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                throw new ArgumentNullException("请填写服务名称：serviceName");
            }

            var host = "";//主机地址
            var serviceHostList = await GetServiceHostListAsync(serviceName);
            if (serviceHostList.IsNotEmpty())
            {
                var serviceHostInfo = serviceHostList.Next();
                host = $"{serviceHostInfo.Ip}:{serviceHostInfo.Port}";
            }
            return host;
        }

        /// <summary>
        /// 获取服务实例列表
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        private static async Task<IEnumerable<ListInstancesHostResponse>> GetServiceHostListAsync(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                return new List<ListInstancesHostResponse>();
            }

            var options = NacosProvider.Settings;//对应的配置
            var baseUrl = options.ServerAddresses.GetServerAddressesUrl();//随机获取服务端配置
            var serviceHostList = cache.Get<IEnumerable<ListInstancesHostResponse>>(serviceName);
            if (serviceHostList == null)
            {
                var token = baseUrl.GetAuthTokenAsync(options.UserName, options.Password).Result;//获取授权的token
                var serviceList = await GetServiceListAsync(baseUrl, serviceName, token);//服务实例信息
                var expireSeconds = options.ServiceSettings.ExpireSeconds;//过期时间(单位：秒)
                if (serviceList == null || serviceList.Hosts.IsNullOrEmpty())
                {
                    serviceHostList = new List<ListInstancesHostResponse>();
                }
                else
                {
                    serviceHostList ??= new List<ListInstancesHostResponse>();
                    foreach (var serviceHost in serviceList.Hosts)
                    {
                        for (int i = 0; i < serviceHost.Weight; i++)
                        {
                            serviceHostList.Add(serviceHost);
                        }
                    }
                }
                return cache.Set(serviceName, serviceHostList, TimeSpan.FromSeconds(expireSeconds));
            }
            return serviceHostList;
        }

        /// <summary>
        /// 获取服务实例列表
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="token">token字段</param>
        /// <returns></returns>
        private static async Task<ListInstancesResponse> GetServiceListAsync(string baseUrl, string serviceName, string token)
        {
            var serviceOptions = serviceName.GetServiceOptions();//获取服务配置节点
            var paramters = new Dictionary<string, string>()
            {
                { "healthyOnly", $"true"},
                { "clusters", $"{serviceOptions.Clusters ?? ""}".Trim() },
                { "groupName", $"{serviceOptions.GroupName ?? ""}".Trim() },
                { "serviceName", $"{serviceOptions.ServiceName ?? ""}".Trim() },
                { "namespaceId", $"{serviceOptions.NamespaceId ?? ""}".Trim() },
            };

            if (!string.IsNullOrEmpty(token))
            {
                //绑定token字段
                paramters.Add("accessToken", token);
            }
            var responseJsonValue = await paramters.GetAsync(RestConstant.GetServiceInstanceListUrl(baseUrl));
            return responseJsonValue.DeserializeObject<ListInstancesResponse>();//反序列化请求参数
        }
    }
}