using Com.GleekFramework.AttributeSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Attribute
{
    /// <summary>
    /// 自定义验证特性单元测试
    /// </summary>
    public class CustomRequiredAttributeTests : BaseUnitTest
    {
        /// <summary>
        /// QueryRequiredAttribute 构造设置错误消息
        /// </summary>
        [Fact(DisplayName = "QueryRequiredAttribute设置错误消息")]
        public void QueryRequiredAttributeSetsErrorMessage()
        {
            var attr = new QueryRequiredAttribute("参数错误");
            Assert.Equal("参数错误", attr.ErrorMessage);
        }

        /// <summary>
        /// QueryRequiredAttribute 设置名称和错误消息
        /// </summary>
        [Fact(DisplayName = "QueryRequiredAttribute设置名称和错误消息")]
        public void QueryRequiredAttributeWithNameSetsProperties()
        {
            var attr = new QueryRequiredAttribute("page_size", "分页大小不能为空");
            Assert.Equal("page_size", attr.Name);
            Assert.Equal("分页大小不能为空", attr.ErrorMessage);
        }
    }
}
