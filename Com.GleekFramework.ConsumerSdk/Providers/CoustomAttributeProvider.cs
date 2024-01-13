using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 消费者特性实现类
    /// </summary>
    public static partial class CoustomAttributeProvider
    {
        /// <summary>
        /// 锁同步
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 消息处理缓存列表
        /// </summary>
        private static readonly Dictionary<string, IEnumerable<Attribute>> AttributeCache = new Dictionary<string, IEnumerable<Attribute>>();

        /// <summary>
        /// 获取自定义特性实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T GetCoustomAttribute<T>(object source) where T : Attribute
        {
            T attribute = default;
            if (source == null)
            {
                return attribute;
            }

            var type = typeof(T);
            var attributeList = GetCoustomAttributeList<T>(source);
            return attributeList.FirstOrDefault(e => e.GetType().FullName == type.FullName);
        }

        /// <summary>
        /// 获取自定义特性实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetCoustomAttributeList<T>(object source) where T : Attribute
        {
            IEnumerable<T> attributeList = new List<T>();
            if (source == null)
            {
                return attributeList;
            }

            var type = source.GetType();
            var attributeType = typeof(T);
            var cacheKey = $"{type.FullName}.{attributeType.FullName}";
            if (!AttributeCache.ContainsKey(cacheKey))
            {
                lock (@lock)
                {
                    if (!AttributeCache.ContainsKey(cacheKey))
                    {
                        var classAttributeList = type.GetCustomAttributes<T>(false);
                        if (classAttributeList.IsNotNull())
                        {
                            attributeList = attributeList.Union(classAttributeList);
                        }

                        var methodAttributeList = type.GetMethods().SelectMany(e => e.GetCustomAttributes(attributeType, false).Select(p => p as T)).Where(e => e != null);
                        if (methodAttributeList.IsNotNull())
                        {
                            var fullNameList = attributeList.Select(e => e.GetType().FullName);
                            attributeList = attributeList.Union(methodAttributeList.Where(e => !fullNameList.Contains(e.GetType().FullName)));
                        }

                        var propertyAttributeList = type.GetProperties().SelectMany(e => e.GetCustomAttributes(attributeType, false).Select(p => p as T)).Where(e => e != null);
                        if (propertyAttributeList.IsNotNull())
                        {
                            var fullNameList = attributeList.Select(e => e.GetType().FullName);
                            attributeList = attributeList.Union(propertyAttributeList.Where(e => !fullNameList.Contains(e.GetType().FullName)));
                        }

                        if (attributeList.IsNotNull())
                        {
                            AttributeCache.Add(cacheKey, attributeList);
                        }
                    }
                }
            }
            else
            {
                attributeList = AttributeCache[cacheKey].Select(e => e as T).Where(e => e != null);
            }
            return attributeList;
        }
    }
}