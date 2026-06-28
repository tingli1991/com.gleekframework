using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Com.GleekFramework.AutofacSdk
{
    /// <summary>
    /// 自定义服务注入工厂
    /// </summary>
    public class CustomServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        /// <summary>
        /// 容器生成器
        /// </summary>
        private readonly Action<ContainerBuilder> _configurationAction;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configurationAction"></param>
        public CustomServiceProviderFactory(Action<ContainerBuilder> configurationAction = null)
        {
            _configurationAction = configurationAction ?? delegate { };
        }

        /// <summary>
        /// 创建容器生成器
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            _configurationAction(containerBuilder);
            return containerBuilder;
        }

        /// <summary>
        /// 创建ServiceProvider容器实现类
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            AutofacProvider.Container = containerBuilder.Build();
            AutofacProvider.Services = new AutofacServiceProvider(AutofacProvider.Container);
            return AutofacProvider.Services;
        }
    }
}