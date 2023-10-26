using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 字典类型扩展
    /// </summary>
    public static partial class DictionaryExtensions
    {
        /// <summary>
        /// 获取对应字典对象的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetValue(this IDictionary<string, string> source, params string[] keys)
        {
            var result = "";
            if (keys == null || !keys.Any())
            {
                return default;
            }

            foreach (var key in keys)
            {
                result = source.GetValue(key);
                if (!string.IsNullOrEmpty(result))
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取对应字典对象的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static T GetValue<T>(this IDictionary<string, string> source, params string[] keys)
        {
            T result = default;
            if (keys == null || !keys.Any())
            {
                return default;
            }

            foreach (var key in keys)
            {
                var value = source.GetValue(key);
                if (!string.IsNullOrEmpty(value))
                {
                    result = value.ToObject<T>();
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取对应字典对象的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(this IDictionary<string, string> source, string key)
        {
            var value = source.GetValue(key);
            return value.ToObject<T>();
        }

        /// <summary>
        /// 获取对应字典对象的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(this IDictionary<string, string> source, string key)
        {
            return source.GetValue<string, string>(key) ?? "";
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            TValue result = default;
            if (source == null || !source.Any())
            {
                return result;
            }

            if (source.ContainsKey(key))
            {
                result = source[key];
            }
            return result;
        }

        /// <summary>
        /// 获取参数字段值
        /// </summary>
        /// <param name="actionArguments">行为参数</param>
        /// <param name="fieldNames">字段名称</param>
        /// <returns></returns>
        public static T GetActionArgumentValue<T>(this IDictionary<string, object> actionArguments, params string[] fieldNames)
        {
            var actionArgumentValue = actionArguments.GetActionArgumentValue(fieldNames);
            return actionArgumentValue.ToObject<T>();
        }

        /// <summary>
        /// 获取参数字段值
        /// </summary>
        /// <param name="actionArguments">行为参数</param>
        /// <param name="fieldNames">字段名称</param>
        /// <returns></returns>
        public static string GetActionArgumentValue(this IDictionary<string, object> actionArguments, params string[] fieldNames)
        {
            var actionArgumentValue = "";
            if (actionArguments == null || !actionArguments.Any() || fieldNames == null || !fieldNames.Any())
                return actionArgumentValue;

            foreach (var actionArgument in actionArguments)
            {
                if (actionArgument.Value == null)
                    continue;

                var type = actionArgument.Value.GetType();
                var actionArgumentInfo = actionArgument.Value;
                if (actionArgumentInfo == null)
                    continue;

                if (type.IsClass && !"string".Equals(type.Name.ToLower()))
                {
                    actionArgumentValue = actionArgumentInfo.GetPropertyValue<string>(fieldNames);
                }
                else
                {
                    foreach (var fieldName in fieldNames)
                    {
                        if (actionArgument.Value == null)
                            continue;

                        if (actionArgument.Key == fieldName)
                        {
                            actionArgumentValue = actionArgument.Value.ToString();
                            if (!string.IsNullOrEmpty(actionArgumentValue))
                                break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(actionArgumentValue))
                    break;
            }
            return actionArgumentValue;
        }
    }
}