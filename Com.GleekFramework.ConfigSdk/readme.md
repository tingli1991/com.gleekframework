## 配置文件拓展工具包
### 约定/规范
#### 环境变量的约定
|           变量名称                 |           变量描述                       |                 用途                                                                              |
|:-----------------------------------|:-----------------------------------------|:--------------------------------------------------------------------------------------------------|
| ENV                                | 系统环境                                 |用于区分开发(dev)/测试(test)/预发布(uat)/生产(pro)等环境，默认为空字符串                           |
| PORT                               | 端口号                                   |定义程序运行和启动的端口号(默认为：8080)                                                           |
| PROJECT                            | 项目名称                                 |用于区分项目(主要用于，把一个解决方案拆分成多个服务来部署的场景)                                   |
| VERSION                            | 版本号                                   |主要用户K8S部署的时候区分环境使用(例如：k8s的蓝绿环境)                                             |
| NOCOS_URL                          | Nacos的监听端口                          |如果设置该环境变量,Nacos则优先使用该地址进行配置监听以及服务注册和发现                             |
| SWAGGER_SWITCH                     | Swagger文档的开关配置                    |通常线上环境我们不会暴露Swagger的文档地址，但是非生产环境则需要，那么就可以使用该环境变量来进行控制|

#### 配置文件约定
|           文件名称                 |           文件描述                       |                 用途                                                                                                                          |
|:-----------------------------------|:-----------------------------------------|:----------------------------------------------------------------------------------------------------------------------------------------------|
| share.json                         | 共享配置                                 |用于解决多个项目(或者解决方案)需要使用相同配置的情况下,我们就可以把这部分配置放到share.json里面                                                |
| application.json                   | 当前应用程序配置                         |隶属于当前项目的所有配置统一放到该文件内                                                                                                       |
| bootstrap.json                     | 当前项目的固定配置                       |用于存放当前项目的固定配置,例如：项目A同时部署了a、b 2个节点(a是4核8g;b是8核16g)，这时候我们需要控制2台机器的线程数，这种配置则可以放到该文件内|
| subscription.json                  | 订阅配置                                 |主要用于存放消费者的订阅配置(例如：kafka的消费者订阅主机以及topic列表等)                                                                       |

#### 规范
1. 启用配置文件必须先在`Program`里面调用`HostingExtensions.cs`的`UseConfig()`方法;
2. 所有的配置文件全部采用`.json`的格式;
3. 所有的配置文件取值统一通过`AppConfig.Configuration.GetValue()`(或者 `AppConfig.Configuration.Get()`)的方式访问(具体还有哪些方法请参见`JsonConfigExtensions.cs`的实现);
4. 属性配置,那么类名必须是待无参构造函数的普通类(不能是静态或者抽象类以及接口等),同时属性必须是静态的;
5. 属性配置,属性处理要是静态意外，还必须添加`Config`特性进行配置的取值;
6. 属性配置，必须在`Program`里面调用`HostingExtensions.cs`的`UseConfigAttribute()`方法来启用属性配置定义和访问规则;
7. 启用了属性配置之后，当属性对应的配置发生变化的时候，程序会自动刷新对应的属性值;
8. 如果设置了ENV的环境变量，那么所有通过SDK读取配置文件的时候都会去读取对应环境的`json`配置(例如：share.json 如果ENV=dev，那么share.json 则变更成share.dev.json，同理PROJECT=test的时候，那么share.json则编程share.dev.test.json);

# 配置文件优先级
当相同的配置key出现在不同的`json`配置文件当中，再回按照如下的优先级进行读取;
> share.json  -->  application.json  --> subscription.json  --> bootstrap.json
#### 程序启动是注入SDK
``` C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
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
            .UseAutofac()
            .UseConfig()
            .UseConfigAttribute()
            .UseGleekWebHostDefaults<Startup>();
    }
}
``` 

#### 普通配置的定义和取值方式
``` C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 天气预报控制器
    /// </summary>
    [Route("weather-forecast")]
    public class WeatherForecastController : BaseController
    {
        /// <summary>
        /// 注入配置信息
        /// </summary>
        [Config(Models.ConfigConstant.SummariesKey)]
        public static string[] Summaries { get; set; }

        /// <summary>
        /// 测试配置
        /// </summary>
        [Config(Models.ConfigConstant.ConfigOptionsKey)]
        public static ConfigOptions ConfitOptions { get; set; }

        /// <summary>
        /// 获取天气预报
        /// </summary>
        /// <param name="id">id(用于测试字段必填)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<WeatherForecastModel>> GetAsync([Required] int id)
        {
            var testValueList = AppConfig.Configuration.Get<List<string>>("Summaries");
            return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = testValueList[Random.Shared.Next(testValueList.Count)]
            }));
        }
    }
}
```

#### 属性配置的定义和取值方式
``` C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AppSvc.Controllers
{
    /// <summary>
    /// 天气预报控制器
    /// </summary>
    [Route("weather-forecast")]
    public class WeatherForecastController : BaseController
    {
        /// <summary>
        /// 注入配置信息
        /// </summary>
        [Config(Models.ConfigConstant.SummariesKey)]
        public static string[] Summaries { get; set; }

        /// <summary>
        /// 测试配置
        /// </summary>
        [Config(Models.ConfigConstant.ConfigOptionsKey)]
        public static ConfigOptions ConfitOptions { get; set; }

        /// <summary>
        /// 获取天气预报
        /// </summary>
        /// <param name="id">id(用于测试字段必填)</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<WeatherForecastModel>> GetAsync([Required] int id)
        {
            var test = ConfitOptions.Id;
            return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecastModel
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }));
        }
    }
}
```

