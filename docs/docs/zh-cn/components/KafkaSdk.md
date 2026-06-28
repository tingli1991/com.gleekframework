## 项目

> Com.GleekFramework.KafkaSdk

## 依赖

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.AutofacSdk

> Com.GleekFramework.ConsumerSdk

> Com.GleekFramework.ContractSdk

## 概述

Kafka 拓展工具包，基于`Confluent.Kafka`拓展的 kafka 中间件生产和消费组件。

?> 切面定义请参考 [消费端组件](/docs/zh-cn/components/ConsumerSdk.md)。

### 目录介绍

```text
Com.GleekFramework.KafkaSdk/
├── Constants/                        -> 常量存放目录
│   ├── KafkaConstant.cs              -> Kafka常量配置
├── Extensions/                       -> 拓展目录
│   ├── CallbackExtensions.cs         -> 回调处理类
│   └── MessageExtensions.cs          -> 消息拓展类
├── Hostings/                         -> 主机拓展(注入)
│   ├── KafkaHostingExtensions.cs     -> 主机拓展类(订阅kafka消费者)
├── Interfaces/                       -> 接口定义目录
│   ├── IKafkaHandler.cs              -> Kafka消息处理基础类
├── Options/                          -> 配置对象目录
│   ├── KafkaConsumerOptions.cs       -> Kafka消费配置选项
├── Providers/                        -> 实现类目录(业务不推荐使用)
│   ├── ConsumerProvider.cs/          -> Kafka消费实现服务
│   ├── ProducerProvider.cs/          -> Kafka生产实现服务
├── Services/                         -> IOC实现服务目录(推荐)
│   ├── KafkaClientService.cs         -> Kafka客户端服务
```

## 示例

### 消息订阅

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.NacosSdk;

namespace Com.GleekFramework.ConsumerSvc
{
    /// <summary>
    /// 主程序
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
                 .SubscribeKafka(config => config.GetValue<KafkaConsumerOptions>(Models.ConfigConstant.KafkaConnectionOptionsKey))
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
            .UseWindowsService()
            .UseConfigAttribute()
            .UseGleekConsumerHostDefaults();
    }
}
```

### 生产消息

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
    /// Kafka测试控制器
    /// </summary>
    [Route("kafka")]
    public class KafkaController : BaseController
    {
        /// <summary>
        /// 流水号生成器
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// Http客户端工厂类
        /// </summary>
        public IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        /// <summary>
        /// Kafka客户端服务
        /// </summary>
        public KafkaClientService KafkaClientService { get; set; }

        /// <summary>
        /// 接口测试
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
                    { "test-header-key", "正确的头部键值对" },
                    { "test_header_key", "不符合规则的头部键值对" }
                };

                var dataList = new List<StudentParam>() { param };
                var response = KafkaClientService.PublishManyAsync(RabbitConfig.KafkaDefaultClientHosts, KafkaTopicConstant.DEFAULT_TOPIC, MessageType.CUSTOMER_TEST_KAFKA_NAME, dataList, serialNo, headers);
                Console.WriteLine($"消息开始时间：{beginTime:yyyy-MM-dd HH:mm:ss}，消息处理耗时：{(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds}");
            }
            await Task.CompletedTask;
        }
    }
}
```

### 消费消息

```C#
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.Models;
using Newtonsoft.Json;

namespace Com.GleekFramework.ConsumerSvc
{
    /// <summary>
    /// 自定义Kafka消费处理类
    /// </summary>
    public class KafkaCustomerHandler : IKafkaHandler
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
