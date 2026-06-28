# Quick Start

Before starting, it's recommended that everyone first installs the [NuGet](https://www.nuget.org/) server-side service. We recommend [Nexus](https://www.sonatype.com/) for this, and there are plenty of tutorials online, so I won’t go into too much detail here.

> Reasons for not uploading to [NuGet](https://www.nuget.org/) official

- On one hand, it is more recommended to build your own [NuGet](https://www.nuget.org/) server-side service, which is more secure when privatized.
- On the other hand, it is hoped that the project can be iterated and receive feedback continuously.
- Together we build the .NET community, so that .NET development is no longer anxious about finding jobs.

> Why recommend [Nexus](https://www.sonatype.com/)

- [Nexus](https://www.sonatype.com/) is currently a widely used binary repository management tool with powerful features and flexible configuration options.
- [Nexus](https://www.sonatype.com/) supports proxying remote repositories and hosting local repositories, capable of managing packages in various formats such as Maven, npm, NuGet, and more.
- Compared to traditional nuget servers, [Nexus](https://www.sonatype.com/) offers more comprehensive repository management features. For instance, [Nexus](https://www.sonatype.com/) supports group repositories, allowing users to access and search multiple repositories through a single gateway.

## Packaging Instructions

Below is a batch command (`bat`) I wrote. In each component's directory, there's a `nuget-push.bat` file, which everyone can adjust according to their own requirements (before using, you need to adjust `source_api_uri` and `api_key`).

```bash
@echo off
if %time:~0,2% LEQ 9 (set now=%date:~0,4%%date:~5,2%%date:~8,2%0%time:~1,1%%time:~3,2%%time:~6,2%) else (set now=%date:~0,4%%date:~5,2%%date:~8,2%%time:~0,2%%time:~3,2%%time:~6,2%)

:: Specify the name of the project to upload
set project_name=Com.GleekFramework.AttributeSdk

:: Specify the api key for uploading
set api_key=278466c7-23cc-3ec8-86d8-43adde285742

:: Specify the url for uploading
set source_api_uri=http://192.168.100.15:8081/repository/nuget-hosted/index.json

:: Get the current folder
set current_dir=%~dp0%

:: Project path (solution path)
set solution_dir=%current_dir%..\

:: Set the full name of the current project file (including the path)
set csproj_path=%solution_dir%%project_name%\%project_name%.csproj

:: Specify the package directory
set packg_dir=%solution_dir%nupkgs\%project_name%\%now%

:: Build the project and output the pack package
echo start build and pack %project_name% ...
dotnet pack %csproj_path%  -c Release -o %packg_dir% -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg

:: Batch push the packages
echo start push %packg_name% ...
dotnet nuget push  %packg_dir%\*.nupkg  -k %api_key% -s %source_api_uri%
echo push %packg_name% finish ...
pause
```

## Core Components Explanation

The so-called core components refer to the foundational elements that all our components depend upon. These components serve as the basis for the implementation of all our components. For example: IOC.

> Com.GleekFramework.AutofacSdk

- All IOC within the project is based on it, and you can activate it using the UseAutofac() method.

> Com.GleekFramework.ConfigSdk

- All configuration file reading within the project is uniformly done through it. Use the UseConfig() method to activate it, and if you want to activate feature configurations, you can use the UseConfigAttribute() method.

## Component List

| Component Name                  | Description               | Purpose                                                                                                                                                           |
| :------------------------------ | :------------------------ | :---------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Com.GleekFramework.AssemblySdk  | Assembly SDK              | Manage and scan assemblies                                                                                                                                        |
| Com.GleekFramework.AttributeSdk | Attribute SDK             | Includes global exception capture for WebAPI, interface request parameter validation, interface aspect logging, and base controller definition, etc.              |
| Com.GleekFramework.AutofacSdk   | IOC (Autofac) SDK         | Use Autofac to rewrite the native NET IOC container (all DI uses property injection)                                                                              |
| Com.GleekFramework.CommonSdk    | Basic SDK                 | Define common attributes, constants, enums, extension methods, data conversion methods, and verification methods, etc.                                            |
| Com.GleekFramework.ConfigSdk    | Configuration SDK         | Manage and define the conventions for configuration definition and reading methods                                                                                |
| Com.GleekFramework.ConsumerSdk  | Consumer SDK              | Manage and define standards for consumer implementation as well as a custom set of aspects for consumer development                                               |
| Com.GleekFramework.ContractSdk  | Contract SDK              | Define and constrain the contracts exposed by the project (e.g., interface return models, project base error codes, and serial number generation standards, etc.) |
| Com.GleekFramework.DapperSdk    | ORM (Dapper) SDK          | ORM components integrated from Dapper and DapperExtensions                                                                                                        |
| Com.GleekFramework.HttpSdk      | HTTP SDK                  | Wrapping of HTTP requests with support for retries, circuit breaking, etc.                                                                                        |
| Com.GleekFramework.KafkaSdk     | Kafka SDK                 | Constrain coding standards for production and consumption, graceful shutdown, and AOP capabilities                                                                |
| Com.GleekFramework.MigrationSdk | Migration SDK             | Primarily used for in-process automatic upgrade of database architecture, data migration, and script execution                                                    |
| Com.GleekFramework.MongodbSdk   | Mongodb SDK               | Mongodb database operation toolkit                                                                                                                                |
| Com.GleekFramework.NacosSdk     | Nacos SDK                 | Configuration center, registry center, service registration and discovery (currently only implemented HTTP communication)                                         |
| Com.GleekFramework.NLogSdk      | NLog SDK                  | Logging framework implemented based on NLog                                                                                                                       |
| Com.GleekFramework.ObjectSdk    | Object Storage SDK        | Upload and download for object storage on Alibaba Cloud and Tencent Cloud (currently only Alibaba Cloud OSS is implemented)                                       |
| Com.GleekFramework.QueueSdk     | Local Queue SDK           | Functional encapsulation of First-In-First-Out queues and Last-In-First-Out stacks                                                                                |
| Com.GleekFramework.RabbitMQSdk  | RabbitMQ SDK              | Work mode, publish-subscribe mode, RPC mode, and delayed queue (delay still under development)                                                                    |
| Com.GleekFramework.RedisSdk     | Redis SDK                 | Basic type encapsulation and powerful distributed lock extensions                                                                                                 |
| Com.GleekFramework.RocketMQSdk  | RocketMQ SDK              | Currently only ordinary messages and delayed messages production and consumption are implemented (basically sufficient)                                           |
| Com.GleekFramework.SwaggerSdk   | Swagger Documentation SDK | Use Knife4jUI to re-render the old SWAGGER documentation theme                                                                                                    |

## Directory Structure Description

| Directory Name   | Description                             | Description                                                                                                                                |
| ---------------- | --------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------ |
| Services         | Services based on DI                    | All dependency injection should be unified under the Services directory (business may use subdirectories)                                  |
| Attributes       | Custom Attributes                       | All custom attributes are placed in this directory                                                                                         |
| Controllers      | Custom Interface Controllers            | Controllers that are custom for the framework (suggested to unify controllers under this directory)                                        |
| Extensions       | Custom Extension Methods                | Contains all extension methods written for the framework                                                                                   |
| Hostings         | Injection Methods for Custom Components | All components must be injected under this directory before they can be used properly                                                      |
| Middlewares      | Custom Middleware                       | Contains all middleware (e.g., logging middleware)                                                                                         |
| Validations      | Custom Validation Methods               | Contains all methods for validation features (e.g., model validation for inequality)                                                       |
| Constants        | Custom Constants                        | Contains all custom constants                                                                                                              |
| Factorys         | Custom Factories                        | Contains all custom factory classes                                                                                                        |
| Interfaces       | Custom Interfaces                       | Contains all custom interfaces                                                                                                             |
| Modules          | Custom Models                           | Contains all custom models                                                                                                                 |
| Providers        | Custom Business Implementation Classes  | Contains all custom business implementation classes (usually used before IOC injection, use Service after IOC)                             |
| Enums            | Custom Enums                            | Contains all custom enums                                                                                                                  |
| Mappers          | Custom Mapping Classes                  | Contains all custom mapping classes (e.g., model mappings and collection mappings)                                                         |
| VerifyExtensions | Custom Data Validation Extensions       | Contains all custom data validation extension classes (e.g., ID card number verification, checking if a string is an integer)              |
| Configs          | Custom Configuration File Objects       | Contains all custom configuration files (e.g., AppConfig, which defines its injection, configuration reading, etc.)                        |
| Watchers         | Custom Listener Services                | Contains all custom listening services (e.g., configuration file hot reload event listeners)                                               |
| Contexts         | Custom Contexts                         | For example, AOP contexts for methods called inside a consumer filter                                                                      |
| Params           | Custom Request Parameters               | For example, interface request parameters or data packets received by consumers, etc.                                                      |
| Results          | Custom Return Models                    | For example, the unified return model of an interface                                                                                      |
| Repositorys      | Custom Database Repositories            | Contains all database repository files                                                                                                     |
| Interceptors     | Custom Interceptors                     | Contains all interceptors                                                                                                                  |
| Options          | Custom Configuration Options            | Contains all configuration options (e.g., before injecting OSS, AccessKey fields are required, so they can be defined in an Options model) |
| Migrations       | Custom Migration Directory              | Contains all migration files                                                                                                               |
| Upgrations       | Custom Upgrade Directory                | Contains all upgrade files                                                                                                                 |
| Clients          | Custom Client Implementation Classes    | Contains all client implementation classes                                                                                                 |
| Entitys          | Custom Entity Objects                   | Contains all data entity objects                                                                                                           |
| Config           | Configuration Files Directory           | Contains all configuration files (e.g., nacos.json files)                                                                                  |
| Handlers         | Defines All Consumer Handlers           | Contains all consumer handler files (similar to interface Controllers)                                                                     |

## WEB Site Injection Example

The following injections are for reference only, the actual project application needs to use the corresponding component, you can find the corresponding injection method in the corresponding Hostings directory.

### Program Injection Reference

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
    /// Program Class
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main Function of the Program
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

### Startup Injection Reference

```C#
/// <summary>
/// Program's startup class.
/// </summary>
public class Startup
{
    /// <summary>
    /// Services registration.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks(); // Add health checks.
        services.AddKnife4Gen("Test Documentation"); // Add Knife4 generator.

        services.AddNewtonsoftJson(); // Add default formatting for JSON.
        services.AddDistributedMemoryCache(); // Add distributed memory cache.
        services.AddGlobalExceptionAttribute(); // Add global exception handling.
        services.AddModelValidAttribute<MessageCode>(); // Add model validation.
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(); // Add Cookie support.
    }

    /// <summary>
    /// Service configuration.
    /// </summary>
    /// <param name="app"></param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseKnife4UI(); // Use Knife4UI interface.
        app.UseStaticFiles(); // Use static resources.
        app.UseRouting(); // Use routing rules.
        app.UseHealthChecks(); // Use health checks.
        app.UseAuthentication(); // Enable authentication.
        app.UseEndpoints(endpoints => endpoints.MapControllers()); // Enable endpoints configuration.
        app.RegisterApplicationStarted(() => Console.Out.WriteLine($"Service successfully started: {EnvironmentProvider.GetHost()}"));
    }
}
```

## Console Injection Example

If your project's API end and its consumers are separated, the consumer side can use a console application, and the API side can use a web-based approach to reduce dependencies.

### Injecting into Program Reference

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
    /// Main program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Program's main function.
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
        /// Create the system's main host.
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
