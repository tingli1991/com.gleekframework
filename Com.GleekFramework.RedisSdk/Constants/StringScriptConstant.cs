namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// Lua脚本
    /// </summary>
    public static partial class StringScriptConstant
    {
        /// <summary>
        /// String类型原子性递增
        /// </summary>
        public static readonly string Increment = $@"
		if(redis.call('exists',KEYS[1])==1)
		then
			local result=redis.call('incrby',KEYS[1],ARGV[1]);
			if(redis.call('ttl',KEYS[1])==-1)
			then
			  redis.call('expire',KEYS[1],ARGV[2]);
			end
			return tonumber(result)
		else
   			local result=redis.call('incrby',KEYS[1],ARGV[1]);
			redis.call('expire',KEYS[1],ARGV[2]);
			return tonumber(result)
		end";

        /// <summary>
        /// String类型原子性递减
        /// </summary>
        public static readonly string Decrement = $@"
		if(redis.call('exists',KEYS[1])==1)
		then
			local result=redis.call('decrby',KEYS[1],ARGV[1]);
			if(redis.call('ttl',KEYS[1])==-1)
			then
			  redis.call('expire',KEYS[1],ARGV[2]);
			end
			return tonumber(result)
		else
   			local result=redis.call('decrby',KEYS[1],ARGV[1]);
			redis.call('expire',KEYS[1],ARGV[2]);
			return tonumber(result)
		end";
    }
}