namespace Com.GleekFramework.RedisSdk
{
    /// <summary>
    /// 缓存常量
    /// </summary>
    public static partial class CacheConstant
    {
        /// <summary>
        /// 系统默认的缓存时间(单位：秒)
        /// </summary>
        public const int EXPIRESECONDS = 604800;

        /// <summary>
        /// 默认的链接字符串名称
        /// </summary>
        public const string DEFAULT_CONNECTION_NAME = "ConnectionStrings:RedisConnectionHost";
    }
}