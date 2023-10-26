using Com.GleekFramework.CommonSdk;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace Com.GleekFramework.RabbitMQSdk
{
    /// <summary>
    /// 连接对象实现类
    /// </summary>
    public static class ConnectionProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 通道列表
        /// </summary>
        private static readonly Dictionary<string, IModel> ChannelList = new Dictionary<string, IModel>();

        /// <summary>
        /// 缓存列表
        /// </summary>
        private static readonly Dictionary<string, IConnection> CacheList = new Dictionary<string, IConnection>();

        /// <summary>
        /// 获取连接通道
        /// </summary>
        /// <param name="host">主机配置</param>
        /// <returns></returns>
        public static IModel GetChannel(string host)
        {
            if (!ChannelList.ContainsKey(host))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(host))
                    {
                        var connection = GetConnection(host);
                        var channel = connection.CreateModel();
                        ChannelList.Add(host, channel);
                    }
                }
            }
            return ChannelList[host];
        }

        /// <summary>
        /// 获取连接对象
        /// </summary>
        /// <param name="host">主机配置</param>
        /// <returns></returns>
        private static IConnection GetConnection(string host)
        {
            if (!CacheList.ContainsKey(host))
            {
                lock (@lock)
                {
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
                        CacheList.Add(host, factory.CreateConnection());
                    }
                }
            }
            return CacheList[host];
        }
    }
}