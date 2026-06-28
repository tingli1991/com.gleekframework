using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Com.GleekFramework.AssemblySdk
{
    /// <summary>
    /// 程序集服务
    /// </summary>
    public partial class AssemblyService : IBaseAutofac
    {
        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>程序集</returns>
        public static Assembly GetAssembly(string assemblyName)
        {
            return AssemblyProvider.GetAssembly(assemblyName);
        }

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

        /// <summary>
        /// 检查当前程序集当中的类是否包含type的子类
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="type">基础类型</param>
        /// <returns>true 表示是子类 false 表示不是子类</returns>
        public static bool CheckIsAssignableFrom(Assembly assembly, Type type)
        {
            return AssemblyProvider.CheckIsAssignableFrom(assembly, type);
        }
    }
}