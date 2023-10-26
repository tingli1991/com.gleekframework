using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 属性自定义特性实现类
    /// </summary>
    public static partial class PropertyAttributeProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 对象类型缓存列表
        /// </summary>
        private static readonly Dictionary<PropertyInfo, IEnumerable<Attribute>> CacheList = new Dictionary<PropertyInfo, IEnumerable<Attribute>>();

        /// <summary>
        /// 获取自定义特性(包含T以及T的子类)
        /// </summary>
        /// <param name="propertyInfo">对象对应的属性信息</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(PropertyInfo propertyInfo) where T : Attribute
        {
            var customAttributeList = GetCustomAttributeList(propertyInfo);
            if (customAttributeList == null)
            {
                return default;
            }

            var type = typeof(T);
            var customAttribute = customAttributeList.FirstOrDefault(e => type.IsAssignableFrom(e.GetType()) || e.GetType() == type);
            if (customAttribute == null)
            {
                return default;
            }
            return (T)customAttribute;
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="propertyInfo">对象对应的属性信息</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(PropertyInfo propertyInfo, Func<Attribute, bool> filter) where T : Attribute
        {
            var customAttributeList = GetCustomAttributeList(propertyInfo);
            if (customAttributeList == null)
            {
                return new List<T>();
            }
            return customAttributeList.Where(filter).Select(e => (T)e);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="propertyInfo">对象对应的属性信息</param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributeList(PropertyInfo propertyInfo)
        {
            if (!CacheList.ContainsKey(propertyInfo))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(propertyInfo))
                    {
                        var customAttributeList = propertyInfo.GetCustomAttributes<Attribute>();
                        CacheList.Add(propertyInfo, customAttributeList ?? new List<Attribute>());
                    }
                }
            }
            return CacheList[propertyInfo];
        }
    }
}