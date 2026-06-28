using Com.GleekFramework.RabbitMQSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.RabbitMQ
{
    /// <summary>
    /// RabbitMQ SDK 类型单元测试
    /// </summary>
    public class RabbitTypeTests : BaseUnitTest
    {
        /// <summary>
        /// RabbitConnectionOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "RabbitConnectionOptions可实例化")]
        public void ConnectionOptionsCanInstantiate() =>
            Assert.NotNull(new RabbitConnectionOptions());

        /// <summary>
        /// RabbitConsumerOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "RabbitConsumerOptions可实例化")]
        public void ConsumerOptionsCanInstantiate() =>
            Assert.NotNull(new RabbitConsumerOptions());

        /// <summary>
        /// RabbitHostOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "RabbitHostOptions可实例化")]
        public void HostOptionsCanInstantiate() =>
            Assert.NotNull(new RabbitHostOptions());

        /// <summary>
        /// RabbitHandler 是抽象类
        /// </summary>
        [Fact(DisplayName = "RabbitHandler是抽象类")]
        public void RabbitHandlerIsAbstract() =>
            Assert.True(typeof(RabbitHandler).IsAbstract);

        /// <summary>
        /// RabbitSubscribeHandler 继承自 RabbitHandler
        /// </summary>
        [Fact(DisplayName = "RabbitSubscribeHandler继承RabbitHandler")]
        public void SubscribeHandlerInheritsRabbitHandler() =>
            Assert.True(typeof(RabbitSubscribeHandler).IsAssignableTo(typeof(RabbitHandler)));
    }
}
