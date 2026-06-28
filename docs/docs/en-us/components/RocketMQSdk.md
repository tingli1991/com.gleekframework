## Project

> Com.GleekFramework.RocketMQSdk

## Dependencies

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.AutofacSdk

> Com.GleekFramework.ConsumerSdk

> Com.GleekFramework.ContractSdk

## Overview

RocketMQ extension tool kit, currently only supports production and consumption in http method. The consumption capacity will drop under high traffic compared to TCP.

## Configuration Interpretation

### Account Configuration

```Json
{
  "RocketAccountOptions": {
    "SecretKey": "SKnRjkl12s93dd",
    "AccessKey": "AKlmo32fjRkls31",
    "InstanceId": "i-0f2g1h2j3k4l5m"
  }
}
```

### Consumption Configuration

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

## Registration

### Register Producer

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
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main function of the program
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// Create the system host
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
            .AddRocketMQAccessOptions(config => config.Get<RocketAccessOptions>("RocketAccountOptions")); // Add Rocket account configuration;
    }
}
```

### Register Consumer

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
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main function of the program
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .SubscribeRocketMQ(config => config.Get<RocketConsumerOptions>("RocketConnectionOptions"))// Subscribe to Rocket consumer service
                 .RunAsync();
        }

        /// <summary>
        /// Create the system host
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
            .AddRocketMQAccessOptions(config => config.Get<RocketAccessOptions>("RocketAccountOptions")); // Add Rocket account configuration;
    }
}
```

## Produce Messages

### Produce a Normal Message

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RocketMQSdk;
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
        public RocketClientService RocketClientService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var topic = "test_topic_01";
            var host = "";
            var weatherForecastInfo = new WeatherForecastModel() { Date = DateTime.Now, Summary = "Testing", TemperatureC = 100 };
            await RocketClientService.PublishMessageBodyAsync(host, topic, MessageType.TEST_QUERY_RPC_NAME, weatherForecastInfo);

            // Set return result
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

### Produce a Delayed Message

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RocketMQSdk;
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
        public RocketClientService RocketClientService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var topic = "test_topic_01";
            var host = "";
            var deliverTimeMillis = 10;
            var weatherForecastInfo = new WeatherForecastModel() { Date = DateTime.Now, Summary = "Testing", TemperatureC = 100 };
            await RocketClientService.PublishMessageBodyAsync(host, topic, MessageType.TEST_QUERY_RPC_NAME, weatherForecastInfo, deliverTimeMillis);

            // Set return result
            return new ContractResult<WeatherForecastModel>().SetSuceccful();
        }
    }
}
```

## Consume Messages

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.RocketMQSdk;
using Newtonsoft.Json;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Test consumer
    /// </summary>
    public class RocketTestHandler : IRocketHandler
    {
        /// <summary>
        /// Method name
        /// </summary>
        public Enum ActionKey => MessageType.CUSTOMER_TEST_KAFKA_NAME;

        /// <summary>
        /// Execution method
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
