using Autofac;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.ConfigSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Com.GleekFramework.UnitTest
{
    /// <summary>
    /// 单元测试基类
    /// </summary>
    public abstract class BaseUnitTest : IDisposable
    {
        /// <summary>
        /// Autofac 容器
        /// </summary>
        private IContainer Container { get; set; }

        /// <summary>
        /// 配置对象
        /// </summary>
        protected IConfiguration Configuration { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseUnitTest()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化配置和 DI 容器
        /// </summary>
        private void Initialize()
        {
            // 1. 通过 HostBuilder 初始化配置系统（加载 Config/*.{env}.json）
            Host.CreateDefaultBuilder()
                .UseConfig()
                .Build();

            Configuration = AppConfig.Configuration;

            // 2. 构建 Autofac 容器
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(Configuration).As<IConfiguration>();
            OnRegisterServices(containerBuilder);
            Container = containerBuilder.Build();

            // 3. 设置全局容器（供框架内部 AutofacProvider.GetService 使用）
            AutofacProvider.Container = Container;
            AutofacProvider.Services = new ContainerServiceProvider(Container);
        }

        /// <summary>
        /// 注册服务（子类可重写添加测试所需的服务注册）
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void OnRegisterServices(ContainerBuilder builder)
        {
        }

        /// <summary>
        /// 从 DI 容器获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetService<T>()
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose()
        {
            Container?.Dispose();
        }

        /// <summary>
        /// 将 IContainer 适配为 IServiceProvider
        /// </summary>
        private class ContainerServiceProvider : IServiceProvider
        {
            private readonly IContainer _container;

            public ContainerServiceProvider(IContainer container)
            {
                _container = container;
            }

            public object GetService(Type serviceType)
            {
                return _container.ResolveOptional(serviceType);
            }
        }
    }
}
