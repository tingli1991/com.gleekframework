using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 非空验证拓展类
    /// </summary>
    public static class EmptyVerifyExtensions
    {
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNull(this object source)
        {
            return source == null;
        }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object source)
        {
            return !source.IsNull();
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="source"></param>
        /// <returns>true：表示为空，false：表示不为空</returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 判断元素不为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns>true：表示为空，false：表示不为空</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }
    }
}