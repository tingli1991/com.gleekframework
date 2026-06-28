using Com.GleekFramework.SecuritySdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Security
{
    /// <summary>
    /// 安全SDK单元测试
    /// </summary>
    public class SecurityTests : BaseUnitTest
    {
        /// <summary>
        /// Md5Service 不加盐加密正确
        /// </summary>
        [Fact(DisplayName = "Md5Service不加盐加密正确")]
        public void Md5ServiceComputeMd5ReturnsCorrectHash()
        {
            Assert.Equal("8b1a9953c4611296a827abf8c47804d7", Md5Service.ComputeMD5("Hello"));
        }

        /// <summary>
        /// Md5Service 加盐加密结果不同
        /// </summary>
        [Fact(DisplayName = "Md5Service加盐加密结果不同")]
        public void Md5ServiceComputeMd5WithSaltDifferentFromUnsalted()
        {
            Assert.NotEqual(Md5Service.ComputeMD5("Hello"), Md5Service.ComputeMD5("Hello", "Salt123"));
        }

        /// <summary>
        /// Md5ComputeHash 计算哈希正确
        /// </summary>
        [Fact(DisplayName = "Md5ComputeHash计算哈希正确")]
        public void Md5ComputeHashComputeHashStringReturnsCorrectHash()
        {
            Assert.Equal("8b1a9953c4611296a827abf8c47804d7", Md5ComputeHash.ComputeHashString("Hello"));
        }

        /// <summary>
        /// Md5ComputeHash 验证哈希匹配
        /// </summary>
        [Fact(DisplayName = "Md5ComputeHash验证哈希匹配")]
        public void Md5ComputeHashVerifyReturnsTrue()
        {
            Assert.True(Md5ComputeHash.Verify("Hello", "8b1a9953c4611296a827abf8c47804d7"));
        }

        /// <summary>
        /// Md5ComputeHash 验证哈希不区分大小写
        /// </summary>
        [Fact(DisplayName = "Md5ComputeHash验证不区分大小写")]
        public void Md5ComputeHashVerifyCaseInsensitiveReturnsTrue()
        {
            Assert.True(Md5ComputeHash.Verify("Hello", "8B1A9953C4611296A827ABF8C47804D7"));
        }

        /// <summary>
        /// Md5ComputeHash null 输入抛异常
        /// </summary>
        [Fact(DisplayName = "Md5ComputeHashNull输入抛异常")]
        public void Md5ComputeHashNullInputThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => Md5ComputeHash.ComputeHash(null));
        }

        /// <summary>
        /// RandomStringGenerator 生成指定长度
        /// </summary>
        [Fact(DisplayName = "RandomStringGenerator生成指定长度")]
        public void RandomStringGeneratorGenerateCorrectLength()
        {
            Assert.Equal(16, RandomStringGenerator.GenAppId(16).Length);
            Assert.Equal(32, RandomStringGenerator.GenAppSecret().Length);
        }

        /// <summary>
        /// RandomStringGenerator 生成结果只包含指定字符集
        /// </summary>
        [Fact(DisplayName = "RandomStringGenerator结果只包含指定字符集")]
        public void RandomStringGeneratorGenerateOnlyContainsAllowedChars()
        {
            var allowed = "ABC123";
            Assert.All(RandomStringGenerator.Generate(100, allowed), c => Assert.Contains(c, allowed));
        }

        /// <summary>
        /// RandomStringGenerator 长度0抛异常
        /// </summary>
        [Fact(DisplayName = "RandomStringGenerator长度0抛异常")]
        public void RandomStringGeneratorZeroLengthThrowsException()
        {
            Assert.Throws<ArgumentException>(() => RandomStringGenerator.Generate(0, "ABC"));
        }
    }
}
