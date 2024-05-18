using System;
using System.Collections.Generic;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 属性特性拓展类
    /// </summary>
    public static class PropertyAttributeExtensions
    {
        /// <summary>
        /// 获取自定义特性(包含T以及T的子类)
        /// </summary>
        /// <param name="propertyInfo">对象对应的属性信息</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            return PropertyAttributeProvider.GetCustomAttribute<T>(propertyInfo);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="propertyInfo">对象对应的属性信息</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(this PropertyInfo propertyInfo, Func<Attribute, bool> filter) where T : Attribute
        {
            return PropertyAttributeProvider.GetCustomAttributeList<T>(propertyInfo, filter);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="propertyInfo">对象对应的属性信息</param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributeList(this PropertyInfo propertyInfo)
        {
            return PropertyAttributeProvider.GetCustomAttributeList(propertyInfo);
        }
    }
}