## Project

> Com.GleekFramework.MongodbSdk

## Dependencies

> Com.GleekFramework.CommonSdk

> Com.GleekFramework.ConfigSdk

## Summary

A toolkit extension based on `MongoDB.Driver`.

- Extending `Lambda` expressions to make it closer to the querying method of `EF`.

## Special Note

- Before the MongoDB feature goes live, please create the tables and shard keys.
- When performing all CRUD actions in MongoDB, please include the shard key in all operations.
- This example does not use partial indexes, but in real applications, this is crucial, directly affecting the efficiency of our queries.

## Directory Introduction

```text
Com.GleekFramework.MongodbSdk/
└── Clients/                          -> Client storage directory
│   ├── MongodbClient.cs              -> Custom client
├── Constants.cs                      -> Constants directory
│   ├── MongoConstant.cs              -> Mongodb constants
├── Entities                          -> Entity definition directory
│   ├── MEntity.cs                    -> Mongo basic entity class
├── Extensions                        -> Extension method directory
│   ├── MongoFindExtensions.cs        -> Query extension class
│   ├── MongoProviderExtensions.cs    -> Mongo implementation class extension
├── Hostings                          -> Host directory
│   ├── MongodbHostingExtensions.cs   -> Mongo injection class
├── Interfaces                        -> Interface directory
│   ├── IMEntity.cs                   -> Mongo basic entity interface
├── Providers                         -> Core implementation directory
│   ├── MongoClientProvider.cs        -> Mongdb client implementation class
│   ├── MongoProvider.cs              -> Mongo database implementation class
├── Repositories                      -> Repository directory
│   ├── MongoRepository.cs            -> Mongo repository class
```

## Injection

You must use `UseMongodb()` to inject it before applying MongoDB.

#### Inject using the default method

Default injection mainly involves the `MongoRepository<T>` repository class, using the `MongoConnectionStrings:DefaultClientHosts` configuration by default.

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
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main program function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .SubscribeStack((config) => 24) // Subscribe to the local stack (first in first out)
                 .SubscribeQueue((config) => 24) // Subscribe to the local queue (first in last out)
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
            .UseMongodb() // Inject MongoDB
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

#### Inject using a specific configuration name

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
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main program function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .SubscribeStack((config) => 24) // Subscribe to the local stack (first in first out)
                 .SubscribeQueue((config) => 24) // Subscribe to the local queue (first in last out)
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
            .UseNacosConf()
            .UseHttpClient()
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>()
            .UseDapper(DatabaseConstant.DefaultMySQLHostsKey)
            .UseMongodb(["MongoConnectionStrings:DefaultClientOnlyHosts", MongoConstant.DEFAULT_CONNECTION_NAME]) // Inject MongoDB
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}
```

## Application

#### Custom Repository

```C#
using Com.GleekFramework.MongodbSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Custom repository
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

#### Example of Repository Instance Introduction

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
        /// Default repository instance
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        /// Custom repository instance
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

#### Insert

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
        /// Default repository instance
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            // Single insert
            await WeatherForecastRepository.InsertOneAsync(new WeatherForecastModel
            {
                TemperatureC = 100,
                Summary = "Test Summary",
                Date = DateTime.Now.ToCstTime()
            });

            // Batch insert
            await WeatherForecastRepository.InsertManyAsync(new List<WeatherForecastModel>()
            {
                new WeatherForecastModel
                {
                    TemperatureC = 10,
                    Summary = "Test Summary_01",
                    Date = DateTime.Now.ToCstTime()
                },
                new WeatherForecastModel
                {
                    TemperatureC = 12,
                    Summary = "Test Summary_02",
                    Date = DateTime.Now.ToCstTime()
                }
            });
        }
    }
}
```

#### Query

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
        /// Default repository instance
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            // Query
            var dataList = await WeatherForecastRepository.GetListAsync(e => e.TemperatureC > 0);
        }
    }
}
```

#### Single Update

!> Note: When updating a single record, please ensure the where condition filters out a unique result. If your where condition is not unique, it will only update one of the records.

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
        /// Default repository instance
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            // Single update
            var update = Builders<WeatherForecastModel>.Update
                .Set(e => e.TemperatureC, 10000)
                .Set(e => e.Summary, "Test Summary for updating multiple fields 01");
            await WeatherForecastRepository.UpdateOneAsync(e => e.TemperatureC > 0, update);
        }
    }
}
```

#### Batch Update

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
        /// Default repository instance
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            // Batch update
            var update = Builders<WeatherForecastModel>.Update
                .Set(e => e.TemperatureC, 1000)
                .Set(e => e.Summary, "Test Summary for updating multiple fields");
            await WeatherForecastRepository.UpdateManyAsync(e => e.TemperatureC > 0, update);
        }
    }
}
```

#### Delete

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
        /// Default repository instance
        /// </summary>
        public MongoRepository<WeatherForecastModel> WeatherForecastRepository { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            // Delete
            await WeatherForecastRepository.DeleteManyAsync(e => e.TemperatureC == 10000);
        }
    }
}
```
