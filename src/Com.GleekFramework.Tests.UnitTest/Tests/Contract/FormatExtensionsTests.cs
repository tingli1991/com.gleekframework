using Com.GleekFramework.ContractSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Contract
{
    /// <summary>
    /// 格式化扩展方法单元测试
    /// </summary>
    public class FormatExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// SafeFormat 正常格式化字符串
        /// </summary>
        [Fact(DisplayName = "SafeFormat正常格式化")]
        public void SafeFormatFormatsCorrectly()
        {
            var result = "Hello {0}, today is {1}".SafeFormat("World", "Monday");
            Assert.Equal("Hello World, today is Monday", result);
        }

        /// <summary>
        /// SafeFormat 参数个数不足时保留占位符
        /// </summary>
        [Fact(DisplayName = "SafeFormat参数不足保留占位符")]
        public void SafeFormatInsufficientArgsKeepsPlaceholder()
        {
            var result = "Values: {0}, {1}, {2}".SafeFormat("A", "B");
            Assert.Equal("Values: A, B, {2}", result);
        }

        /// <summary>
        /// SafeFormat 空字符串返回原值
        /// </summary>
        [Fact(DisplayName = "SafeFormat空字符串返回原值")]
        public void SafeFormatEmptyReturnsEmpty()
        {
            var result = "".SafeFormat("arg");
            Assert.Equal("", result);
        }

        /// <summary>
        /// SafeFormat null 返回 null
        /// </summary>
        [Fact(DisplayName = "SafeFormatNull返回Null")]
        public void SafeFormatNullReturnsNull()
        {
            string input = null;
            var result = input.SafeFormat("arg");
            Assert.Null(result);
        }

        /// <summary>
        /// SafeFormat 无占位符返回原文
        /// </summary>
        [Fact(DisplayName = "SafeFormat无占位符返回原文")]
        public void SafeFormatNoPlaceholdersReturnsOriginal()
        {
            var input = "Just a plain string";
            var result = input.SafeFormat("arg1", "arg2");
            Assert.Equal(input, result);
        }

        /// <summary>
        /// SafeFormat 参数数组中包含 null 时转为空字符串
        /// </summary>
        [Fact(DisplayName = "SafeFormat参数数组元素为Null转空")]
        public void SafeFormatNullArgInArrayBecomesEmpty()
        {
            var result = "Value: {0}".SafeFormat(new object[] { null });
            Assert.Equal("Value: ", result);
        }

        /// <summary>
        /// SafeFormat 多占位符均正确替换
        /// </summary>
        [Fact(DisplayName = "SafeFormat多占位符替换")]
        public void SafeFormatMultiplePlaceholders()
        {
            var result = "{0} + {1} = {2}".SafeFormat(1, 2, 3);
            Assert.Equal("1 + 2 = 3", result);
        }

        /// <summary>
        /// SafeFormat 无参数时返回原文
        /// </summary>
        [Fact(DisplayName = "SafeFormat无参数返回原文")]
        public void SafeFormatNoArgsReturnsOriginal()
        {
            var input = "Test {0} string";
            var result = input.SafeFormat();
            Assert.Equal(input, result);
        }
    }
}
