using CSRedis;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Lua脚本拓展类
    /// </summary>
    public static class ScriptExtensions
    {
        /// <summary>
        /// 执行lua脚本
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="script">lua脚本</param>
        /// <param name="key">缓存键值</param>
        /// <param name="args">lua脚本参数列表</param>
        /// <returns></returns>
        public static T ExecuteScript<T>(this CSRedisClient client, string script, string key, params object[] args)
        {
            return ScriptProvider.ExecuteScript<T>(client, script, key, args);
        }

        /// <summary>
        /// 执行lua脚本
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="script">lua脚本</param>
        /// <param name="key">缓存键值</param>
        /// <param name="args">lua脚本参数列表</param>
        /// <returns></returns>
        public static async Task<T> ExecuteScriptAsync<T>(this CSRedisClient client, string script, string key, params object[] args)
        {
            return await ScriptProvider.ExecuteScriptAsync<T>(client, script, key, args);
        }
    }
}