using Com.GleekFramework.NLogSdk;
using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// Kafka实现服务
    /// </summary>
    public static partial class ConsumerProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 消费者缓存
        /// </summary>
        private static readonly Dictionary<string, IConsumer<string, string>> ConsumerCache = new Dictionary<string, IConsumer<string, string>>();

        /// <summary>
        /// 取消订阅
        /// </summary>
        public static void UnSubscribe()
        {
            if (ConsumerCache == null || !ConsumerCache.Any())
            {
                return;
            }

            foreach (var consumer in ConsumerCache)
            {
                //取消订阅
                consumer.Value.Unsubscribe();
            }
        }

        /// <summary>
        /// 获取消费对象实例
        /// </summary>
        /// <param name="hosts">主机地址</param>
        /// <param name="groupId">分组Id</param>
        /// <param name="topic">主题</param>
        /// <param name="autoAck">是否开启自动应答(默认：开启)</param>
        /// <param name="autoOffset">偏移方向</param>
        /// <returns></returns>
        public static IConsumer<string, string> GetConsumerSingle(string hosts, string groupId, string topic, bool autoAck, AutoOffsetReset autoOffset)
        {
            if (string.IsNullOrEmpty(hosts))
            {
                throw new ArgumentNullException("hosts");
            }

            if (string.IsNullOrEmpty(groupId))
            {
                throw new ArgumentNullException("groupId");
            }

            if (string.IsNullOrEmpty(topic))
            {
                throw new ArgumentNullException("topic");
            }

            var cacheKey = $"{hosts}:{groupId}:{topic}";
            if (!string.IsNullOrEmpty(cacheKey) && !ConsumerCache.ContainsKey(cacheKey))
            {
                lock (@lock)
                {
                    if (!string.IsNullOrEmpty(cacheKey) && !ConsumerCache.ContainsKey(cacheKey))
                    {
                        var consumerOptions = new ConsumerConfig()
                        {
                            GroupId = groupId,
                            //Acks = Acks.None,
                            //FetchMinBytes = 10,
                            BootstrapServers = hosts,
                            EnablePartitionEof = true,
                            //SessionTimeoutMs = 6000,
                            //StatisticsIntervalMs = 5000,
                            AutoOffsetReset = autoOffset,
                            //AutoCommitIntervalMs = 5000,
                            //EnableAutoCommit = enableAutoCommit,//设置非自动偏移,业务逻辑完成后手动处理偏移,防止数据丢失
                            EnableAutoOffsetStore = autoAck,
                            FetchMaxBytes = KafkaConstant.FetchMaxBytes,
                            MessageMaxBytes = KafkaConstant.MessageMaxBytes,
                            MaxPartitionFetchBytes = KafkaConstant.FetchMaxBytes,
                        };

                        var consumer = new ConsumerBuilder<string, string>(consumerOptions)
                            //.SetValueDeserializer(new KafkaDeserializer())
                            .SetErrorHandler((p, msg) => NLogProvider.Error($"【Kafka消费者】错误码：{msg.Code}；错误原因：{msg.Reason}；是否错误信息：{msg.IsError}"))
                            .Build();

                        ConsumerCache.Add(cacheKey, consumer);
                    }
                }
            }
            return ConsumerCache[cacheKey];
        }
    }
}