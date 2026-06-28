using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.UnitTest;
using Xunit;

namespace Com.GleekFramework.Tests.UnitTest.Tests.Common
{
    /// <summary>
    /// Base64 和 MD5 扩展方法单元测试
    /// </summary>
    public class Base64Md5ExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// ToBase64 编码正确
        /// </summary>
        [Fact(DisplayName = "ToBase64编码正确")]
        public void ToBase64EncodesCorrectly()
        {
            Assert.Equal("SGVsbG8=", "Hello".ToBase64());
        }

        /// <summary>
        /// Base64ToString 解码正确
        /// </summary>
        [Fact(DisplayName = "Base64ToString解码正确")]
        public void Base64ToStringDecodesCorrectly()
        {
            Assert.Equal("Hello", "SGVsbG8=".Base64ToString());
        }

        /// <summary>
        /// Base64 编解码往返一致
        /// </summary>
        [Fact(DisplayName = "Base64编解码往返一致")]
        public void Base64RoundTrip()
        {
            var original = "测试中文Base64编码";
            Assert.Equal(original, original.ToBase64().Base64ToString());
        }

        /// <summary>
        /// IsBase64 对有效 Base64 返回 true
        /// </summary>
        [Fact(DisplayName = "IsBase64有效返回True")]
        public void IsBase64ValidReturnsTrue()
        {
            Assert.True("Hello".ToBase64().IsBase64());
        }

        /// <summary>
        /// EncryptMd5 加密结果正确
        /// </summary>
        [Fact(DisplayName = "EncryptMd5加密正确")]
        public void EncryptMd5EncodesCorrectly()
        {
            Assert.Equal("8b1a9953c4611296a827abf8c47804d7", "Hello".EncryptMd5());
        }

        /// <summary>
        /// EncryptMd5 不同输入产生不同哈希值
        /// </summary>
        [Fact(DisplayName = "EncryptMd5不同输入不同哈希")]
        public void EncryptMd5DifferentInputsDifferentHashes()
        {
            Assert.NotEqual("Hello".EncryptMd5(), "World".EncryptMd5());
        }
    }
}
