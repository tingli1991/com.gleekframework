## 项目

> Com.GleekFramework.AttributeSdk

### 概述

特性开发工具包，它主要是地 Web API 类型项目的 AOP 拓展。

### 核心功能

- 全局异常过滤器
- 提供心跳检测能力
- 基础控制器（BaseController）
- 日志收集扩展（方便记录全局的 HTTP 请求和返回日志，同时也支持自定义日志级别）
- 模型验证过滤器，体的验证规则请参见命名空间：System.ComponentModel.DataAnnotations，[微软官方](https://learn.microsoft.com/zh-cn/dotnet/api/system.componentmodel.dataannotations?view=net-8.0)

### 依赖

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

## 目录介绍

```text
Com.GleekFramework.AttributeSdk/
├── Attributes/                                     -> 自定义特性目录
│   ├── GlobalExceptionAttribute.cs                 -> 全局异常过滤器
│   ├── ModelValidAttribute.cs                      -> 模型验证过滤器
├── Controllers/                                    -> 自定义控制器
│   ├── BaseController.cs                           -> 基础控制器
├── Extensions/                                     -> 自定义拓展目录
│   ├── ModelValidExtensions.cs                     -> 模型验证扩展方类
│   ├── NLogMiddlewareExtensions.cs                 -> 请求日志收集扩展
├── Hostings/                                       -> 自定义Host目录
│   ├── AttributeHostingExtensions.cs               -> 模型验证扩展方类
├── Middlewares/                                    -> 自定义中间件目录
│   ├── NLogMiddleware.cs                           -> 接口日志收集中间件
└── Validations/                                    -> 自定义模型验证目录
    ├── NotEqualAttribute.cs                        -> 不等于模型验证

```

## 全局异常

这里主要是通过`GlobalExceptionAttribute`特性类来实现，Web API 项目中全局异常的捕获，并将异常信息通过`Com.GleekFramework.NLogSdk`记录到日志文件当中，方便后续排查问题

### 注入

> 完成注入之后，系统将自动实现异常信息的捕获，在开发过程中除开特定的场景，我们则再也不需要去些大量的 try catch 了，注入也超级简单，一行代码搞定。

```C#
/// <summary>
/// 程序激动类
/// </summary>
public class Startup
{
    /// <summary>
    /// 服务注册
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGlobalExceptionAttribute();//添加全局异常
    }
}
```

## 心跳检测

所谓的心跳检测，实际就是生成一个心跳检测接口(不需要认为手动创建)，该接口可以提供给到 K8S 去监控我们服务的健康状态。

> 接口路径：/health

> 验证方式：http://localhost:8080/health

### 注入

```C#
/// <summary>
/// 程序激动类
/// </summary>
public class Startup
{
    /// <summary>
    /// 服务注册
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
         services.AddHealthChecks();//添加心跳
    }
}

/// <summary>
/// 配置服务
/// </summary>
/// <param name="app"></param>
public void Configure(IApplicationBuilder app)
{
    app.UseHealthChecks();//使用心跳检测
    app.RegisterApplicationStarted(() => Console.Out.WriteLine($"服务启动成功：{EnvironmentProvider.GetHost()}"));
}
```

## 基础控制器

```C#
 /// <summary>
 /// 天气预报控制器
 /// </summary>
 [Route("weather-forecast")]
 public class WeatherForecastController : BaseController
 {

 }
```

## 接口日志

由于日志速出使用的是 NLog 组件，想了解 NLog 可查阅 [NLog 组件](/docs/zh-cn/components/NLogSdk.md)文档，这里只将怎么使用这个功能，感谢您的支持。

### 注入

```C#
/// <summary>
/// 程序激动类
/// </summary>
public class Startup
{
    /// <summary>
    /// 服务注册
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        app.UseNLogMiddleware(NLog.LogLevel.Trace);
    }
}
```

## 模型验证

[Attribute 组件](/docs/zh-cn/components/AttributeSdk.md?id=项目) 中所有的模型验证语法必须按照`[Required(ErrorMessage = nameof(MessageCode.PARAM_REQUIRED_DATE))]`这个约定来进行编写，否则将不会生效。

### 注入

```C#
/// <summary>
/// 程序激动类
/// </summary>
public class Startup
{
    /// <summary>
    /// 服务注册
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddModelValidAttribute<MessageCode>();//添加模型验证
    }
}
```

### 参数验证

> `Required`主要是必填验证，其他的验证[微软官方](https://learn.microsoft.com/zh-cn/dotnet/api/system.componentmodel.dataannotations?view=net-8.0)；

```C#
/// <summary>
/// 请求参数
/// </summary>
public class WeatherForecastParam
{
    /// <summary>
    /// 时间
    /// </summary>
    [JsonProperty("date"), JsonPropertyName("date")]
    [Required(ErrorMessage = nameof(MessageCode.PARAM_REQUIRED_DATE))]
    public DateTime Date { get; set; }

    /// <summary>
    /// 温度(摄氏度)
    /// </summary>
    [JsonProperty("temperature_c"), JsonPropertyName("temperature_c")]
    [Required(ErrorMessage = nameof(GlobalMessageCode.PARAM_REQUIRED))]
    public int TemperatureC { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [JsonProperty("summary"), JsonPropertyName("summary")]
    public string Summary { get; }

    /// <summary>
    /// 温度(华氏度)
    /// </summary>
    [JsonProperty("temperature_f"), JsonPropertyName("temperature_f")]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
```

## JSON 格式

`AddNewtonsoftJson` 主要是帮我们约定好，对外输出的数据格式(主要包括：设置时间格式为 yyyy-MM-dd HH:mm:ss:ffffff、不添加驼峰)

!> NET 项目默认返回值的属性名称首字母会被转换成小写，使用了`AddNewtonsoftJson`之后，这个功能将会被取消。

### 注入

```C#
/// <summary>
/// 程序激动类
/// </summary>
public class Startup
{
    /// <summary>
    /// 服务注册
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
         services.AddNewtonsoftJson();//添加对JSON的默认格式化
    }
}
```
