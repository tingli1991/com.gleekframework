using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Redis列表仓储
    /// </summary>
    public class RedisListRepository : RedisRepository, IBaseAutofac
    {
        /// <summary>
        /// 获取列表的长度
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(string key)
            => await Client.LLenAsync(key);

        /// <summary>
        /// 获取列表的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引值</param>
        /// <returns></returns>
        public T Get<T>(string key, long index)
            => Client.LIndex<T>(key, index);

        /// <summary>
        /// 获取列表的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引值</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key, long index)
            => await Client.LIndexAsync<T>(key, index);

        /// <summary>
        /// 获取列表中指定区间内的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="start">起始位置</param>
        /// <param name="stop">结束位置</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetRangeAsync<T>(string key, long start, long stop)
            => await Client.LRangeAsync<T>(key, start, stop);

        /// <summary>
        /// 删除并返回List的头部元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<T> LPopAsync<T>(string key)
            => await Client.LPopAsync<T>(key);

        /// <summary>
        /// 删除并返回List的尾部元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<T> RPopAsync<T>(string key)
            => await Client.RPopAsync<T>(key);

        /// <summary>
        /// 从List中删除指定数量的元素(根据参数 count 的值，移除列表中与参数 value 相等的元素)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="count">移除的数量，大于0时从表头删除数量count，小于0时从表尾删除数量-count，等于0移除所有</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<long> RemAsync<T>(string key, long count, T value)
            => await Client.LRemAsync(key, count, value);

        /// <summary>
        /// 通过索引设置列表元素的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="index">索引</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, long index, T value)
            => await Client.LSetAsync(key, index, value);

        /// <summary>
        /// 将一个或多个值插入到列表头部
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值集</param>
        /// <param name="expireSeconds">过期时间</param>
        /// <returns></returns>
        public async Task<long> LPushAsync<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(ListScriptConstant.LPUSH, key, value, expireSeconds);

        /// <summary>
        /// 将一个或多个值插入到列表头部
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值集</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> LPushAsync<T>(string key, T value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(ListScriptConstant.LPUSH, key, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 将一个或多个值插入到列表头部
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="values">值集合</param>
        /// <param name="expireSeconds">过期时间</param>
        /// <param name="pageSize">分页的大小（默认：200一个批次）</param>
        /// <returns></returns>
        public async Task<bool> LPushAsync<T>(string key, IEnumerable<T> values, int expireSeconds = CacheConstant.EXPIRESECONDS, int pageSize = 200)
        {
            await values.ToPageDictionary(pageSize).ForEachAsync(e => Client.ExecuteScriptAsync<long>(ListScriptConstant.LPUSH, key, e.Value, expireSeconds));
            return true;
        }

        /// <summary>
        /// 将一个或多个值插入到列表头部
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="values">值集合</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <param name="pageSize">分页的大小（默认：200一个批次）</param>
        /// <returns></returns>
        public async Task<bool> LPushAsync<T>(string key, IEnumerable<T> values, int minExpireSeconds, int maxExpireSeconds, int pageSize = 200)
            => await LPushAsync<T>(key, values, GetExpireSeconds(minExpireSeconds, maxExpireSeconds), pageSize);

        /// <summary>
        /// 将一个或多个值插入到列表尾部
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值集</param>
        /// <param name="expireSeconds">过期时间</param>
        /// <returns></returns>
        public async Task<long> RPushAsync<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(ListScriptConstant.RPUSH, key, value, expireSeconds);

        /// <summary>
        /// 将一个或多个值插入到列表尾部
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值集</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> RPushAsync<T>(string key, T value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(ListScriptConstant.RPUSH, key, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 将一个或多个值插入到列表尾部
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="values">值集合</param>
        /// <param name="expireSeconds">过期时间</param>
        /// <param name="pageSize">分页的大小（默认：200一个批次）</param>
        /// <returns></returns>
        public async Task<bool> RPushAsync<T>(string key, IEnumerable<T> values, int expireSeconds = CacheConstant.EXPIRESECONDS, int pageSize = 200)
        {
            await values.ToPageDictionary(pageSize).ForEachAsync(e => Client.ExecuteScriptAsync<long>(ListScriptConstant.RPUSH, key, e.Value, expireSeconds));
            return true;
        }

        /// <summary>
        /// 将一个或多个值插入到列表尾部
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="values">值集合</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <param name="pageSize">分页的大小（默认：200一个批次）</param>
        /// <returns></returns>
        public async Task<bool> RPushAsync<T>(string key, IEnumerable<T> values, int minExpireSeconds, int maxExpireSeconds, int pageSize = 200)
            => await RPushAsync<T>(key, values, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));
    }
}