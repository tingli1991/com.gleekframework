using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 程序集实现类
    /// </summary>
    public static partial class AssemblyProvider
    {
        /// <summary>
        /// 线程对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 程序集列表
        /// </summary>
        private static IEnumerable<Assembly> AssemblyList = new List<Assembly>();

        /// <summary>
        /// 获取程序集列表
        /// </summary>
        /// <param name="type">父级类型</param>
        /// <returns>程序集列表</returns>
        public static IEnumerable<Assembly> GetAssemblyList(Type type)
        {
            return GetAssemblyList().Where(e => CheckIsAssignableFrom(e, type));
        }

        /// <summary>
        /// 获取程序集列表
        /// </summary>
        /// <returns>程序集列表</returns>
        public static IEnumerable<Assembly> GetAssemblyList()
        {
            try
            {
                if (AssemblyList.IsNullOrEmpty())
                {
                    lock (@lock)
                    {
                        if (AssemblyList.IsNullOrEmpty())
                        {
                            var assemblyNameList = LibraryProvider.GetLibraryNameList();
                            AssemblyList = assemblyNameList.Select(assemblyName => GetAssembly(assemblyName));
                        }
                    }
                }
            }
            catch (Exception)
            {
                //直接屏蔽出现的异常
            }
            return AssemblyList.Where(e => e != null);
        }

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>程序集</returns>
        public static Assembly GetAssembly(string assemblyName)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch (Exception)
            {
                //直接屏蔽出现的异常
            }
            return assembly;
        }

        /// <summary>
        /// 检查当前程序集当中的类是否包含type的子类
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="type">基础类型</param>
        /// <returns>true 表示是子类 false 表示不是子类</returns>
        public static bool CheckIsAssignableFrom(Assembly assembly, Type type)
        {
            var isSuccess = false;
            try
            {
                if (assembly == null)
                {
                    return isSuccess;
                }

                var typeList = assembly.GetTypes();
                isSuccess = typeList.Any(e => e != type && (type.IsAssignableFrom(e) || ImplementsGenericInterface(e, type)));
            }
            catch (Exception)
            {
                //直接屏蔽出现的异常
            }
            return isSuccess;
        }

        /// <summary>
        /// 判定是否实现了某个特性的接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool ImplementsGenericInterface(Type type, Type interfaceType)
        {
            // 检查是否有直接实现的接口
            var isDirectlyImplemented = type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType);

            if (isDirectlyImplemented)
            {
                return true;
            }

            // 检查基类
            var baseType = type.BaseType;
            if (baseType == null)
            {
                return false;
            }

            return ImplementsGenericInterface(baseType, interfaceType);
        }
    }
}