using System.Collections.Generic;
using System.Reflection;
using System;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 属性反射拓展类
    /// </summary>
    public static partial class PropertyExtensions
    {
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