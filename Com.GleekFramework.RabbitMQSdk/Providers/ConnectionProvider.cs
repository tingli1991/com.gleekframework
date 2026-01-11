using Com.GleekFramework.CommonSdk;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 连接对象实现类
    /// </summary>
    public static class ConnectionProvider
    {
        /// <summary>
        /// 定义一个异步锁，初始为1表示只有一个线程可以进入
        /// </summary>
        private static readonly SemaphoreSlim AsyncLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 通道列表
        /// </summary>
        private static readonly Dictionary<string, IChannel> ChannelList = new Dictionary<string, IChannel>();

        /// <summary>
        /// 缓存列表
        /// </summary>
        private static readonly Dictionary<string, IConnection> CacheList = new Dictionary<string, IConnection>();

        /// <summary>
        /// 获取连接通道
        /// </summary>
        /// <param name="host">主机配置</param>
        /// <returns></returns>
        public static async Task<IChannel> GetChannelAsync(string host)
        {
            try
            {
                if (!ChannelList.ContainsKey(host))
                {
                    await AsyncLock.WaitAsync();//获取异步锁
                    if (!CacheList.ContainsKey(host))
                    {
                        var connection = await GetConnectionAsync(host);
                        var channel = await connection.CreateChannelAsync();
                        ChannelList.Add(host, channel);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                AsyncLock.Release();//释放异步锁
            }
            return ChannelList[host];
        }

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <param name="host">主机配置</param>
        /// <returns></returns>
        private static async Task<IConnection> GetConnectionAsync(string host)
        {
            try
            {
                if (!CacheList.ContainsKey(host))
                {
                    await AsyncLock.WaitAsync();//获取异步锁
                    if (!CacheList.ContainsKey(host))
                    {
                        var options = host.ToConnectionObject<RabbitConnectionOptions>();
                        if (options == null || options.Host == null)
                        {
                            throw new ArgumentNullException(nameof(options));
                        }

                        var factory = new ConnectionFactory()
                        {
                            Port = options.Port,
                            HostName = options.Host,
                            Password = options.Password,
                            UserName = options.UserName,
                            VirtualHost = options.VirtualHost,
                        };
                        CacheList.Add(host, await factory.CreateConnectionAsync());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                AsyncLock.Release();//释放异步锁
            }
            return CacheList[host];
        }
    }
}