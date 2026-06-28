using Com.GleekFramework.CommonSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Common
{
    /// <summary>
    /// 空值验证扩展方法单元测试
    /// </summary>
    public class EmptyVerifyExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// IsNull 对 null 对象返回 true
        /// </summary>
        [Fact(DisplayName = "IsNull对象为Null返回True")]
        public void IsNullObjectIsNullReturnsTrue()
        {
            object obj = null;
            Assert.True(obj.IsNull());
        }

        /// <summary>
        /// IsNull 对非 null 对象返回 false
        /// </summary>
        [Fact(DisplayName = "IsNull对象不为Null返回False")]
        public void IsNullObjectIsNotNullReturnsFalse()
        {
            object obj = new object();
            Assert.False(obj.IsNull());
        }

        /// <summary>
        /// IsNotNull 对非 null 对象返回 true
        /// </summary>
        [Fact(DisplayName = "IsNotNull对象不为Null返回True")]
        public void IsNotNullObjectIsNotNullReturnsTrue()
        {
            object obj = new object();
            Assert.True(obj.IsNotNull());
        }

        /// <summary>
        /// IsNotNull 对 null 对象返回 false
        /// </summary>
        [Fact(DisplayName = "IsNotNull对象为Null返回False")]
        public void IsNotNullObjectIsNullReturnsFalse()
        {
            object obj = null;
            Assert.False(obj.IsNotNull());
        }

        /// <summary>
        /// IsNullOrEmpty 对空字符串返回 true
        /// </summary>
        [Fact(DisplayName = "IsNullOrEmpty字符串为空返回True")]
        public void IsNullOrEmptyEmptyStringReturnsTrue()
        {
            Assert.True("".IsNullOrEmpty());
        }

        /// <summary>
        /// IsNullOrEmpty 对非空字符串返回 false
        /// </summary>
        [Fact(DisplayName = "IsNullOrEmpty字符串不为空返回False")]
        public void IsNullOrEmptyNotEmptyStringReturnsFalse()
        {
            Assert.False("test".IsNullOrEmpty());
        }

        /// <summary>
        /// IsNullOrEmpty 对 null 集合返回 true
        /// </summary>
        [Fact(DisplayName = "IsNullOrEmpty集合为Null返回True")]
        public void IsNullOrEmptyCollectionNullReturnsTrue()
        {
            IEnumerable<int> list = null;
            Assert.True(list.IsNullOrEmpty());
        }

        /// <summary>
        /// IsNotEmpty 对有元素的集合返回 true
        /// </summary>
        [Fact(DisplayName = "IsNotEmpty集合有元素返回True")]
        public void IsNotEmptyHasElementsReturnsTrue()
        {
            var list = new List<int> { 1, 2, 3 };
            Assert.True(list.IsNotEmpty());
        }

        /// <summary>
        /// IsNullOrWhiteSpace 对空白字符串返回 true
        /// </summary>
        [Fact(DisplayName = "IsNullOrWhiteSpace空白字符串返回True")]
        public void IsNullOrWhiteSpaceWhiteSpaceReturnsTrue()
        {
            Assert.True("   ".IsNullOrWhiteSpace());
        }
    }
}
