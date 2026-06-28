using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// Kafka Topic管理帮助类。
    /// 提供 Topic 存在性检查、Topic 创建以及 Broker 数量查询等能力。
    /// </summary>
    public static class KafkaTopicProvider
    {
        /// <summary>
        /// 用于保护 Topic 缓存读写操作的异步互斥锁。
        /// </summary>
        private static readonly SemaphoreSlim TopicCacheLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 已存在的主题缓存，Key 为 bootstrapServers，Value 为已确认存在的 Topic 名称集合。
        /// </summary>
        private static readonly Dictionary<string, HashSet<string>> TopicCaches = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 批量创建 Kafka Topic，并设置其分区数、副本因子以及基础保留策略。
        /// </summary>
        /// <param name="bootstrapServers">
        /// Kafka 集群地址列表，格式为 "host1:port1,host2:port2"。
        /// <para>示例："localhost:9092" 或 "broker1.confluent.cloud:9092,broker2.confluent.cloud:9092"</para>
        /// </param>
        /// <param name="topics">
        /// 要创建的 Topic 名称集合。
        /// <para>系统会自动过滤空值、去除首尾空白，并对重复 Topic 去重。</para>
        /// </param>
        /// <remarks>
        /// <para>本方法使用 <see cref="AdminClient"/> 通过 Kafka 管理 API 创建 Topic，可自定义分区数、副本因子及 Topic 级别配置。</para>
        /// <para><strong>重要提示：</strong> 如果 Topic 已存在，本方法不会覆盖其配置。</para>
        /// <para>配置字典 <see cref="TopicSpecification.Configs"/> 支持 Kafka 官方文档中的动态 Topic 配置，如 retention.ms、cleanup.policy 等。</para>
        /// <para>使用示例：</para>
        /// <code>
        /// await KafkaTopicProvider.CreateTopicAsync("localhost:9092", new[] { "my-topic" });
        /// </code>
        /// </remarks>
        public static async Task CreateTopicAsync(string bootstrapServers, IEnumerable<string> topics)
        {
            ValidateBootstrapServers(bootstrapServers);
            var existsTopics = await ExistAsync(bootstrapServers, topics);
            var pendingTopics = topics.Except(existsTopics);
            if (pendingTopics == null || !pendingTopics.Any())
            {
                // 所有 Topic 已存在，无需创建，直接返回
                return;
            }

            var brokerCount = await GetBrokerCountAsync(bootstrapServers);
            if (brokerCount <= 0)
            {
                throw new InvalidOperationException("无法获取Kafka集群中的Broker节点数量,请检查BootstrapServers配置是否正确以及集群是否可用");
            }

            await TopicCacheLock.WaitAsync();
            try
            {
                var topicSpecifications = BuildTopicSpecifications(pendingTopics, brokerCount);
                using var adminClient = BuildAdminClient(bootstrapServers);
                await adminClient.CreateTopicsAsync(topicSpecifications);
                if (!TopicCaches.TryGetValue(bootstrapServers, out var cacheTopics))
                {
                    // 初始化缓存集合，使用 HashSet 以提高查询效率，并且使用 Ordinal 比较器确保大小写敏感
                    cacheTopics = new HashSet<string>(StringComparer.Ordinal);
                }
                cacheTopics.UnionWith(pendingTopics);
                TopicCaches[bootstrapServers] = cacheTopics;
            }
            finally
            {
                TopicCacheLock.Release();
            }
        }

        /// <summary>
        /// 批量检查指定 Topic 是否已经存在于目标 Kafka 集群中。
        /// </summary>
        /// <param name="bootstrapServers">
        /// Kafka 集群地址列表，格式为 "host1:port1,host2:port2"。
        /// <para>示例："localhost:9092"</para>
        /// </param>
        /// <param name="topics">
        /// 要检查的 Topic 名称集合。
        /// <para>该参数不允许为 null；若为空集合，则直接返回空结果。</para>
        /// </param>
        /// <returns>
        /// 返回已经存在的 Topic 名称集合。
        /// </returns>
        /// <remarks>
        /// <para>本方法使用 <see cref="AdminClient.DescribeTopicsAsync(TopicCollection, DescribeTopicsOptions)"/> 进行精确查询，不会拉取集群全量 Topic 列表，效率更高。</para>
        /// <para>当 Topic 不存在时，会将其视为正常结果并返回不存在，不会向上抛出异常。</para>
        /// <para>使用示例：</para>
        /// <code>
        /// var existsTopics = await KafkaTopicProvider.ExistAsync("localhost:9092", new[] { "topic1", "topic2" });
        /// </code>
        /// </remarks>
        public static async Task<IEnumerable<string>> ExistAsync(string bootstrapServers, IEnumerable<string> topics)
        {
            ValidateBootstrapServers(bootstrapServers);
            if (topics == null || !topics.Any())
            {
                return Array.Empty<string>();
            }

            if (!TopicCaches.TryGetValue(bootstrapServers, out var cacheTopics))
            {
                // 初始化缓存集合，使用 HashSet 以提高查询效率，并且使用 Ordinal 比较器确保大小写敏感
                cacheTopics = new HashSet<string>(StringComparer.Ordinal);
            }

            if (cacheTopics.IsSupersetOf(topics))
            {
                // 如果缓存中已经包含所有请求的 Topic，则直接返回缓存结果
                return topics;
            }

            topics = topics.Except(cacheTopics);// 从请求的 Topic 列表中排除掉缓存中已经确认存在的 Topic，减少后续 Kafka 查询的负担
            await TopicCacheLock.WaitAsync();
            try
            {
                using var adminClient = BuildAdminClient(bootstrapServers);
                try
                {
                    var topicCollection = TopicCollection.OfTopicNames(topics);
                    var describeResult = await adminClient.DescribeTopicsAsync(topicCollection);
                    cacheTopics.UnionWith(GetTopicNames(describeResult.TopicDescriptions));
                }
                catch (DescribeTopicsException ex)
                {
                    if (ex.Results?.TopicDescriptions != null)
                    {
                        cacheTopics.UnionWith(GetTopicNames(ex.Results.TopicDescriptions));
                    }
                }
                catch (KafkaException ex) when (IsTopicNotFoundError(ex))
                {
                    // Topic 不存在属于正常场景，此时直接返回缓存中已确认存在的 Topic 即可。
                }
                return cacheTopics;
            }
            finally
            {
                TopicCacheLock.Release();
            }
        }

        /// <summary>
        /// 获取 Kafka 集群中当前可用的 Broker 节点数量。
        /// </summary>
        /// <param name="bootstrapServers">
        /// Kafka 集群地址列表，格式为 "host1:port1,host2:port2"。
        /// <para>示例："localhost:9092"</para>
        /// </param>
        /// <returns>
        /// 当前 Kafka 集群在线的 Broker 节点数量。
        /// </returns>
        /// <remarks>
        /// <para>本方法通过 <see cref="AdminClient"/> 的 DescribeClusterAsync 方法获取集群信息，并从中提取 Broker 节点列表。</para>
        /// <para>返回的节点数量是 Controller 返回的当前所有在线 Broker 数量。</para>
        /// <para>使用示例：</para>
        /// <code>
        /// int brokerCount = await KafkaTopicProvider.GetBrokerCountAsync("localhost:9092");
        /// Console.WriteLine($"集群 Broker 数量：{brokerCount}");
        /// </code>
        /// </remarks>
        public static async Task<int> GetBrokerCountAsync(string bootstrapServers)
        {
            ValidateBootstrapServers(bootstrapServers);
            using var adminClient = BuildAdminClient(bootstrapServers);
            var clusterMetadata = await adminClient.DescribeClusterAsync();
            return clusterMetadata.Nodes?.Count ?? 0;
        }

        /// <summary>
        /// 根据指定的 Kafka 集群地址构建管理客户端实例。
        /// </summary>
        /// <param name="bootstrapServers">Kafka 集群地址列表。</param>
        /// <returns>可用于执行 Topic 和集群管理操作的 <see cref="IAdminClient"/> 实例。</returns>
        private static IAdminClient BuildAdminClient(string bootstrapServers)
        {
            return new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
        }

        /// <summary>
        /// 从 Topic 描述集合中提取成功查询到的 Topic 名称。
        /// </summary>
        /// <param name="topicDescriptions">Kafka 返回的 Topic 描述信息集合。</param>
        /// <returns>没有错误的 Topic 名称集合。</returns>
        private static IEnumerable<string> GetTopicNames(IEnumerable<TopicDescription> topicDescriptions)
        {
            return topicDescriptions
                .Where(topic => !string.IsNullOrWhiteSpace(topic.Name) && !topic.Error.IsError)
                .Select(topic => topic.Name);
        }

        /// <summary>
        /// 判断 Kafka 异常是否表示 Topic 或分区不存在。
        /// </summary>
        /// <param name="exception">待判断的 Kafka 异常。</param>
        /// <returns>若异常表示 Topic 或分区不存在，则返回 true；否则返回 false。</returns>
        private static bool IsTopicNotFoundError(KafkaException exception)
        {
            return exception.Error.Code == ErrorCode.UnknownTopicOrPart
                || exception.Error.Code == ErrorCode.Local_UnknownTopic
                || exception.Error.Code == ErrorCode.Local_UnknownPartition;
        }

        /// <summary>
        /// 根据 Broker 数量为待创建的 Topic 构建默认的 Topic 配置集合。
        /// </summary>
        /// <param name="topics">待创建的 Topic 名称集合。</param>
        /// <param name="brokerCount">当前 Kafka 集群中的 Broker 节点数量。</param>
        /// <returns>用于调用 Kafka 创建接口的 Topic 规格集合。</returns>
        private static IEnumerable<TopicSpecification> BuildTopicSpecifications(IEnumerable<string> topics, int brokerCount)
        {
            var replicationFactor = brokerCount <= 1 ? 1 : (brokerCount >= 3 ? 3 : brokerCount);
            var partitionCount = Math.Max(1, brokerCount * 3);
            return topics.Select(topic => new TopicSpecification
            {
                Name = topic,
                NumPartitions = partitionCount,
                ReplicationFactor = (short)replicationFactor,
                Configs = new Dictionary<string, string>
                {
                    { "retention.ms", "604800000" },
                    { "cleanup.policy", "delete" },
                }
            }).ToArray();
        }

        /// <summary>
        /// 校验 Kafka 集群地址参数是否有效。
        /// </summary>
        /// <param name="bootstrapServers">待校验的 Kafka 集群地址列表。</param>
        /// <exception cref="ArgumentNullException">当 <paramref name="bootstrapServers"/> 为 null 时抛出。</exception>
        /// <exception cref="ArgumentException">当 <paramref name="bootstrapServers"/> 为空字符串或仅包含空白字符时抛出。</exception>
        private static void ValidateBootstrapServers(string bootstrapServers)
        {
            if (bootstrapServers == null)
            {
                throw new ArgumentNullException(nameof(bootstrapServers));
            }

            if (string.IsNullOrWhiteSpace(bootstrapServers))
            {
                throw new ArgumentException("BootstrapServers 不能为空或仅包含空白字符", nameof(bootstrapServers));
            }
        }
    }
}