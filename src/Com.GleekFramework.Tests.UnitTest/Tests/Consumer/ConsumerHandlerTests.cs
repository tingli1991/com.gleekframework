using Com.GleekFramework.ConsumerSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Consumer
{
    /// <summary>
    /// 消费者基础接口单元测试
    /// </summary>
    public class ConsumerHandlerTests : BaseUnitTest
    {
        /// <summary>
        /// IHandler 是接口类型
        /// </summary>
        [Fact(DisplayName = "IHandler是接口类型")]
        public void IHandlerIsInterface()
        {
            Assert.True(typeof(IHandler).IsInterface);
        }

        /// <summary>
        /// ITopicHandler 是接口类型
        /// </summary>
        [Fact(DisplayName = "ITopicHandler是接口类型")]
        public void ITopicHandlerIsInterface()
        {
            Assert.True(typeof(ITopicHandler).IsInterface);
        }

        /// <summary>
        /// ITopicHandler 继承 IHandler
        /// </summary>
        [Fact(DisplayName = "ITopicHandler继承IHandler")]
        public void ITopicHandlerExtendsIHandler()
        {
            Assert.True(typeof(ITopicHandler).IsAssignableTo(typeof(IHandler)));
        }

        /// <summary>
        /// TopicServiceModel 属性初始化默认
        /// </summary>
        [Fact(DisplayName = "TopicServiceModel属性初始化")]
        public void TopicServiceModelDefaultValues()
        {
            var model = new TopicServiceModel<object>();
            Assert.Null(model.Topic);
            Assert.Null(model.ServiceList);
        }

        /// <summary>
        /// TopicServiceModel 设置属性后读取正确
        /// </summary>
        [Fact(DisplayName = "TopicServiceModel设置属性")]
        public void TopicServiceModelSetProperties()
        {
            var serviceList = new List<object> { new object() };
            var model = new TopicServiceModel<object>
            {
                Topic = "test_topic",
                ServiceList = serviceList
            };
            Assert.Equal("test_topic", model.Topic);
            Assert.Same(serviceList, model.ServiceList);
        }

        /// <summary>
        /// CustomActionExecutedContext 构造函数
        /// </summary>
        [Fact(DisplayName = "CustomActionExecutedContext构造函数")]
        public void CustomActionExecutedContextConstructor()
        {
            var context = new CustomActionExecutedContext();
            Assert.NotNull(context);
        }

        /// <summary>
        /// CustomActionExecutingContext 构造函数
        /// </summary>
        [Fact(DisplayName = "CustomActionExecutingContext构造函数")]
        public void CustomActionExecutingContextConstructor()
        {
            var context = new CustomActionExecutingContext();
            Assert.NotNull(context);
        }

        /// <summary>
        /// CustomAuthorizationContext 构造函数
        /// </summary>
        [Fact(DisplayName = "CustomAuthorizationContext构造函数")]
        public void CustomAuthorizationContextConstructor()
        {
            var context = new CustomAuthorizationContext();
            Assert.NotNull(context);
        }
    }
}
