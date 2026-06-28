# 快速开始

开始前建议大家先安装 [NuGet](https://www.nuget.org/) 服务端，这里推荐大家安装 [Nexus](https://www.sonatype.com/)，教程网上比较多，这里就不做过多的赘述了。

> 未上传 [NuGet](https://www.nuget.org/) 官方说明

- 一方面更推荐大家自建 [NuGet](https://www.nuget.org/) 服务端，私有化更安全。
- 另一方面是期望项目能够得到持续的迭代和反馈。
- 一起共建 NET 社群，让 NET 开发不再为了找工作而焦虑。

> 推荐[Nexus](https://www.sonatype.com/)理由

- [Nexus](https://www.sonatype.com/) 是目前业界广泛使用的二进制仓库管理工具，它拥有强大的功能和灵活的配置选项。
- [Nexus](https://www.sonatype.com/) 支持代理远程仓库和托管本地仓库，能够管理 Maven、npm、NuGet 等多种格式的包。
- [Nexus](https://www.sonatype.com/) 相比传统的 nuget server，[Nexus](https://www.sonatype.com/) 提供了更加全面的仓库管理功能。例如，[Nexus](https://www.sonatype.com/) 支持组仓库（Group Repository），允许用户通过单一的入口访问和搜索多个仓库。

## 打包指令

下面是我这边写的一个 bat 指令，在每个组件的目录下都有一个 nuget-push.bat 文件，大家也可以按照自己的需求进行调整(在使用之前需要调整 source_api_uri 和 api_key)

```bash
@echo off
if %time:~0,2% LEQ 9 (set now=%date:~0,4%%date:~5,2%%date:~8,2%0%time:~1,1%%time:~3,2%%time:~6,2%) else (set now=%date:~0,4%%date:~5,2%%date:~8,2%%time:~0,2%%time:~3,2%%time:~6,2%)

:: 指定上传的工程名称
set project_name=Com.GleekFramework.AttributeSdk

:: 指定上传的api key
set api_key=278466c7-23cc-3ec8-86d8-43adde285742

:: 指定上传的url
set source_api_uri=http://192.168.100.15:8081/repository/nuget-hosted/index.json

:: 获取当前文件夹
set current_dir=%~dp0%

:: 项目路径(解决方案路径)
set solution_dir=%current_dir%..\

:: 设置当前工程文件的全名称(包含路径)
set csproj_path=%solution_dir%%project_name%\%project_name%.csproj

:: 指定packg目录
set packg_dir=%solution_dir%nupkgs\%project_name%\%now%

:: 编译项目输出pack包
echo start build and pack %project_name% ...
dotnet pack %csproj_path%  -c Release -o %packg_dir% -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg

:: 批量推送包
echo start push %packg_name% ...
dotnet nuget push  %packg_dir%\*.nupkg  -k %api_key% -s %source_api_uri%
echo push %packg_name% finish ...
pause
```

## 核心组件说明

所谓的核心组件，就是我们所有的组件都将依赖于这些组件，它将作为我们所有组件实现的基础，例如：IOC

> Com.GleekFramework.AutofacSdk

- 项目内所有的 IOC 都是基于它来完成的，可以使用 UseAutofac() 方法来进行激活。

> Com.GleekFramework.ConfigSdk

- 项目内所有的配置文件的读取统一通过它来完成，可以使用 UseConfig()方法来激活，同时如果想要激活特性配置可以使用 UseConfigAttribute()方法。

## 组件清单列表

| 组件名称                        | 描述                   | 用途                                                                                     |
| :------------------------------ | :--------------------- | :--------------------------------------------------------------------------------------- |
| Com.GleekFramework.AssemblySdk  | 程序集开发工具包       | 管理和扫码程序集                                                                         |
| Com.GleekFramework.AttributeSdk | 特性工具包             | 例如 WebAPI 的全局异常捕获、接口请求参数模型验证、接口切面日志以及基础控制器的定义等     |
| Com.GleekFramework.AutofacSdk   | IOC(Autofac)开发工具包 | 利用 Autofac 重写 NET 自带的 IOC 容器(所有的 DI 统一采用属性注入的方式)                  |
| Com.GleekFramework.CommonSdk    | 基础的开发工具包       | 约定和定义通用的特性、常量、枚举、拓展方法、数据转换方法以及验证方法等                   |
| Com.GleekFramework.ConfigSdk    | 配置文件拓展工具包     | 管理和约定配置的定义以及读取方式                                                         |
| Com.GleekFramework.ConsumerSdk  | 消费者开发工具包       | 管理和约定消费的的实现规范以及自定义了一套基于消费者的消费者的切面开发规范和切面开发能力 |
| Com.GleekFramework.ContractSdk  | 公共契约拓展工具包     | 定义和约束项目暴露的契约数据(例如：接口的返回模型、项目的基础错误码以及流水号生成规范等) |
| Com.GleekFramework.DapperSdk    | ORM(Dapper)拓展工具包  | 基于 Dapper 和 DapperExtensions 整合出来的 ORM 组件                                      |
| Com.GleekFramework.HttpSdk      | Http 拓展工具包        | HTTP 请求的包装、支持重试、熔断等功能                                                    |
| Com.GleekFramework.KafkaSdk     | Kafka 拓展工具包       | 用于约束生产和消费的编码规范、优雅停机以及 AOP 等能力                                    |
| Com.GleekFramework.MigrationSdk | 版本迁移拓展工具包     | 主要用于在进程内实现自动升级数据库架构、数据迁移、初始化脚本执行等                       |
| Com.GleekFramework.MongodbSdk   | Mongodb 拓展工具包     | Mongodb 的数据库操作工具包                                                               |
| Com.GleekFramework.NacosSdk     | Nacos 拓展工具包       | 配置中心、注册中心、服务注册与发现(服务发现目前只实现了 http 的通信)                     |
| Com.GleekFramework.NLogSdk      | NLog 拓展工具包        | 基于 NLog 实现的日志输出框架                                                             |
| Com.GleekFramework.ObjectSdk    | 对象存储开发工具包     | 阿里云、腾讯云的对象存储上传和下载(目前只实现了阿里云 OSS)                               |
| Com.GleekFramework.QueueSdk     | 本地队列扩展工具包       | 先进先出的队列和先进后出的栈的功能封装                                                   |
| Com.GleekFramework.RabbitMQSdk  | RabbitMQ 拓展工具包    | 工作模式、发布订阅模式、RPC 模式以及延迟队列(延迟还在开发)                               |
| Com.GleekFramework.RedisSdk     | Redis 拓展工具包       | 基础类型的封装以及强大的分布式锁的拓展                                                   |
| Com.GleekFramework.RocketMQSdk  | RocketMQ 拓展工具包    | 目前只实现了普通消息和延迟消息的生产和消费(基本够用)                                     |
| Com.GleekFramework.SwaggerSdk   | Swagger 文档拓展工具包 | 利用 Knife4jUI 对旧的 SWAGGER 文档主题进行重新渲染                                       |

## 目录结构描述

| 目录名称         | 描述                   | 备注                                                                                                          |
| :--------------- | :--------------------- | :------------------------------------------------------------------------------------------------------------ |
| Services         | 基于属性注入实现的服务 | 所有的属性注入建议统一放到 Services 目录下面(业务可以用二级目录区分)                                          |
| Attributes       | 自定义特性目录         | 所有自定义的特性统一都在该目录下面                                                                            |
| Controllers      | 自定义接口控制器       | 框架内自定义的通用控制器(建议控制器统一放到该目录)                                                            |
| Extensions       | 自定义的拓展方法       | 存放框架编写的所有拓展方法                                                                                    |
| Hostings         | 自定义组件的注入方法   | 所有的组件使用必须先在该目录下面找到其注入方法并进行注入之后才能正常使用                                      |
| Middlewares      | 自定义中间件           | 存放所有的中间件(例如日志中间件)                                                                              |
| Validations      | 自定义验证方法         | 存放所有的校验功能的方法(例如：不等于的模型验证)                                                              |
| Constants        | 自定义常量             | 存放所有自定义的常量                                                                                          |
| Factorys         | 自定义工厂             | 存放所有自定义的工厂类                                                                                        |
| Interfaces       | 自定义接口             | 存放所有自定义的自定义接口                                                                                    |
| Modules          | 自定义模型             | 存放所有自定义的自定义模型                                                                                    |
| Providers        | 自定义业务实现类       | 存放所有自定义业务实现类(通常这些类都是在 IOC 注入之前使用，IOC 注入完成之后，请使用 Service 进行调用)        |
| Enums            | 自定义枚举             | 存放所有自定义枚举                                                                                            |
| Mappers          | 自定义映射类           | 存放所有自定义映射类(例如：但是你模型映射和集合映射等)                                                        |
| VerifyExtensions | 自定义数据验证拓展     | 存放所有自定义的数据验证拓展类(例如，验证省份证号码、字符串是否是整数等)                                      |
| Configs          | 自定义配置文件对象     | 存放所有自定义配置文件（例如：AppConfig 配置文件对象，则定义了它的注入，配置读取等功能）                      |
| Watchers         | 自定义监听类业务       | 存放所有自定义监听业务(例如：配置文件热重载事件监听)                                                          |
| Contexts         | 自定义上下文           | 例如：消费者 AOP 里面的方法调用过滤器上下文                                                                   |
| Params           | 自定义请求参数         | 例如：接口的请求参数或者消费者接收的数据包等等                                                                |
| Results          | 自定义返回模型         | 例如：接口的统一返回模型                                                                                      |
| Repositorys      | 自定义数据库仓储文件   | 存放所有数据库仓储文件                                                                                        |
| Interceptors     | 自定义拦截器           | 存放所有拦截器                                                                                                |
| Options          | 自定义配置选项         | 存放所有配置选项(例如：使用 OSS 的时候在注入之前必须要有 AccessKey 的字段，那么就可以用 Options 的模型来定义) |
| Migrations       | 自定义版本迁移目录     | 存放所有的版本迁移文件                                                                                        |
| Upgrations       | 自定义版本升级目录     | 存放所有的版本升级文件                                                                                        |
| Clients          | 自定义客户端实现类     | 存放所有的客户端实现类                                                                                        |
| Entitys          | 自定义实体对象         | 存放所有的数据实体对象                                                                                        |
| Config           | 配置文件存放目录       | 存放所有的配置文件(例如：nacos.json 文件)                                                                     |
| Handlers         | 定义所有的消费者处理类 | 存放所有的消费者处理类文件(类似于接口的 Controllers)                                                          |

## WEB 网站注入示例

以下的注入仅供参考，实际项目应用中需要用到那个组件，可以在对应的 Hostings 目录找到对应的注入方法

### 注入 Program 参考

```C#
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
```

### 注入 Startup 参考

```C#
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

## 控制台注入示例

如果您的项目 API 接口和消费者是分开的，那么消费端即可采用控制台，API 端则可以采用 Web 站点的方式，从而减少一些依赖。

### 注入 Program 参考

```C#
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
