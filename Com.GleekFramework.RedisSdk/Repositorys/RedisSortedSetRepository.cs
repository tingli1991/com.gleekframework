using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Redis有序集合仓储
    /// </summary>
    public class RedisSortedSetRepository : RedisRepository, IBaseAutofac
    {
        /// <summary>
        /// 移除有序集合中的一个或多个成员
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="members"></param>
        /// <returns></returns>
        public async Task<long> RemAsync(string key, params string[] members)
            => await Client.ZRemAsync(key, members);

        /// <summary>
        /// 移除有序集合中给定的分数区间的所有成员
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="min">分数最小值</param>
        /// <param name="max">分数最大值</param>
        /// <returns></returns>
        public async Task<long> RemRangeByScoreAsync(string key, decimal min, decimal max)
            => await Client.ZRemRangeByScoreAsync(key, min, max);

        /// <summary>
        /// 返回有序集中指定区间内的成员，通过索引，分数从高到底
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start">开始位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <param name="stop">结束位置，0表示第一个元素，-1表示最后一个元素</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetRevRangeAsync(string key, long start, long stop)
            => await Client.ZRevRangeAsync(key, start, stop);

        /// <summary>
        /// 返回有序集中指定分数区间内的成员，分数从高到低排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="min">分数最小值</param>
        /// <param name="max">分数最大值</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetRangeByScoreAsync<T>(string key, long min, long max)
            => await Client.ZRevRangeByScoreAsync(key, max, min);

        /// <summary>
        /// 获取有序集合的成员数
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(string key)
            => await Client.ZCardAsync(key);

        /// <summary>
        /// 计算有序集合中指定分数区间的成员数量
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="min">分数最小值</param>
        /// <param name="max">区间结束位置</param>
        /// <returns></returns>
        public async Task<long> GetCountAsync(string key, decimal min, decimal max)
            => await Client.ZCountAsync(key, min, max);

        /// <summary>
        /// 返回成员的索引
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public async Task<long> GetMemberIndexAsync(string key, string member)
            => await Client.ZRankAsync(key, member) ?? 0L;

        /// <summary>
        /// 通过分数返回有序集合指定区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="min">分数最小值</param>
        /// <param name="max">分数最大值</param>
        /// <param name="count">返回多少成员</param>
        /// <param name="offset">返回条件偏移位置</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetRangeByScoreAsync(string key, decimal min, decimal max, long? count = null, long offset = 0L)
            => await Client.ZRangeByScoreAsync(key, min, max, count, offset);

        /// <summary>
        /// 通过索引区间返回有序集合成指定区间内的成员
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="start">起始位置</param>
        /// <param name="stop">结束位置</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetRangeAsync(string key, long start, long stop)
            => await Client.ZRangeAsync(key, start, stop);

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值集</param>
        /// <param name="expireSeconds">过期时间</param>
        /// <returns></returns>
        public async Task<long> AddAsync(string key, string value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(RedisSortedScriptConstant.ZADD, key, value, expireSeconds);

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值集</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> AddAsync(string key, string value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(RedisSortedScriptConstant.ZADD, key, value, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 将一个或多个成员元素及其分数值加入到有序集当中
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值集合</param>
        /// <param name="expireSeconds">过期时间</param>
        /// <param name="pageSize">分页的大小（默认：200一个批次）</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, IDictionary<string, decimal> values, int expireSeconds = CacheConstant.EXPIRESECONDS, int pageSize = 200)
        {
            IEnumerable<object> args = new List<object>();
            var pageList = values.ToPageDictionary(pageSize);
            foreach (var pageInfo in pageList)
            {
                args = args.Add(pageInfo.Value);
                args = args.Add(pageInfo.Key);
            }
            await pageList.ForEachAsync(e => Client.ExecuteScriptAsync<long>(RedisSortedScriptConstant.ZADD, key, args, expireSeconds));
            return true;
        }

        /// <summary>
        /// 将一个或多个成员元素及其分数值加入到有序集当中
        ///    如果key不存在，一个空列表会被创建并执行LPUSH操作,并设置List的过期时间
        ///    当key存在但不是列表类型时，返回一个错误
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="values">值集合</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <param name="pageSize">分页的大小（默认：200一个批次）</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(string key, IDictionary<string, decimal> values, int minExpireSeconds, int maxExpireSeconds, int pageSize = 200)
        {
            var expireSeconds = GetExpireSeconds(minExpireSeconds, maxExpireSeconds);
            return await AddAsync(key, values, expireSeconds, pageSize);
        }

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public long Increment(string key, string member, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => Client.ExecuteScript<long>(RedisSortedScriptConstant.Increment, key, value, member, expireSeconds);

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public long Increment(string key, string member, long value, int minExpireSeconds, int maxExpireSeconds)
            => Client.ExecuteScript<long>(RedisSortedScriptConstant.Increment, key, value, member, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> IncrementAsync(string key, string member, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(RedisSortedScriptConstant.Increment, key, value, member, expireSeconds);

        /// <summary>
        /// 原子递增
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> IncrementAsync(string key, string member, long value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(RedisSortedScriptConstant.Increment, key, value, member, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public long Decrement(string key, string member, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => Client.ExecuteScript<long>(RedisSortedScriptConstant.Increment, key, -value, member, expireSeconds);

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public long Decrement(string key, string member, long value, int minExpireSeconds, int maxExpireSeconds)
            => Client.ExecuteScript<long>(RedisSortedScriptConstant.Increment, key, -value, member, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">缓存值</param>
        /// <param name="expireSeconds">过期时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> DecrementAsync(string key, string member, long value, int expireSeconds = CacheConstant.EXPIRESECONDS)
            => await Client.ExecuteScriptAsync<long>(RedisSortedScriptConstant.Increment, key, -value, member, expireSeconds);

        /// <summary>
        /// 原子递减
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="member">成员</param>
        /// <param name="value">缓存值</param>
        /// <param name="minExpireSeconds">最小的超时时间(单位：秒)</param>
        /// <param name="maxExpireSeconds">最大的超时时间(单位：秒)</param>
        /// <returns></returns>
        public async Task<long> DecrementAsync(string key, string member, long value, int minExpireSeconds, int maxExpireSeconds)
            => await Client.ExecuteScriptAsync<long>(RedisSortedScriptConstant.Increment, key, -value, member, GetExpireSeconds(minExpireSeconds, maxExpireSeconds));
    }
}