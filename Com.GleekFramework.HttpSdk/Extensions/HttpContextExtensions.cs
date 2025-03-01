using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Text.Json;

namespace Com.GleekFramework.HttpSdk
{
    /// <summary>
    /// Http请求参数拓展类
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="context">当前请求的上下文</param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetParamterValue(this FilterContext context, string propertyName)
        {
            var paramValue = context.HttpContext.GetHeaderValue(propertyName);
            if (string.IsNullOrEmpty(paramValue))
            {
                paramValue = context.GetQueryValue(propertyName);
            }

            if (string.IsNullOrEmpty(paramValue))
            {
                paramValue = context.GetRouteValue(propertyName);
            }

            if (string.IsNullOrEmpty(paramValue))
            {
                paramValue = context.GetBodyValue(propertyName);
            }
            return paramValue;
        }

        /// <summary>
        /// 获取Query参数值
        /// </summary>
        /// <param name="context">当前请求的上下文</param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetHeaderValue(this FilterContext context, string propertyName)
        {
            return context.HttpContext.GetHeaderValue(propertyName);
        }

        /// <summary>
        /// 获取Query参数值
        /// </summary>
        /// <param name="context">当前请求的上下文</param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetHeaderValue(this HttpContext context, string propertyName)
        {
            var paramValue = "";
            if (string.IsNullOrEmpty(propertyName))
            {
                return paramValue;
            }

            if (context.Request.Headers == null)
            {
                return paramValue;
            }

            if (context.Request.Headers.ContainsKey(propertyName))
            {
                paramValue = context.Request.Headers[propertyName];
            }
            return paramValue;
        }

        /// <summary>
        /// 获取Query参数值
        /// </summary>
        /// <param name="context">当前请求的上下文</param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetQueryValue(this FilterContext context, string propertyName)
        {
            return context.HttpContext.GetQueryValue(propertyName);
        }

        /// <summary>
        /// 获取Query参数值
        /// </summary>
        /// <param name="context">当前请求的上下文</param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetQueryValue(this HttpContext context, string propertyName)
        {
            var paramValue = "";
            if (string.IsNullOrEmpty(propertyName))
            {
                return paramValue;
            }

            if (context.Request.Query == null)
            {
                return paramValue;
            }

            if (context.Request.Query.ContainsKey(propertyName))
            {
                paramValue = context.Request.Query[propertyName];
            }
            return paramValue;
        }

        /// <summary>
        /// 获取路由参数值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetRouteValue(this FilterContext context, string propertyName)
        {
            return context.RouteData.GetRouteValue(propertyName);
        }

        /// <summary>
        /// 获取路由参数值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetRouteValue(this ActionContext context, string propertyName)
        {
            return context.RouteData.GetRouteValue(propertyName);
        }

        /// <summary>
        /// 获取路由参数值
        /// </summary>
        /// <param name="routeData">路由参数</param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetRouteValue(this RouteData routeData, string propertyName)
        {
            var paramValue = "";
            if (routeData == null)
            {
                return paramValue;
            }

            var routeValues = routeData.Values;//路由数据值
            if (routeValues == null && !routeValues.Any())
            {
                return paramValue;
            }

            // 检查路由参数是否存在
            if (routeValues.TryGetValue(propertyName, out var value))
            {
                paramValue = value?.ToString() ?? "";
            }
            return paramValue;
        }

        /// <summary>
        /// 获取Body参数值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetBodyValue(this FilterContext context, string propertyName)
        {
            return context.HttpContext.GetBodyValue(propertyName);
        }

        /// <summary>
        /// 获取Body参数值
        /// </summary>
        /// <param name="context"></param>
        /// <param name="propertyName">参数名称</param>
        /// <returns></returns>
        public static string GetBodyValue(this HttpContext context, string propertyName)
        {
            var paramValue = "";
            try
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    return paramValue;
                }

                if (context.Request.Body == null)
                {
                    return paramValue;
                }

                if (context.Request.Body.Length <= 0)
                {
                    return paramValue;
                }

                //启用请求体缓冲，允许多次读取
                context.Request.EnableBuffering();

                // 直接使用 JsonSerializer 解析请求体流
                var jsonDoc = JsonDocument.Parse(context.Request.Body);

                // 重置流的位置，以便后续读取
                context.Request.Body.Position = 0;

                // 获取根元素
                var root = jsonDoc.RootElement;

                // 检查是否存在指定的参数
                if (root.TryGetProperty(propertyName, out var paramValueProperty))
                {
                    paramValue = paramValueProperty.ToString();
                }
            }
            catch (Exception)
            {
            }
            return paramValue;
        }
    }
}