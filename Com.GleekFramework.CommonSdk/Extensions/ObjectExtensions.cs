using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 对象拓展类
    /// </summary>
    public static partial class ObjectExtensions
    {
        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object obj, params string[] fieldNames)
        {
            T result = default;
            try
            {
                if (fieldNames == null || !fieldNames.Any())
                    return result;

                foreach (var fieldName in fieldNames)
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
        /// <param name="fieldName">属性名称</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string fieldName)
        {
            object result = null;
            try
            {
                if (obj == null)
                {
                    return result;
                }

                Type type = obj.GetType();
                var propertypeList = type.GetProperties();
                var propertyInfo = propertypeList.FirstOrDefault(e => e.Name.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
                if (propertyInfo != null)
                {
                    result = propertyInfo.GetValue(obj, null);
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
        /// <param name="fieldName">属性名称</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object obj, string fieldName)
        {
            T result = default;
            try
            {
                if (obj == null)
                {
                    return result;
                }

                var jobject = JObject.Parse(obj.ToString());
                var data = $"{jobject[fieldName] ?? ""}";
                if (data != null)
                {
                    result = data.ToObject<T>();
                }
                else
                {
                    Type type = obj.GetType();
                    var propertypeList = type.GetProperties();
                    var propertyInfo = propertypeList.FirstOrDefault(e => e.Name.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
                    if (propertyInfo != null)
                    {
                        var value = propertyInfo.GetValue(obj, null);
                        if (value != null)
                        {
                            result = value.ToString().ToObject<T>();
                        }
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                    Type type = obj.GetType();
                    var propertypeList = type.GetProperties();
                    var propertyInfo = propertypeList.FirstOrDefault(e => e.Name.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
                    if (propertyInfo != null)
                    {
                        var value = propertyInfo.GetValue(obj, null);
                        if (value != null)
                        {
                            result = value.ToString().ToObject<T>();
                        }
                    }
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
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">字段值</param>
        /// <returns></returns>
        public static bool SetPropertyValue(this object obj, string fieldName, object value)
        {
            try
            {
                if (obj == null || string.IsNullOrEmpty(fieldName))
                {
                    return false;
                }

                Type type = obj.GetType();
                type.GetProperty(fieldName).SetValue(obj, value, null);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}