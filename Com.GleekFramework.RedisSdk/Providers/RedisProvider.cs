using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using CSRedis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Redis仓储实现类
    /// </summary>
    public static partial class RedisProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 客户字典缓存
        /// </summary>
        private static readonly Dictionary<string, CSRedisClient> ClientCache = new Dictionary<string, CSRedisClient>();

        /// <summary>
        /// 获取客户端单例
        /// </summary>
        /// <param name="connectionName">连接字符串名称</param>
        /// <returns></returns>
        public static CSRedisClient GetClientSingle(string connectionName)
        {
            if (!ClientCache.ContainsKey(connectionName))
            {
                //配置文件名称不存在，请先注入配置
                throw new NullReferenceException($"{connectionName} does not exist, please inject the configuration first");
            }
            return ClientCache[connectionName];
        }

        /// <summary>
        /// 注册客户端对象池
        /// </summary>
        /// <param name="configuration">配置对象</param>
        /// <param name="connectionNames">连接字符串名称</param>
        public static void RegisterClientPool(IConfiguration configuration, params string[] connectionNames)
        {
            if (connectionNames.IsNullOrEmpty())
            {
                //如果没传配置名称的情况下，使用默认的配置名称
                connectionNames = new string[] { CacheConstant.DEFAULT_CONNECTION_NAME };
            }

            foreach (var connectionName in connectionNames)
            {
                if (!ClientCache.ContainsKey(connectionName))
                {
                    lock (@lock)
                    {
                        if (!ClientCache.ContainsKey(connectionName))
                        {
                            var connectionString = configuration.Get(connectionName);
                            if (connectionString == null)
                            {
                                throw new ArgumentNullException($"{connectionName} does not exist");
                            }

                            var dbConnectionString = $"{connectionString.TrimEnd(',', ';')}";
                            if (!dbConnectionString.Equals("defaultDatabase", StringComparison.InvariantCultureIgnoreCase))
                            {
                                dbConnectionString += ",defaultDatabase=0";
                            }
                            ClientCache.Add(connectionName, new CSRedisClient(dbConnectionString));
                        }
                    }
                }
            }
        }
    }
}