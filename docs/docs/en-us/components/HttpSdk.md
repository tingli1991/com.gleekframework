## Project

> Com.GleekFramework.HttpSdk

### Overview

Http Extension Toolkit, the activation method can be referred to in the `HostingExtensions` class.

- Supports property injection.
- Adjust the default timeout to 3 seconds.
- Adjust the default number of connections to 10,000, and support custom extensions.
- Supports adding circuit breakers (only advanced breakers are extended here, need to be manually added, use of circuit breakers should be cautious).
- Supports adding intermittent retry mechanisms (manually added, if retry mechanism is configured, idempotent processing is recommended on the server).

### Dependencies

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.CommonSdk

> Com.GleekFramework.ConfigSdk

> Com.GleekFramework.ContractSdk

> Com.GleekFramework.AutofacSdk

## UseHttpClient()

!> `UseHttpClient()` is the only method to activate Http requests, this method will adjust the request timeout to 3 seconds by default, and the TCP connection count will also be defaulted to 10,000. This threshold is for reference only and can be adjusted in the project via the overloaded `UseHttpClient()` method, which is located in `HostingExtensions.cs`.

### Default Injection

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Program Class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// Create System Host
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

### Custom Injection

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Program Class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// Create System Host
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
                TimeOutSeconds = 3, // Timeout duration (default: 3 seconds)
                ClientName = HttpConstant.DEFAULT_CLIENT_NAME, // Client name (default: GEEK_FRAMEWORK_HTTP_CLIENT)
                MaxConnectionsPerServer = 10000, // TCP connections (default: 10000)
                SleepDurations = new List<TimeSpan>() // Retry policy (default: none, as follows are retries at intervals of 3 seconds, 5 seconds, and 10 seconds)
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                },
                AdvancedCircuitBreakerOptions = new AdvancedCircuitBreakerOptions()
                {
                    DurationOfBreak = 3, // Duration of break (unit: seconds, default value: 3 seconds)
                    FailureThreshold = 80, // Failure threshold (break if at least n% are failing, default value: 80%)
                    SamplingDuration = 10, // Sampling duration (unit: seconds, default value: 10 seconds)
                    MinimumThroughput = 10000 // Minimum throughput (how many times it's called at least, default: 10000)
                }
            })
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

### Configuration Injection (Recommended)

Configuration injection refers to placing the custom HttpClientOptions used for injection into the configuration file. This approach is beneficial in a cluster environment to unify configurations through the configuration center.

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;

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
        /// Create system host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateDefaultHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseAutofac()
            .UseConfig()
            .UseNacosConf()
            .UseHttpClient(config => config.Get<HttpClientOptions>("HttpClientOptionsConfigKey
```

## Service Introduction

Both `HttpContractService` and `HttpClientService` are services for sending HTTP requests. The difference between them lies in the type of model data they return:

- The `HttpContractService` returns data models uniformly as `ContractResult` and `ContractResult<T>`, which is more convenient for calls within internal microservices.
- The `HttpClientService` returns a broader range of data models, mainly targeting external systems and scenarios that don't fit the `ContractResult` and `ContractResult<T>` model.

?> Note: The DELETE and POST methods inside the `HttpContractService` and `HttpClientService` follow the same calling convention as shown below.

### HttpContractService

#### Request and return `ContractResult`

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
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
        /// HTTP client service
        /// </summary>
        public HttpContractService HttpContractService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult> ExecuteAsync()
        {
            var url = $"http://localhost:8080/api/test/get"; // API address
            var param = new Dictionary<string, string>() { { "id", $"{Guid.NewGuid()}" } }; // Optional
            var headers = new Dictionary<string, string>() { { "Token", $"SASDASDOOWERWEWERWERWE" } }; // Optional
            return await HttpContractService.GetAsync(url, param, headers);
        }
    }
}
```

#### Request and return `ContractResult<T>`

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.Models;
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
        /// HTTP client service
        /// </summary>
        public HttpContractService HttpContractService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var url = $"http://localhost:8080/api/test/get"; // API address
            var param = new Dictionary<string, string>() { { "id", $"{Guid.NewGuid()}" } }; // Optional
            var headers = new Dictionary<string, string>() { { "Token", $"SASDASDOOWERWEWERWERWE" } }; // Optional
            return await HttpContractService.GetAsync<WeatherForecastModel>(url, param, headers);
        }
    }
}
```

### HttpClientService

#### Request and return a `String`

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
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
        /// HTTP client service
        /// </summary>
        public HttpClientService HttpClientService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult<string>> ExecuteAsync()
        {
            var result = new ContractResult<string>();
            var url = $"http://localhost:8080/api/test/get"; // API address
            var param = new Dictionary<string, string>() { { "id", $"{Guid.NewGuid()}" } }; // Optional
            var headers = new Dictionary<string, string>() { { "Token", $"SASDASDOOWERWEWERWERWE" } }; // Optional
            var response = await HttpClientService.GetAsync(url, param, headers);
            return result.SetSuceccful(response);
        }
    }
}
```

#### Request and return a generic type `T`

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.Models;
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
        /// HTTP client service
        /// </summary>
        public HttpClientService HttpClientService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpGet("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var result = new ContractResult<WeatherForecastModel>();
            var url = $"http://localhost:8080/api/test/get"; // API address
            var param = new Dictionary<string, string>() { { "id", $"{Guid.NewGuid()}" } }; // Optional
            var headers = a Dictionary<string, string>() { { "Token", $"SASDASDOOWERWEWERWERWE" } }; // Optional
            var response = await HttpClientService.GetAsync<WeatherForecastModel>(url, param, headers);
            return result.SetSuceccful(response);
        }
    }
}
```
