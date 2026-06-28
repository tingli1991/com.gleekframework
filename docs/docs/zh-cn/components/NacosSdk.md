## 项目

> Com.GleekFramework.NacosSdk

## 依赖

> Com.GleekFramework.HttpSdk

> Com.GleekFramework.NLogSdk

> Com.GleekFramework.ConfigSdk

## 概述

一款基于接口实现的 `Nacos` SKD。

- 支持配置中心订阅。
- 配置监控可配置化。
- 支持服务注册与发现。
- 支持配置和服务心跳监听，配置自动更新。
- 配置监听与配置文件读取分离(配置读取使用 ConfigSDK 进行读取)。

## 配置管理

`NacosSdk`固定使用`nacos.json`作为订阅`Nacos`的配置文件，那些文件需要走`Nacos`都可以在这里进行配置。

!> 对于`NacoSDK`而言，所有的配置都能够订阅`Nacos`，但是在实际应用场景中，不建议大家把`nacos.json`和`bootstrap.json`也进行订阅。

### 配置解读

```Json
{
  "Switch": true,                                              //Nacos功能配置开关，默认：开启
  "UserName": "nacos",                                         //Nacos登录账号(没有可不传)
  "ListenInterval": 15000,                                     //心跳间隔时长（单位：毫秒），默认：3000毫秒
  "Password": "ChinaNet910111",                                //Nacos登录密码(没有可不传)
  "GroupName": "org-gleek-fromework",                          //分组Id，默认：DEFAULT_GROUP
  "NamespaceId": "bc8b30e8-5bd4-42b2-9099-82031950928a",       //空间Id，默认：public
  "ServerAddresses": [                                         //Nacos地址，如果是集群可以传多个
    "http://192.168.100.25:8848/"
  ],
  "ConfigSettings": {                                          //配置中心参数节点
    "RelativePath": true,                                      //是否使用相对路径，如果为true，那么读取配置从程序执行目录开始
    "ConfigPath": "Config",                                    //配置文件存放路径，如果RelativePath为false，则传绝对路径，为true，传为序执行目录后面即可
    "ConfitOptions": [                                         //具体的配置项
      {
        "NamespaceId": "public",                               //空间Id，默认：采用父级的空间Id
        "ConfigName": "share.json",                            //程序内的配置文件名称
        "DataId": "org-gleek-fromework-share.json"             //Nacos对应的dataId
      },
      {
        "ConfigName": "application.json",                      //程序内的配置文件名称
        "DataId": "org-gleek-fromework-application.json"       //Nacos对应的dataId
      }
    ]
  }
}
```

### 配置注册

注册使用`UseNacosConf()`方法即可完成。

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
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
            .UseGleekWebHostDefaults<Startup>();
    }
}
```

## 服务管理

### 服务配置

```C#
{
  "Switch": true,                                              //Nacos功能配置开关，默认：开启
  "UserName": "nacos",                                         //Nacos登录账号(没有可不传)
  "ListenInterval": 15000,                                     //心跳间隔时长（单位：毫秒），默认：3000毫秒
  "Password": "ChinaNet910111",                                //Nacos登录密码(没有可不传)
  "GroupName": "org-gleek-fromework",                          //分组Id，默认：DEFAULT_GROUP
  "NamespaceId": "bc8b30e8-5bd4-42b2-9099-82031950928a",       //空间Id，默认：public
  "ServerAddresses": [                                         //Nacos地址，如果是集群可以传多个
    "http://192.168.100.25:8848/"
  ],
  "ServiceSettings": {                                         //监听的服务选项
    "IP": "192.168.100.1",                                     //IP地址，默认：获取当前机器的网卡IP(内网)
    "Port": 8080,                                              //端口，默认：获取环境变量PORT的端口
    "Scheme": "http",                                          //协议，默认:http
    "PrivateService":false,                                    //是否私有部署，默认：false
    "NamespaceId": "public",                                   //空间Id，默认：采用父级的空间Id
    "ConfigName": "share.json",                                //程序内的配置文件名称
    "ClusterName": "",                                         //集群名称
    "Weight": 10,                                              //当前实例的权重，默认：10
    "ExpireSeconds": 10,                                       //过期时间(单位：秒)，默认：10
    "ClientOptions": [                                         //Nacos服务客户端配置列表(可选，不使用`NacosHttpService`和`NacosHttpContractService`客户端的情况下无需配置)
      {
        "Clusters":"",                                         //集群名称(字符串，多个集群用逗号分隔)
        "NamespaceId": "public",                               //空间Id，默认：采用父级的空间Id
        "GroupName": "share.json",                             //分组Id，默认：DEFAULT_GROUP
        "ServiceName": "userService"                           //Nacos对应的dataId
      },
      {
        "Clusters":"",                                         //集群名称(字符串，多个集群用逗号分隔)
        "NamespaceId": "public",                               //空间Id，默认：采用父级的空间Id
        "GroupName": "share.json",                             //分组Id，默认：DEFAULT_GROUP
        "ServiceName": "authService"                           //Nacos对应的dataId
      }
    ]
  }
}
```

### 配置注册

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
                 .UseNacosService("testService")//注册服务，testService标识服务名称
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
            .UseGleekWebHostDefaults<Startup>()
            .UseDapper(DatabaseConstant.DefaultMySQLHostsKey);
    }
}
```

### 服务调用

`NacosHttpContractService` 跟 `NacosHttpService` 最大的区别在于返回的模型不同，`NacosHttpContractService`用于接收`ContractResult`的返回模型，而`NacosHttpService`接受原生模型结果。

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;
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
        ///
        /// </summary>
        public NacosHttpContractService NacosHttpContractService { get; set; }

        /// <summary>
        /// 测试执行方法
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
