using System.Collections.Generic;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 依赖常量
    /// </summary>
    public static class DependencyConstant
    {
        /// <summary>
        /// 排查的库类型列表
        /// </summary>
        public static readonly IEnumerable<string> ExcludeLibraryTypeList = new List<string>() { "project", "package" };

        /// <summary>
        /// 排查的库名称列表
        /// </summary>
        public static readonly IEnumerable<string> ExcludeLibraryNameList = new List<string>() { "Microsoft", "System", "runtime" };
    }
}