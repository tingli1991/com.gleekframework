using Com.GleekFramework.KafkaSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Kafka
{
    /// <summary>
    /// Kafka SDK 类型单元测试
    /// </summary>
    public class KafkaTypeTests : BaseUnitTest
    {
        /// <summary>
        /// KafkaConsumerOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "KafkaConsumerOptions可实例化")]
        public void ConsumerOptionsCanInstantiate() =>
            Assert.NotNull(new KafkaConsumerOptions());

        /// <summary>
        /// IKafkaHandler 是接口类型
        /// </summary>
        [Fact(DisplayName = "IKafkaHandler是接口")]
        public void IKafkaHandlerIsInterface() =>
            Assert.True(typeof(IKafkaHandler).IsInterface);
    }
}
