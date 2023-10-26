using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.AutofacSdk
{
    /// <summary>
    /// 服务注入拓展类
    /// </summary>
    public static partial class ServiceExtensions
    {
        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">父级对象模型</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>继承于父类的所有服务</returns>
        public static IEnumerable<T> GetServiceList<T>(this T source, Func<Type, bool> filter = null)
        {
            return source.GetType().GetServiceList<T>(filter);
        }

        /// <summary>
        /// 获取服务列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">父级对象模型</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>继承于父类的所有服务</returns>
        public static IEnumerable<T> GetServiceList<T>(this Type type, Func<Type, bool> filter = null)
        {
            var typeList = type.GetTypeList();
            if (filter != null)
            {
                typeList = typeList.Where(filter);
            }
            return typeList.Select(e => e.GetService<T>()).Where(e => e != null);
        }
    }
}