using System;
using System.Collections.Generic;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 类特性拓展类
    /// </summary>
    public static class ClassAttributeExtensions
    {
        /// <summary>
        /// 获取自定义特性(包含T以及T的子类)
        /// </summary>
        /// <param name="type">对象对应的属性信息</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this Type type) where T : Attribute
        {
            return ClassAttributeProvider.GetCustomAttribute<T>(type);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="type">对象</param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributeList(this Type type)
        {
            return ClassAttributeProvider.GetCustomAttributeList(type);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="type">对象对应的属性信息</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(Type type) where T : Attribute
        {
            return ClassAttributeProvider.GetCustomAttributeList<T>(type);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="type">对象对应的属性信息</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(this Type type, Func<Attribute, bool> filter) where T : Attribute
        {
            return ClassAttributeProvider.GetCustomAttributeList<T>(type, filter);
        }
    }
}