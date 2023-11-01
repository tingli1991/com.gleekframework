using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// 内存缓存对象
    /// </summary>
    public partial class MemoryCacheRepository : IBaseAutofac
    {
        /// <summary>
        /// 内存缓存对象
        /// </summary>
        private static readonly IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public void Remove(string key)
            => cache.Remove(key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        public async Task RemoveAsync(string key)
        {
            Remove(key);
            await Task.CompletedTask;
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            var bytes = cache.Get<byte[]>(key);
            return bytes.DeserializeObject<T>();
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            return await Task.FromResult(Get<T>(key));
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
        {
            var bytes = value.SerializeBinary();
            cache.Set(key, bytes, TimeSpan.FromSeconds(expireSeconds));
            return true;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Task.FromResult(Set(key, value, expireSeconds));

        /// <summary>
        /// 获取并创建(没值的时候才会创建)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public T GetOrCreate<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
        {
            var response = cache.GetOrCreate(key, entry =>
            {
                var bytes = value.SerializeBinary();
                entry.Value = bytes;
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireSeconds);
                return bytes;
            });
            return response.DeserializeObject<T>();
        }

        /// <summary>
        /// 获取并创建(没值的时候才会创建)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<T> GetOrCreateAsync<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
        {
            var response = await cache.GetOrCreateAsync(key, async entry =>
            {
                var bytes = value.SerializeBinary();
                entry.Value = bytes;
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireSeconds);
                return await Task.FromResult(bytes);
            });
            return response.DeserializeObject<T>();
        }
    }
}