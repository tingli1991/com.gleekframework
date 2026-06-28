using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 成员特性实现类
    /// </summary>
    public static partial class MemberAttributeProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 对象类型缓存列表
        /// </summary>
        private static readonly Dictionary<MemberInfo, IEnumerable<Attribute>> CacheList = new Dictionary<MemberInfo, IEnumerable<Attribute>>();

        /// <summary>
        /// 获取自定义特性(包含T以及T的子类)
        /// </summary>
        /// <param name="memberInfo">对象对应的成员信息</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            var customAttributeList = GetCustomAttributeList(memberInfo);
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
        /// <param name="memberInfo">对象对应的成员信息</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(MemberInfo memberInfo, Func<Attribute, bool> filter) where T : Attribute
        {
            var customAttributeList = GetCustomAttributeList(memberInfo);
            if (customAttributeList == null)
            {
                return new List<T>();
            }
            return customAttributeList.Where(filter).Select(e => (T)e);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="memberInfo">对象对应的成员信息</param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributeList(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                return new List<Attribute>();
            }

            if (!CacheList.ContainsKey(memberInfo))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(memberInfo))
                    {
                        var customAttributeList = memberInfo.GetCustomAttributes<Attribute>();
                        CacheList.Add(memberInfo, customAttributeList ?? new List<Attribute>());
                    }
                }
            }
            return CacheList[memberInfo];
        }
    }
}