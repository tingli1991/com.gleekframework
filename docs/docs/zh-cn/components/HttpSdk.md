## 项目

> Com.GleekFramework.HttpSdk

### 概述

Http 拓展工具包，具体的激活方式，可以参考`HostingExtensions`类。

- 支持属性注入。
- 调整默认超时时间为 3 秒。
- 调整默认的连接数为 10000，并支持自定义拓展。
- 支持添加熔断(这里只拓展了高级断路器，需要手动添加，断路器使用需慎重)。
- 支持添加间隔性重试机制(需要手动添加，如果配置了重试机制，建议服务端做幂等处理)。

### 依赖

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.CommonSdk

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.ContractSdk

> Com.GleekFramework.AutofacSdk

## UseHttpClient()

!> `UseHttpClient()`激活 Http 请求的唯一方法，该方法默认会将请求的超时时间调整为 3 秒，同时 tcp 连接数也会被默认调整为 10000，该阈值仅仅作为参考，项目中可以通过 `UseHttpClient()`的重载方法进行调整，重载方法位于`HostingExtensions.cs` 中。

### 默认注入

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;

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
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

### 自定义注入

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;

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
            .UseHttpClient(config => new HttpClientOptions()
            {
                TimeOutSeconds = 3,//超时时间（默认：3秒）
                ClientName = HttpConstant.DEFAULT_CLIENT_NAME,//客户端名称(默认：GEEK_FRAMEWORK_HTTP_CLIENT)
                MaxConnectionsPerServer = 10000,//TCP连接数(默认：10000)
                SleepDurations = new List<TimeSpan>()//重试规则(默认：无，如下则是间隔按照3秒、5秒、10秒进行重试)
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                },
                AdvancedCircuitBreakerOptions = new AdvancedCircuitBreakerOptions()
                {
                    DurationOfBreak = 3,//中断持续时长(单位：秒，默认值：3秒)
                    FailureThreshold = 80,//故障阈值(至少n%有异常则熔断，默认值：80%)
                    SamplingDuration = 10,//采样持续时间(单位：秒，默认值：10秒)
                    MinimumThroughput = 10000//最小吞吐量(最少调用多少次，默认：10000)
                }
            })
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

### 配置注入（推荐）

所谓的配置注入，则是将自定义的注入用到的`HttpClientOptions`放入到配置文件当中，在集群环境中有利于统一走配置中心。

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;

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
            .UseHttpClient(config => config.Get<HttpClientOptions>("HttpClientOptionsConfigKey"))
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

## 服务介绍

`HttpContractService`和`HttpClientService`都是发送 Http 请求的服务，二者区别在于，返回的模型数据不同

- `HttpContractService`服务返回的数据模型统一为`ContractResult`和`ContractResult<T>`，针对内部的微服务调用更加方便。
- `HttpClientService`服务则返回的数据模型更宽泛，主要正对外部系统，非`ContractResult`和`ContractResult<T>`的场景。

?> **提示：**`HttpContractService`和`HttpClientService` 这两个服务里面 DELETE、POST 相关的方法跟如下的调用方式一致。

### HttpContractService

#### 请求并返回 `ContractResult`

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
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
        /// Http客户端服务
        /// </summary>
        public HttpContractService HttpContractService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult> ExecuteAsync()
        {
            var url = $"http://localhost:8080/api/test/get";//接口地址
            var param = new Dictionary<string, string>() { { "id", $"{Guid.NewGuid()}" } };//可选
            var headers = new Dictionary<string, string>() { { "Token", $"SASDASDOOWERWEWERWERWE" } };//可选
            return await HttpContractService.GetAsync(url, param, headers);
        }
    }
}
```

#### 请求并返回 `ContractResult<T>`

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.Models;
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
        /// Http客户端服务
        /// </summary>
        public HttpContractService HttpContractService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var url = $"http://localhost:8080/api/test/get";//接口地址
            var param = new Dictionary<string, string>() { { "id", $"{Guid.NewGuid()}" } };//可选
            var headers = new Dictionary<string, string>() { { "Token", $"SASDASDOOWERWEWERWERWE" } };//可选
            return await HttpContractService.GetAsync<WeatherForecastModel>(url, param, headers);
        }
    }
}
```

### HttpClientService

#### 请求并返回 `String`

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
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
        /// Http客户端服务
        /// </summary>
        public HttpClientService HttpClientService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult<string>> ExecuteAsync()
        {
            var result = new ContractResult<string>();
            var url = $"http://localhost:8080/api/test/get";//接口地址
            var param = new Dictionary<string, string>() { { "id", $"{Guid.NewGuid()}" } };//可选
            var headers = new Dictionary<string, string>() { { "Token", $"SASDASDOOWERWEWERWERWE" } };//可选
            var response = await HttpClientService.GetAsync(url, param, headers);
            return result.SetSuceccful(response);
        }
    }
}
```

#### 请求并返回泛型 `T`

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.Models;
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
        /// Http客户端服务
        /// </summary>
        public HttpClientService HttpClientService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var result = new ContractResult<WeatherForecastModel>();
            var url = $"http://localhost:8080/api/test/get";//接口地址
            var param = new Dictionary<string, string>() { { "id", $"{Guid.NewGuid()}" } };//可选
            var headers = new Dictionary<string, string>() { { "Token", $"SASDASDOOWERWEWERWERWE" } };//可选
            var response = await HttpClientService.GetAsync<WeatherForecastModel>(url, param, headers);
            return result.SetSuceccful(response);
        }
    }
}
```
