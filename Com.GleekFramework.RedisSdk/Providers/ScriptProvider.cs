using CSRedis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Lua脚本执行实现类
    /// </summary>
    public static class ScriptProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// Lua脚本字典缓存
        /// </summary>
        private static readonly Dictionary<string, string> ClientCache = new Dictionary<string, string>();

        /// <summary>
        /// 执行lua脚本
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="script">lua脚本</param>
        /// <param name="key">缓存键值</param>
        /// <param name="args">lua脚本参数列表</param>
        /// <returns></returns>
        public static T ExecuteScript<T>(CSRedisClient client, string script, string key, params object[] args)
        {
            var sha = GetSha(client, script);
            if (string.IsNullOrEmpty(sha))
            {
                return default;
            }
            return (T)client.EvalSHA(sha, key, args);
        }

        /// <summary>
        /// 执行lua脚本
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="script">lua脚本</param>
        /// <param name="key">缓存键值</param>
        /// <param name="args">lua脚本参数列表</param>
        /// <returns></returns>
        public static async Task<T> ExecuteScriptAsync<T>(CSRedisClient client, string script, string key, params object[] args)
        {
            var sha = GetSha(client, script);
            if (string.IsNullOrEmpty(sha))
            {
                return default;
            }
            return (T)(await client.EvalSHAAsync(sha, key, args));
        }

        /// <summary>
        /// 获取lua脚本的sha值
        /// </summary>
        /// <param name="client">redis客户端</param>
        /// <param name="script">lua脚本</param>
        /// <returns></returns>
        private static string GetSha(CSRedisClient client, string script)
        {
            if (!ClientCache.ContainsKey(script))
            {
                lock (@lock)
                {
                    if (!ClientCache.ContainsKey(script))
                    {
                        var sha = client.ScriptLoad(script);
                        ClientCache.Add(script, sha);
                    }
                }
            }
            return ClientCache[script];
        }
    }
}