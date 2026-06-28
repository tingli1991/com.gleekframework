using Com.GleekFramework.NacosSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Nacos
{
    /// <summary>
    /// Nacos SDK 类型单元测试
    /// </summary>
    public class NacosTypeTests : BaseUnitTest
    {
        /// <summary>
        /// NacosConfigAttribute 可实例化
        /// </summary>
        [Fact(DisplayName = "NacosConfigAttribute可实例化")]
        public void ConfigAttributeCanInstantiate() =>
            Assert.NotNull(new NacosConfigAttribute("test"));

        /// <summary>
        /// NacosSettings 可实例化
        /// </summary>
        [Fact(DisplayName = "NacosSettings可实例化")]
        public void NacosSettingsCanInstantiate() =>
            Assert.NotNull(new NacosSettings());

        /// <summary>
        /// ConfigSettings 可实例化
        /// </summary>
        [Fact(DisplayName = "ConfigSettings可实例化")]
        public void ConfigSettingsCanInstantiate() =>
            Assert.NotNull(new ConfigSettings());

        /// <summary>
        /// ServiceSettings 可实例化
        /// </summary>
        [Fact(DisplayName = "ServiceSettings可实例化")]
        public void ServiceSettingsCanInstantiate() =>
            Assert.NotNull(new ServiceSettings());

        /// <summary>
        /// ServiceClientOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "ServiceClientOptions可实例化")]
        public void ServiceClientOptionsCanInstantiate() =>
            Assert.NotNull(new ServiceClientOptions());

        /// <summary>
        /// ListInstancesHostResponse 可实例化
        /// </summary>
        [Fact(DisplayName = "ListInstancesHostResponse可实例化")]
        public void ListInstancesHostResponseCanInstantiate() =>
            Assert.NotNull(new ListInstancesHostResponse());
    }
}
