using Com.GleekFramework.ContractSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Contract
{
    /// <summary>
    /// 契约结果模型单元测试
    /// </summary>
    public class ContractResultTests : BaseUnitTest
    {
        /// <summary>
        /// ContractResult 默认 Success 为 false
        /// </summary>
        [Fact(DisplayName = "默认Success为False")]
        public void DefaultSuccessIsFalse()
        {
            var result = new ContractResult();
            Assert.False(result.Success);
        }

        /// <summary>
        /// ContractResult 默认 Code 为 FAIL 的哈希值
        /// </summary>
        [Fact(DisplayName = "默认Code为错误码")]
        public void DefaultCodeIsFail()
        {
            var result = new ContractResult();
            Assert.Equal($"{GlobalMessageCode.FAIL.GetHashCode()}", result.Code);
        }

        /// <summary>
        /// 设置 Success 后能正确读取
        /// </summary>
        [Fact(DisplayName = "设置Success后正确")]
        public void SetSuccessWorks()
        {
            var result = new ContractResult { Success = true };
            Assert.True(result.Success);
        }

        /// <summary>
        /// ContractResult 的 SerialNo 默认不为空
        /// </summary>
        [Fact(DisplayName = "默认SerialNo不为空")]
        public void DefaultSerialNoIsNotEmpty()
        {
            var result = new ContractResult();
            Assert.False(string.IsNullOrWhiteSpace(result.SerialNo));
        }

        /// <summary>
        /// ContractResult 的 TimeStamp 大于 0
        /// </summary>
        [Fact(DisplayName = "默认TimeStamp大于0")]
        public void DefaultTimeStampIsPositive()
        {
            var result = new ContractResult();
            Assert.True(result.TimeStamp > 0);
        }

        /// <summary>
        /// ContractResult 的 Message 默认不为空
        /// </summary>
        [Fact(DisplayName = "默认Message不为空")]
        public void DefaultMessageIsNotEmpty()
        {
            var result = new ContractResult();
            Assert.False(string.IsNullOrWhiteSpace(result.Message));
        }

        /// <summary>
        /// ContractResult 的 SerialNo 每次创建不同
        /// </summary>
        [Fact(DisplayName = "每次SerialNo不同")]
        public void SerialNoIsUnique()
        {
            var result1 = new ContractResult();
            var result2 = new ContractResult();
            Assert.NotEqual(result1.SerialNo, result2.SerialNo);
        }

        /// <summary>
        /// ContractResult(T) 继承自 ContractResult
        /// </summary>
        [Fact(DisplayName = "泛型继承自基类")]
        public void GenericInheritsFromBase()
        {
            var result = new ContractResult<string>();
            Assert.IsAssignableFrom<ContractResult>(result);
        }

        /// <summary>
        /// ContractResult(T) 设置 Data 后能正确读取
        /// </summary>
        [Fact(DisplayName = "泛型设置Data正确")]
        public void GenericDataWorks()
        {
            var expected = "测试数据";
            var result = new ContractResult<string> { Data = expected };
            Assert.Equal(expected, result.Data);
        }
    }
}
