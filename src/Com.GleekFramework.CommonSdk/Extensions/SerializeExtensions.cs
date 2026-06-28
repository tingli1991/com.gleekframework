using Newtonsoft.Json;
using System;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 序列化拓展类
    /// </summary>
    public static partial class SerializeExtensions
    {
        /// <summary>
        /// 序列化请求参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(this T source)
        {
            if (source == null)
            {
                return "";
            }

            return JsonConvert.SerializeObject(source);
        }

        /// <summary>
        /// 反序列化请求参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">Http响应内容</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string content)
        {
            var result = default(T);
            try
            {
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }

                //设置返回数据
                result = JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception)
            {
                //序列化转换失败，直接转对象
                result = content.ToObject<T>();
            }
            return result;
        }
    }
}