using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 编译库实现类
    /// </summary>
    public static partial class LibraryProvider
    {
        /// <summary>
        /// 线程对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 编译库的名称列表缓存
        /// </summary>
        private static IEnumerable<string> LibrarieNnameList = new List<string>();

        /// <summary>
        /// 获取编译库的名称列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetLibraryNameList()
        {
            if (LibrarieNnameList == null || !LibrarieNnameList.Any())
            {
                lock (@lock)
                {
                    if (LibrarieNnameList == null || !LibrarieNnameList.Any())
                    {
                        var context = DependencyContext.Default;
                        var compileLibrarieList = context.CompileLibraries;
                        var stringComparison = StringComparison.CurrentCultureIgnoreCase;
                        var excludeLibraryTypeList = DependencyConstant.ExcludeLibraryTypeList;
                        var excludeLibraryNameList = DependencyConstant.ExcludeLibraryNameList;
                        LibrarieNnameList = compileLibrarieList
                            .Where(e => excludeLibraryTypeList.Contains(e.Type) && !excludeLibraryNameList.Any(p => e.Name.StartsWith(p, stringComparison)))
                            .Select(e => e.Name);
                    }
                }
            }
            return LibrarieNnameList;
        }
    }
}