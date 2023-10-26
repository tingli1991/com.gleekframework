using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 头部信息拓展类
    /// </summary>
    public static partial class HeaderExtensions
    {
        /// <summary>
        /// 添加头部信息
        /// </summary>
        /// <param name="headers">头部信息</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddHeader(this Dictionary<string, string> headers, string key, string value)
        {
            //非空校验
            if (headers == null || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value) || headers.ContainsKey(key))
            {
                return;
            }
            headers.Add(key, value);
        }

        /// <summary>
        /// 追加头部信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="headers">头部信息</param>
        public static Dictionary<string, string> AddHeaders(this Dictionary<string, string> source, Dictionary<string, string> headers)
        {
            if (headers != null && headers.Any())
            {
                if (source == null || !source.Any())
                {
                    source = new Dictionary<string, string>();
                }
                headers.ForEach(header => source.AddHeader(header.Key, header.Value));
            }
            return source;
        }

        /// <summary>
        /// 获取地址信息
        /// </summary>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public static string GetUrl(this Dictionary<string, string> headers)
        {
            if (headers == null || !headers.Any())
            {
                return "";
            }

            if (!headers.ContainsKey("x-scheme") || !headers.ContainsKey("x-host") || !headers.ContainsKey("x-path"))
            {
                return "";
            }

            return $"{headers["x-scheme"]}://{headers["x-host"].TrimStart('/').TrimEnd('/')}/{headers["x-path"].TrimStart('/').TrimEnd('/')}";
        }
    }
}