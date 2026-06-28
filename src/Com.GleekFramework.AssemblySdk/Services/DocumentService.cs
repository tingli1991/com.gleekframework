using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;

namespace Com.GleekFramework.AssemblySdk
{
    /// <summary>
    /// 文档服务
    /// </summary>
    public partial class DocumentService : IBaseAutofac
    {
        /// <summary>
        /// 获取当前程序集的所有XML路径列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetXmlDocumentFileList()
        {
            return DocumentProvider.GetXmlDocumentFileList();
        }
    }
}