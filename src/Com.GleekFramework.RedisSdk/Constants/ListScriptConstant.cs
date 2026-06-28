namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// 集合类型的Lua脚本
    /// </summary>
    public static class ListScriptConstant
    {
        /// <summary>
        /// 将一个或多个值插入到列表头部
        /// </summary>
        public static readonly string LPUSH = $@"
        local result
        local expiration = ARGV[#ARGV]
        local values = {{unpack(ARGV, 1, #ARGV-1)}}
        if redis.call('EXISTS', 'KEYS[1]') == 0 then
            result = tonumber(redis.call('LPUSH', 'KEYS[1]', unpack(values)))
            redis.call('EXPIRE', 'KEYS[1]', expiration)
        else
            result = tonumber(redis.call('LPUSH', 'KEYS[1]', unpack(values)))
        end
        return result";

        /// <summary>
        /// 将一个或多个值插入到列表尾部
        /// </summary>
        public static readonly string RPUSH = $@"
        local result
        local expiration = ARGV[#ARGV]
        local values = {{unpack(ARGV, 1, #ARGV-1)}}
        if redis.call('EXISTS', 'KEYS[1]') == 0 then
            result = tonumber(redis.call('RPUSH', 'KEYS[1]', unpack(values)))
            redis.call('EXPIRE', 'KEYS[1]', expiration)
        else
            result = tonumber(redis.call('RPUSH', 'KEYS[1]', unpack(values)))
        end
        return result";
    }
}