using System;
using System.Collections.Generic;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 程序集对象类型
    /// </summary>
    public static partial class AssemblyTypeProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 对象类型缓存列表
        /// </summary>
        private static readonly Dictionary<Assembly, IEnumerable<Type>> CacheList = new Dictionary<Assembly, IEnumerable<Type>>();

        /// <summary>
        /// 获取程序集合下面的所有类型列表
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypeList(Assembly assembly)
        {
            if (!CacheList.ContainsKey(assembly))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(assembly))
                    {
                        var propertyInfoList = assembly.GetTypes();//属性列表
                        CacheList.Add(assembly, propertyInfoList ?? new Type[] { });
                    }
                }
            }
            return CacheList[assembly];
        }
    }
}