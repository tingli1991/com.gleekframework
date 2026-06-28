using Com.GleekFramework.HttpSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Http
{
    /// <summary>
    /// HTTP SDK 扩展方法单元测试
    /// </summary>
    public class HttpExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// ToStringContent 序列化对象正确
        /// </summary>
        [Fact(DisplayName = "ToStringContent序列化对象正确")]
        public async Task ToStringContentSerializesObject()
        {
            var data = new { Name = "Test", Value = 123 };
            var content = data.ToStringContent();
            var json = await content.ReadAsStringAsync();
            Assert.Contains("Test", json);
            Assert.Contains("123", json);
        }

        /// <summary>
        /// ToStringContent 字符串直接使用
        /// </summary>
        [Fact(DisplayName = "ToStringContent字符串直接使用")]
        public async Task ToStringContentStringValue()
        {
            var result = await "rawString".ToStringContent().ReadAsStringAsync();
            Assert.Equal("rawString", result);
        }

        /// <summary>
        /// ToGetUrl 拼接查询参数正确
        /// </summary>
        [Fact(DisplayName = "ToGetUrl拼接查询参数正确")]
        public void ToGetUrlAppendsQueryParams()
        {
            var param = new Dictionary<string, string> { { "name", "张三" }, { "age", "25" } };
            Assert.Equal("http://example.com/api?name=张三&age=25", "http://example.com/api".ToGetUrl(param));
        }

        /// <summary>
        /// ToGetParamters 字典转查询字符串
        /// </summary>
        [Fact(DisplayName = "ToGetParamters字典转查询字符串")]
        public void ToGetParamtersConvertsDict()
        {
            var param = new Dictionary<string, string> { { "a", "1" }, { "b", "2" } };
            Assert.Equal("a=1&b=2", param.ToGetParamters());
        }
    }
}
