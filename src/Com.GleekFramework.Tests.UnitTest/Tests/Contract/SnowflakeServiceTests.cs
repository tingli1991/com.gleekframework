using Com.GleekFramework.ContractSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Contract
{
    /// <summary>
    /// 雪花算法服务单元测试
    /// </summary>
    public class SnowflakeServiceTests : BaseUnitTest
    {
        /// <summary>
        /// GetSerialNo 返回不为空
        /// </summary>
        [Fact(DisplayName = "获取流水号返回不为空")]
        public void GetSerialNoReturnsNotEmpty()
        {
            var service = new SnowflakeService();
            var serialNo = service.GetSerialNo();
            Assert.False(string.IsNullOrWhiteSpace(serialNo));
        }

        /// <summary>
        /// GetSerialNo 返回 28 位长度（yyMMddHHmmssffff+3位机器码+9位序号）
        /// </summary>
        [Fact(DisplayName = "获取流水号长度为28位")]
        public void GetSerialNoLengthIs28()
        {
            var service = new SnowflakeService();
            var serialNo = service.GetSerialNo();
            Assert.Equal(28, serialNo.Length);
        }

        /// <summary>
        /// 多次获取流水号不重复
        /// </summary>
        [Fact(DisplayName = "多次获取流水号不重复")]
        public void GetSerialNoIsUnique()
        {
            var service = new SnowflakeService();
            var set = new HashSet<string>();
            for (int i = 0; i < 1000; i++)
            {
                var serialNo = service.GetSerialNo();
                Assert.True(set.Add(serialNo), $"第{i+1}次生成的流水号重复：{serialNo}");
            }
            Assert.Equal(1000, set.Count);
        }

        /// <summary>
        /// GetVersionNo 返回大于 0
        /// </summary>
        [Fact(DisplayName = "获取版本号大于0")]
        public void GetVersionNoIsPositive()
        {
            var service = new SnowflakeService();
            var versionNo = service.GetVersionNo();
            Assert.True(versionNo > 0);
        }

        /// <summary>
        /// 多次获取版本号不重复
        /// </summary>
        [Fact(DisplayName = "多次获取版本号不重复")]
        public void GetVersionNoIsUnique()
        {
            var service = new SnowflakeService();
            var set = new HashSet<decimal>();
            for (int i = 0; i < 500; i++)
            {
                var versionNo = service.GetVersionNo();
                Assert.True(set.Add(versionNo), $"第{i+1}次生成的版本号重复：{versionNo}");
            }
        }

        /// <summary>
        /// 流水号以日期开头
        /// </summary>
        [Fact(DisplayName = "流水号以日期开头")]
        public void GetSerialNoStartsWithDate()
        {
            var service = new SnowflakeService();
            var serialNo = service.GetSerialNo();
            var datePart = serialNo.Substring(0, 6);
            Assert.Matches(@"^\d{6}", datePart);
        }
    }
}
