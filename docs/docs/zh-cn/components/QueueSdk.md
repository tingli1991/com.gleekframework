## 项目

> Com.GleekFramework.QueueSdk

## 依赖

[消费端组件](/docs/zh-cn/components/ConsumerSdk.md)

## 概述

本地队列扩展工具包，具体的激活方式，可以参考`HostingQueueExtensions`和`HostingStackExtensions`类。

- 支持属性注入。
- 支持设置分区数量。
- 基于`PartitionedStack<T>`构建的本地栈(后进先出)生产消费模式。
- 基于`ConcurrentQueue<T>`构建的本地队列(先进先出)生产和消费模式。
- 消息体支持头部信息透传(headers 参数支持持小写+中横线的方式，其他格式不支持)。

!> **注意：** 所有的消费端组件都将依赖 [消费端组件](/docs/zh-cn/components/ConsumerSdk.md)

## 本地队列

### 注入

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
            var partitionCount = 24;//分区数量
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .SubscribeQueue((config) => partitionCount)//订阅本地队列(先进先出)
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
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}
```

### 生产

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 本地队列测试控制器
    /// </summary>
    [Route("queue")]
    public class QueueController : BaseController
    {
        /// <summary>
        /// 流水号生成器
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// 客户端服务
        /// </summary>
        public QueueClientService QueueClientService { get; set; }

        /// <summary>
        /// 生产测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TestAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"张三_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "不符合规则的头部键值对" }, { "test-header-key", "正确的头部键值对" } };
                await QueueClientService.PublishAsync(MessageType.CUSTOMER_TEST_QUEUE_NAME, param, serialNo, headers);
            }
            await Task.CompletedTask;
        }
    }
}
```

### 消费

!> **注意：** `QueueHandler`将作为所有本地队列的消费者基类，所有继承于`QueueHandler`的 Handler 将被视为本地队列的消费者。

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 自定义队列消费处理类
    /// </summary>
    public class QueueCustomerHandler : QueueHandler
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_QUEUE_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
            Console.WriteLine($"主题：{Topic}，方法名称：{ActionKey}，耗时：{totalMilliseconds}，参数：{JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```

## 本地栈

### 注入

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
            var partitionCount = 24;//分区数量
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .SubscribeStack((config) => partitionCount)//订阅本地栈(先进后出)
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
            .UseMigrations((config) => new MigrationOptions()
            {
                DatabaseType = DatabaseType.MySQL,
                ConnectionString = config.Get(DatabaseConstant.DefaultMySQLHostsKey)
            });
    }
}
```

### 生产

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 本地栈测试控制器
    /// </summary>
    [Route("stack")]
    public class StackController : BaseController
    {
        /// <summary>
        /// 流水号生成器
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// 客户端服务
        /// </summary>
        public StackClientService StackClientService { get; set; }

        /// <summary>
        /// 生产测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TestAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam() { Id = i, Name = $"张三_{i}" };
                var headers = new Dictionary<string, string>() { { "test_header_key", "不符合规则的头部键值对" }, { "test-header-key", "正确的头部键值对" } };
                await StackClientService.PublishAsync(MessageType.CUSTOMER_TEST_STACK_NAME, param, serialNo, headers);
            }
            await Task.CompletedTask;
        }
    }
}
```

### 消费

```C#
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.QueueSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 自定义栈消费处理类
    /// </summary>
    public class StackCustomerHandler : StackHandler
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public override Enum ActionKey => MessageType.CUSTOMER_TEST_STACK_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            var beginTime = messageBody.TimeStamp.ToDateTime();
            var totalMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
            Console.WriteLine($"主题：{Topic}，方法名称：{ActionKey}，耗时：{totalMilliseconds}，参数：{JsonConvert.SerializeObject(param)}");
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```
