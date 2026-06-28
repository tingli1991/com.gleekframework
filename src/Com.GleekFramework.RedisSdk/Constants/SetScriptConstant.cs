namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// 集合数据类型Lua脚本
    /// </summary>
    public static class SetScriptConstant
    {
        /// <summary>
        /// 将一个或多个成员元素加入到集合中
        /// </summary>
        public static readonly string SADD= $@"
        local result
        local expiration = ARGV[#ARGV]
        local values = {{unpack(ARGV, 1, #ARGV-1)}}
        if redis.call('EXISTS', 'KEYS[1]') == 0 then
            result = tonumber(redis.call('SADD', 'KEYS[1]', unpack(values)))
            redis.call('EXPIRE', 'KEYS[1]', expiration)
        else
            result = tonumber(redis.call('SADD', 'KEYS[1]', unpack(values)))
        end
        return result";
    }
}