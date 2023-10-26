using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 字段自定义特性实现类
    /// </summary>
    public static partial class FieldAttributeProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 对象类型缓存列表
        /// </summary>
        private static readonly Dictionary<FieldInfo, IEnumerable<Attribute>> CacheList = new Dictionary<FieldInfo, IEnumerable<Attribute>>();

        /// <summary>
        /// 获取自定义特性(包含T以及T的子类)
        /// </summary>
        /// <param name="fieldInfo">对象对应的字段信息</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(FieldInfo fieldInfo) where T : Attribute
        {
            var customAttributeList = GetCustomAttributeList(fieldInfo);
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
        /// <param name="fieldInfo">对象对应的字段信息</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(FieldInfo fieldInfo, Func<Attribute, bool> filter) where T : Attribute
        {
            var customAttributeList = GetCustomAttributeList(fieldInfo);
            if (customAttributeList == null)
            {
                return new List<T>();
            }
            return customAttributeList.Where(filter).Select(e => (T)e);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="fieldInfo">对象对应的字段信息</param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributeList(FieldInfo fieldInfo)
        {
            if (!CacheList.ContainsKey(fieldInfo))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(fieldInfo))
                    {
                        var customAttributeList = fieldInfo.GetCustomAttributes<Attribute>();
                        CacheList.Add(fieldInfo, customAttributeList ?? new List<Attribute>());
                    }
                }
            }
            return CacheList[fieldInfo];
        }
    }
}