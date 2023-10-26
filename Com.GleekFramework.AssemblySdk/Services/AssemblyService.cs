using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Com.GleekFramework.AssemblySdk
{
    /// <summary>
    /// 程序集实现类
    /// </summary>
    public partial class AssemblyService : IBaseAutofac
    {
        /// <summary>
        /// 获取程序集列表
        /// </summary>
        /// <returns>程序集列表</returns>
        public IEnumerable<Assembly> GetAssemblyList()
        {
            return AssemblyProvider.GetAssemblyList();
        }

        /// <summary>
        /// 获取程序集列表
        /// </summary>
        /// <param name="type">父级类型</param>
        /// <returns>程序集列表</returns>
        public IEnumerable<Assembly> GetAssemblyList(Type type)
        {
            return AssemblyProvider.GetAssemblyList(type);
        }
    }
}