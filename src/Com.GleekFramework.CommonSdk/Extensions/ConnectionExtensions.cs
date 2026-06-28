using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 连接字符串拓展
    /// </summary>
    public static class ConnectionExtensions
    {
        /// <summary>
        /// 获取连接字符串对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration">配置文件对象</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public static IEnumerable<T> GetConnectionObjectList<T>(this IConfiguration configuration, params string[] keys) where T : new()
        {
            if (keys.IsNullOrEmpty())
            {
                return new List<T>();
            }

            return keys.Select(key => configuration[key].ToConnectionObject<T>());
        }

        /// <summary>
        /// 获取连接字符串对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration">配置文件对象</param>
        /// <param name="keys">键</param>
        /// <returns></returns>
        public static IEnumerable<T> GetConnectionObjectList<T>(this IConfiguration configuration, IEnumerable<string> keys) where T : new()
        {
            if (keys.IsNullOrEmpty())
            {
                return new List<T>();
            }

            return keys.Select(key => configuration[key].ToConnectionObject<T>());
        }

        /// <summary>
        /// 获取连接字符串对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration">配置文件对象</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static T GetConnectionObject<T>(this IConfiguration configuration, string key) where T : new()
        {
            var connectionStrings = configuration[key];
            return connectionStrings.ToConnectionObject<T>();
        }

        /// <summary>
        /// 转换成连接字符串对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionStrings">连接字符串</param>
        /// <returns></returns>
        public static T ToConnectionObject<T>(this string connectionStrings) where T : new()
        {
            var dictionary = connectionStrings.ToConnectionDictionary();
            return dictionary.ToConnectionObject<T>();
        }

        /// <summary>
        /// 转换成连接字符串对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">字典集合</param>
        /// <returns></returns>
        public static T ToConnectionObject<T>(this Dictionary<string, string> dictionary) where T : new()
        {
            T obj = new T();
            foreach (var propertyInfo in typeof(T).GetPropertyInfoList())
            {
                if (dictionary.TryGetValue(propertyInfo.Name.ToLower(), out string value))
                {
                    propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                }
            }
            return obj;
        }

        /// <summary>
        /// 转化成对象的连接字符串
        /// </summary>
        /// <param name="source">连接对象</param>
        /// <returns></returns>
        public static string ToConnectionStrings<T>(this T source)
        {
            var connectionStrings = "";
            if (source == null)
            {
                return connectionStrings;
            }

            foreach (var propertyInfo in typeof(T).GetPropertyInfoList())
            {
                var propertyName = propertyInfo.Name.ToLower();
                var propertyValue = propertyInfo.GetValue(source, null);
                if (propertyValue == null)
                {
                    continue;
                }
                connectionStrings += $"{propertyName}={propertyValue};";
            }
            return connectionStrings;
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="connectionStrings">连接字符串</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToConnectionDictionary(this string connectionStrings)
        {
            var dictionary = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(connectionStrings))
            {
                return dictionary;
            }

            var pairs = connectionStrings.Trim(';').Split(';');
            if (pairs.IsNullOrEmpty())
            {
                return dictionary;
            }

            foreach (string pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.IsNullOrEmpty())
                {
                    continue;
                }

                if (keyValue.Length != 2)
                {
                    continue;
                }

                string key = keyValue[0].Trim();
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                string value = keyValue[1].Trim();
                if (dictionary.ContainsKey(key))
                {
                    continue;
                }
                dictionary.Add(key.ToLower(), value);
            }
            return dictionary;
        }
    }
}