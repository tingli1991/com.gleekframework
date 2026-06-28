## 项目

> Com.GleekFramework.ObjectSdk

## 依赖

- [Config 组件](/docs/zh-cn/components/ConfigSdk.md)
- [Autofac 组件](/docs/zh-cn/components/AutofacSdk.md)

## 概述

对象存储开发工具包，目前只实现了阿里云的 oss 对象存储，支持对象的上传和下载，具体实现可以参考`AliyunOSSService`

## 注入

下面是一个简单是示例，高阶的用法是我们可以将`AliyunOSSOptions`的配置直接通过`config`将其放入到配置文件当中，方便统一管理和维护。

```C#
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.ObjectSdk;

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
            .UseAliyunOSS(config => new AliyunOSSOptions()
            {
                AccessKey = "",//SDK AppID
                AccessSecret = "",//App Key
                BucketUrl = "",//Bucket 域名
                Endpoint = "",//上传/下载是使用的终结点地址
            });
    }
}
```

## 示例

### 上传文件

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.ObjectSdk;
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
        /// 阿里云OSS服务
        /// </summary>
        public AliyunOSSService AliyunOSSService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<string>> ExecuteAsync(IFormFile file)
        {
            var result = new ContractResult<string>();
            var bucketName = "test_bucketName";//Bucket名称
            var filePath = @"/text/temp";//文件存放路径
            var response = await AliyunOSSService.PutObjectAsync(bucketName, filePath, file);//上传文件（并返回文件路径）
            return result.SetSuceccful(response);
        }
    }
}
```

### 上传文本

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.ObjectSdk;
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
        /// 阿里云OSS服务
        /// </summary>
        public AliyunOSSService AliyunOSSService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<string>> ExecuteAsync(string content)
        {
            var result = new ContractResult<string>();
            var bucketName = "test_bucketName";//Bucket名称
            var filePath = @"/text/temp";//文件存放路径
            var response = await AliyunOSSService.PutContentAsync(bucketName, filePath, content);//上传文本（并返回文件路径）
            return result.SetSuceccful(response);
        }
    }
}
```

### 获取文本

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.ObjectSdk;
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
        /// 阿里云OSS服务
        /// </summary>
        public AliyunOSSService AliyunOSSService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<string>> ExecuteAsync()
        {
            var result = new ContractResult<string>();
            var bucketName = "test_bucketName";//Bucket名称
            var fileName = @"/text/temp/xxx.txt";//文件存放路径
            var content = await AliyunOSSService.GetContentAsync(bucketName, fileName);//获取文本
            return result.SetSuceccful(content);
        }
    }
}
```

### 下载文件

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.ObjectSdk;
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
        /// 阿里云OSS服务
        /// </summary>
        public AliyunOSSService AliyunOSSService { get; set; }

        /// <summary>
        /// 测试执行方法
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<string>> ExecuteAsync()
        {
            var result = new ContractResult<string>();
            var bucketName = "test_bucketName";//Bucket名称
            var path = "/text/temp/";//路径
            var fileName = @"xxx.txt";//文件存放路径
            var downFileName = await AliyunOSSService.Download(bucketName, path, fileName);//下载文件(并返回文件存放目录)
            return result.SetSuceccful(downFileName);
        }
    }
}
```
