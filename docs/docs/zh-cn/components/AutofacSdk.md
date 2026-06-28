## 项目

> Com.GleekFramework.AutofacSdk

- 描述：IOC(Autofac)开发工具包。
- 用途：它将作为整个 [gleekframework](https://www.gleekframework.com/) IOC 的基础。

## 特点

- 使用简单
- 代码风格统一
- 弱化 IOC 的生命周期

## 注入

建议大家使用 UseAutofac() 方法进行 IOC 的激活，如果有定制的需求，则可以参考`AutofacHostingExtensions`的其他注入方式

!> [gleekframework](https://www.gleekframework.com/) 默认将全部使用属性注入的方式，从而保证代码风格统一且高效

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;

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
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

## IBaseAutofac

> IBaseAutofac 接口将作为所有 IOC 实现类的基础接口，定义 IOC 的实现类，我们只需要将我们的实现类继承自 IBaseAutofac 接口（或者定义一个基类，IOC 实现类继承基类，然后基类继承 IBaseAutofac）即可，当然这里需要排除 Web Api 的控制器(Controller 不需要继承 IBaseAutofac 就可以通过属性注入的方式获取到具体的实例)

### 定义 IOC 实现类

```C#
using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 雪花算法实现类
    /// </summary>
    public partial class SnowflakeService : IBaseAutofac
    {
        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="suffix">流水号前缀</param>
        /// <returns></returns>
        public string GetSerialNo(int suffix = 100)
        {
            return SnowflakeProvider.GetSerialNo(suffix);
        }
    }
}
```

## 获取实例

!> 请不要在非 IOC 实例对象上去获取实例，这样是无效的，也是不规范的做法。

### 控制器获取实例

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.QueueSdk;
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
        /// 流水号生成器
        /// </summary>
        public SnowflakeService SnowflakeService { get; set; }

        /// <summary>
        /// 客户端服务
        /// </summary>
        public StackClientService StackClientService { get; set; }

        /// <summary>
        /// 测试执行方法
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

### 实现类获取实例

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

### 特殊场景实例获取

> 当我们程序里面需要一些特殊场景(例如：工厂模式)时，我们可以考虑`AutofacProviderExtensions` 和 `ServiceExtensions`拓展类来实现这些应用场景，但是不建议大家普片去使用，这样对我们的代码会照成严重的风格不统一，下面就是一个关于消息工厂的实现示例：

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
