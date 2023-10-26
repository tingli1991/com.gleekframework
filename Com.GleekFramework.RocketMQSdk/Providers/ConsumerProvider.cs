using Aliyun.MQ;
using System.Collections.Concurrent;

namespace Com.GleekFramework.RocketMQSdk
{
    /// <summary>
    /// 消费者的实现类
    /// </summary>
    public static partial class ConsumerProvider
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
        /// <param name="groupId">分组Id</param>
        /// <returns></returns>
        private static string GetCacheKey(string instanceId, string topic, string groupId) => $"{instanceId}_{topic}_{groupId}";

        /// <summary>
        /// 消费者者字典
        /// </summary>
        public static readonly ConcurrentDictionary<string, MQConsumer> CacheList = new ConcurrentDictionary<string, MQConsumer>();

        /// <summary>
        /// 获取生产者单例
        /// </summary>
        /// <param name="host"></param>
        /// <param name="topic"></param>
        /// <param name="groupId">分组Id</param>
        /// <returns></returns>
        public static MQConsumer GetConsumerSingle(string host, string topic, string groupId)
        {
            var options = RocketAccessProvider.AccessOptions;
            var cacheKey = GetCacheKey(options.InstanceId, topic, groupId);
            if (!CacheList.ContainsKey(cacheKey))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(topic))
                    {
                        var client = MQClientProvider.GetClientSingle(host);
                        var consumer = client.GetConsumer(options.InstanceId, topic, groupId, RocketMQConstant.DEFAULT_TAGES);
                        CacheList.TryAdd(cacheKey, consumer);
                    }
                }
            }
            return CacheList[cacheKey];
        }
    }
}