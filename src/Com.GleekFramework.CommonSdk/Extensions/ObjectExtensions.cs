using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 对象拓展类
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// 转换为字典
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var result = new Dictionary<string, object>();
            if (obj == null)
            {
                return result;
            }
            var type = obj.GetType();
            var propertyInfoList = type.GetPropertyInfoList();
            return propertyInfoList.ToDictionary(k => k.Name, v => obj.GetPropertyValue(v.Name));
        }

        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object obj, params string[] propertyNames)
        {
            T result = default;
            try
            {
                if (obj == null)
                {
                    return result;
                }

                if (propertyNames.IsNullOrEmpty())
                {
                    return result;
                }

                foreach (var fieldName in propertyNames)
                {
                    result = obj.GetPropertyValue<T>(fieldName);
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {

            }
            return result;
        }


        /// <summary>
        /// 获取一个类指定的属性值
        /// </summary>
        /// <param name="obj">object对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetPropertyValue<object>(propertyName);
        }

        /// <summary>
        /// 获取一个类指定的属性值
        /// </summary>
        /// <param name="obj">object对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            T result = default;
            try
            {
                if (obj == null)
                {
                    return result;
                }

                var propertypeList = PropertyProvider.GetPropertyInfoList(obj.GetType());
                var propertyInfo = propertypeList.FirstOrDefault(e => e.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
                if (propertyInfo == null)
                {
                    throw new ArgumentNullException(nameof(propertyName));
                }

                var value = propertyInfo.GetValue(obj, null);
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                try
                {
                    var jsonObj = JObject.Parse(obj.ToString());
                    var value = $"{jsonObj[propertyName] ?? ""}";
                    result = value.ToObject<T>();
                }
                catch (Exception)
                {
                }
            }
            return result;
        }

        /// <summary>
        /// 设置对象属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName">字段名称</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public static bool SetPropertyValue(this object obj, string propertyName, object value)
        {
            try
            {
                if (obj == null || string.IsNullOrEmpty(propertyName))
                {
                    return false;
                }

                var propertypeInfo = PropertyProvider.GetPropertyInfo(obj.GetType(), propertyName);
                propertypeInfo.SetValue(obj, value, null);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}