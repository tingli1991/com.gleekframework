using Com.GleekFramework.NLogSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.NLog
{
    /// <summary>
    /// NLog 模型单元测试
    /// </summary>
    public class NLogModelTests : BaseUnitTest
    {
        /// <summary>
        /// NLogModel 属性赋值正确
        /// </summary>
        [Fact(DisplayName = "NLogModel属性赋值正确")]
        public void NLogModelPropertiesWork()
        {
            var model = new NLogModel
            {
                SerialNo = "SN001",
                Url = "/api/test",
                Content = "测试内容",
                TotalMilliseconds = 150
            };
            Assert.Equal("SN001", model.SerialNo);
            Assert.Equal("/api/test", model.Url);
            Assert.Equal("测试内容", model.Content);
            Assert.Equal(150, model.TotalMilliseconds);
        }

        /// <summary>
        /// NLogModel ContentLength 随 Content 变化
        /// </summary>
        [Fact(DisplayName = "NLogModel内容长度随内容变化")]
        public void NLogModelContentLengthMatchesContent()
        {
            var model = new NLogModel { Content = "Hello" };
            Assert.Equal(5, model.ContentLength);
            model.Content = "你好世界";
            Assert.Equal(4, model.ContentLength);
        }

        /// <summary>
        /// NLogModel ServiceTime 不为空
        /// </summary>
        [Fact(DisplayName = "NLogModelServiceTime不为空")]
        public void NLogModelServiceTimeIsNotEmpty()
        {
            var model = new NLogModel();
            Assert.False(string.IsNullOrWhiteSpace(model.ServiceTime));
        }
    }
}
