## 特性工具包
该项目主要是定义和约束一些主机的注入规范/全局异常捕获/约束心跳检测(k8s交互使用)/请求参数模型验证以及全局日志中间件

### 心跳检测接口统一规范说明
该心跳接口默认返回 "ok"，也可以通过方法 UseHealthChecks 的 callback 参数自定义返回结果
##### 接口定义如下
``` C#
http://localhost:8080/health
```
##### 自定义心跳返回结果
``` C#
//使用心跳检测(自定义返回结果为当前的服务器时间)
app.UseHealthChecks(() => $"{DateTime.Now:yyyyMMdd HH:mm:ss}");
```

### 主机注入方法介绍(HostingExtensions)
|           方法名称                 |           方法描述                |              方法用途                                                                          |
|:-----------------------------------|:----------------------------------|:-----------------------------------------------------------------------------------------------|
| UseGleekConsumerHostDefaults       | 使用默认的消费者主机               |主要用于控制台项目注入使用(会自动添加心跳检测接口/约定Host环境变量)                             |
| UseGleekWebHostDefaults            | 默认的Web主机                     |主要用于Web站点的项目注入(该主机默认会将Net自带的控制台日志调整为警告级别/约定Host环境变量)     |
| LibraryService                     | 编译库服务                        |快速获取运行目录下的所有编译库名称列表                                                          |
| UseHealthChecks                    | 使用心跳检测                      |使用心跳检测(调用了UseGleekConsumerHostDefaults该方法则不需要重复调用)                          |
| AddGlobalExceptionAttribute        | 添加全局异常                      |主要是Web站点用来捕获全局异常(建议：如果添加了这个，那么代码实现的时候除异步方法以外都可以不用try catch，程序会自动捕获并使用NLog输出日志)|
| AddModelValidAttribute             | 添加模型验证                      |主要针对Web项目，用来做请求参数的格式验证，例如：字段必填，长度限制等等                         |
| AddNewtonsoftJson                  | 添加默认的JSON配置参数            |主要针对Web项目，定义固定的时间输出格式，禁用默认的驼峰命名，使用本地时区等                     |

#### Web项目的注入示例
``` C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.MigrationSdk;
using Com.GleekFramework.Models;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.QueueSdk;

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
                 .SubscribeStack((config) => 24)//订阅本地栈(先进显出)
                 .SubscribeQueue((config) => 24)//订阅本地队列(先进后出)
                 .UseMigrations((config) => new MigrationOptions()
                 {
                     MigrationSwitch = true,
                     UpgrationSwitch = true,
                     DatabaseType = DatabaseType.MySQL,
                     ConnectionString = config.GetConnectionString(DatabaseConstant.DefaultMySQLHostsKey)
                 })
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
        services.AddKnife4Gen("测试文档");//添加Knife4生成器

        services.AddNewtonsoftJson();//添加对JSON的默认格式化
        services.AddDistributedMemoryCache();//添加分布式内存缓存
        services.AddGlobalExceptionAttribute();//添加全局异常
        services.AddModelValidAttribute<MessageCode>();//添加模型验证
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();//添加Cookie支持
    }

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="app"></param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseKnife4UI();//使用Knife4UI界面
        app.UseStaticFiles();//使用静态资源
        app.UseRouting();//使用路由规则
        app.UseHealthChecks();//使用心跳检测
        app.UseAuthentication();//启用授权
        app.UseEndpoints(endpoints => endpoints.MapControllers());//启用终结点配置
        app.RegisterApplicationStarted(() => Console.Out.WriteLine($"服务启动成功：{EnvironmentProvider.GetHost()}"));
    }
}
```

#### 控制台项目的注入示例
``` C#
using Com.GleekFramework.AttributeSdk;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.KafkaSdk;
using Com.GleekFramework.NacosSdk;
using Com.GleekFramework.RabbitMQSdk;

namespace Com.GleekFramework.ConsumerSvc
{
    /// <summary>
    /// 主程序
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
                 .SubscribeKafka(config => config.Get<KafkaConsumerOptions>(Models.ConfigConstant.KafkaConnectionOptionsKey))
                 .SubscribeRabbitMQ(config => config.Get<RabbitConsumerOptions>(Models.ConfigConstant.RabbitConnectionOptionsKey))
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
            .UseWindowsService()
            .UseConfigAttribute()
            .UseGleekConsumerHostDefaults();
    }
}
```