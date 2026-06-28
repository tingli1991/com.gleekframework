using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using ProtoBuf;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Redis
{
    /// <summary>
    /// 内存缓存仓储单元测试
    /// </summary>
    public class MemoryCacheRepositoryTests : BaseUnitTest
    {
        /// <summary>
        /// 设置缓存后能读取到相同值
        /// </summary>
        [Fact(DisplayName = "设置缓存后能读取到相同值")]
        public void SetAndGetString()
        {
            var repo = new MemoryCacheRepository();
            repo.Set("test_key_1", "hello_world");
            var value = repo.Get<string>("test_key_1");
            Assert.Equal("hello_world", value);
        }

        /// <summary>
        /// 设置缓存后异步读取到相同值
        /// </summary>
        [Fact(DisplayName = "设置缓存后异步读取到相同值")]
        public async Task SetAndGetAsyncString()
        {
            var repo = new MemoryCacheRepository();
            await repo.SetAsync("test_key_2", "async_value");
            var value = await repo.GetAsync<string>("test_key_2");
            Assert.Equal("async_value", value);
        }

        /// <summary>
        /// 缓存过期后返回默认值
        /// </summary>
        [Fact(DisplayName = "缓存过期后返回默认值")]
        public async Task ExpiredCacheReturnsDefault()
        {
            var repo = new MemoryCacheRepository();
            repo.Set("temp_key", "temp_value", expireSeconds: 1);
            Assert.Equal("temp_value", repo.Get<string>("temp_key"));
            await Task.Delay(1500);
            Assert.Null(repo.Get<string>("temp_key"));
        }

        /// <summary>
        /// 删除缓存后读取为 null
        /// </summary>
        [Fact(DisplayName = "删除缓存后读取为Null")]
        public void RemoveCacheReturnsNull()
        {
            var repo = new MemoryCacheRepository();
            repo.Set("remove_key", "will_be_removed");
            Assert.Equal("will_be_removed", repo.Get<string>("remove_key"));
            repo.Remove("remove_key");
            Assert.Null(repo.Get<string>("remove_key"));
        }

        /// <summary>
        /// 删除缓存后异步读取为 null
        /// </summary>
        [Fact(DisplayName = "删除缓存后异步读取为Null")]
        public async Task RemoveCacheAsyncReturnsNull()
        {
            var repo = new MemoryCacheRepository();
            await repo.SetAsync("remove_key2", "async_remove");
            await repo.RemoveAsync("remove_key2");
            var value = await repo.GetAsync<string>("remove_key2");
            Assert.Null(value);
        }

        /// <summary>
        /// GetOrCreate 在缓存未命中时创建
        /// </summary>
        [Fact(DisplayName = "GetOrCreate在未命中时创建")]
        public void GetOrCreateCreatesWhenNotExists()
        {
            var repo = new MemoryCacheRepository();
            var value = repo.GetOrCreate("create_key", "default_value");
            Assert.Equal("default_value", value);
        }

        /// <summary>
        /// GetOrCreate 在缓存命中时返回缓存值
        /// </summary>
        [Fact(DisplayName = "GetOrCreate在命中时返回缓存")]
        public void GetOrCreateReturnsCachedValue()
        {
            var repo = new MemoryCacheRepository();
            repo.Set("existing_key", "original");
            var value = repo.GetOrCreate("existing_key", "should_not_use");
            Assert.Equal("original", value);
        }

        /// <summary>
        /// 不存在的 key 返回 null
        /// </summary>
        [Fact(DisplayName = "不存在的Key返回Null")]
        public void NonExistentKeyReturnsNull()
        {
            var repo = new MemoryCacheRepository();
            Assert.Null(repo.Get<string>("non_existent_key"));
        }

        /// <summary>
        /// 存储整数类型并正确读取
        /// </summary>
        [Fact(DisplayName = "存储整数类型并正确读取")]
        public void SetAndGetInt()
        {
            var repo = new MemoryCacheRepository();
            repo.Set("int_key", 12345);
            Assert.Equal(12345, repo.Get<int>("int_key"));
        }

        /// <summary>
        /// 存储对象类型并正确读取（需 ProtoContract 支持）
        /// </summary>
        [Fact(DisplayName = "存储对象类型并正确读取")]
        public void SetAndGetObject()
        {
            var repo = new MemoryCacheRepository();
            var obj = new WeatherForecastModel
            {
                Date = DateTime.Now,
                TemperatureC = 25,
                Summary = "TestSummary"
            };
            repo.Set("obj_key", obj);
            var result = repo.Get<WeatherForecastModel>("obj_key");
            Assert.NotNull(result);
            Assert.Equal(25, result.TemperatureC);
            Assert.Equal("TestSummary", result.Summary);
        }
    }
}
