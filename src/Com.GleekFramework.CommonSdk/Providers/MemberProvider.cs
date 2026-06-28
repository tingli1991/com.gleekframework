using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 成员实现类
    /// </summary>
    public static partial class MemberProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 对象类型缓存列表
        /// </summary>
        private static readonly Dictionary<Type, IEnumerable<MemberInfo>> CacheList = new Dictionary<Type, IEnumerable<MemberInfo>>();

        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName">成员名称</param>
        /// <returns></returns>
        public static MemberInfo GetMemberInfo(Type type, string memberName)
        {
            var propertyInfoList = GetMemberInfoList(type);
            if (propertyInfoList.IsNullOrEmpty())
            {
                return null;
            }
            return propertyInfoList.FirstOrDefault(p => p.Name == memberName);
        }

        /// <summary>
        /// 获取类型列表的成员信息列表
        /// </summary>
        /// <param name="typeList">类型列表</param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMemberInfoList(IEnumerable<Type> typeList)
        {
            if (typeList.IsNullOrEmpty())
            {
                return new List<MemberInfo>();
            }

            return typeList.SelectMany(type => GetMemberInfoList(type));
        }

        /// <summary>
        /// 获取Type的成员列表
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMemberInfoList(Type type, Func<MemberInfo, bool> filter)
        {
            var memberInfoList = GetMemberInfoList(type);
            if (memberInfoList.IsNullOrEmpty())
            {
                return new List<MemberInfo>();
            }
            return memberInfoList.Where(filter);
        }

        /// <summary>
        /// 获取Type的成员列表
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMemberInfoList(Type type)
        {
            if (!CacheList.ContainsKey(type))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(type))
                    {
                        var memberInfoList = type.GetMembers();//成员列表
                        CacheList.Add(type, memberInfoList ?? new MemberInfo[] { });
                    }
                }
            }
            return CacheList[type];
        }
    }
}