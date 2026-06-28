using Com.GleekFramework.AssemblySdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Assembly
{
    /// <summary>
    /// 文档服务单元测试
    /// </summary>
    public class DocumentServiceTests : BaseUnitTest
    {
        /// <summary>
        /// 获取 XML 文档列表应返回非空集合
        /// </summary>
        [Fact(DisplayName = "获取XML文档列表应返回非空集合")]
        public void GetXmlDocumentFileListReturnsNonEmptyList()
        {
            var service = new DocumentService();
            var fileList = service.GetXmlDocumentFileList();
            Assert.NotNull(fileList);
            Assert.NotEmpty(fileList);
        }

        /// <summary>
        /// 所有返回的文件路径应以 .xml 结尾
        /// </summary>
        [Fact(DisplayName = "所有返回的文件路径应以Xml结尾")]
        public void GetXmlDocumentFileListAllPathsEndWithXml()
        {
            var service = new DocumentService();
            var fileList = service.GetXmlDocumentFileList();
            Assert.NotNull(fileList);
            Assert.All(fileList, path => Assert.EndsWith(".xml", path, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 所有返回的文件路径应为绝对路径
        /// </summary>
        [Fact(DisplayName = "所有返回的文件路径应为绝对路径")]
        public void GetXmlDocumentFileListAllPathsAreAbsolute()
        {
            var service = new DocumentService();
            var fileList = service.GetXmlDocumentFileList();
            Assert.NotNull(fileList);
            Assert.All(fileList, path => Assert.True(Path.IsPathRooted(path), $"路径不是绝对路径：{path}"));
        }

        /// <summary>
        /// 多次调用应返回相同的结果（缓存机制）
        /// </summary>
        [Fact(DisplayName = "多次调用应返回相同的结果")]
        public void GetXmlDocumentFileListMultipleCallsReturnsSameResult()
        {
            var service = new DocumentService();
            var firstCall = service.GetXmlDocumentFileList();
            var secondCall = service.GetXmlDocumentFileList();
            Assert.NotNull(firstCall);
            Assert.Equal(firstCall.Count(), secondCall.Count());
        }
    }
}
