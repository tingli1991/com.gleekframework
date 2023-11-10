using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
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
using System.Text;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// Web主题拓展类
    /// </summary>
    public static partial class HostingExtensions
    {
        /// <summary>
        /// 使用默认的消费者主机
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
                    app.RegisterApplicationStarted(() => Console.Out.WriteLine($"服务启动成功：{EnvironmentProvider.GetHost()}"));
                });
            });
            return builder;
        }

        /// <summary>
        /// 使用默认的Web主机
        /// </summary>
        /// <typeparam name="Startup"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseGleekWebHostDefaults<Startup>(this IHostBuilder builder)
            where Startup : class
        {
            builder.UseContentRoot(AppContext.BaseDirectory);
            builder.ConfigureLogging((log) => log.SetMinimumLevel(LogLevel.Warning));
            builder.ConfigureWebHostDefaults(host =>
            {
                host.UseStartup<Startup>();
                host.UseUrls(EnvironmentProvider.GetHost());
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