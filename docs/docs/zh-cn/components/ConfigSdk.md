## 项目

> Com.GleekFramework.ConfigSdk

- 描述：配置文件拓展工具包
- 用途：它将作为整个 [gleekframework](https://www.gleekframework.com/) 配置文件读取的核心组件包

## 特点

- 自定义的静态特性变量(需要调用方法 UseConfigAttribute()来激活该功能)
- 自动支持热重载(NET 自带的特性被默认支持，只需调用注入配置方法 UseConfig 即可)
- 系统内置配置文件(bootstrap.json subscription.json application.json share.json)，并为其定义了读取的优先级
- 系统内置环境变量(ENV PROT SCHEME PROJECT VERSION NACOS_URL SWAGGER_SWITCH MIGRATION_SWITCH UPGRATION_SWITCH)，同时也支持自定义环境变量

## 配置定义

### 文件说明

| 文件名称          | 描述                 | 用途                                                                                                                                        |
| :---------------- | :------------------- | :------------------------------------------------------------------------------------------------------------------------------------------ |
| bootstrap.json    | 应用程序本地配置文件 | 用于控制服务线程数大小之类的本地参数，这些参数有且旨在当前服务进程内生效(不建议加入配置中心)                                                |
| subscription.json | 订阅配置问价         | 用于订阅 Kafka、RabbitMQ、RocketMQ 相关队列信息参数(例如：主机地址、topic、队列名称等)                                                      |
| application.json  | 应用程序配置         | 当前服务自身的一些配置信息，统一放到该文件内                                                                                                |
| share.json        | 共享配置             | 当多个服务同时需要访问同一个配置的时候，则可以将配置定义到该文件内，从而实现共性的目的，该文件只有走了配置中心才能体现它的优势(例如：Nacos) |

### 访问优先级

!> 当配置文件的 key 相同的时候，则会按照如下的优先级进行配置的读取

> bootstrap.json > subscription.json > application.json > share.json

### AppConfig.Configuration

> AppConfig.Configuration 是所有配置文件读取的入口

### ConfigAttribute

> ConfigAttribute 是所有静态属性配置的基础特性，我们只需要将该特性加到静态属性上面并指定配置 key，则可以[进实时]的获取相应的配置

## 注入

### 注入基础配置

!> 使用 Config 组件之前必须先通过注入来激活它

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;

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
            .UseConfig()
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

### 注入静态属性配置

!> 在使用静态属性配置前，必须先激活 Config 组件，然后再通过注入静态属性配置来进行功能的激活

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;

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
            .UseConfig()
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

## 基础配置应用

> 更多读取方式，可以查看拓展类 `JsonConfigExtensions.cs` 文件

### 读取字符串

```C#
AppConfig.Configuration.Get("ConnectionStrings");
```

### 读取数组

```C#
//方式一
AppConfig.Configuration.Get<string[]>("Summaries");

//方式二
AppConfig.Configuration.Get<List<string>>("Summaries");
```

### 读取对象

```C#
config.Get<RabbitConsumerOptions>("RabbitConsumerOptionsKey");
```

### 读取对象集合

```C#
config.Get<List<RabbitConsumerOptions>>("RabbitConsumerOptionsListKey");
```

### 读取枚举值

```C#
config.Get<EmptyEnum>("EnumKey");
```

### 读取字典

```C#
AppConfig.Configuration.Get<Dictionary<string,string>>("DicKey");
```

## 静态属性配置应用

!> 注意：静态属性配置，则是只允许应用在静态属性上面的配置，通过 Config 特性来读取配置文件的信息

### 读取字符串

```C#
[Config("TestKey")]
public static string TestVlue { get; set; }
```

### 读取数组

```C#
/// <summary>
/// 读取数组
/// </summary>
[Config("Summaries")]
public static string[] Summaries { get; set; }
```

### 读取对象

```C#
[Config("RabbitConsumerOptionsKey")]
public static RabbitConsumerOptions Options { get; set; }
```

### 读取对象集合

```C#
[Config("RabbitConsumerOptionsListKey")]
public static List<RabbitConsumerOptions> Options { get; set; }
```

### 读取枚举值

```C#
[Config("EnumKey")]
public static List<EmptyEnum> Type { get; set; }
```

### 读取字典

```C#
[Config("DicKey")]
public static Dictionary<string,string> ObjDic { get; set; }
```

## 环境变量

### 内置环境变量

> 组件内置的环境变量名称统一放在`EnvironmentConstant`常量文件内，如果业务里面需要读取环境变量则可以使用`EnvironmentService`来进行读取

#### ENV

> 用于区分系统的环境，在企业项目开发过程中，我们通常会把我们的项目分为 dev test uat pro 等环境，为了满足该需求，我们则可以通过该变量来实现

!> 注意：当我们定义了 ENV 环境变量的时候，所有的配置读取将发生变化，例如：没有配置环境变量组件将读取 bootstrap.json 的配置，反则我们配置的`ENV=dev`的话，那么则读取 bootstrap.dev.json 的配置

#### PORT

> 用于指定当前项目启动的端口，默认为 8080

#### SCHEME

> 用于指定当前项目启动的 HTTP 协议类型，默认为：http，如果要使用 https 则需要通过该变量进行指定

#### PROJECT

> 用于指定项目分类，它的应用场景主要主要在于，例如：一个消费者订阅了很多 kafka 的 topic 进行消费，随着业务发展，我们需要对这些 topic 进行拆分部署的时候，那么它就是一个很好的选择

!> 注意：当我们设置了该环境变量，同时又设置了 ENV 环境变量的同时，配置文件的名称则会变成`bootstrap.dev.projectName.json` 这种格式(ENV 在前 PROJECT 在后)

#### VERSION

> 它主要用户 K8S 集群的蓝绿部署，或者灰度发布的应用场景，用于区分不同的项目版本

#### NOCOS_URL

> 当配置了 NOCOS_URL 并且使用了 [Nacos] 组件的时候，[Nacos] 组将将优先使用该环境变量的值作为 [Nacos] 的主机地址

#### SWAGGER_SWITCH

> 当配置了 SWAGGER_SWITCH 并且 使用了[Swagger] 组件的时候，[Swagger] 会优先判断该开关是否开启，如果开启则自动生成[Swagger]文档

#### MIGRATION_SWITCH

> 当配置了 MIGRATION_SWITCH 并且 使用了[Migration] 组件的配置迁移功能时，[Migration] 会优先判断该开关是否开启，如果开启则启用[Migration]的配置迁移功能

#### UPGRATION_SWITCH

> 当配置了 UPGRATION_SWITCH 并且 使用了[Migration] 组件的升级功能时，[Migration] 会优先判断该开关是否开启，如果开启则启用[Migration]升级功能

### 读取内置环境变量值

> `EnvironmentService`服务内都会有一个 Get+环境变量名称的方法跟随来实现对用的配置读取

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 天气预报控制器
    /// </summary>
    [Route("weather-forecast")]
    public class WeatherForecastController : BaseController
    {
        /// <summary>
        /// 环境变量服务
        /// </summary>
        public EnvironmentService EnvironmentService { get; set; }

        /// <summary>
        /// 新增方法
        /// </summary>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        [HttpPost("test")]
        public async Task<ContractResult> TestAsync(WeatherForecastParam param)
        {
            var result = new ContractResult();
            var env = EnvironmentService.GetEnv();//读取ENE
            return await Task.FromResult(result.SetSuceccful());
        }
    }
}
```

### 读取自定义环境变量值

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 天气预报控制器
    /// </summary>
    [Route("weather-forecast")]
    public class WeatherForecastController : BaseController
    {
        /// <summary>
        /// 环境变量服务
        /// </summary>
        public EnvironmentService EnvironmentService { get; set; }

        /// <summary>
        /// 新增方法
        /// </summary>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        [HttpPost("test")]
        public async Task<ContractResult> TestAsync(WeatherForecastParam param)
        {
            var result = new ContractResult();
            var environmentVariableStr = EnvironmentService.GetEnvironmentVariable("EnvironmentName");//字符串接受
            var environmentVariableObj = EnvironmentService.GetEnvironmentVariable<object>("EnvironmentName");//泛型对象接受
            return await Task.FromResult(result.SetSuceccful());
        }
    }
}
```
