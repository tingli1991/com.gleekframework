using Aliyun.MQ;
using System.Collections.Concurrent;

namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// 生产者实现类
    /// </summary>
    public static partial class ProducerProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 获取键
        /// </summary>
        /// <param name="instanceId">实例Id</param>
        /// <param name="topic">主题</param>
        /// <returns></returns>
        private static string GetCacheKey(string instanceId, string topic) => $"{instanceId}_{topic}";

        /// <summary>
        /// 消费者者字典
        /// </summary>
        public static readonly ConcurrentDictionary<string, MQProducer> CacheList = new ConcurrentDictionary<string, MQProducer>();

        /// <summary>
        /// 获取生产者单例
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <param name="topic">主题名称</param>
        /// <returns></returns>
        public static MQProducer GetProducerSingle(string host, string topic)
        {
            var options = RocketAccessProvider.AccessOptions;
            var cacheKey = GetCacheKey(options.InstanceId, topic);
            if (!CacheList.ContainsKey(cacheKey))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(topic))
                    {
                        var client = MQClientProvider.GetClientSingle(host);
                        var producer = client.GetProducer(options.InstanceId, topic);
                        CacheList.TryAdd(cacheKey, producer);
                    }
                }
            }
            return CacheList[cacheKey];
        }
    }
}