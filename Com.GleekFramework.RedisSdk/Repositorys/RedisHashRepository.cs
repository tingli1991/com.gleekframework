using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Redis哈希仓储类
    /// </summary>
    public partial class RedisHashRepository : RedisRepository, IBaseAutofac
    {
        /// <summary>
        /// 检查缓存Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public bool Exists(string key, string field)
            => Client.HExists(key, field);

        /// <summary>
        /// 检查缓存Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key, string field)
            => await Client.HExistsAsync(key, field);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public bool Delete(string key, string field)
            => Client.HDel(key, field) > 0;

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fields">缓存字段集合</param>
        /// <returns></returns>
        public bool Delete(string key, IEnumerable<string> fields)
        {
            fields.ToPageDictionary().ForEach(e => Client.HDel(key, e.Value.ToArray()));
            return true;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string key, string field)
            => await Client.HDelAsync(key, field) > 0;

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="fields">字段集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string key, IEnumerable<string> fields)
        {
            await fields.ToPageDictionary().ForEachAsync(e => Client.HDelAsync(key, e.Value.ToArray()));
            return true;
        }

        /// <summary>
        /// 获取Hash表的所有字段值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public Dictionary<string, T> GetAll<T>(string key)
            => Client.HGetAll<T>(key) ?? new Dictionary<string, T>();

        /// <summary>
        /// 获取Hash表的所有字段值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> GetAllAsync<T>(string key)
            => await Client.HGetAllAsync<T>(key) ?? new Dictionary<string, T>();

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public T Get<T>(string key, string field)
            => Client.HGet<T>(key, field);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key, string field)
            => await Client.HGetAsync<T>(key, field);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public bool Set<T>(string key, string field, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => Client.ExecuteScript<bool>(HashScriptConstant.Hset, key, field, value, expireSeconds);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public bool Set<T>(string key, string field, T value, int minExpireSeconds, int maxExpireSeconds)
            => Client.ExecuteScript<bool>(HashScriptConstant.Hset, key, field, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, string field, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<bool>(HashScriptConstant.Hset, key, field, value, (long)expireSeconds);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, string field, T value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<bool>(HashScriptConstant.Hset, key, field, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public bool SetNx<T>(string key, string field, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => Client.ExecuteScript<bool>(HashScriptConstant.HsetNx, key, field, value, expireSeconds);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public bool SetNx<T>(string key, string field, T value, int minExpireSeconds, int maxExpireSeconds)
            => Client.ExecuteScript<bool>(HashScriptConstant.HsetNx, key, field, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetNxAsync<T>(string key, string field, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<bool>(HashScriptConstant.HsetNx, key, field, value, expireSeconds);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetNxAsync<T>(string key, string field, T value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<bool>(HashScriptConstant.HsetNx, key, field, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public long Increment(string key, string field, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => Client.ExecuteScript<long>(HashScriptConstant.Increment, key, field, value, expireSeconds);

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public long Increment(string key, string field, long value = 1, int minExpireSeconds, int maxExpireSeconds)
            => Client.ExecuteScript<long>(HashScriptConstant.Increment, key, field, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> IncrementAsync(string key, string field, long value = 1, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(HashScriptConstant.Increment, key, field, value, expireSeconds);

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> IncrementAsync(string key, string field, long value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(HashScriptConstant.Increment, key, field, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public long Decrement(string key, string field, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => Client.ExecuteScript<long>(HashScriptConstant.Increment, key, field, -value, expireSeconds);

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public long Decrement(string key, string field, long value, int minExpireSeconds, int maxExpireSeconds)
            => Client.ExecuteScript<long>(HashScriptConstant.Increment, key, field, -value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> DecrementAsync(string key, string field, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(HashScriptConstant.Increment, key, field, -value, expireSeconds);

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="field">字段</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> DecrementAsync(string key, string field, long value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(HashScriptConstant.Increment, key, field, -value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));
    }
}