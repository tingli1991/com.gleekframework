using Com.GleekFramework.NLogSdk;
using Confluent.Kafka;
using System.Collections.Concurrent;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// 生产者实现类
    /// </summary>
    public partial class ProducerProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// kafka tcp producer
        /// </summary>
        private static readonly ConcurrentDictionary<string, IProducer<string, string>> ProducerCache = new ConcurrentDictionary<string, IProducer<string, string>>();

        /// <summary>
        /// 获取生产者客户端对象
        /// </summary>
        /// <param name="host">主机地址</param>
        /// <returns></returns>
        public static IProducer<string, string> GetProducerSingle(string host)
        {
            if (!ProducerCache.ContainsKey(host))
            {
                lock (@lock)
                {

                    if (!ProducerCache.ContainsKey(host))
                    {
                        var confg = new ProducerConfig()
                        {
                            BatchSize = 1000000,//包分批处理的大小
                            BootstrapServers = host,
                            MessageMaxBytes = KafkaConstant.MessageMaxBytes
                        };

                        var builder = new ProducerBuilder<string, string>(confg)
                            .SetErrorHandler((p, msg) => NLogProvider.Error($"【Kafka生产者】错误码：{msg.Code}；错误原因：{msg.Reason}；是否错误信息：{msg.IsError}"))
                            .Build();

                        ProducerCache.TryAdd(host, builder);
                    }
                }
            }
            return ProducerCache[host];
        }
    }
}