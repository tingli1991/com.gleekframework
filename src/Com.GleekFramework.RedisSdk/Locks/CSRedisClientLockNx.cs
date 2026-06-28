using CSRedis;
using System;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// 
    /// </summary>
    public class CSRedisClientLockNx : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string _key;

        /// <summary>
        /// CsRedis客户端
        /// </summary>
        private readonly CSRedisClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="client"></param>
        internal CSRedisClientLockNx(string key, CSRedisClient client)
        {
            _key = key;
            _client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Unlock()
        {
            //删除缓存
            return _client.Del(_key) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Unlock();
        }
    }
}
