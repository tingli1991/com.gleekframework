## Project

> Com.GleekFramework.AttributeSdk

### Overview

Attribute development toolkit, mainly extending AOP for Web API type projects.

### Core Functions

- Global exception filter
- Provides heartbeat detection capabilities
- Basic controller (BaseController)
- Log collection extension (convenient for recording global HTTP requests and return logs, and also supports custom log levels)
- Model validation filter, for validation rules see namespace: System.ComponentModel.DataAnnotations, Microsoft official

### Dependencies

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

## Directory Introduction

```text
Com.GleekFramework.AttributeSdk/
├── Attributes/                                     -> Custom attribute directory
│   ├── GlobalExceptionAttribute.cs                 -> Global exception filter
│   ├── ModelValidAttribute.cs                      -> Model validation filter
├── Controllers/                                    -> Custom controller
│   ├── BaseController.cs                           -> Basic controller
├── Extensions/                                     -> Custom extension directory
│   ├── ModelValidExtensions.cs                     -> Model validation extension method class
│   ├── NLogMiddlewareExtensions.cs                 -> Request log collection extension
├── Hostings/                                       -> Custom Host directory
│   ├── AttributeHostingExtensions.cs               -> Model validation extension method class
├── Middlewares/                                    -> Custom middleware directory
│   ├── NLogMiddleware.cs                           -> Interface log collection middleware
└── Validations/                                    -> Custom model validation directory
    ├── NotEqualAttribute.cs                        -> Not equal model validation
```

## Global Exceptions

This is mainly implemented through the GlobalExceptionAttribute attribute class, which captures global exceptions in Web API projects and records the exception information in the log file through Com.GleekFramework.NLogSdk, which is convenient for troubleshooting later.

### Injection

> After completing the injection, the system will automatically capture abnormal information. During the development process, except for specific scenarios, we no longer need to do a lot of try catch. Injection is also super simple, and can be done in one line of code.

```C#
/// <summary>
/// Program excitement class
/// </summary>
public class Startup
{
    /// <summary>
    /// Service registration
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGlobalExceptionAttribute();//Add a global exception
    }
}
```

## Heartbeat Detection

> Interface path: /health

> Verification method: http://localhost:8080/health

### Injection

```C#
/// <summary>
/// Program excitement class
/// </summary>
public class Startup
{
    /// <summary>
/// Service registration
/// </summary>
/// <param name="services"></param>
public void ConfigureServices(IServiceCollection services)
{
     services.AddHealthChecks();//Add a heartbeat
}

/// <summary>
/// Configure services
/// </summary>
/// <param name="app"></param>
public void Configure(IApplicationBuilder app)
{
    app.UseHealthChecks();//Use heartbeat detection
    app.RegisterApplicationStarted(() => Console.Out.WriteLine($"Service start successfully: {EnvironmentProvider.GetHost()}"));
}
```

## Basic Controller

```C#
 /// <summary>
 /// Weather forecast controller
 /// </summary>
 [Route("weather-forecast")]
 public class WeatherForecastController : BaseController
 {

 }
```

## Interface Logs

> Due to the use of NLog component in log output, if you want to learn about NLog, please refer to the [NLog Component](/docs/en-us/components/NLogSdk.md) document. Here, we will only explain how to use this feature. Thank you for your support.

### Injection

```C#
/// <summary>
/// Program excitement class
/// </summary>
public class Startup
{
    /// <summary>
    /// Service registration
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        app.UseNLogMiddleware(NLog.LogLevel.Trace);
    }
}
```

## Model Validation

The syntax of all model verifications in Attribute component must be written as per this convention: `[Required(ErrorMessage = nameof(MessageCode.PARAM_REQUIRED_DATE))]`, otherwise it will not take effect.

### Injection

```C#
/// <summary>
/// Program activation class
/// </summary>
public class Startup
{
    /// <summary>
    /// Service registration
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddModelValidAttribute<MessageCode>();//Add model validation
    }
}
```

### Parameter verification

> Required is mainly required for verification, other verification [Microsoft Official](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-8.0);

```C#
/// <summary>
/// Request parameters
/// </summary>
public class WeatherForecastParam
{
    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("date"), JsonPropertyName("date")]
    [Required(ErrorMessage = nameof(MessageCode.PARAM_REQUIRED_DATE))]
    public DateTime Date { get; set; }

    /// <summary>
    /// Temperature (Celsius)
    /// </summary>
    [JsonProperty("temperature_c"), JsonPropertyName("temperature_c")]
    [Required(ErrorMessage = nameof(GlobalMessageCode.PARAM_REQUIRED))]
    public int TemperatureC { get; set; }

    /// <summary>
    /// Summary
    /// </summary>
    [JsonProperty("summary"), JsonPropertyName("summary")]
    public string Summary { get; }

    /// <summary>
    /// Temperature (Fahrenheit)
    /// </summary>
    [JsonProperty("temperature_f"), JsonPropertyName("temperature_f")]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
```

## JSON Format

AddNewtonsoftJson primarily helps us set a convention for the data format we expose externally (mainly includes: setting the date format to yyyy-MM-dd HH:mm:ss:ffffff, not adding camel casing)

!> By default, the first letter of the property name in the return value of the .NET project will be converted to lowercase. After using AddNewtonsoftJson, this feature will be cancelled.

### Injection

```C#
/// <summary>
/// The Startup class of the program
/// </summary>
public class Startup
{
    /// <summary>
    /// Services registration
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
         services.AddNewtonsoftJson();//Add default JSON formatting
    }
}
```
