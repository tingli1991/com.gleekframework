using Com.GleekFramework.CommonSdk;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 用户认证拓展类
    /// </summary>
    public static class UserAuthExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        private const string START_WITH_TOKEN_KEY = "Bearer";

        /// <summary>
        /// 默认的用户授权键
        /// </summary>
        private const string DEFAULT_USER_AUTH_KEY = "AUTHORIZATION_KEY";

        /// <summary>
        /// 从上下文获取访问令牌
        /// </summary>
        /// <param name="contextAccessor">上下文</param>
        /// <returns></returns>
        public static string GetAccessToken(this IHttpContextAccessor contextAccessor)
        {
            return contextAccessor.HttpContext.GetAccessToken();
        }

        /// <summary>
        /// 从上下文获取访问令牌
        /// </summary>
        /// <param name="httpContext">上下文</param>
        /// <returns></returns>
        public static string GetAccessToken(this HttpContext httpContext)
        {
            var headers = httpContext.Request.Headers;//请求头
            var isSuccess = headers.TryGetValue(HeaderNames.Authorization, out StringValues token); //解析访问令牌
            if (!isSuccess)
            {
                return "";
            }

            var accessToken = token.ToString() ?? "";
            if (string.IsNullOrEmpty(accessToken))
            {
                return "";
            }

            if (accessToken.StartsWith(START_WITH_TOKEN_KEY))
            {
                accessToken = accessToken.TrimStart(START_WITH_TOKEN_KEY);
            }
            return $"{accessToken ?? ""}".TrimStart(" ");
        }

        /// <summary>
        /// 设置授权对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContext"></param>
        /// <param name="userAuthInfo"></param>
        /// <param name="userAuthKey"></param>
        public static void SetUserAuth<T>(this HttpContext httpContext, T userAuthInfo = default, string userAuthKey = DEFAULT_USER_AUTH_KEY)
        {
            if (userAuthInfo == null)
            {
                return;
            }

            if (httpContext.Items.ContainsKey(userAuthKey))
            {
                //修改
                httpContext.Items[userAuthKey] = userAuthInfo;
            }
            else
            {
                //新增
                httpContext.Items.Add(userAuthKey, userAuthInfo);
            }
        }

        /// <summary>
        /// 转换授权对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contextAccessor"></param>
        /// <param name="userAuthKey"></param>
        /// <returns></returns>
        public static T GetUserAuth<T>(this IHttpContextAccessor contextAccessor, string userAuthKey = DEFAULT_USER_AUTH_KEY)
        {
            T result = default;
            var httpContext = contextAccessor.HttpContext;//当前的Http请求上下文
            if (httpContext == null)
            {
                return result;
            }

            httpContext.Items.TryGetValue(userAuthKey, out object userAuthInfo);
            return userAuthInfo == null ? result : (T)httpContext.Items[userAuthKey];
        }
    }
}