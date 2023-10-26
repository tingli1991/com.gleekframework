using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;

namespace Com.GleekFramework.AssemblySdk
{
    /// <summary>
    /// 编译库实现类
    /// </summary>
    public partial class LibraryService : IBaseAutofac
    {
        /// <summary>
        /// 获取编译库的名称列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCompileLibrarieNnameList()
        {
            return LibraryProvider.GetLibraryNameList();
        }
    }
}