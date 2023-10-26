using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Redis无序集合仓储
    /// </summary>
    public class RedisSetRepository : RedisRepository, IBaseAutofac
    {
        /// <summary>
        /// 移除集合中一个或多个成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<long> RemAsync<T>(string key, params T[] values)
            => await Client.SRemAsync(key, values);

        /// <summary>
        /// 移除并返回集合中的一个随机元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<T> PopAsync<T>(string key)
            => await Client.SPopAsync<T>(key);

        /// <summary>
        /// 返回集合中一个或多个随机数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="count">
        /// 成员数量
        ///  如果count为正数且小于集合基数，那么命令返回一个包含count个元素的数组，数组中的元素各不相同。
        ///  如果count大于等于集合基数，那么返回整个集合
        /// </param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> RandMemberAsync<T>(string key, int count)
            => await Client.SRandMembersAsync<T>(key, count);

        /// <summary>
        /// 获取集合的成员数
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task GetCountAsync(string key)
            => await Client.SCardAsync(key);

        /// <summary>
        /// 获取集合的所有成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(string key)
            => await Client.SMembersAsync<T>(key);

        /// <summary>
        /// 计算多个集合的差集
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> DiffAsync<T>(params string[] keys)
            => await Client.SDiffAsync<T>(keys);

        /// <summary>
        /// 计算多个集合的交集
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> IntersectAsync<T>(params string[] keys)
            => await Client.SInterAsync<T>(keys);

        /// <summary>
        /// 计算多个集合的并集
        /// </summary>
        /// <param name="keys">键集合</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> UnionAsync<T>(params string[] keys)
            => await Client.SUnionAsync<T>(keys);

        /// <summary>
        /// 判断成员元素是否是集合的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<bool> SisMemberAsync<T>(string key, T value)
            => await Client.SIsMemberAsync(key, value);

        /// <summary>
        /// 将一个或多个成员元素加入到集合中
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值集</param>
        /// <param name="expireSeconds">过期时间</param>
        /// <returns></returns>
        public async Task<long> AddAsync<T>(string key, T value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(SetScriptConstant.SADD, key, value, expireSeconds);

        /// <summary>
        /// 将一个或多个成员元素加入到集合中
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值集</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> AddAsync<T>(string key, T value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(SetScriptConstant.SADD, key, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 将一个或多个成员元素加入到集合中
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="values">值集合</param>
        /// <param name="expireSeconds">过期时间</param>
        /// <param name="pageSize">分页的大小（默认：200一个批次）</param>
        /// <returns></returns>
        public async Task<bool> AddAsync<T>(string key, IEnumerable<T> values, int expireSeconds = CacheConstant.EXPIRESECONDS, int pageSize = 200)
        {
            await values.ToPageDictionary(pageSize).ForEachAsync(e => Client.ExecuteScriptAsync<long>(SetScriptConstant.SADD, key, e.Value, expireSeconds));
            return true;
        }

        /// <summary>
        /// 将一个或多个成员元素加入到集合中
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
        public async Task<bool> AddAsync<T>(string key, IEnumerable<T> values, int minExpireSeconds, int maxExpireSeconds, int pageSize = 200)
        {
            return await AddAsync<T>(key, values, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));
        }
    }
}