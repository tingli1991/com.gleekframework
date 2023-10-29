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
        /// <param name="source">对象对应的属性信息</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this object source) where T : Attribute
        {
            return ClassAttributeProvider.GetCustomAttribute<T>(source);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="source">对象</param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributeList(this object source)
        {
            return ClassAttributeProvider.GetCustomAttributeList(source);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="source">对象对应的属性信息</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(this object source, Func<Attribute, bool> filter) where T : Attribute
        {
            return ClassAttributeProvider.GetCustomAttributeList<T>(source, filter);
        }
    }
}