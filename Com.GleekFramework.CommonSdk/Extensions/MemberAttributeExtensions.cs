using System;
using System.Collections.Generic;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 成员特性拓展类
    /// </summary>
    public static class MemberAttributeExtensions
    {
        /// <summary>
        /// 获取自定义特性(包含T以及T的子类)
        /// </summary>
        /// <param name="memberInfo">对象对应的成员信息</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this MemberInfo memberInfo) where T : Attribute
        {
            return MemberAttributeProvider.GetCustomAttribute<T>(memberInfo);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="memberInfo">对象对应的成员信息</param>
        /// <param name="filter">过滤条件</param>
        /// <returns></returns>
        public static IEnumerable<T> GetCustomAttributeList<T>(this MemberInfo memberInfo, Func<Attribute, bool> filter) where T : Attribute
        {
            return MemberAttributeProvider.GetCustomAttributeList<T>(memberInfo, filter);
        }

        /// <summary>
        /// 获取自定义特性
        /// </summary>
        /// <param name="memberInfo">对象对应的成员信息</param>
        /// <returns></returns>
        public static IEnumerable<Attribute> GetCustomAttributeList(this MemberInfo memberInfo)
        {
            return MemberAttributeProvider.GetCustomAttributeList(memberInfo);
        }
    }
}
