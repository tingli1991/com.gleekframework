using Com.GleekFramework.RocketMQSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.RocketMQ
{
    /// <summary>
    /// RocketMQ SDK 类型单元测试
    /// </summary>
    public class RocketTypeTests : BaseUnitTest
    {
        /// <summary>
        /// RocketAccessOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "RocketAccessOptions可实例化")]
        public void AccessOptionsCanInstantiate() =>
            Assert.NotNull(new RocketAccessOptions());

        /// <summary>
        /// RocketConsumerOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "RocketConsumerOptions可实例化")]
        public void ConsumerOptionsCanInstantiate() =>
            Assert.NotNull(new RocketConsumerOptions());

        /// <summary>
        /// RocketConsumerHostOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "RocketConsumerHostOptions可实例化")]
        public void ConsumerHostOptionsCanInstantiate() =>
            Assert.NotNull(new RocketConsumerHostOptions());

        /// <summary>
        /// IRocketHandler 是接口类型
        /// </summary>
        [Fact(DisplayName = "IRocketHandler是接口")]
        public void IRocketHandlerIsInterface() =>
            Assert.True(typeof(IRocketHandler).IsInterface);
    }
}
