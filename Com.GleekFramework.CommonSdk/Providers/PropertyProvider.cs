using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 反射属性实现类
    /// </summary>
    public static partial class PropertyProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 对象类型缓存列表
        /// </summary>
        private static readonly Dictionary<Type, IEnumerable<PropertyInfo>> CacheList = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(Type type, string propertyName)
        {
            var propertyInfoList = GetPropertyInfoList(type);
            if (propertyInfoList.IsNullOrEmpty())
            {
                return null;
            }
            return propertyInfoList.FirstOrDefault(p => p.Name == propertyName);
        }

        /// <summary>
        /// 获取类型列表的属性信息列表
        /// </summary>
        /// <param name="typeList">类型列表</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfoList(IEnumerable<Type> typeList)
        {
            if (typeList.IsNullOrEmpty())
            {
                return new List<PropertyInfo>();
            }

            return typeList.SelectMany(type => GetPropertyInfoList(type));
        }

        /// <summary>
        /// 获取Type的属性列表
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfoList(Type type, Func<PropertyInfo, bool> filter)
        {
            var propertyInfoList = GetPropertyInfoList(type);
            if (propertyInfoList.IsNullOrEmpty())
            {
                return new List<PropertyInfo>();
            }
            return propertyInfoList.Where(filter);
        }

        /// <summary>
        /// 获取Type的属性列表
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetPropertyInfoList(Type type)
        {
            if (!CacheList.ContainsKey(type))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(type))
                    {
                        var propertyInfoList = type.GetProperties();//属性列表
                        CacheList.Add(type, propertyInfoList ?? new PropertyInfo[] { });
                    }
                }
            }
            return CacheList[type];
        }
    }
}