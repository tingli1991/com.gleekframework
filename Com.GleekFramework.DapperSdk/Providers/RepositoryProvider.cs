using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// Dapper仓储实现类
    /// </summary>
    public partial class RepositoryProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 连接字符串字典缓存
        /// </summary>
        private static readonly Dictionary<string, string> ConnectionStrCache = new Dictionary<string, string>();

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="connectionName">连接字符串名称</param>
        /// <returns></returns>
        public static string GetConnectionString(string connectionName)
        {
            if (!ConnectionStrCache.ContainsKey(connectionName))
            {
                //配置文件名称不存在，请先注入配置
                throw new NullReferenceException($"{connectionName} does not exist, please inject the configuration first");
            }
            return ConnectionStrCache[connectionName];
        }

        /// <summary>
        /// 注册数据库连接字符串
        /// </summary>
        /// <param name="configuration">配置对象</param>
        /// <param name="connectionNames">连接字符串名称</param>
        public static void RegisterConnectionStrings(IConfiguration configuration, params string[] connectionNames)
        {
            if (connectionNames == null || !connectionNames.Any())
            {
                //如果没传配置名称的情况下，使用默认的配置名称
                throw new ArgumentNullException($"connectionNames does not exist");
            }

            foreach (var connectionName in connectionNames)
            {
                if (!ConnectionStrCache.ContainsKey(connectionName))
                {
                    lock (@lock)
                    {
                        if (!ConnectionStrCache.ContainsKey(connectionName))
                        {
                            var connectionString = configuration.GetValue(connectionName);
                            if (connectionString == null)
                            {
                                throw new ArgumentNullException($"{connectionName} does not exist");
                            }

                            ConnectionStrCache.Add(connectionName, connectionString);
                        }
                    }
                }
            }
        }
    }
}