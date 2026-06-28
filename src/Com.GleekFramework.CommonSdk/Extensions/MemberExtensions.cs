using System;
using System.Collections.Generic;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 成员拓展类
    /// </summary>
    public static partial class MemberExtensions
    {
        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberName">成员名称</param>
        /// <returns></returns>
        public static MemberInfo GetMemberInfo(Type type, string memberName)
        {
            return MemberProvider.GetMemberInfo(type, memberName);
        }

        /// <summary>
        /// 获取类型列表的成员信息列表
        /// </summary>
        /// <param name="typeList">类型列表</param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMemberInfoList(this IEnumerable<Type> typeList)
        {
            return MemberProvider.GetMemberInfoList(typeList);
        }

        /// <summary>
        /// 获取Type的成员列表
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMemberInfoList(this Type type, Func<MemberInfo, bool> filter)
        {
            return MemberProvider.GetMemberInfoList(type, filter);
        }

        /// <summary>
        /// 获取Type的成员列表
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMemberInfoList(this Type type)
        {
            return MemberProvider.GetMemberInfoList(type);
        }

        /// <summary>
        /// 获取Type的成员列表
        /// </summary>
        /// <param name="source">对象类型</param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetMemberInfoList<T>(this T source) where T : class
        {
            var type = typeof(T);
            return MemberProvider.GetMemberInfoList(type);
        }
    }
}