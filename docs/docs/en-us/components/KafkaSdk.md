## Project

> Com.GleekFramework.KafkaSdk

## Dependencies

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.AutofacSdk

> Com.GleekFramework.ConsumerSdk

> Com.GleekFramework.ContractSdk

## Overview

Kafka extension toolkit, expanded kafka middleware production and consumption components based on `Confluent.Kafka`.

?> For the definition of aspects, please refer to [Consumer Components](/docs/en-us/components/ConsumerSdk.md). Continue with the translation.

### Directory Introduction

```text
Com.GleekFramework.KafkaSdk/
├── Constants/                        -> Constants storage directory
│   ├── KafkaConstant.cs              -> Kafka constant configuration
├── Extensions/                       -> Extensions directory
│   ├── CallbackExtensions.cs         -> Callback handler class
│   └── MessageExtensions.cs          -> Message extension class
├── Hostings/                         -> Hosting extensions (injection)
│   ├── KafkaHostingExtensions.cs     -> Hosting extension class (subscribe to kafka consumers)
├── Interfaces/                       -> Interface definitions directory
│   ├── IKafkaHandler.cs              -> Kafka message handler base class
├── Options/                          -> Configuration object directory
│   ├── KafkaConsumerOptions.cs       -> Kafka consumer configuration options
├── Providers/                        -> Implementation directory (not recommended for business use)
│   ├── ConsumerProvider.cs/          -> Kafka consumer service implementation
│   ├── ProducerProvider.cs/          -> Kafka producer service implementation
├── Services/                         -> IOC implementation services directory (recommended)
│   ├── KafkaClientService.cs         -> Kafka client service
```

## Example

### Message Subscription

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.NacosSdk;

namespace Com.GleekFramework.ConsumerSvc
{
    /// <summary>
    /// Main program
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Program's main function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .SubscribeKafka(config => config.GetValue<KafkaConsumerOptions>(Models.ConfigConstant.KafkaConnectionOptionsKey))
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
            .UseWindowsService()
            .UseConfigAttribute()
            .UseGleekConsumerHostDefaults();
    }
}
```

### Produce Message

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Kafka test controller
    /// </summary>
    [Route("kafka")]
    public class KafkaController : BaseController
    {
        /// <summary>
        /// Serial number generator
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// HTTP client factory class
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// HTTP request context
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// Kafka client service
        /// </summary>
        public KafkaClientService KafkaClientService { get; set; }

        /// <summary>
        /// Interface test
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task TestAsync()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var beginTime = DateTime.Now.ToCstTime();
                var serialNo = SnowflakeService.GetSerialNo();
                var param = new StudentParam()
                {
                    Id = i,
                    Name = $"张三_{i}"
                };

                var headers = new Dictionary<string, string>()
                {
                    { "test-header-key", "Correct header key value pair" },
                    { "test_header_key", "Header key value pair that doesn’t follow naming convention" }
                };

                var dataList = new List<StudentParam>() { param };
                var response = KafkaClientService.PublishManyAsync(RabbitConfig.KafkaDefaultClientHosts, KafkaTopicConstant.DEFAULT_TOPIC, MessageType.CUSTOMER_TEST_KAFKA_NAME, dataList, serialNo, headers);
                Console.WriteLine($"Message start time: {beginTime:yyyy-MM-dd HH:mm:ss}, message processing time: {(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}
```

### Consume Message

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.Models;
using Newtonsoft.Json;

namespace Com.GleekFramework.ConsumerSvc
{
    /// <summary>
    /// Custom Kafka consumption handler class
    /// </summary>
    public class KafkaCustomerHandler : IKafkaHandler
    {
        /// <summary>
        /// Method name
        /// </summary>
        public Enum ActionKey => MessageType.CUSTOMER_TEST_KAFKA_NAME;

        /// <summary>
        /// Execute method
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
