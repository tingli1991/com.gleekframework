## Project

> Com.GleekFramework.AutofacSdk

- Description: IOC (Autofac) development toolkit.
- Purpose: It will serve as the foundation of the entire [gleekframework](https://www.gleekframework.com/) IOC.

## Features

- Easy to use
- Uniform code style
- Weakening the lifecycle of IOC

## Injection

It is recommended to use the UseAutofac() method to activate IOC. If there are custom needs, you can refer to other injection methods in `AutofacHostingExtensions`.

[gleekframework](https://www.gleekframework.com/) will default to property injection to ensure a unified and efficient code style.

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;

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
        /// Create the system host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateDefaultHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseAutofac()
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

## IBaseAutofac Interface

> The IBaseAutofac interface will serve as the basic interface for all IOC class implementations. To define an IOC class implementation, we only need to inherit our implementation class from the IBaseAutofac interface (or define a base class, where the IOC class inherits from the base class, and then the base class inherits from IBaseAutofac). Of course, this needs to exclude Web Api controllers (Controllers do not need to inherit from IBaseAutofac and can obtain specific instances through property injection).

### Define IOC Class Implementations

```C#
using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// Snowflake algorithm implementation class
    /// </summary>
    public partial class SnowflakeService : IBaseAutofac
    {
        /// <summary>
        /// Get serial number
        /// </summary>
        /// <param name="suffix">Serial number prefix</param>
        /// <returns></returns>
        public string GetSerialNo(int suffix = 100)
        {
            return SnowflakeProvider.GetSerialNo(suffix);
        }
    }
}
```

## Obtaining Instances

> Please avoid retrieving instances on non-IOC objects, as this is ineffective and does not conform to best practices.

### Controller Instance Retrieval

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.QueueSdk;
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
        /// Snowflake ID generator
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// Client service
        /// </summary>
        public StackClientService StackClientService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult> ExecuteAsync()
        {
            return await Task.FromResult(new ContractResult());
        }
    }
}
```

### Implementing Class Instance Retrieval

```C#
using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.KafkaSdk
{
    /// <summary>
    /// Kafka客户端服务
    /// </summary>
    public class KafkaClientService : IBaseAutofac
    {
        /// <summary>
        /// 环境变量服务
        /// </summary>
        public EnvironmentService EnvironmentService { get; set; }

        /// <summary>
        /// Http请求上下文
        /// </summary>
        public IHttpContextAccessor HttpContextAccessor { get; set; }
    }
}
```

### Special Scenario Instance Retrieval

> When our program needs some special scenarios (such as: Factory Pattern), we can consider AutofacProviderExtensions and ServiceExtensions extension classes to implement these application scenarios, but it is not recommended for everyone to use them extensively, which would cause severe inconsistency in our code style. Below is an implementation example of a message factory

```C#
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 消息处理工厂类
    /// </summary>
    public static partial class HandlerFactory
    {
        /// <summary>
        /// 并发所
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 消息处理类字典
        /// </summary>
        private static readonly Dictionary<Type, IEnumerable<IHandler>> MessageHandlerServiceList = new Dictionary<Type, IEnumerable<IHandler>>();

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="actionKey">消息类型</param>
        /// <returns></returns>
        public static T GetInstance<T>(string actionKey) where T : IHandler
        {
            var messageHandlerServiceList = GetHandlerServiceList(typeof(T));
            return (T)messageHandlerServiceList.FirstOrDefault(e => e.ActionKey.EqualsActionKey(actionKey));
        }

        /// <summary>
        /// 获取消息类型列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> GetHandlerServiceList<T>()
        {
            var messageHandlerServiceList = GetHandlerServiceList(typeof(T));
            if (messageHandlerServiceList.IsNullOrEmpty())
            {
                return new List<T>();
            }
            return messageHandlerServiceList.Select(e => (T)e);
        }

        /// <summary>
        /// 获取事件服务列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TopicServiceModel<T>> GetTopicServiceList<T>() where T : ITopicHandler
        {
            var messageHandlerServiceList = GetHandlerServiceList<T>();
            if (messageHandlerServiceList.IsNullOrEmpty())
            {
                return new List<TopicServiceModel<T>>();
            }

            return messageHandlerServiceList
                .GroupBy(e => e.Topic)
                .Select(e => new TopicServiceModel<T>()
                {
                    Topic = e.Key,
                    ServiceList = e.OrderBy(p => p.Order).ToList()
                });
        }

        /// <summary>
        /// 获取消息类型列表
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <returns></returns>
        private static IEnumerable<IHandler> GetHandlerServiceList(Type type)
        {
            if (!MessageHandlerServiceList.ContainsKey(type))
            {
                lock (@lock)
                {
                    if (!MessageHandlerServiceList.ContainsKey(type))
                    {
                        var messageHandlerList = type.GetServiceList<IHandler>();
                        if (messageHandlerList.IsNullOrEmpty())
                        {
                            return new List<IHandler>();
                        }
                        else
                        {
                            //获取或者新增
                            MessageHandlerServiceList.Add(type, messageHandlerList);
                        }
                    }
                }
            }
            return MessageHandlerServiceList[type];
        }
    }
}
```
