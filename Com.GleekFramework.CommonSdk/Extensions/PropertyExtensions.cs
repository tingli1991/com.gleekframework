using System;
using System.Collections.Generic;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 属性反射拓展类
    /// </summary>
    public static partial class PropertyExtensions
    {
        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            return PropertyProvider.GetPropertyInfo(type, propertyName);
        }

        /// <summary>
        /// 获取类型列表的属性信息列表
        /// </summary>
        /// <param name="typeList">类型列表</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfoList(this IEnumerable<Type> typeList)
        {
            return PropertyProvider.GetPropertyInfoList(typeList);
        }

        /// <summary>
        /// 获取Type的属性列表
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfoList(this Type type, Func<PropertyInfo, bool> filter)
        {
            return PropertyProvider.GetPropertyInfoList(type, filter);
        }

        /// <summary>
        /// 获取Type的属性列表
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfoList(this Type type)
        {
            return PropertyProvider.GetPropertyInfoList(type);
        }
    }
}