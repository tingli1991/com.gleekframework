using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net.Http;
using System.Text;

namespace Com.GleekFramework.HttpSdk
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
        /// <param name="paramters"></param>
        /// <returns></returns>
        public static StringContent ToStringContent<T>(this T paramters)
        {
            var jsonValue = "";
            if (paramters != null)
            {
                if (typeof(T).Name.Equals("string", StringComparison.CurrentCultureIgnoreCase))
                {
                    //如果是字符串则直接转换成string
                    jsonValue = paramters.ToString();
                }
                else
                {
                    var jsonConverter = new IsoDateTimeConverter();
                    jsonValue = JsonConvert.SerializeObject(paramters, Formatting.Indented, jsonConverter);
                }
            }
            return new StringContent(jsonValue, Encoding.UTF8);
        }
    }
}