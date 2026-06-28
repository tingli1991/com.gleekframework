## Project

> Com.GleekFramework.NacosSdk

## Dependencies

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.ConfigSdk

## Overview

A Nacos SDK based on interface implementation.

- Supports configuration center subscription.
- Configurable configuration monitoring.
- Supports service registration and discovery.
- Supports configuration and service heartbeat monitoring, automatic configuration update.
- Configuration listening is separated from configuration file reading (configuration reading uses ConfigSDK for reading).

## Configuration Management

`NacosSdk` uses `nacos.json` as the subscription configuration file for Nacos by default. Any files that need to go through `Nacos` can be configured here.

> It is not recommended to subscribe to `nacos.json` and `bootstrap.json` for actual application scenarios with `NacoSDK`.

### Configuration Interpretation

```Json
{
  "Switch": true,                                              //Nacos feature configuration switch, default: on
  "UserName": "nacos",                                         //Nacos login account (optional if not available)
  "ListenInterval": 15000,                                     //Heartbeat interval (in milliseconds), default: 3000 milliseconds
  "Password": "ChinaNet910111",                                //Nacos login password (optional if not available)
  "GroupName": "org-gleek-fromework",                          //Group ID, default: DEFAULT_GROUP
  "NamespaceId": "bc8b30e8-5bd4-42b2-9099-82031950928a",       //Namespace ID, default: public
  "ServerAddresses": [                                         //Nacos addresses, multiple can be provided for clusters
    "http://192.168.100.25:8848/"
  ],
  "ConfigSettings": {                                          //Configuration center parameter node
    "RelativePath": true,                                      //Whether to use a relative path, if true, then read configuration from the execution directory of the program
    "ConfigPath": "Config",                                    //Configuration file storage path, if RelativePath is false, then a full path is provided, if true, path after the execution directory is sufficient
    "ConfitOptions": [                                         //Specific configuration items
      {
        "NamespaceId": "public",                               //Namespace ID, default: uses parent level's NamespaceId
        "ConfigName": "share.json",                            //Configuration file name within the program
        "DataId": "org-gleek-fromework-share.json"             //Nacos corresponding dataId
      },
      {
        "ConfigName": "application.json",                      //Configuration file name within the program
        "DataId": "org-gleek-fromework-application.json"       //Nacos corresponding dataId
      }
    ]
  }
}
```

### Configuration Registration

To register, use the `UseNacosConf()` method.

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
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
        /// Create the system host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IHostBuilder CreateDefaultHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseAutofac()
            .UseConfig()
            .UseNacosConf()
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

## Service Management

### Service Configuration

```C#
{
  "Switch": true,                                              //Nacos feature configuration switch, default: on
  "UserName": "nacos",                                         //Nacos login account (optional if not available)
  "ListenInterval": 15000,                                     //Heartbeat interval (in milliseconds), default: 3000 milliseconds
  "Password": "ChinaNet910111",                                //Nacos login password (optional if not available)
  "GroupName": "org-gleek-fromework",                          //Group ID, default: DEFAULT_GROUP
  "NamespaceId": "bc8b30e8-5bd4-42b2-9099-82031950928a",       //Namespace ID, default: public
  "ServerAddresses": [                                         //Nacos addresses, multiple can be provided for clusters
    "http://192.168.100.25:8848/"
  ],
  "ServiceSettings": {                                         //Service options to listen for
    "IP": "192.168.100.1",                                     //IP address, default: obtains current machine's NIC IP (internal network)
    "Port": 8080,                                              //Port, default: obtains environment variable PORT
    "Scheme": "http",                                          //Protocol, default: http
    "PrivateService":false,                                    //Whether private deployment or not, default: false
    "NamespaceId": "public",                                   //Namespace ID, default: uses parent level's NamespaceId
    "ConfigName": "share.json",                                //Configuration file name within the program
    "ClusterName": "",                                         //Cluster name
    "Weight": 10,                                              //Current instance weight, default: 10
    "ExpireSeconds": 10,                                       //Expiration time (in seconds), default: 10
    "ClientOptions": [                                         //List of Nacos service client configurations (optional, not needed if `NacosHttpService` and `NacosHttpContractService` clients aren't used)
      {
        "Clusters":"",                                         //Cluster name(s) (string, multiple clusters separated by commas)
        "NamespaceId": "public",                               //Namespace ID, default: uses parent level's NamespaceId
        "GroupName": "share.json",                             //Group ID, default: DEFAULT_GROUP
        "ServiceName": "userService"                           //Corresponding dataId for Nacos
      },
      {
        "Clusters":"",                                         //Cluster name(s) (string, multiple clusters separated by commas)
        "NamespaceId": "public",                               //Namespace ID, default: uses parent level's NamespaceId
        "GroupName": "share.json",                             //Group ID, default: DEFAULT_GROUP
        "ServiceName": "authService"                           //Corresponding dataId for Nacos
      }
    ]
  }
}
```

### Configuration Registration

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.DapperSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.Models;
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
                 .UseNacosService("testService")//Register service, testService indicates the service name
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
            .UseConfig()
            .UseNacosConf()
            .UseHttpClient()
            .UseGleekWebHostDefaults<Startup>()
            .UseDapper(DatabaseConstant.DefaultMySQLHostsKey);
    }
}
```

### Service Invocation

The biggest difference between `NacosHttpContractService` and `NacosHttpService` is the return model they use, `NacosHttpContractService` is for receiving a return model of `ContractResult`, whereas `NacosHttpService` accepts a native model result.

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;
using Microsoft.AspNetCore.Mvc;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// Test Controller
    /// </summary>
    [Route("test")]
    public class TestController : BaseController
    {
        /// <summary>
        ///
        /// </summary>
        public NacosHttpContractService NacosHttpContractService { get; set; }

        /// <summary>
        /// Test execute method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<WeatherForecastModel>> ExecuteAsync()
        {
            var serviceName = "userService";
            var param = new Dictionary<string, string>()
            {
                { "userId","100"}
            };
            return await NacosHttpContractService.GetAsync<WeatherForecastModel>(serviceName, "api/user/get", param);
        }
    }
}
```
