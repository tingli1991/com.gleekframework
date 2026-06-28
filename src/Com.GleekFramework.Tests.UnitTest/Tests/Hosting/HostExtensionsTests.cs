using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.SwaggerSdk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Hosting
{
    /// <summary>
    /// 主机扩展方法单元测试
    /// </summary>
    public class HostExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// UseSemaphoreSwitch 设置信号量后 ExecuteAsync 正常执行
        /// </summary>
        [Fact(DisplayName = "UseSemaphoreSwitch后ExecuteAsync正常")]
        public async Task UseSemaphoreSwitchThenExecuteAsync()
        {
            var builder = Host.CreateDefaultBuilder()
                .UseSemaphoreSwitch(4, 8);
            using var host = builder.Build();

            var result = await SemaphoreProvider.ExecuteAsync(async (count) =>
            {
                await Task.CompletedTask;
                return 42;
            });
            Assert.Equal(42, result);
        }

        /// <summary>
        /// UseSemaphoreSwitch 使用默认值执行
        /// </summary>
        [Fact(DisplayName = "UseSemaphoreSwitch默认值执行")]
        public async Task UseSemaphoreSwitchWithDefaults()
        {
            var builder = Host.CreateDefaultBuilder()
                .UseSemaphoreSwitch(0, 0);
            using var host = builder.Build();

            var executed = false;
            await SemaphoreProvider.ExecuteAsync(async (count) =>
            {
                executed = true;
                await Task.CompletedTask;
            });
            Assert.True(executed);
        }

        /// <summary>
        /// UseConfig 扩展方法不抛异常
        /// </summary>
        [Fact(DisplayName = "UseConfig扩展方法不抛异常")]
        public void UseConfigDoesNotThrow()
        {
            var builder = Host.CreateDefaultBuilder()
                .UseConfig();
            Assert.NotNull(builder);
        }

        /// <summary>
        /// UseConfig 构建后 AppConfig 不为空
        /// </summary>
        [Fact(DisplayName = "UseConfig构建后AppConfig不为空")]
        public void UseConfigSetsAppConfig()
        {
            var builder = Host.CreateDefaultBuilder()
                .UseConfig();
            using var host = builder.Build();
            Assert.NotNull(AppConfig.Configuration);
        }

        /// <summary>
        /// UseHttpClient 扩展方法不抛异常
        /// </summary>
        [Fact(DisplayName = "UseHttpClient扩展方法不抛异常")]
        public void UseHttpClientDoesNotThrow()
        {
            var builder = Host.CreateDefaultBuilder()
                .UseHttpClient();
            Assert.NotNull(builder);
        }

        /// <summary>
        /// AddKnife4Gen 注册 Swagger 服务不抛异常
        /// </summary>
        [Fact(DisplayName = "AddKnife4Gen注册Swagger服务不抛异常")]
        public void AddKnife4GenDoesNotThrow()
        {
            var services = new ServiceCollection();
            services.AddKnife4Gen("测试文档");
            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider);
        }

        /// <summary>
        /// RegisterApplicationStarted 注册回调后正常构建
        /// </summary>
        [Fact(DisplayName = "RegisterApplicationStarted注册回调正常")]
        public void RegisterApplicationStartedWorks()
        {
            var callbackExecuted = false;
            var builder = Host.CreateDefaultBuilder();

            using var host = builder.Build();
            var lifetime = host.RegisterApplicationStarted(() => { callbackExecuted = true; });
            Assert.NotNull(lifetime);
        }
    }
}
