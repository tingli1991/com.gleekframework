## 项目

> Com.GleekFramework.RocketMQSdk

## 依赖

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.AutofacSdk

> Com.GleekFramework.ConsumerSdk

> Com.GleekFramework.ContractSdk

## 概述

RocketMQ 拓展工具包，目前仅支持 http 的方式进行生产和消费，在大流量下消费能力会比 TCP 有所下降。

## 配置解读

### 账号配置

```Json
{
  "RocketAccountOptions": {
    "SecretKey": "SKnRjkl12s93dd",
    "AccessKey": "AKlmo32fjRkls31",
    "InstanceId": "i-0f2g1h2j3k4l5m"
  }
}
```

### 消费配置

```Json
{
  "RabbitConnectionOptions": {
    "AwaitTask": false,
    "VirtualHost": "/",
    "HostOptions": [
      {
        "Port": 5672,
        "UserName": "guest",
        "Host": "192.168.100.29",
        "Password": "ChinaNet910111"
      }
    ]
  }
}
```

## 注册

### 注册生产端

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.RocketMQSdk;

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
            .UseNacosConf()
            .UseHttpClient()
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>()
            .AddRocketMQAccessOptions(config => config.Get<RocketAccessOptions>("RocketAccountOptions")); //添加Rocket账号配置;
    }
}
```

### 注册消费端

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.RocketMQSdk;

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
                 .SubscribeRocketMQ(config => config.Get<RocketConsumerOptions>("RocketConnectionOptions"))//订阅Rocket消费服务
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
            .AddRocketMQAccessOptions(config => config.Get<RocketAccessOptions>("RocketAccountOptions")); //添加Rocket账号配置;
    }
}
```

## 生产消息

### 生产普通消息

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RocketMQSdk;
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
        public RocketClientService RocketClientService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var topic = "test_topic_01";
            var host = "";
            var weatherForecastInfo = new WeatherForecastModel() { Date = DateTime.Now, Summary = "测试", TemperatureC = 100 };
            await RocketClientService.PublishMessageBodyAsync(host, topic, MessageType.TEST_QUERY_RPC_NAME, weatherForecastInfo);

            //设置返回结果
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

### 生产延迟消息

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RocketMQSdk;
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
        public RocketClientService RocketClientService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var topic = "test_topic_01";
            var host = "";
            var deliverTimeMillis = 10;
            var weatherForecastInfo = new WeatherForecastModel() { Date = DateTime.Now, Summary = "测试", TemperatureC = 100 };
            await RocketClientService.PublishMessageBodyAsync(host, topic, MessageType.TEST_QUERY_RPC_NAME, weatherForecastInfo, deliverTimeMillis);

            //设置返回结果
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

## 消费消息

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RocketMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// 测试消费
    /// </summary>
    public class RocketTestHandler : IRocketHandler
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public Enum ActionKey => MessageType.CUSTOMER_TEST_KAFKA_NAME;

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="messageBody"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ContractResult> ExecuteAsync(MessageBody messageBody)
        {
            var param = messageBody.GetData<StudentParam>();
            Console.WriteLine(JsonConvert.SerializeObject(param));
            return await Task.FromResult(new ContractResult().SetSuceccful(messageBody.SerialNo));
        }
    }
}
```
