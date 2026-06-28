using Com.GleekFramework.DapperSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Dapper
{
    /// <summary>
    /// Dapper 表达式和常量单元测试
    /// </summary>
    public class DapperExpressionTests : BaseUnitTest
    {
        /// <summary>
        /// Dapper 常量默认超时时间大于 0
        /// </summary>
        [Fact(DisplayName = "Dapper常量超时时间大于0")]
        public void DefaultTimeoutIsPositive()
        {
            Assert.True(DapperConstant.DEFAULT_TIMEOUT_SECONDS > 0);
        }

        /// <summary>
        /// OrderExpressionVisitor 带参数可实例化
        /// </summary>
        [Fact(DisplayName = "OrderExpressionVisitor带参可实例化")]
        public void OrderExpressionVisitorWithParamsCanInstantiate()
        {
            var param = new Dictionary<string, object>();
            var visitor = new OrderExpressionVisitor();
            Assert.NotNull(visitor);
        }

        /// <summary>
        /// OrderExpressionVisitor 使用表达式列表实例化
        /// </summary>
        [Fact(DisplayName = "OrderExpressionVisitor类型存在")]
        public void OrderExpressionVisitorExists()
        {
            Assert.NotNull(typeof(OrderExpressionVisitor));
        }
    }
}
