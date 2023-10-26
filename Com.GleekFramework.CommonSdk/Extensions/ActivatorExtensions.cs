using System;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 实例激活拓展类
    /// </summary>
    public static partial class ActivatorExtensions
    {
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static T CreateInstance<T>(this Type type) where T : class, new()
        {
            return ActivatorProvider.CreateInstance<T>(type);
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static object CreateInstance(this Type type)
        {
            return ActivatorProvider.CreateInstance(type);
        }
    }
}