using RabbitMQ.Client;
using System.Collections.Generic;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 应答队列声明实现类
    /// </summary>
    public static partial class ReplyQueueProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 缓存列表
        /// </summary>
        private static readonly Dictionary<string, string> CacheList = new Dictionary<string, string>();

        /// <summary>
        /// 声明应答的队列名称
        /// </summary>
        /// <param name="channel">通道</param>
        /// <param name="queueName">生产的队列名称</param>
        /// <returns></returns>
        public static string QueueDeclare(IModel channel, string queueName)
        {
            if (!CacheList.ContainsKey(queueName))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(queueName))
                    {
                        var replyQueueName = $"{queueName}.reply";
                        channel.QueueDeclare(queue: replyQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                        CacheList.Add(queueName, replyQueueName);
                    }
                }
            }
            return CacheList[queueName];
        }
    }
}