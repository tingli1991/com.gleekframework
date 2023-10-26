using Autofac;
using Com.GleekFramework.CommonSdk;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Com.GleekFramework.AutofacSdk
{
    /// <summary>
    /// 主机拓展
    /// </summary>
    public static partial class HostingExtensions
    {
        /// <summary>
        /// 使用默认的IOC注入方式
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseAutofac(this IHostBuilder builder)
        {
            return builder.UseAutofac<DefaultModule>();
        }

        /// <summary>
        /// 使用Autofac
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHostBuilder UseAutofac<T>(this IHostBuilder builder) where T : Autofac.Module, new()
        {
            builder.UseServiceProviderFactory(new CustomServiceProviderFactory());
            builder.ConfigureContainer<ContainerBuilder>(context => context.UseAutofac<T>());
            builder.ConfigureServices((context, services) => services.AddControllerActivator());
            return builder;
        }

        /// <summary>
        /// 使用属性注入
        /// </summary>
        /// <param name="builder"></param>
        private static void UseAutofac<T>(this ContainerBuilder builder) where T : Autofac.Module, new()
        {
            builder.RegisterModule<T>();
        }

        /// <summary>
        /// 注入控制器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddControllerActivator(this IServiceCollection services)
        {
            //添加http上下文
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //检查是否包含值类型
            var checkIsAssignableFrom = Assembly.GetEntryAssembly().CheckIsAssignableFrom(AutofacConstant.CONTROLLERBASE_TYPE);

            //如果包含值类型则进行控制器的依赖注入
            if (checkIsAssignableFrom)
            {
                //注入IControllerActivator的实现
                services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            }
            return services;
        }
    }
}