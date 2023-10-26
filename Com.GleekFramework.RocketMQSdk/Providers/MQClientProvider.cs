using Aliyun.MQ;
using System.Collections.Concurrent;

namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// 客户端实现类
    /// </summary>
    public static partial class MQClientProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 客户端消息
        /// </summary>
        private static readonly ConcurrentDictionary<string, MQClient> CacheList = new ConcurrentDictionary<string, MQClient>();

        /// <summary>
        /// 获取键
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="endpoint">终结点地址</param>
        /// <returns></returns>
        private static string GetCacheKey(string accessKey, string secretKey, string endpoint) => $"{accessKey}_{secretKey}_{endpoint}";

        /// <summary>
        /// 获取客户端信息
        /// </summary>
        /// <param name="endpoint">总结点地址</param>
        /// <returns></returns>
        public static MQClient GetClientSingle(string endpoint)
        {
            var options = RocketAccessProvider.AccessOptions;
            var cacheKey = GetCacheKey(endpoint, options.AccessKey, options.SecretKey);
            if (!CacheList.ContainsKey(cacheKey))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(cacheKey))
                    {
                        var client = new MQClient(options.AccessKey, options.SecretKey, endpoint);
                        CacheList.TryAdd(endpoint, client);
                    }
                }
            }
            return CacheList[cacheKey];
        }
    }
}