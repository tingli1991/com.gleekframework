## Project

> Com.GleekFramework.QueueSdk

## Dependencies

[Consumer Component](/docs/en-us/components/ConsumerSdk.md)

## Overview

Local queue extension toolkit, specific activation methods, can refer to the HostingQueueExtensions and HostingStackExtensions classes.

- Supports attribute injection.
- Supports setting the number of partitions.
- Local stack (LIFO) production and consumption model built on PartitionedStack<T>.
- Local queue (FIFO) production and consumption model built on ConcurrentQueue<T>.
- Message bodies support the transmission of header information (headers parameter supports lowercase + hyphen format, other formats are not supported).

!> **Note:** All consumer components will rely on Consumer Component

## Local Queue

### Injection

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

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            var partitionCount = 24;//Number of partitions
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .SubscribeQueue((config) => partitionCount)//Subscribe to local queue (FIFO)
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
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}
```

### Production

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Local queue test controller
    /// </summary>
    [Route("queue")]
    public class QueueController : BaseController
    {
        /// <summary>
        /// Serial number generator
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// Client service
        /// </summary>
        public QueueClientService QueueClientService { get; set; }

        /// <summary>
        /// Production test
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TestAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"Zhang San_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "Header key-value pair not conforming to the rules" }, { "test-header-key", "Correct header key-value pair" } };
                await QueueClientService.PublishAsync(MessageType.CUSTOMER_TEST_QUEUE_NAME, param, serialNo, headers);
            }
            await Task.CompletedTask;
        }
    }
}
```

### Consumption

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Custom queue consumption handler class
    /// </summary>
    public class QueueCustomerHandler : QueueHandler
    {
        /// <summary>
        /// Method name
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_QUEUE_NAME;

        /// <summary>
        /// Execution method
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
            Console.WriteLine($"Topic: {Topic}, Method name: {ActionKey}, Time taken: {totalMilliseconds}, Parameters: {JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```

## Local Stack

### Injection

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

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            var partitionCount = 24;//Number of partitions
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .SubscribeStack((config) => partitionCount)//Subscribe to local stack (LIFO)
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
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}
```

### Production

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Local stack test controller
    /// </summary>
    [Route("stack")]
    public class StackController : BaseController
    {
        /// <summary>
        /// Serial number generator
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// Client service
        /// </summary>
        public StackClientService StackClientService { get; set; }

        /// <summary>
        /// Production test
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TestAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"Zhang San_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "Header key-value pair not conforming to the rules" }, { "test-header-key", "Correct header key-value pair" } };
                await StackClientService.PublishAsync(MessageType.CUSTOMER_TEST_STACK_NAME, param, serialNo, headers);
            }
            await Task.CompletedTask;
        }
    }
}
```

### Consumption

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Custom stack consumption handler class
    /// </summary>
    public class StackCustomerHandler : StackHandler
    {
        /// <summary>
        /// Method name
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_STACK_NAME;

        /// <summary>
        /// Execution method
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
            Console.WriteLine($"Topic: {Topic}, Method name: {ActionKey}, Time taken: {totalMilliseconds}, Parameters: {JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```
