using Com.GleekFramework.AutofacSdk;
using CSRedis;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Redis分布式锁仓储
    /// </summary>
    public class RedisLockRepository : RedisRepository, IBaseAutofac
    {
        /// <summary>
        /// 通过SetNx的方式上锁(锁固定的时长)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">缓存过期时间(单位：秒)</param>
        /// <returns></returns>
        public bool SetNx(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
            => Client.Set(key, 1, expireSeconds, RedisExistence.Nx);

        /// <summary>
        /// 通过SetNx的方式上锁(锁固定的时长)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">缓存过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetNxAsync(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
            => await Client.SetAsync(key, 1, expireSeconds, RedisExistence.Nx);

        /// <summary>
        /// 原子递增的方式上锁(锁固定的时长,需要调用Decrement方法手动释放)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public long Increment(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
            => Client.ExecuteScript<long>(StringScriptConstant.Increment, key, 1, expireSeconds);

        /// <summary>
        /// 原子递增(锁固定的时长,需要调用DecrementAsync方法手动释放)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> IncrementAsync(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(StringScriptConstant.Increment, key, 1, expireSeconds);

        /// <summary>
        /// 释放原子锁
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public long Decrement(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
            => Client.ExecuteScript<long>(StringScriptConstant.Decrement, key, 1, expireSeconds);

        /// <summary>
        /// 释放原子锁
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> DecrementAsync(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(StringScriptConstant.Decrement, key, 1, expireSeconds);

        /// <summary>
        /// 排队锁(过期时间内等待)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">缓存时间(单位：秒)</param>
        /// <returns></returns>
        public CSRedisClientLock LockUp(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
            => Client.Lock(key, expireSeconds, true);

        /// <summary>
        /// 排队锁(过期时间内等待)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">缓存时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<CSRedisClientLock> LockUpAsync(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
            => await Task.FromResult(Client.Lock(key, expireSeconds, true));

        /// <summary>
        /// 分布式锁(不等待)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">缓存时间(单位：秒)</param>
        /// <returns></returns>
        public CSRedisClientLockNx LockNx(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
        {
            CSRedisClientLockNx lockClient = null;
            try
            {
                var isSuccess = SetNx(key, expireSeconds);
                if (isSuccess)
                {
                    //创建客户端
                    lockClient = new CSRedisClientLockNx(key, Client);
                }
            }
            catch (Exception)
            {
                Delete(key);
            }
            return lockClient;
        }

        /// <summary>
        /// 分布式锁(不等待)
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">缓存时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<CSRedisClientLockNx> LockNxAsync(string key, int expireSeconds = CacheConstant.LOCK_EXPIRESECONDS)
        {
            CSRedisClientLockNx lockClient = null;
            try
            {
                var isSuccess = await SetNxAsync(key, expireSeconds);
                if (isSuccess)
                {
                    //创建客户端
                    lockClient = new CSRedisClientLockNx(key, Client);
                }
            }
            catch (Exception)
            {
                await DeleteAsync(key);
            }
            return lockClient;
        }
    }
}