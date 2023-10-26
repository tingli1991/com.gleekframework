using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// Web主题拓展类
    /// </summary>
    public static partial class HostingExtensions
    {
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