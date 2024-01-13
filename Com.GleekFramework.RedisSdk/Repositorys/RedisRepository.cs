using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using CSRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Redis仓储基类
    /// </summary>
    public abstract class RedisRepository : IBaseAutofac
    {
        /// <summary>
        /// 随机因子
        /// </summary>
        private static Random Random => new Random((int)DateTime.Now.ToCstTime().Ticks);

        /// <summary>
        /// 客户端
        /// </summary>
        protected CSRedisClient Client => RedisProvider.GetClientSingle(ConnectionName);

        /// <summary>
        /// 配置文件名称
        /// </summary>
        public virtual string ConnectionName { get; } = CacheConstant.DEFAULT_CONNECTION_NAME;

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public T DeserializeObject<T>(Func<byte[]> func) => func().DeserializeObject<T>();

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<T> DeserializeObjectAsync<T>(Func<Task<byte[]>> func) => (await func()).DeserializeObject<T>();

        /// <summary>
        /// 序列化方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">对象值</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public bool SerializeBinary<T>(T value, Func<byte[], bool> func) => func(value.SerializeBinary());

        /// <summary>
        /// 序列化方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">对象值</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<bool> SerializeBinaryAsync<T>(T value, Func<byte[], Task<bool>> func) => await func(value.SerializeBinary());

        /// <summary>
        /// 序列化方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="R"></typeparam>
        /// <param name="value">对象值</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<R> SerializeBinaryAsync<T, R>(T value, Func<byte[], Task<R>> func) => await func(value.SerializeBinary());

        /// <summary>
        /// 检查缓存Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key) => Client.Exists(key);

        /// <summary>
        /// 检查缓存Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key) => await Client.ExistsAsync(key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public bool Delete(string key) => Client.Del(key) > 0;

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="keys">缓存键</param>
        /// <returns></returns>
        public bool Delete(IEnumerable<string> keys)
        {
            if (keys.IsNullOrEmpty())
            {
                return false;
            }

            var pageList = keys.ToPageDictionary();
            pageList.ForEach(e => Client.Del(e.Value.ToArray()));
            return true;
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string key) => await Client.DelAsync(key) > 0;

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="keys">缓存键</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(IEnumerable<string> keys)
        {
            await keys.ToPageDictionary().ForEachAsync(e => Client.DelAsync(e.Value.ToArray()));
            return true;
        }

        /// <summary>
        /// 获取随机过期时间
        /// </summary>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public int GetExpireSeconds(int minExpireSeconds, int maxExpireSeconds) => Random.Next(minExpireSeconds, maxExpireSeconds);

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public bool SetExpire(string key, int expireSeconds = CacheConstant.EXPIRESECONDS) => Client.Expire(key, expireSeconds);

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public bool SetExpire(string key, int minExpireSeconds, int maxExpireSeconds) => Client.Expire(key, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetExpireAsync(string key, int expireSeconds = CacheConstant.EXPIRESECONDS) => await Client.ExpireAsync(key, expireSeconds);

        /// <summary>
        /// 设置过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<bool> SetExpireAsync(string key, int minExpireSeconds, int maxExpireSeconds) => await Client.ExpireAsync(key, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));
    }
}