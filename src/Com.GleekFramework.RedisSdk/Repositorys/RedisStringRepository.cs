using Com.GleekFramework.AutofacSdk;
using CSRedis;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Redis字符串类型仓储
    /// </summary>
    public partial class RedisStringRepository : RedisRepository, IBaseAutofac
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public T Get<T>(string key) => DeserializeObject<T>(() 
            => Client.Get<byte[]>(key));

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key) 
            => await DeserializeObjectAsync<T>(() => Client.GetAsync<byte[]>(key));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => SerializeBinary(value, bytes => Client.Set(key, bytes, expireSeconds));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, int minExpireSeconds, int maxExpireSeconds)
            => SerializeBinary(value, bytes => Client.Set(key, bytes, GetExpireSeconds(minExpireSeconds, maxExpireSeconds)));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await SerializeBinaryAsync(value, bytes => Client.SetAsync(key, bytes, expireSeconds));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T value, int minExpireSeconds, int maxExpireSeconds)
            => await SerializeBinaryAsync(value, bytes => Client.SetAsync(key, bytes, GetExpireSeconds(minExpireSeconds, maxExpireSeconds)));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public bool SetNx<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => SerializeBinary(value, bytes => Client.Set(key, bytes, expireSeconds, RedisExistence.Nx));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public bool SetNx<T>(string key, T value, int minExpireSeconds, int maxExpireSeconds)
            => SerializeBinary(value, bytes => Client.Set(key, bytes, GetExpireSeconds(minExpireSeconds, maxExpireSeconds), RedisExistence.Nx));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetNxAsync<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await SerializeBinaryAsync(value, bytes => Client.SetAsync(key, bytes, expireSeconds, RedisExistence.Nx));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetNxAsync<T>(string key, T value, int minExpireSeconds, int maxExpireSeconds)
            => await SerializeBinaryAsync(value, bytes => Client.SetAsync(key, bytes, GetExpireSeconds(minExpireSeconds, maxExpireSeconds), RedisExistence.Nx));

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public long Increment(string key, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => Client.ExecuteScript<long>(StringScriptConstant.Increment, key, value, expireSeconds);

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public long Increment(string key, long value, int minExpireSeconds, int maxExpireSeconds)
            => Client.ExecuteScript<long>(StringScriptConstant.Increment, key, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> IncrementAsync(string key, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(StringScriptConstant.Increment, key, value, expireSeconds);

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> IncrementAsync(string key, long value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(StringScriptConstant.Increment, key, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public long Decrement(string key, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => Client.ExecuteScript<long>(StringScriptConstant.Decrement, key, value, expireSeconds);

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public long Decrement(string key, long value, int minExpireSeconds, int maxExpireSeconds)
            => Client.ExecuteScript<long>(StringScriptConstant.Decrement, key, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> DecrementAsync(string key, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(StringScriptConstant.Decrement, key, value, expireSeconds);

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> DecrementAsync(string key, long value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(StringScriptConstant.Decrement, key, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));
    }
}