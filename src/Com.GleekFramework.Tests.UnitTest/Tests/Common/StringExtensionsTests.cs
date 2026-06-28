using Com.GleekFramework.CommonSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Common
{
    /// <summary>
    /// 字符串扩展方法单元测试
    /// </summary>
    public class StringExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// ContainsOf 包含字符串时返回 true
        /// </summary>
        [Fact(DisplayName = "ContainsOf包含字符串返回True")]
        public void ContainsOfContainsValueReturnsTrue()
        {
            Assert.True("Hello World".ContainsOf("World"));
        }

        /// <summary>
        /// EqualIgnoreCases 忽略大小写相等
        /// </summary>
        [Fact(DisplayName = "EqualIgnoreCases忽略大小写相等")]
        public void EqualIgnoreCasesCaseInsensitiveReturnsTrue()
        {
            Assert.True("Hello".EqualIgnoreCases("hello"));
            Assert.True("HELLO".EqualIgnoreCases("Hello"));
        }

        /// <summary>
        /// TrimStart 去掉开头字符串
        /// </summary>
        [Fact(DisplayName = "TrimStart去掉开头字符串")]
        public void TrimStartRemovesPrefix()
        {
            Assert.Equal("World", "HelloWorld".TrimStart("Hello"));
        }

        /// <summary>
        /// TrimEnd 去掉结尾字符串
        /// </summary>
        [Fact(DisplayName = "TrimEnd去掉结尾字符串")]
        public void TrimEndRemovesSuffix()
        {
            Assert.Equal("Hello", "HelloWorld".TrimEnd("World"));
        }

        /// <summary>
        /// ToObject 字符串转 int
        /// </summary>
        [Fact(DisplayName = "ToObject字符串转Int")]
        public void ToObjectStringToIntReturnsInt()
        {
            Assert.Equal(123, "123".ToObject<int>());
        }

        /// <summary>
        /// ToObject 字符串"1"转 bool 为 true
        /// </summary>
        [Fact(DisplayName = "ToObject字符串1转Bool为True")]
        public void ToObjectStringOneToBoolReturnsTrue()
        {
            Assert.True("1".ToObject<bool>());
        }

        /// <summary>
        /// ToObject 字符串转 Guid
        /// </summary>
        [Fact(DisplayName = "ToObject字符串转Guid")]
        public void ToObjectStringToGuidReturnsGuid()
        {
            var guidStr = "A0B1C2D3-E4F5-6789-ABCD-EF0123456789";
            Assert.Equal(Guid.Parse(guidStr), guidStr.ToObject<Guid>());
        }

        /// <summary>
        /// ToList 单个字符串转列表
        /// </summary>
        [Fact(DisplayName = "ToList单个字符串转列表")]
        public void ToListSingleStringReturnsList()
        {
            var result = "test".ToList();
            Assert.Single(result);
            Assert.Equal("test", result[0]);
        }

        /// <summary>
        /// HashMaxLength 超长截断
        /// </summary>
        [Fact(DisplayName = "HashMaxLength超长截断")]
        public void HashMaxLengthTruncatesLongString()
        {
            Assert.Equal("Hello", "Hello World!".HashMaxLength(5));
        }

        /// <summary>
        /// Substring 按标签截取中间内容
        /// </summary>
        [Fact(DisplayName = "Substring按标签截取")]
        public void SubstringByTagsReturnsContent()
        {
            Assert.Equal("World", "Hello[World]End".Substring("[", "]"));
        }
    }
}
