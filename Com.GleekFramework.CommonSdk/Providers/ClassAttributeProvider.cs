﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 类特性实现类
    /// </summary>
    public static partial class ClassAttributeProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 对象类型缓存列表
        /// </summary>
        private static readonly Dictionary<Type, IEnumerable<Attribute>> CacheList = new Dictionary<Type, IEnumerable<Attribute>>();

        /// <summary>
        /// 获取自定义特性(包含T以及T的子类)
        /// </summary>
        /// <param name="type">对象对应的属性信息</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(Type type) where T : Attribute
        {
            var customAttributeList = GetCustomAttributeList(type);
            if (customAttributeList == null)
            {
                return default;
            }

            var customAttribute = customAttributeList.FirstOrDefault(e => typeof(T).IsAssignableFrom(e.GetType()) || e.GetType() == type);
            if (customAttribute == null)
            {
                return default;
            }
            return (T)customAttribute;
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="type">对象对应的属性信息</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(Type type, Func<Attribute, bool> filter) where T : Attribute
        {
            var customAttributeList = GetCustomAttributeList(type);
            if (customAttributeList == null)
            {
                return new List<T>();
            }
            return customAttributeList.Where(filter).Select(e => (T)e);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="type">对象对应的属性信息</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(Type type) where T : Attribute
        {
            var fullName = typeof(T).FullName;
            var customAttributeList = GetCustomAttributeList(type);
            if (customAttributeList == null)
            {
                return new List<T>();
            }
            return customAttributeList.Where(e => e.GetType().FullName == fullName).Select(e => (T)e);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="type">对象</param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributeList(Type type)
        {
            if (!CacheList.ContainsKey(type))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(type))
                    {
                        var customAttributeList = CustomAttributeExtensions.GetCustomAttributes<Attribute>(type);
                        CacheList.Add(type, customAttributeList ?? new List<Attribute>());
                    }
                }
            }
            return CacheList[type];
        }
    }
}