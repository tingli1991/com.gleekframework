using Com.GleekFramework.AssemblySdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Assembly
{
    /// <summary>
    /// 编译库服务单元测试
    /// </summary>
    public class LibraryServiceTests : BaseUnitTest
    {
        /// <summary>
        /// 获取编译库名称列表应返回非空集合
        /// </summary>
        [Fact(DisplayName = "获取编译库名称列表应返回非空集合")]
        public void GetCompileLibrarieNnameListReturnsNonEmptyList()
        {
            var service = new LibraryService();
            var nameList = service.GetCompileLibrarieNnameList();
            Assert.NotNull(nameList);
            Assert.NotEmpty(nameList);
        }

        /// <summary>
        /// 编译库名称不应为空或空白字符串
        /// </summary>
        [Fact(DisplayName = "编译库名称不应为空")]
        public void GetCompileLibrarieNnameListAllNamesAreValid()
        {
            var service = new LibraryService();
            var nameList = service.GetCompileLibrarieNnameList();
            Assert.NotNull(nameList);
            Assert.All(nameList, name => Assert.False(string.IsNullOrWhiteSpace(name)));
        }

        /// <summary>
        /// 编译库名称应包含 Gleek 核心程序集
        /// </summary>
        [Fact(DisplayName = "编译库名称应包含Gleek核心程序集")]
        public void GetCompileLibrarieNnameListContainsKnownAssembly()
        {
            var service = new LibraryService();
            var nameList = service.GetCompileLibrarieNnameList();
            Assert.NotNull(nameList);
            Assert.Contains(nameList, name => name.Contains("Com.GleekFramework"));
        }

        /// <summary>
        /// 不包含以 Microsoft 开头的系统库
        /// </summary>
        [Fact(DisplayName = "不包含以Microsoft开头的系统库")]
        public void GetCompileLibrarieNnameListExcludesMicrosoft()
        {
            var service = new LibraryService();
            var nameList = service.GetCompileLibrarieNnameList();
            Assert.NotNull(nameList);
            Assert.DoesNotContain(nameList, name => name.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 不包含以 System 开头的系统库
        /// </summary>
        [Fact(DisplayName = "不包含以System开头的系统库")]
        public void GetCompileLibrarieNnameListExcludesSystem()
        {
            var service = new LibraryService();
            var nameList = service.GetCompileLibrarieNnameList();
            Assert.NotNull(nameList);
            Assert.DoesNotContain(nameList, name => name.StartsWith("System", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 多次调用应返回相同的结果（缓存机制）
        /// </summary>
        [Fact(DisplayName = "多次调用应返回相同的结果")]
        public void GetCompileLibrarieNnameListMultipleCallsReturnsSameResult()
        {
            var service = new LibraryService();
            var firstCall = service.GetCompileLibrarieNnameList();
            var secondCall = service.GetCompileLibrarieNnameList();
            Assert.NotNull(firstCall);
            Assert.Equal(firstCall.Count(), secondCall.Count());
        }
    }
}
