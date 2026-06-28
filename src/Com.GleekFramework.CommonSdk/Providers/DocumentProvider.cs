using System;
using System.Collections.Generic;
using System.IO;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 程序集的文档实现类
    /// </summary>
    public static partial class DocumentProvider
    {
        /// <summary>
        /// 线程对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 当前程序集的所有XML文件名称缓存
        /// </summary>
        private static IEnumerable<string> XmlDocumentFileList = new List<string>();

        /// <summary>
        /// 获取当前程序集的所有XML路径列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetXmlDocumentFileList()
        {
            if (XmlDocumentFileList.IsNullOrEmpty())
            {
                lock (@lock)
                {
                    if (XmlDocumentFileList.IsNullOrEmpty())
                    {
                        XmlDocumentFileList = GetExistsXmlDocumentFileList();
                    }
                }
            }
            return XmlDocumentFileList;
        }

        /// <summary>
        /// 获取当前程序集的所有XML路径列表
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string> GetExistsXmlDocumentFileList()
        {
            var documentFileNameList = new List<string>();
            var compileLibrarieNnameList = LibraryProvider.GetLibraryNameList();
            if (compileLibrarieNnameList.IsNullOrEmpty())
            {
                return documentFileNameList;
            }

            foreach (var compileLibrarieNname in compileLibrarieNnameList)
            {
                var documentFileName = Path.Combine(AppContext.BaseDirectory, $"{compileLibrarieNname}.xml");
                if (File.Exists(documentFileName))
                {
                    documentFileNameList.Add(documentFileName);
                }
            }
            return documentFileNameList;
        }
    }
}