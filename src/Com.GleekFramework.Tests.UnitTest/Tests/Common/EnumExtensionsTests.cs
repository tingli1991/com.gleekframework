using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Common
{
    /// <summary>
    /// 枚举扩展方法单元测试
    /// </summary>
    public class EnumExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// GetDescription 返回枚举的成功描述
        /// </summary>
        [Fact(DisplayName = "GetDescription返回成功描述")]
        public void GetDescriptionReturnsSuccess()
        {
            Assert.Equal("成功", GlobalMessageCode.SUCCESS.GetDescription());
        }

        /// <summary>
        /// GetDescription 返回枚举的失败描述
        /// </summary>
        [Fact(DisplayName = "GetDescription返回失败描述")]
        public void GetDescriptionReturnsFail()
        {
            Assert.Equal("失败", GlobalMessageCode.FAIL.GetDescription());
        }

        /// <summary>
        /// GetDescription 返回枚举的未授权描述
        /// </summary>
        [Fact(DisplayName = "GetDescription返回未授权描述")]
        public void GetDescriptionReturnsUnauthorized()
        {
            Assert.Equal("未经授权", GlobalMessageCode.UNAUTHORIZED.GetDescription());
        }

        /// <summary>
        /// EqualsActionKey 匹配时返回 true
        /// </summary>
        [Fact(DisplayName = "EqualsActionKey匹配返回True")]
        public void EqualsActionKeyMatchesReturnsTrue()
        {
            var code = GlobalMessageCode.SUCCESS;
            Assert.True(code.EqualsActionKey($"{code.GetHashCode()}"));
        }

        /// <summary>
        /// EqualsActionKey 不匹配时返回 false
        /// </summary>
        [Fact(DisplayName = "EqualsActionKey不匹配返回False")]
        public void EqualsActionKeyNotMatchReturnsFalse()
        {
            Assert.False(GlobalMessageCode.SUCCESS.EqualsActionKey("99999"));
        }
    }
}
