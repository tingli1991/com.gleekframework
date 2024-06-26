﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 针对Type类型的扩展
    /// </summary>
    public static partial class AssemblyExtensions
    {
        /// <summary>
        /// 获取程序集合下面的所有类型列表
        /// </summary>
        /// <param name="type">父级类型</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypeList(this Type type)
        {
            return GetAssemblyList(type).GetTypeList().Where(e => type.IsAssignableFrom(e));
        }

        /// <summary>
        /// 获取程序集合下面的所有类型列表
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypeList(this Assembly assembly)
        {
            return AssemblyTypeProvider.GetTypeList(assembly);
        }

        /// <summary>
        /// 获取程序集合下面的所有类型列表
        /// </summary>
        /// <param name="assemblyList">程序集列表</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypeList(this IEnumerable<Assembly> assemblyList)
        {
            if (assemblyList.IsNullOrEmpty())
            {
                return new List<Type>();
            }

            return assemblyList.SelectMany(e => GetTypeList(e));
        }

        /// <summary>
        /// 获取程序集列表
        /// </summary>
        /// <param name="type">父级类型</param>
        /// <returns>程序集列表</returns>
        public static IEnumerable<Assembly> GetAssemblyList(this Type type)
        {
            return AssemblyProvider.GetAssemblyList(type);
        }

        /// <summary>
        /// 检查当前程序集当中的类是否包含type的子类
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="type">基础类型</param>
        /// <returns>true 表示是子类 false 表示不是子类</returns>
        public static bool CheckIsAssignableFrom(this Assembly assembly, Type type)
        {
            return AssemblyProvider.CheckIsAssignableFrom(assembly, type);
        }

        /// <summary>
        /// 判定是否实现了某个特性的接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            return AssemblyProvider.ImplementsGenericInterface(type, interfaceType);
        }
    }
}