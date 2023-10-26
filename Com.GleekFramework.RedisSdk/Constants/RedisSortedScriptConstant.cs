namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// 有序集合数据类型Lua脚本
    /// </summary>
    public static partial class RedisSortedScriptConstant
    {
        /// <summary>
        /// Hash原子性递增
        /// </summary>
        public static readonly string Increment = $@"
		if(redis.call('exists',KEYS[1])==1)
		then
			local result=redis.call('ZINCRBY',KEYS[1],ARGV[1],ARGV[2]);
			if(redis.call('ttl',KEYS[1])==-1)
			then
			  redis.call('expire',KEYS[1],ARGV[3]);
			end
			return tonumber(result)
		else
   			local result=redis.call('ZINCRBY',KEYS[1],ARGV[1],ARGV[2]);
			redis.call('expire',KEYS[1],ARGV[3]);
			return tonumber(result)
		end";

        /// <summary>
        /// 向有序集合添加一个或多个成员，或者更新已存在成员的分数
        /// </summary>
        public static readonly string ZADD = $@"
        local result
        local expiration = ARGV[#ARGV]
        local values = {{unpack(ARGV, 1, #ARGV-1)}}
        local members = {{}}
        for i = 1, #values, 2 do
            local member = values[i]
            local score = values[i+1]
            table.insert(members, score)
            table.insert(members, member)
        end
        if redis.call('EXISTS', KEYS[1]) == 0 then
            result = tonumber(redis.call('ZADD', KEYS[1], unpack(members)))
            redis.call('EXPIRE', KEYS[1], expiration)
        else
            result = tonumber(redis.call('ZADD', KEYS[1], unpack(members)))
        end
        return result";
    }
}