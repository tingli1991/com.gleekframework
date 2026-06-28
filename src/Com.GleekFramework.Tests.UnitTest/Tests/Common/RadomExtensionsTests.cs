using Com.GleekFramework.CommonSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Common
{
    /// <summary>
    /// 随机数扩展方法单元测试
    /// </summary>
    public class RadomExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// NextInt 在指定范围内
        /// </summary>
        [Fact(DisplayName = "NextInt在指定范围内")]
        public void NextIntWithinRange()
        {
            object obj = new object();
            for (int i = 0; i < 100; i++)
                Assert.InRange(obj.NextInt(10, 20), 10, 19);
        }

        /// <summary>
        /// NextLong 在指定范围内
        /// </summary>
        [Fact(DisplayName = "NextLong在指定范围内")]
        public void NextLongWithinRange()
        {
            object obj = new object();
            for (int i = 0; i < 100; i++)
                Assert.InRange(obj.NextLong(100, 200), 100L, 199L);
        }

        /// <summary>
        /// Next 从列表中随机取值
        /// </summary>
        [Fact(DisplayName = "Next从列表中随机取值")]
        public void NextFromListReturnsElement()
        {
            var list = new List<string> { "A", "B", "C" };
            for (int i = 0; i < 50; i++)
                Assert.Contains(list.Next(), list);
        }

        /// <summary>
        /// Next 从空列表返回默认值
        /// </summary>
        [Fact(DisplayName = "Next从空列表返回默认值")]
        public void NextEmptyListReturnsDefault()
        {
            Assert.Null(new List<string>().Next());
        }

        /// <summary>
        /// Next 从 null 列表返回默认值
        /// </summary>
        [Fact(DisplayName = "Next从Null列表返回默认值")]
        public void NextNullListReturnsDefault()
        {
            List<string> list = null;
            Assert.Null(list.Next());
        }
    }
}
