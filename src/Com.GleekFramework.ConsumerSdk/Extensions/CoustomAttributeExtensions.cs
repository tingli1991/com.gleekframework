using System;
using System.Collections.Generic;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 关于自定义特性的扩展
    /// </summary>
    public static partial class CoustomAttributeExtensions
    {
        /// <summary>
        /// 获取自定义特性实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T GetCoustomAttribute<T>(this object source) where T : Attribute
        {
            return CoustomAttributeProvider.GetCoustomAttribute<T>(source);
        }

        /// <summary>
        /// 获取自定义特性实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetCoustomAttributeList<T>(this object source) where T : Attribute
        {
            return CoustomAttributeProvider.GetCoustomAttributeList<T>(source);
        }
    }
}