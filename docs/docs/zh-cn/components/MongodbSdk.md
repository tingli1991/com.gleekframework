## Project

> Com.GleekFramework.MongodbSdk

## 依赖

> Com.GleekFramework.CommonSdk

> Com.GleekFramework.ConfigSdk

## 概述

基于`MongoDB.Driver`的拓展工具包。

- 拓展 `Lambda` 表达式，让其更接近 `EF` 的查询方式。

## 特别说明

- Mongodb 功能上线之前，先把表和分片键建好。
- Mongodb 在执行所有的增删改查动作时，请全部带上分片键。
- 本示例没有使用分偏见，但实际应用中这个很关键，直接决定我们的查询效率问题。

## 目录介绍

```text
Com.GleekFramework.MongodbSdk/
└── Clients/                          -> 客户端存放目录
│   ├── MongodbClient.cs              -> 自定义客户端
├── Constants.cs                      -> 常量目录
│   ├── MongoConstant.cs              -> Mongodb常量
├── Entitys                           -> 实体定义目录
│   ├── MEntity.cs                    -> Mongo基础实体类
├── Extensions                        -> 拓展方法目录
│   ├── MongoFindExtensions.cs        -> 查询拓展类
│   ├── MongoProviderExtensions.cs    -> Mongo实现类拓展
├── Hostings                          -> Host目录
│   ├── MongodbHostingExtensions.cs   -> Mongo注入类
├── Interfaces                        -> 接口目录
│   ├── IMEntity.cs                   -> Mongo基础实体接口
├── Providers                         -> 核心实现目录
│   ├── MongoClientProvider.cs        -> Mongdb客户端实现类
│   ├── MongoProvider.cs              -> Mongo数据库实现类
├── Repositorys                       -> 仓储目录
│   ├── MongoRepository.cs            -> Mongo仓储类
```

## 注入

在应用 Mongodb 之前 必须先使用`UseMongodb()`进行注入。

#### 使用默认的方式注入

默认注入主要是`MongoRepository<T>`仓储类，同时使用的配置是`MongoConnectionStrings:DefaultClientHosts`的配置。

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.QueueSdk;
using Com.GleekFramework.DapperSdk;
using Com.GleekFramework.MongodbSdk;

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
                 .SubscribeStack((config) => 24)//订阅本地栈(先进显出)
                 .SubscribeQueue((config) => 24)//订阅本地队列(先进后出)
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
            .UseMongodb()//注入Mongodb
            .UseNacosConf()
            .UseHttpClient()
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>()
            .UseDapper(DatabaseConstant.DefaultMySQLHostsKey)
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}
```

#### 指定配置名称进行注入

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.QueueSdk;
using Com.GleekFramework.DapperSdk;
using Com.GleekFramework.MongodbSdk;

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
                 .SubscribeStack((config) => 24)//订阅本地栈(先进显出)
                 .SubscribeQueue((config) => 24)//订阅本地队列(先进后出)
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
            .UseNacosConf()
            .UseHttpClient()
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>()
            .UseDapper(DatabaseConstant.DefaultMySQLHostsKey)
            .UseMongodb(["MongoConnectionStrings:DefaultClientOnlyHosts", MongoConstant.DEFAULT_CONNECTION_NAME])//注入Mongodb
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}
```

## 应用

### 自定义仓储

```C#
using Com.GleekFramework.MongodbSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 自定义仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoOnlyRepository<T> : BaseMongoRepository<T> where T : class, IMEntity
    {
        /// <summary>
        ///
        /// </summary>
        public override string ConnectionName => "MongoConnectionStrings:DefaultClientOnlyHosts";
    }
}
```

### 仓储的实例引入示例

```C#
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.MongodbSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    ///
    /// </summary>
    public class TestService : IBaseAutofac
    {
        /// <summary>
        /// 默认的仓储实例
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        /// 自定义仓储实例
        /// </summary>
        public MongoOnlyRepository<WeatherForecastModel> WeatherForecastOnlyRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {

        }
    }
}
```

### 新增

```C#
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.MongodbSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    ///
    /// </summary>
    public class TestService : IBaseAutofac
    {
        /// <summary>
        /// 默认的仓储实例
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            // 当条插入
            await WeatherForecastRepository.InsertOneAsync(new WeatherForecastModel
            {
                TemperatureC = 100,
                Summary = "测试摘要",
                Date = DateTime.Now.ToCstTime()
            });

            //批量插入
            await WeatherForecastRepository.InsertManyAsync(new List<WeatherForecastModel>()
            {
                new WeatherForecastModel
                {
                    TemperatureC = 10,
                    Summary = "测试摘要_01",
                    Date = DateTime.Now.ToCstTime()
                },
                new WeatherForecastModel
                {
                    TemperatureC = 12,
                    Summary = "测试摘要_02",
                    Date = DateTime.Now.ToCstTime()
                }
            });
        }
    }
}
```

### 查询

```C#
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.MongodbSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    ///
    /// </summary>
    public class TestService : IBaseAutofac
    {
        /// <summary>
        /// 默认的仓储实例
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            // 当条插入
            var dataList = await WeatherForecastRepository.GetListAsync(e => e.TemperatureC > 0);
        }
    }
}
```

### 单条更新

!> 注意：单挑更新的时候，请确认 where 条件过滤出来的结果是唯一的，如果你的 where 不唯一，它只会更新其中一条数据。

```C#
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.MongodbSdk;
using MongoDB.Driver;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    ///
    /// </summary>
    public class TestService : IBaseAutofac
    {
        /// <summary>
        /// 默认的仓储实例
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            //当条更新
            var update = Builders<WeatherForecastModel>.Update
                .Set(e => e.TemperatureC, 10000)
                .Set(e => e.Summary, "同时更新多个字段测试描述001");
            await WeatherForecastRepository.UpdateOneAsync(e => e.TemperatureC > 0, update);
        }
    }
}
```

### 批量更新

```C#
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.MongodbSdk;
using MongoDB.Driver;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    ///
    /// </summary>
    public class TestService : IBaseAutofac
    {
        /// <summary>
        /// 默认的仓储实例
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            //批量更新
            var update = Builders<WeatherForecastModel>.Update
                .Set(e => e.TemperatureC, 1000)
                .Set(e => e.Summary, "同时更新多个字段测试描述");
            await WeatherForecastRepository.UpdateManyAsync(e => e.TemperatureC > 0, update);
        }
    }
}
```

### 删除

```C#
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.MongodbSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    ///
    /// </summary>
    public class TestService : IBaseAutofac
    {
        /// <summary>
        /// 默认的仓储实例
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            //删除
            await WeatherForecastRepository.DeleteManyAsync(e => e.TemperatureC == 10000);
        }
    }
}
```
