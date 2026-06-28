using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 应答队列声明实现类
    /// </summary>
    public static partial class ReplyQueueProvider
    {
        /// <summary>
        /// 定义一个异步锁，初始为1表示只有一个线程可以进入
        /// </summary>
        private static readonly SemaphoreSlim AsyncLock = new SemaphoreSlim(1, 1);

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
        public static async Task<string> QueueDeclareAsync(IChannel channel, string queueName)
        {
            if (!CacheList.ContainsKey(queueName))
            {
                try
                {
                    await AsyncLock.WaitAsync();//获取异步锁
                    if (!CacheList.ContainsKey(queueName))
                    {
                        var replyQueueName = $"{queueName}.reply";
                        await channel.QueueDeclareAsync(queue: replyQueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                        CacheList.Add(queueName, replyQueueName);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    //释放异步锁
                    AsyncLock.Release();
                }
            }
            return CacheList[queueName];
        }
    }
}