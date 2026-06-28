using System;
using System.ComponentModel;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 枚举拓展类
    /// </summary>
    public static partial class EnumExtensions
    {
        /// <summary>
        /// 对比Action
        /// </summary>
        /// <param name="source">原枚举数据</param>
        /// <param name="actionKey">行为键</param>
        /// <returns></returns>
        public static bool EqualsActionKey(this Enum source, string actionKey)
        {
            if (string.IsNullOrEmpty(actionKey))
            {
                return false;
            }

            var sourceAciontKey = source.GetActionKey();
            if (string.IsNullOrEmpty(sourceAciontKey))
            {
                return false;
            }

            if (sourceAciontKey.Equals(actionKey, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return $"{source.GetHashCode()}" == actionKey;
        }

        /// <summary>
        /// 自定义枚举扩展
        /// </summary>
        /// <param name="source">定义枚举</param>
        /// <returns></returns>
        public static string GetDescription(this Enum source)
        {
            var customAttribute = source.GetCustomAttribute<DescriptionAttribute>();
            return customAttribute?.Description ?? "";
        }

        /// <summary>
        /// 获取行为键
        /// </summary>
        /// <param name="source">枚举</param>
        /// <returns></returns>
        public static string GetActionKey(this Enum source)
        {
            var customAttribute = source.GetCustomAttribute<ActionAttribute>();
            return customAttribute?.Key ?? $"{source}";
        }

        /// <summary>
        /// 获取自定的自定义特性值
        /// </summary>
        /// <typeparam name="T">特性对象</typeparam>
        /// <param name="source">枚举</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this Enum source) where T : Attribute
        {
            T result = default;
            if (source == null)
            {
                return result;
            }

            var type = source.GetType();
            var fieldInfo = FieldProvider.GetField(type, source.ToString());
            if (fieldInfo == null)
            {
                return result;
            }
            return FieldAttributeProvider.GetCustomAttribute<T>(fieldInfo);
        }
    }
}