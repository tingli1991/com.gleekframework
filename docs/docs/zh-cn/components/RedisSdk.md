## 项目

> Com.GleekFramework.RedisSdk

## 依赖

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.AutofacSdk

## 概述

Redis 拓展工具包，支持内存缓存以及常用的 redis 数据类型操作，能够满足绝大部分的应用场景。

- 支持内存缓存操作`MemoryCacheRepository`
- 支持 Hash 缓存操作`RedisHashRepository`
- 支持 List 缓存操作`RedisListRepository`
- 支持分布式锁的应用场景(等待锁，拦截锁、原子递增、原子递减) `RedisLockRepository`
- 支持 Set 缓存操作 `RedisSetRepository`
- 支持 SortedSet 缓存操作 `RedisSortedSetRepository`
- 支持 字符串 缓存操作 `RedisStringRepository`
- 所有的操作默认都会有过期时间，默认为：`604800` 秒
- 部分类型和场景增加`ProtoBuf`压缩，例如：String
- 支持单机、哨兵以及集群模式

## 注册 Redis

使用`UseCsRedis()`方法即可

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.RedisSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 程序类
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 程序主函数
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// 创建系统主机
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateDefaultHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseAutofac()
            .UseConfig()
            .UseCsRedis()
            .UseNacosConf()
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

## 基本类型使用

这里以内存、`String`和分布式锁为例，其他请参阅`RedisHashRepository`、`RedisListRepository`、 `RedisSetRepository`、`RedisSortedSetRepository`

### String 缓存

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public RedisStringRepository RedisStringRepository { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var expireSeconds = 604800;
            var cacheKey = "gleekframework:test;weatherforecastmodel";

            // 获取缓存
            var weatherForecastInfo = await RedisStringRepository.GetAsync<WeatherForecastModel>(cacheKey);

            //设置缓存
            weatherForecastInfo = new WeatherForecastModel() { Date = DateTime.Now, Summary = "测试", TemperatureC = 100 };
            await RedisStringRepository.SetAsync(cacheKey, weatherForecastInfo, expireSeconds);

            //删除缓存
            await RedisStringRepository.DeleteAsync(cacheKey);

            //设置返回结果
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

### 内存缓存

!> 内存缓存，应用的时候需要注意，业务本身是可以接受数据延迟(或者数据差异)才能进行使用，否则在集群环境下，会导致各种数据不一致的问题。

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public MemoryCacheRepository MemoryCacheRepository { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var expireSeconds = 10;
            var cacheKey = "gleekframework:test;weatherforecastmodel";

            // 获取缓存
            var weatherForecastInfo = await MemoryCacheRepository.GetAsync<WeatherForecastModel>(cacheKey);

            //设置缓存
            weatherForecastInfo = new WeatherForecastModel() { Date = DateTime.Now, Summary = "测试", TemperatureC = 100 };
            await MemoryCacheRepository.SetAsync(cacheKey, weatherForecastInfo, expireSeconds);

            //删除缓存
            await MemoryCacheRepository.RemoveAsync(cacheKey);

            //设置返回结果
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

### 分布式锁

#### 等待锁

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public RedisLockRepository RedisLockRepository { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var expireSeconds = 3;
            var cacheKey = "gleekframework:test:lock";
            using (var lockClient = await RedisLockRepository.LockUpAsync(cacheKey, expireSeconds))
            {
                if (lockClient == null)
                {
                    //加锁失败，失败原因：通常都是超过等待市场，例如：3秒钟的等待时长
                }

                //加锁成功，处理正常的业务代码逻辑
            }

            //设置返回结果
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

#### 排他锁

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public RedisLockRepository RedisLockRepository { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var result = new ContractResult<WeatherForecastModel>();

            var expireSeconds = 3;
            var cacheKey = "gleekframework:test:lock";
            using (var lockClient = await RedisLockRepository.LockNxAsync(cacheKey, expireSeconds))
            {
                if (lockClient == null)
                {
                    //加锁失败，失败原因：资源被锁住（通常这里提示并发请求类错误）
                    return result.SetError(MessageCode.PARAM_REQUIRED_DATE);
                }

                //加锁成功，处理正常的业务代码逻辑
            }

            //设置返回结果
            return result.SetSuceccful();
        }
    }
}
```

#### 原子锁

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public RedisLockRepository RedisLockRepository { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var result = new ContractResult<WeatherForecastModel>();

            var expireSeconds = 30;
            var cacheKey = "gleekframework:test:lock";

            //原子递增判断并发锁
            var number = await RedisLockRepository.IncrementAsync(cacheKey, expireSeconds);
            if (number > 1)
            {
                //资源被锁住（通常这里提示并发请求类错误）
                return result.SetError(MessageCode.PARAM_REQUIRED_DATE);
            }

            //加锁成功，处理正常的业务代码逻辑

            //释放锁（最好放在finally里面，防止异常的情况下锁么有被正常释放)
            await RedisLockRepository.DeleteAsync(cacheKey);

            //设置返回结果
            return result.SetSuceccful();
        }
    }
}
```
