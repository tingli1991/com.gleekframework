using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.SwaggerSdk;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// Web主题拓展类
    /// </summary>
    public static partial class HostingExtensions
    {
        /// <summary>
        /// 使用默认的消费者Web主机
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseGleekConsumerHostDefaults(this IHostBuilder builder)
        {
            builder.ConfigureWebHostDefaults(host =>
            {
                host.UseUrls(EnvironmentProvider.GetHost());
                host.ConfigureServices((services) => services.AddHealthChecks());
                builder.ConfigureLogging((log) => log.SetMinimumLevel(LogLevel.Warning));
                host.Configure((app) =>
                {
                    app.UseRouting();//使用路由规则
                    app.UseHealthChecks();//使用心跳检测
                    var lifeTime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
                    lifeTime.ApplicationStarted.Register(() => Console.Out.WriteLine($"服务启动成功：{EnvironmentProvider.GetHost()}"));
                });
            });
            return builder;
        }

        /// <summary>
        /// 使用默认的Web主机
        /// </summary>
        /// <typeparam name="E">错误消息模型</typeparam>
        /// <param name="builder"></param>
        /// <param name="docTitle">文档的标题</param>
        /// <returns></returns>
        public static IHostBuilder UseGleekWebHostDefaults<E>(this IHostBuilder builder, string docTitle = "测试文档") where E : Enum
        {
            builder.ConfigureWebHostDefaults(host =>
            {
                host.UseUrls(EnvironmentProvider.GetHost());
                builder.ConfigureLogging((log) => log.SetMinimumLevel(LogLevel.Warning));
                host.Configure((app) =>
                {
                    var swaggerSwitch = EnvironmentProvider.GetSwaggerSwitch();//Swagger开关配置
                    if (swaggerSwitch == "true")
                    {
                        app.UseKnife4UI();//使用Knife4UI界面
                        app.UseDeveloperExceptionPage();//使用开发人员异常页面
                    }

                    app.UseStaticFiles();//使用静态资源
                    app.UseRouting();//使用路由规则
                    app.UseEndpoints();
                    app.UseHealthChecks();//使用心跳检测
                    app.UseAuthentication();//启用授权
                    var lifeTime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
                    lifeTime.ApplicationStarted.Register(() => Console.Out.WriteLine($"服务启动成功：{EnvironmentProvider.GetHost()}"));
                });
                host.ConfigureServices((services) =>
                {
                    services.AddHealthChecks();//添加心跳
                    services.AddKnife4Gen(docTitle);//添加Knife4生成器
                    services.AddNewtonsoftJson();//添加对JSON的默认格式化
                    services.AddDistributedMemoryCache();//添加分布式内存缓存
                    services.AddGlobalExceptionAttribute();//添加全局异常
                    services.AddModelValidAttribute<E>();//添加模型验证
                    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();//添加Cookie支持
                });
            });
            return builder;
        }

        /// <summary>
        /// 使用心跳检测
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="text">输出的内容(默认为：ok)</param>
        /// <returns></returns>
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder builder, string text = "ok")
        {
            builder.UseEndpoints(endpoints => endpoints.MapHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = (context, result) => context.Response.WriteAsync(text, Encoding.UTF8)
            }));
            return builder;
        }

        /// <summary>
        /// 使用终结点配置
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseEndpoints(this IApplicationBuilder builder)
        {
            builder.UseEndpoints(endpoints => endpoints.MapControllers());
            return builder;
        }

        /// <summary>
        /// 添加全局异常
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddGlobalExceptionAttribute(this IServiceCollection services)
        {
            //添加全局异常处理类
            services.AddControllers(options => options.Filters.Add<GlobalExceptionAttribute>());
            return services;
        }

        /// <summary>
        /// 添加模型验证
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddModelValidAttribute<T>(this IServiceCollection services) where T : Enum
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //禁用默认验证行为
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers(options =>
            {
                //添加全局模型验证
                options.Filters.Add<ModelValidAttribute, T>();
            });
            return services;
        }

        /// <summary>
        /// 添加默认的JSON配置参数
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddNewtonsoftJson(this IServiceCollection services)
        {
            services.AddControllers(options => { }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss:ffffff";//设置时间格式
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();//不添加驼峰
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;//使用本地时区
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;//忽略循环引用
            });
            return services;
        }
    }
}