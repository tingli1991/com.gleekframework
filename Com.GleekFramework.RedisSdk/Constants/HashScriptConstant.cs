namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Lua脚本
    /// </summary>
    public static class HashScriptConstant
    {
        /// <summary>
        /// HsetExpire脚本脚本
        /// </summary>
        public static readonly string Hset = $@"
		if(redis.call('exists',KEYS[1])==1)
		then
			local result=redis.call('hset',KEYS[1],ARGV[1],ARGV[2]);
			if(redis.call('ttl',KEYS[1])==-1)
			then
			  redis.call('expire',KEYS[1],ARGV[3]);
			end
			return result == 1
		else
   			local result=redis.call('hset',KEYS[1],ARGV[1],ARGV[2]);	
			redis.call('expire',KEYS[1],ARGV[3]);
			return result == 1
		end";

        /// <summary>
        /// HsetExpire脚本脚本
        /// </summary>
        public static readonly string HsetNx = $@"
		if(redis.call('exists',KEYS[1])==1)
		then
			local result=redis.call('hsetnx',KEYS[1],ARGV[1],ARGV[2]);
			if(redis.call('ttl',KEYS[1])==-1)
			then
			  redis.call('expire',KEYS[1],ARGV[3]);
			end
			return result == 1
		else
   			local result=redis.call('hsetnx',KEYS[1],ARGV[1],ARGV[2]);
			redis.call('expire',KEYS[1],ARGV[3]);
			return result == 1
		end";

        /// <summary>
        /// Hash原子性递增
        /// </summary>
        public static readonly string Increment = $@"
		if(redis.call('exists',KEYS[1])==1)
		then
			local result=redis.call('hincrby',KEYS[1],ARGV[1],ARGV[2]);
			if(redis.call('ttl',KEYS[1])==-1)
			then
			  redis.call('expire',KEYS[1],ARGV[3]);
			end
			return tonumber(result)
		else
   			local result=redis.call('hincrby',KEYS[1],ARGV[1],ARGV[2]);
			redis.call('expire',KEYS[1],ARGV[3]);
			return tonumber(result)
		end";
    }
}