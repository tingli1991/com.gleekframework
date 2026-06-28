## Project

> Com.GleekFramework.RedisSdk

## Dependencies

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.AutofacSdk

## Overview

Redis extension tool package, supporting memory cache and common Redis data type operations, able to meet most application scenarios.

- Supports memory cache operations `MemoryCacheRepository`
- Supports Hash cache operations `RedisHashRepository`
- Supports List cache operations `RedisListRepository`
- Supports scenarios requiring distributed locks (waiting for locks, interception locks, atomic increment, atomic decrement) `RedisLockRepository`
- Supports Set cache operations `RedisSetRepository`
- Supports SortedSet cache operations `RedisSortedSetRepository`
- Supports String cache operations `RedisStringRepository`
- All operations will have an expiration time by default, which is: `604800` seconds
- Adds `ProtoBuf` compression to certain types and scenes, for example: String
- Supports standalone, sentinel, and cluster modes

## Register Redis

Use the `UseCsRedis()` method to register.

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.RedisSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main function of the program
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// Create system host
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

## Using Basic Types

Here, memory, String, and distributed lock are taken as examples. For others, please refer to `RedisHashRepository`, `RedisListRepository`, `RedisSetRepository`, `RedisSortedSetRepository`.

### String Cache

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Test controller
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public RedisStringRepository RedisStringRepository { get; set; }

        /// <summary>
        /// Test execute method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var expireSeconds = 604800;
            var cacheKey = "gleekframework:test;weatherforecastmodel";

            // Get cache
            var weatherForecastInfo = await RedisStringRepository.GetAsync<WeatherForecastModel>(cacheKey);

            // Set cache
            weatherForecastInfo = new WeatherForecastModel() { Date = DateTime.Now, Summary = "testing", TemperatureC = 100 };
            await RedisStringRepository.SetAsync(cacheKey, weatherForecastInfo, expireSeconds);

            // Delete cache
            await RedisStringRepository.DeleteAsync(cacheKey);

            // Set result
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

### Memory Cache

Be cautious when using memory cache, as it is acceptable to use only if your business can tolerate data delay (or difference). Otherwise, in a clustered environment, various data inconsistencies may arise.

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Test controller
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public MemoryCacheRepository MemoryCacheRepository { get; set; }

        /// <summary>
        /// Test execute method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var expireSeconds = 10;
            var cacheKey = "gleekframework:test;weatherforecastmodel";

            // Get cache
            var weatherForecastInfo = await MemoryCacheRepository.GetAsync<WeatherForecastModel>(cacheKey);

            // Set cache
            weatherForecastInfo = new WeatherForecastModel() { Date = DateTime.Now, Summary = "testing", TemperatureC = 100 };
            await MemoryCacheRepository.SetAsync(cacheKey, weatherForecastInfo, expireSeconds);

            // Delete cache
            await MemoryCacheRepository.RemoveAsync(cacheKey);

            // Set result
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

### Distributed Lock

#### Lock with Wait

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Test controller
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public RedisLockRepository RedisLockRepository { get; set; }

        /// <summary>
        /// Test execute method
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
                    // Lock failed: typically due to exceeding the wait time, for example: a 3-second waiting period.
                }

                // Lock successful, proceed with normal business logic code.
            }

            // Set return result
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

#### Exclusive Lock

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Test controller
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public RedisLockRepository RedisLockRepository { get; set; }

        /// <summary>
        /// Test execute method
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
                    // Lock failed: the resource has been locked (usually indicate concurrency error here)
                    return result.SetError(MessageCode.PARAM_REQUIRED_DATE);
                }

                // Lock successful, proceed with normal business logic code.
            }

            // Set return result
            return result.SetSuceccful();
        }
    }
}
```

#### Atomic Lock

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RedisSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Test controller
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public RedisLockRepository RedisLockRepository { get; set; }

        /// <summary>
        /// Test execute method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var result = new ContractResult<WeatherForecastModel>();

            var expireSeconds = 30;
            var cacheKey = "gleekframework:test:lock";

            // Atomic increment to judge concurrency lock
            var number = await RedisLockRepository.IncrementAsync(cacheKey, expireSeconds);
            if (number > 1)
            {
                // The resource has been locked (usually indicate concurrency error here)
                return result.SetError(MessageCode.PARAM_REQUIRED_DATE);
            }

            // Lock successful, proceed with normal business logic code.

            // Release lock (ideally in a finally block to prevent the lock from not being released in the event of an exception)
            await RedisLockRepository.DeleteAsync(cacheKey);

            // Set return result
            return result.SetSuceccful();
        }
    }
}
```
