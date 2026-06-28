## Project

> Com.GleekFramework.ObjectSdk

## Dependencies

- [Quick Start](/docs/en-us/quickstart.md)
- [Structure Recommendation](/docs/en-us/recommended.md)

## Overview

Object storage development toolkit, currently only the implementation for Alibaba Cloud's OSS object storage is available, supporting object upload and download. For specific implementation, refer to `AliyunOSSService`.

## Injection

Here is a simple example, and for advanced usages, we can directly place the configuration of `AliyunOSSOptions` into the config file through config, for convenient centralized management and maintenance.

```C#
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.ObjectSdk;

namespace Com.GleekFramework.AppSvc
{
    /// <summary>
    /// Application Class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main method of the program
        /// </summary>
        /// <param name="args"></param>
        public static async Task Main(string[] args)
        {
            await CreateDefaultHostBuilder(args)
                 .Build()
                 .RunAsync();
        }

        /// <summary>
        /// Create default system host
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
                AccessKey = "", //SDK AppID
                AccessSecret = "", //App Key
                BucketUrl = "", //Bucket domain name
                Endpoint = "", //Endpoint address for upload/download
            });
    }
}
```

## Example

### Upload File

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.ObjectSdk;
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
        /// Alibaba Cloud OSS Service
        /// </summary>
        public AliyunOSSService AliyunOSSService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<string>> ExecuteAsync(IFormFile file)
        {
            var result = new ContractResult<string>();
            var bucketName = "test_bucketName"; //Bucket name
            var filePath = @"/text/temp"; //File path
            var response = await AliyunOSSService.PutObjectAsync(bucketName, filePath, file); //Upload file (and return file path)
            return result.SetSuceccful(response);
        }
    }
}
```

### Upload Text

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.ObjectSdk;
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
        /// Alibaba Cloud OSS Service
        /// </summary>
        public AliyunOSSService AliyunOSSService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<string>> ExecuteAsync(string content)
        {
            var result = new ContractResult<string>();
            var bucketName = "test_bucketName"; //Bucket name
            var filePath = @"/text/temp"; //File path
            var response = await AliyunOSSService.PutContentAsync(bucketName, filePath, content); //Upload text (and return file path)
            return result.SetSuceccful(response);
        }
    }
}
```

### Retrieve Text

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.ObjectSdk;
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
        /// Alibaba Cloud OSS Service
        /// </summary>
        public AliyunOSSService AliyunOSSService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<string>> ExecuteAsync()
        {
            var result = new ContractResult<string>();
            var bucketName = "test_bucketName"; //Bucket name
            var fileName = @"/text/temp/xxx.txt"; //File path
            var content = await AliyunOSSService.GetContentAsync(bucketName, fileName); //Get text content
            return result.SetSuceccful(content);
        }
    }
}
```

### Download File

```C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.ObjectSdk;
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
        /// Alibaba Cloud OSS Service
        /// </summary>
        public AliyunOSSService AliyunOSSService { get; set; }

        /// <summary>
        /// Test execution method
        /// </summary>
        /// <returns></returns>
        [HttpPost("execute")]
        public async Task<ContractResult<string>> ExecuteAsync()
        {
            var result = new ContractResult<string>();
            var bucketName = "test_bucketName"; //Bucket name
            var path = "/text/temp/"; //Path
            var fileName = @"xxx.txt"; //File path
            var downFileName = await AliyunOSSService.Download(bucketName, path, fileName); //Download file (and return the directory where the file is stored)
            return result.SetSuceccful(downFileName);
        }
    }
}
```
