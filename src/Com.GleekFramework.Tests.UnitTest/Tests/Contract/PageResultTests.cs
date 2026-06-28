using Com.GleekFramework.ContractSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Contract
{
    /// <summary>
    /// 分页结果模型单元测试
    /// </summary>
    public class PageResultTests : BaseUnitTest
    {
        /// <summary>
        /// PageResult 属性赋值正确
        /// </summary>
        [Fact(DisplayName = "PageResult属性赋值正确")]
        public void PageResultPropertiesSetCorrectly()
        {
            var items = new[] { "item1", "item2" };
            var result = new PageResult<string> { NextId = 100, PageSize = 20, Results = items };
            Assert.Equal(100m, result.NextId);
            Assert.Equal(20, result.PageSize);
            Assert.Equal(2, result.Results.Count());
        }

        /// <summary>
        /// PageDataResult 计算分页属性
        /// </summary>
        [Fact(DisplayName = "PageDataResult分页计算正确")]
        public void PageDataResultPagingCalculationsCorrect()
        {
            var result = new PageDataResult<string> { PageIndex = 1, PageSize = 10, TotalCount = 95 };
            Assert.Equal(10, result.PageCount);
            Assert.True(result.HasNext);
            Assert.False(result.HasPrevious);
            result.PageIndex = 10;
            Assert.False(result.HasNext);
            Assert.True(result.HasPrevious);
        }

        /// <summary>
        /// PageDataResult HasNext 和 HasPrevious 边界值
        /// </summary>
        [Fact(DisplayName = "PageDataResult边界值正确")]
        public void PageDataResultEdgeCases()
        {
            var emptyResult = new PageDataResult<string> { PageIndex = 1, PageSize = 10, TotalCount = 0 };
            Assert.Equal(0, emptyResult.PageCount);
            Assert.False(emptyResult.HasNext);
            Assert.False(emptyResult.HasPrevious);
            var onePage = new PageDataResult<string> { PageIndex = 1, PageSize = 10, TotalCount = 10 };
            Assert.Equal(1, onePage.PageCount);
            Assert.False(onePage.HasNext);
        }

        /// <summary>
        /// PageDataResult PageSize 为 0 时 PageCount 使用除数 1
        /// </summary>
        [Fact(DisplayName = "PageDataResult除零保护")]
        public void PageDataResultZeroPageSizeProtection()
        {
            var result = new PageDataResult<string> { PageIndex = 0, PageSize = 0, TotalCount = 100 };
            Assert.Equal(100, result.PageCount);
        }

        /// <summary>
        /// PageParam 默认 PageSize 为 20
        /// </summary>
        [Fact(DisplayName = "PageParam默认PageSize为20")]
        public void PageParamDefaultPageSizeIs20()
        {
            var param = new PageParam();
            Assert.Equal(20, param.PageSize);
        }

        /// <summary>
        /// PageParam NextId 默认 null
        /// </summary>
        [Fact(DisplayName = "PageParam默认NextId为Null")]
        public void PageParamDefaultNextIdIsNull()
        {
            var param = new PageParam();
            Assert.Null(param.NextId);
        }
    }
}
