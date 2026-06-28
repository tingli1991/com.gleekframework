using Com.GleekFramework.ContractSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Contract
{
    /// <summary>
    /// 结果扩展方法单元测试
    /// </summary>
    public class ResultExtensionsTests : BaseUnitTest
    {
        /// <summary>
        /// SetSuceccful 后 Success 为 true
        /// </summary>
        [Fact(DisplayName = "SetSuceccful后Success为True")]
        public void SetSuceccfulSuccessIsTrue()
        {
            var result = new ContractResult().SetSuceccful();
            Assert.True(result.Success);
        }

        /// <summary>
        /// SetSuceccful 后 Code 为 SUCCESS 的哈希值
        /// </summary>
        [Fact(DisplayName = "SetSuceccful后Code为成功码")]
        public void SetSuceccfulCodeIsSuccess()
        {
            var result = new ContractResult().SetSuceccful();
            Assert.Equal($"{GlobalMessageCode.SUCCESS.GetHashCode()}", result.Code);
        }

        /// <summary>
        /// SetSuceccful 后 Message 为"成功"
        /// </summary>
        [Fact(DisplayName = "SetSuceccful后Message为成功")]
        public void SetSuceccfulMessageIsSuccess()
        {
            var result = new ContractResult().SetSuceccful();
            Assert.Equal("成功", result.Message);
        }

        /// <summary>
        /// SetSuceccful 带流水号参数
        /// </summary>
        [Fact(DisplayName = "SetSuceccful带流水号参数")]
        public void SetSuceccfulWithSerialNo()
        {
            var serialNo = "TEST_SERIAL_001";
            var result = new ContractResult().SetSuceccful(serialNo);
            Assert.Equal(serialNo, result.SerialNo);
        }

        /// <summary>
        /// SetSuceccful 带格式化参数
        /// </summary>
        [Fact(DisplayName = "SetSuceccful带格式化参数")]
        public void SetSuceccfulWithFormatArgs()
        {
            var result = new ContractResult().SetSuceccful("", "额外信息");
            Assert.True(result.Success);
        }

        /// <summary>
        /// SetSuceccful(T) 后 Data 正确
        /// </summary>
        [Fact(DisplayName = "泛型SetSuceccful设置Data正确")]
        public void SetSuceccfulGenericDataWorks()
        {
            var expected = "测试数据";
            var result = new ContractResult<string>().SetSuceccful(expected);
            Assert.True(result.Success);
            Assert.Equal(expected, result.Data);
        }

        /// <summary>
        /// SetError 后 Success 为 false
        /// </summary>
        [Fact(DisplayName = "SetError后Success为False")]
        public void SetErrorSuccessIsFalse()
        {
            var result = new ContractResult().SetError(GlobalMessageCode.ERROR);
            Assert.False(result.Success);
        }

        /// <summary>
        /// SetError 后 Code 为对应错误码的哈希值
        /// </summary>
        [Fact(DisplayName = "SetError后Code为对应错误码")]
        public void SetErrorCodeMatches()
        {
            var result = new ContractResult().SetError(GlobalMessageCode.PARAM_ERROR);
            Assert.Equal($"{GlobalMessageCode.PARAM_ERROR.GetHashCode()}", result.Code);
        }

        /// <summary>
        /// SetError 后 Message 为错误描述的枚举描述
        /// </summary>
        [Fact(DisplayName = "SetError后Message为错误描述")]
        public void SetErrorMessageMatches()
        {
            var result = new ContractResult().SetError(GlobalMessageCode.UNAUTHORIZED);
            Assert.Equal("未经授权", result.Message);
        }

        /// <summary>
        /// SetError 带流水号后 SerialNo 正确
        /// </summary>
        [Fact(DisplayName = "SetError带流水号")]
        public void SetErrorWithSerialNo()
        {
            var serialNo = "ERR_SERIAL_001";
            var result = new ContractResult().SetError(GlobalMessageCode.FREQUENT_OPERATION, serialNo);
            Assert.Equal(serialNo, result.SerialNo);
        }

        /// <summary>
        /// IsSuceccful 返回 Success 的值
        /// </summary>
        [Fact(DisplayName = "IsSuceccful返回正确状态")]
        public void IsSuceccfulReturnsCorrectStatus()
        {
            var successResult = new ContractResult().SetSuceccful();
            Assert.True(successResult.IsSuceccful());
            var failResult = new ContractResult().SetError(GlobalMessageCode.FAIL);
            Assert.False(failResult.IsSuceccful());
        }

        /// <summary>
        /// IsSuceccful 对 null 返回 false
        /// </summary>
        [Fact(DisplayName = "IsSuceccful对Null返回False")]
        public void IsSuceccfulNullReturnsFalse()
        {
            ContractResult nullResult = null;
            Assert.False(nullResult.IsSuceccful());
        }

        /// <summary>
        /// SetError 泛型后 Success 为 false 且 Data 为 default
        /// </summary>
        [Fact(DisplayName = "SetError泛型后状态正确")]
        public void SetErrorGenericWorks()
        {
            var result = new ContractResult<string>().SetError(GlobalMessageCode.ILLEGAL_REQUEST);
            Assert.False(result.Success);
            Assert.Equal("非法请求", result.Message);
            Assert.Null(result.Data);
        }
    }
}
