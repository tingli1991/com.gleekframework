## Project

> Com.GleekFramework.ConfigSdk

- Description: Configuration file extension toolkit
- Purpose: It serves as the core component package for reading the configuration files of the entire [gleekframework](https://www.gleekframework.com/)

## Features

- Custom static characteristic variables (requires the `UseConfigAttribute()` method to activate this feature)
- Automatic support for hot reloading (NET's built-in feature is supported by default, just call the `UseConfig` method to inject configuration)
- System built-in configuration files (`bootstrap.json`, `subscription.json`, `application.json`, `share.json`), with defined reading priority
- System built-in environment variables (`ENV`, `PROT`, `SCHEME`, `PROJECT`, `VERSION`, `NACOS_URL`, `SWAGGER_SWITCH`, `MIGRATION_SWITCH`, `UPGRATION_SWITCH`), while also supporting custom environment variables

## Configuration Definition

### File Descriptions

| File Name           | Description                          | Purpose                                                                                                                                                                                                      |
| ------------------- | ------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `bootstrap.json`    | Local application configuration file | Controls local parameters such as the size of service threads, which are only effective within the current service process (not recommended for inclusion in the configuration center)                       |
| `subscription.json` | Subscription configuration file      | For subscribing parameters of Kafka, RabbitMQ, RocketMQ related queue information (e.g., host address, topic, queue name, etc.)                                                                              |
| `application.json`  | Application configuration            | Configuration information of the current service itself, all placed within this file                                                                                                                         |
| `share.json`        | Shared configuration                 | When multiple services need to access the same configuration, it can be defined in this file to achieve a common goal, and this file only shows its advantage through the configuration center (e.g., Nacos) |

### Access Priority

!> When the keys in the configuration files are the same, the following priority is used to read the configurations:

> `bootstrap.json` > `subscription.json` > `application.json` > `share.json`

### AppConfig.Configuration

> `AppConfig.Configuration` is the entry point for reading all configuration files.

### ConfigAttribute

> `ConfigAttribute` is the basic characteristic of all static property configurations. By adding this attribute to a static property and specifying the configuration key, you can [in real time] obtain the corresponding configuration.

## Injection

### Injecting Basic Configuration

!> Before using the Config component, it must first be activated through injection.

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;

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
        /// Create system host
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

### Injecting Static Property Configuration

!> Before using the static property configuration, you must first activate the Config component, and then activate the functionality by injecting static property configuration.

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Program class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main program function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// Create system host
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

## Basic Configuration Usage

> For more ways to read, see the extension class `JsonConfigExtensions.cs` file

### Reading a String

```C#
AppConfig.Configuration.Get("ConnectionStrings");
```

### Reading an Array

```C#
// Method One
AppConfig.Configuration.Get<string[]>("Summaries");

// Method Two
AppConfig.Configuration.Get<List<string>>("Summaries");
```

### Reading an Object

```C#
config.Get<RabbitConsumerOptions>("RabbitConsumerOptionsKey");
```

### Reading a Collection of Objects

```C#
config.Get<List<RabbitConsumerOptions>>("RabbitConsumerOptionsListKey");
```

### Reading an Enum Value

```C#
config.Get<EmptyEnum>("EnumKey");
```

### Reading a Dictionary

```C#
AppConfig.Configuration.Get<Dictionary<string,string>>("DicKey");
```

## Static Property Configuration Usage

!> Note: Static property configuration is only allowed to be applied on static properties, reading configuration file information through the Config attribute.

### Reading a String

```C#
[Config("TestKey")]
public static string TestVlue { get; set; }
```

### Reading an Array

```C#
/// <summary>
/// Reading an array
/// </summary>
[Config("Summaries")]
public static string[] Summaries { get; set; }
```

### Reading an Object

```C#
[Config("RabbitConsumerOptionsKey")]
public static RabbitConsumerOptions Options { get; set; }
```

### Reading a Collection of Objects

```C#
[Config("RabbitConsumerOptionsListKey")]
public static List<RabbitConsumerOptions> Options { get; set; }
```

### Reading an Enum Value

```C#
[Config("EnumKey")]
public static List<EmptyEnum> Type { get; set; }
```

### Reading a Dictionary

```C#
[Config("DicKey")]
public static Dictionary<string,string> ObjDic { get; set; }
```

## Environment Variables

### Built-in Environment Variables

> The names of built-in environment variables of the component are unified in the `EnvironmentConstant` constant file. If you need to read environment variables in your business logic, you can use `EnvironmentService` to do so.

#### ENV

> Used to differentiate the system environment. In enterprise project development, we usually divide our projects into environments such as dev, test, uat, pro, etc. To meet this requirement, we can use this variable to achieve differentiation.

!> Note: When the ENV environment variable is defined, all configuration reads will change. For example: if there is no configured environment variable, the component will read the configuration from bootstrap.json. Conversely, if we configure `ENV=dev`, then it will read the configuration from bootstrap.dev.json.

#### PORT

> Used to specify the port on which the current project starts, the default is 8080.

#### SCHEME

> Used to specify the HTTP protocol type on which the current project starts, the default is http. If you want to use https, you need to specify it through this variable.

#### PROJECT

> Used to specify the project classification. Its application scenario mainly lies in, for example: a consumer subscribes to consume many kafka topics, with business development, when we need to split and deploy these topics, it is a good choice.

!> Note: When this environmental variable is set along with the ENV environmental variable, the configuration file's name will become `bootstrap.dev.projectName.json` (ENV comes first, PROJECT comes after).

#### VERSION

> It is mainly used for blue-green deployment or canary release scenarios in K8S clusters, used to distinguish different project versions.

#### NOCOS_URL

> When NOCOS_URL is configured and the [Nacos] component is used, [Nacos] will prioritize the value of this environment variable as the [Nacos] host address.

#### SWAGGER_SWITCH

> When SWAGGER_SWITCH is configured and the [Swagger] component is used, [Swagger] will first determine whether this switch is on. If it is on, [Swagger] documentation will be automatically generated.

#### MIGRATION_SWITCH

> When MIGRATION_SWITCH is configured and the [Migration] component's configuration migration function is used, [Migration] will first determine whether this switch is on. If it is on, the [Migration] configuration migration function will be enabled.

#### UPGRATION_SWITCH

> When UPGRATION_SWITCH is configured and the [Migration] component's upgrade function is used, [Migration] will first determine whether this switch is on. If it is on, the [Migration] upgrade function will be enabled.

### Reading Built-in Environment Variable Values

> `EnvironmentService` service will have a Get+environment variable name method to implement the corresponding configuration read.

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Weather Forecast Controller
    /// </summary>
    [Route("weather-forecast")]
    public class WeatherForecastController : BaseController
    {
        /// <summary>
        /// Environment Variable Service
        /// </summary>
        public EnvironmentService EnvironmentService { get; set; }

        /// <summary>
        /// New Method
        /// </summary>
        /// <param name="param">Request parameter</param>
        /// <returns></returns>
        [HttpPost("test")]
        public async Task<ContractResult> TestAsync(WeatherForecastParam param)
        {
            var result = new ContractResult();
            var env = EnvironmentService.GetEnv();//Reading ENV
            return await Task.FromResult(result.SetSuccessful());
        }
    }
}
```

### Reading Custom Environment Variable Values

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Weather Forecast Controller
    /// </summary>
    [Route("weather-forecast")]
    public class WeatherForecastController : BaseController
    {
        /// <summary>
        /// Environment Variable Service
        /// </summary>
        public EnvironmentService EnvironmentService { get; set; }

        /// <summary>
        /// New Method
        /// </summary>
        /// <param name="param">Request parameter</param>
        /// <returns></returns>
        [HttpPost("test")]
        public async Task<ContractResult> TestAsync(WeatherForecastParam param)
        {
            var result = new ContractResult();
            var environmentVariableStr = EnvironmentService.GetEnvironmentVariable("EnvironmentName");//Receive as string
            var environmentVariableObj = EnvironmentService.GetEnvironmentVariable<object>("EnvironmentName");//Receive as generic object
            return await Task.FromResult(result.SetSuccessful());
        }
    }
}
```
