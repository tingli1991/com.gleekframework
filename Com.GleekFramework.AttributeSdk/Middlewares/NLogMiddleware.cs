﻿using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NLogSdk;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 接口日志收集中间件
    /// </summary>
    public class NLogMiddleware
    {
        /// <summary>
        /// 请求管道
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        public NLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 中间件执行方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var beginTime = DateTime.Now.ToCstTime();//开始时间
            var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}";
            string requestBodyStr = "";
            string responseBodyStr = "";
            string serialNo = "";
            try
            {
                requestBodyStr = await context.Request.GetRequestBody();//请求参数
                responseBodyStr = await GetResponseBodyJsonStr(context);//响应结果
                var (request, response) = ConvertObject(requestBodyStr, responseBodyStr);//转换对象
                serialNo = GetSerialNo(context.Request.Headers, request, response);//流水编号

                var path = context.Request.Path;
                if (!path.HasValue || path.Value.Contains("swagger") || path.Value.Contains("api-docs"))
                {
                    //非空校验
                    await _next(context);
                    return;
                }

                var exclActionList = NLogMiddlewareExtensions.ExclActionList;
                if (exclActionList.AnyOf(e => url.ContainsOf(e)))
                {
                    //拦截一些特定日志
                    return;
                }

                context.Response.OnCompleted(async () =>
                {
                    await Task.CompletedTask;
                    var totalMilliseconds = (long)(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
                    if (NLogMiddlewareExtensions.Level != null)
                    {
                        NLogProvider.Save(NLogMiddlewareExtensions.Level, new NLogModel()
                        {
                            Url = url,
                            SerialNo = serialNo,
                            TotalMilliseconds = totalMilliseconds,
                            Content = @$"请求参数：{requestBodyStr.JsonCompressAndEscape()}，响应结果：{responseBodyStr.JsonCompressAndEscape()}，头部信息：{context.Request.Headers.JsonCompressAndEscape()}",
                        });
                    }
                    else
                    {
                        NLogProvider.Trace(@$"请求参数：{requestBodyStr.JsonCompressAndEscape()}，响应结果：{responseBodyStr.JsonCompressAndEscape()}，头部信息：{context.Request.Headers.JsonCompressAndEscape()}", serialNo, url, totalMilliseconds);
                    }
                });
            }
            catch (Exception ex)
            {
                var totalMilliseconds = (long)(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
                NLogProvider.Error(@$"请求参数：{requestBodyStr.JsonCompressAndEscape()}，响应结果：{responseBodyStr.JsonCompressAndEscape()}，头部信息：{context.Request.Headers.JsonCompressAndEscape()}，错误信息：{ex}", serialNo, url, totalMilliseconds);
            }
        }

        /// <summary>
        /// 获取返回结果的JSON字符串
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<string> GetResponseBodyJsonStr(HttpContext context)
        {
            var responseBodyString = "";
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _next(context);
                responseBodyString = await context.Response.GetResponseBody();
                await responseBody.CopyToAsync(originalBodyStream);
            }
            return responseBodyString;
        }

        /// <summary>
        /// 转换对象
        /// </summary>
        /// <param name="requestJson">请求参数</param>
        /// <param name="responseJson">返回结果</param>
        /// <returns></returns>
        private static (object request, object response) ConvertObject(string requestJson, string responseJson)
        {
            var request = JsonConvert.DeserializeObject(requestJson);
            var response = JsonConvert.DeserializeObject(responseJson);
            return (request, response);
        }

        /// <summary>
        /// 获取业务流水号
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="request">请求参数</param>
        /// <param name="response">返回结果的Json字符串</param>
        /// <returns></returns>
        private static string GetSerialNo(IHeaderDictionary headers, object request, object response)
        {
            var serialNo = headers.GetSerialNo();
            if (string.IsNullOrEmpty(serialNo))
            {
                serialNo = request.GetPropertyValue<string>("serial_no", "SerialNo");
                if (string.IsNullOrEmpty(serialNo))
                {
                    serialNo = response.GetPropertyValue<string>("serial_no", "SerialNo");
                }
            }

            if (string.IsNullOrEmpty(serialNo))
            {
                //默认给一个新的流水编号
                serialNo = SnowflakeProvider.GetSerialNo();
            }
            return serialNo;
        }
    }
}