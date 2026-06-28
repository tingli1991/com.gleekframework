using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// 头部信息拓展类
    /// </summary>
    public static partial class HeaderExtensions
    {
        /// <summary>
        /// 获取业务流水号
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string GetSerialNo(this IHeaderDictionary headers)
        {
            var serialNo = SnowflakeProvider.GetSerialNo();
            if (headers == null)
            {
                return serialNo;
            }

            if (headers.ContainsKey(HttpConstant.HEADER_SERIAL_NO_KEY))
            {
                serialNo = headers[HttpConstant.HEADER_SERIAL_NO_KEY];
            }
            else
            {
                headers.Add(HttpConstant.HEADER_SERIAL_NO_KEY, serialNo);
            }
            return serialNo;
        }

        /// <summary>
        /// 获取业务流水号
        /// </summary>
        /// <param name="context">http上下文</param>
        /// <param name="serialNo">流水号</param>
        /// <returns>流水编号</returns>
        public static string GetSerialNo(this IHttpContextAccessor context, string serialNo)
        {
            var headers = context.ToHeaders();
            return headers.GetSerialNo(serialNo);
        }

        /// <summary>
        /// 获取业务流水号
        /// </summary>
        /// <param name="headers">头部信息</param>
        /// <param name="serialNo">业务流水号</param>
        /// <returns></returns>
        public static string GetSerialNo(this Dictionary<string, string> headers, string serialNo)
        {
            if (headers != null && headers.ContainsKey(HttpConstant.HEADER_SERIAL_NO_KEY))
            {
                //绑定网关(或上层业务)透传的流水编号
                serialNo = headers[HttpConstant.HEADER_SERIAL_NO_KEY];
            }

            if (!string.IsNullOrEmpty(serialNo))
            {
                return serialNo;
            }

            if (string.IsNullOrEmpty(serialNo))
            {
                serialNo = SnowflakeProvider.GetSerialNo();
            }
            return serialNo;
        }

        /// <summary>
        /// 设置ContentType
        /// </summary>
        /// <param name="headers">头部信息</param>
        /// <param name="contentType">类型字符串</param>
        public static void SetContentType(this Dictionary<string, string> headers, string contentType = ContentTypeConstant.JSON)
        {
            if (headers.IsNotNull() && !headers.ContainsKey(HttpConstant.CONTENT_TYPE_HEADER_NAME))
            {
                headers.Add(HttpConstant.CONTENT_TYPE_HEADER_NAME, contentType);
            }
        }

        /// <summary>
        /// 获取ContentType
        /// </summary>
        /// <param name="headers">头部信息</param>
        /// <returns></returns>
        public static string GetContentType(this Dictionary<string, string> headers)
        {
            if (headers == null || !headers.ContainsKey(HttpConstant.CONTENT_TYPE_HEADER_NAME))
            {
                return ContentTypeConstant.JSON;
            }
            return headers[HttpConstant.CONTENT_TYPE_HEADER_NAME];
        }

        /// <summary>
        /// 添加头部信息
        /// </summary>
        /// <param name="requestHaaders"></param>
        /// <param name="headers"></param>
        public static void AddHeaders(this HttpHeaders requestHaaders, Dictionary<string, string> headers)
        {
            if (headers.IsNullOrEmpty())
            {
                return;
            }

            foreach (var header in headers)
            {
                if (requestHaaders.Contains(header.Key))
                {
                    continue;
                }
                requestHaaders.Add(header.Key, header.Value);
            }
        }

        /// <summary>
        /// 转换头部信息
        /// </summary>
        /// <param name="httpContext">http上下文</param>
        /// <returns>屏蔽完非中横线和大写字母的头部信息结果</returns>
        public static Dictionary<string, string> ToHeaders(this IHttpContextAccessor httpContext)
        {
            var headerDic = new Dictionary<string, string>();
            var request = httpContext?.HttpContext?.Request;
            if (request == null)
            {
                return headerDic;
            }

            if (request.Host.HasValue)
            {
                headerDic.AddHeader("x-host", request.Host.Value);
            }

            headerDic.AddHeader("x-path", request.Path);//接口请求路径
            headerDic.AddHeader("x-scheme", request.Scheme);//请求协议
            headerDic.AddHeader("x-method", request.Method);//请求方法
            var headers = httpContext.HttpContext.Request.Headers;
            if (headers.IsNotNull())
            {
                foreach (var header in headers)
                {
                    //非空校验
                    if (string.IsNullOrEmpty(header.Key) || string.IsNullOrEmpty(header.Value))
                    {
                        continue;
                    }

                    //如果即不再白名单也不符合通配规则的时候直接过滤
                    //Regex.IsMatch(header.Key, "[A-Z]") || 
                    if (header.Key.IndexOf(HttpConstant.WHITE_CONTAINS_STR) <= 0)
                    {
                        continue;
                    }
                    headerDic.AddHeader(header.Key, header.Value);
                }
            }
            return headerDic;
        }
    }
}