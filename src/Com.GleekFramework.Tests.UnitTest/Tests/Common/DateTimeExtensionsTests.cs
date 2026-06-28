using Com.GleekFramework.CommonSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Common
{
    /// <summary>
    /// 时间扩展方法单元测试
    /// </summary>
    public class DateTimeExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// ToUnixTimeForSeconds 精确到秒的时间戳转换
        /// </summary>
        [Fact(DisplayName = "ToUnixTimeForSeconds精确到秒")]
        public void ToUnixTimeForSecondsReturnsSecondsTimestamp()
        {
            var dateTime = new DateTime(2024, 1, 15, 10, 30, 0);
            var expected = (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
            Assert.Equal(expected, dateTime.ToUnixTimeForSeconds());
        }

        /// <summary>
        /// ToUnixTimeForMilliseconds 精确到毫秒的时间戳转换
        /// </summary>
        [Fact(DisplayName = "ToUnixTimeForMilliseconds精确到毫秒")]
        public void ToUnixTimeForMillisecondsReturnsMillisecondsTimestamp()
        {
            var dateTime = new DateTime(2024, 6, 15, 14, 30, 0, 500);
            var expected = (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
            Assert.Equal(expected, dateTime.ToUnixTimeForMilliseconds());
        }

        /// <summary>
        /// 10 位 Unix 时间戳正确转换为 DateTime
        /// </summary>
        [Fact(DisplayName = "ToDateTime10位时间戳转时间")]
        public void ToDateTimeUnixTime10DigitsReturnsDateTime()
        {
            long timestamp = 1705300000;
            var expected = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(8).AddSeconds(timestamp);
            Assert.Equal(expected, timestamp.ToDateTime());
        }

        /// <summary>
        /// GetRandomTime 在指定时间范围内
        /// </summary>
        [Fact(DisplayName = "GetRandomTime在指定范围内")]
        public void GetRandomTimeWithinRange()
        {
            var begin = new DateTime(2024, 1, 1);
            var end = new DateTime(2024, 12, 31);
            for (int i = 0; i < 20; i++)
                Assert.InRange(begin.GetRandomTime(end), begin, end);
        }

        /// <summary>
        /// GetRandomTime 开始大于结束时自动交换
        /// </summary>
        [Fact(DisplayName = "GetRandomTime开始大于结束自动交换")]
        public void GetRandomTimeSwapsWhenBeginGreater()
        {
            var begin = new DateTime(2024, 12, 31);
            var end = new DateTime(2024, 1, 1);
            Assert.InRange(begin.GetRandomTime(end), end, begin);
        }
    }
}
