using Com.GleekFramework.SwaggerSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Swagger
{
    /// <summary>
    /// Swagger 常量单元测试
    /// </summary>
    public class SwaggerConstantTests : BaseUnitTest
    {
        /// <summary>
        /// Swagger 作者常量不为空
        /// </summary>
        [Fact(DisplayName = "Swagger作者常量不为空")]
        public void AuthorConstantIsNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(SwaggerConstant.AUTHOR));
        }

        /// <summary>
        /// Swagger 邮件地址常量不为空
        /// </summary>
        [Fact(DisplayName = "Swagger邮件地址常量不为空")]
        public void EmailConstantIsNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(SwaggerConstant.EMAIL));
        }

        /// <summary>
        /// Swagger 分组名称常量不为空
        /// </summary>
        [Fact(DisplayName = "Swagger分组名称常量不为空")]
        public void GroupNameConstantIsNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(SwaggerConstant.SWAGGERGROUPNAME));
        }

        /// <summary>
        /// Swagger 主页地址常量不为空
        /// </summary>
        [Fact(DisplayName = "Swagger主页地址常量不为空")]
        public void HomeConstantIsNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(SwaggerConstant.HOME));
        }

        /// <summary>
        /// Swagger 终结点地址常量不为空
        /// </summary>
        [Fact(DisplayName = "Swagger终结点地址常量不为空")]
        public void EndpointUrlConstantIsNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(SwaggerConstant.SWAGGERENDPOINTURL));
        }
    }
}
