using Com.GleekFramework.RedisSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Redis
{
    /// <summary>
    /// Redis SDK 类型单元测试
    /// </summary>
    public class RedisTypeTests : BaseUnitTest
    {
        /// <summary>
        /// CSRedisClientLockNx 类型存在（构造函数需参数）
        /// </summary>
        [Fact(DisplayName = "CSRedisClientLockNx类型存在")]
        public void LockNxTypeExists() =>
            Assert.NotNull(typeof(CSRedisClientLockNx));

        /// <summary>
        /// MemoryCacheRepository 可实例化（内存缓存，无需 Redis 服务）
        /// </summary>
        [Fact(DisplayName = "MemoryCacheRepository可实例化")]
        public void MemoryCacheRepoCanInstantiate() =>
            Assert.NotNull(new MemoryCacheRepository());

        /// <summary>
        /// CacheConstant 静态类型存在
        /// </summary>
        [Fact(DisplayName = "CacheConstant类型存在")]
        public void CacheConstantExists() =>
            Assert.NotNull(typeof(CacheConstant));
    }
}
