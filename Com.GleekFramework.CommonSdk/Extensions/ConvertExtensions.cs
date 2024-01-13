using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;

namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// 数据转换方法
    /// </summary>
    public static partial class ConvertExtensions
    {
        /// <summary>
        /// 转换成Get请求的Url接口地址
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public static string ToGetUrl(this string url, Dictionary<string, string> paramters)
        {
            var text = paramters.ToGetParamters();
            return $"{url.TrimEnd('?', '&', '=')}?{text}";
        }

        /// <summary>
        /// 转换成Get请求参数
        /// </summary>
        /// <param name="paramters"></param>
        /// <returns></returns>
        public static string ToGetParamters(this Dictionary<string, string> paramters)
        {
            var text = "";
            if (paramters.IsNullOrEmpty())
            {
                return text;
            }

            foreach (KeyValuePair<string, string> paramter in paramters)
            {
                text += paramter.Key + "=" + paramter.Value + "&";
            }
            return text.TrimEnd('&');
        }
    }
}