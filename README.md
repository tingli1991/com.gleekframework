## [com.gleekframework](http://www.gleekframework.com/)
该框架是一个高性能的组件集合，适用于承载大流量网站。采用分布式架构和微服务的设计理念，经历过单日40亿PV流量的考验。该框架遵循极简的原则，以约定大于配置的规则为基础，帮助开发人员快速构建稳定可靠的系统。 无论是大型企业级项目还是小型应用，该框架都能满足各种需求，提供高效的开发和部署方案，同时代码风格也及其简单且统一。  

## 官方文档
更多的介绍请查阅[官方文档](http://www.gleekframework.com/)

## 阅读建议
在每个项目的根目录都会有对用的MD文档，具体的代码使用示例、规范以及注意事项等，我都会在对应的项目目录会采用图文+代码示例的方式给大家进行详细讲解，也希望能给大家带来更高效和简洁的开发模式，同时也希望大家多给点建议(在下感激不尽)。

## 项目概览
|           项目名称                 |           项目描述                |              项目用途                                                                  |
|:-----------------------------------|:----------------------------------|:---------------------------------------------------------------------------------------|
| Com.GleekFramework.AssemblySdk     | 程序集开发工具包                  |管理和扫码程序集                                                                        |
| Com.GleekFramework.AttributeSdk    | 特性工具包                        |例如WebAPI的全局异常捕获、接口请求参数模型验证、接口切面日志以及基础控制器的定义等      |
| Com.GleekFramework.AuthorizeSdk    | 认证授权开发工具包                |主要用于做认证授权相关的功能拓展和规范 (待完善)                                         |
| Com.GleekFramework.AutofacSdk      | IOC(Autofac)开发工具包            |利用Autofac重写NET自带的IOC容器(所有的DI统一采用属性注入的方式)                         |
| Com.GleekFramework.CommonSdk       | 基础的开发工具包                  |约定和定义通用的特性、常量、枚举、拓展方法、数据转换方法以及验证方法等                  |
| Com.GleekFramework.ConfigSdk       | 配置文件拓展工具包                |管理和约定配置的定义以及读取方式                                                        |
| Com.GleekFramework.ConsumerSdk     | 消费者开发工具包                  |管理和约定消费的的实现规范以及自定义了一套基于消费者的消费者的切面开发规范和切面开发能力|
| Com.GleekFramework.ContractSdk     | 公共契约拓展工具包                |定义和约束项目暴露的契约数据(例如：接口的返回模型、项目的基础错误码以及流水号生成规范等)|
| Com.GleekFramework.DapperSdk       | ORM(Dapper)拓展工具包             |基于Dapper和DapperExtensions整合出来的ORM组件                                           |
| Com.GleekFramework.ElasticsearchSdk| ES拓展工具包                      |基于ES的CRUD等相关共嗯的约束和拓展 (待完善)                                             |
| Com.GleekFramework.GrpcSdk         | Grpc拓展工具包                    |基于Grpc的注入以及客户端的实现(待完善)                                                  |
| Com.GleekFramework.HttpSdk         | Http拓展工具包                    |HTTP请求的包装、支持重试、熔断等功能                                                    |
| Com.GleekFramework.KafkaSdk        | Kafka拓展工具包                   |用于约束生产和消费的编码规范、优雅停机以及AOP等能力                                     |
| Com.GleekFramework.MigrationSdk    | 版本迁移拓展工具包                |主要用于在进程内实现自动升级数据库架构、数据迁移、初始化脚本执行等                      |
| Com.GleekFramework.MongodbSdk      | Mongodb拓展工具包                 |Mongodb的数据库操作工具包                                                               |
| Com.GleekFramework.MqttSdk         | Mqtt拓展工具包                    |点对点的消息发送(带完善)                                                                |
| Com.GleekFramework.NacosSdk        | Nacos拓展工具包                   |配置中心、注册中心、服务注册与发现(服务发现目前只实现了http的通信)                      |
| Com.GleekFramework.NLogSdk         | NLog拓展工具包                    |基于NLog实现的日志输出框架                                                              |
| Com.GleekFramework.ObjectSdk       | 对象存储开发工具包                |阿里云、腾讯云的对象存储上传和下载(目前只实现了阿里云OSS)                               |
| Com.GleekFramework.OpenSdk         | OpenAPI拓展工具包                 |调用网关的统一封装(待完善)                                                              |
| Com.GleekFramework.QueueSdk        | 本地队列展工具包                  |先进先出的队列和先进后出的栈的功能封装                                                  |
| Com.GleekFramework.RabbitMQSdk     | RabbitMQ拓展工具包                |工作模式、发布订阅模式、RPC模式以及延迟队列(延迟还在开发)                               |
| Com.GleekFramework.RedisSdk        | Redis拓展工具包                   |基础类型的封装以及强大的分布式锁的拓展                                                  |
| Com.GleekFramework.RocketMQSdk     | RocketMQ拓展工具包                |目前只实现了普通消息和延迟消息的生产和消费(基本够用)                                    |
| Com.GleekFramework.SecuritySdk     | 加密/解密拓展工具包               |主要用于一些数据安全方面的加解密算法(待完善)                                            |
| Com.GleekFramework.SwaggerSdk      | Swagger文档拓展工具包             |利用Knife4jUI对旧的SWAGGER文档主题进行重新渲染                                          |

## 项目目录结构的规范概览
|           目录名称                 |           目录描述                |              规范描述                                                                                 |
|:-----------------------------------|:----------------------------------|:------------------------------------------------------------------------------------------------------|
| Services                           | 基于属性注入实现的服务            |所有的属性注入建议统一放到Services目录下面(业务可以用二级目录区分)                                     |
| Attributes                         | 自定义特性目录                    |所有自定义的特性统一都在该目录下面                                                                     |
| Controllers                        | 自定义接口控制器                  |框架内自定义的通用控制器(建议控制器统一放到该目录)                                                     |
| Extensions                         | 自定义的拓展方法                  |存放框架编写的所有拓展方法                                                                             |
| Hostings                           | 自定义组件的注入方法              |所有的组件使用必须先在该目录下面找到其注入方法并进行注入之后才能正常使用                               |
| Middlewares                        | 自定义中间件                      |存放所有的中间件(例如日志中间件)                                                                       |
| Validations                        | 自定义验证方法                    |存放所有的校验功能的方法(例如：不等于的模型验证)                                                       |
| Constants                          | 自定义常量                        |存放所有自定义的常量                                                                                   |
| Factorys                           | 自定义工厂                        |存放所有自定义的工厂类                                                                                 |
| Interfaces                         | 自定义接口                        |存放所有自定义的自定义接口                                                                             |
| Modules                            | 自定义模型                        |存放所有自定义的自定义模型                                                                             |
| Providers                          | 自定义业务实现类                  |存放所有自定义业务实现类(通常这些类都是在IOC注入之前使用，IOC注入完成之后，请使用Service进行调用)      |
| Enums                              | 自定义枚举                        |存放所有自定义枚举                                                                                     |
| Mappers                            | 自定义映射类                      |存放所有自定义映射类(例如：但是你模型映射和集合映射等)                                                 |
| VerifyExtensions                   | 自定义数据验证拓展                |存放所有自定义的数据验证拓展类(例如，验证省份证号码、字符串是否是整数等)                               |
| Configs                            | 自定义配置文件对象                |存放所有自定义配置文件（例如：AppConfig配置文件对象，则定义了它的注入，配置读取等功能）                |
| Watchers                           | 自定义监听类业务                  |存放所有自定义监听业务(例如：配置文件热重载事件监听)                                                   |
| Contexts                           | 自定义上下文                      |例如：消费者AOP里面的方法调用过滤器上下文                                                              |
| Params                             | 自定义请求参数                    |例如：接口的请求参数或者消费者接收的数据包等等                                                         |
| Results                            | 自定义返回模型                    |例如：接口的统一返回模型                                                                               |
| Repositorys                        | 自定义数据库仓储文件              |存放所有数据库仓储文件                                                                                 |
| Interceptors                       | 自定义拦截器                      |存放所有拦截器                                                                                         |
| Options                            | 自定义配置选项                    |存放所有配置选项(例如：使用OSS的时候在注入之前必须要有AccessKey的字段，那么就可以用Options的模型来定义)|                                                                                 |
| Migrations                         | 自定义版本迁移目录                |存放所有的版本迁移文件                                                                                 |
| Upgrations                         | 自定义版本升级目录                |存放所有的版本升级文件                                                                                 |
| Clients                            | 自定义客户端实现类                |存放所有的客户端实现类                                                                                 |
| Entitys                            | 自定义实体对象                    |存放所有的数据实体对象                                                                                 |
| Config                             | 配置文件存放目录                  |存放所有的配置文件(例如：nacos.json文件)                                                               |
| Handlers                           | 定义所有的消费者处理类            |存放所有的消费者处理类文件(类似于接口的Controllers)                                                    |

## 项目启动时常用注入参考
### WEB项目的注入示例
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
                 .SubscribeStack((config) => 24)//订阅本地栈(先进先出)
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
/// 程序启动类
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

### 控制台项目的注入示例
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