using Microsoft.AspNetCore.Http;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 用户认证拓展类
    /// </summary>
    public static class UserAuthExtensions
    {
        /// <summary>
        /// 设置授权对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContext"></param>
        /// <param name="userAuthKey"></param>
        /// <param name="userAuthInfo"></param>
        public static void SetUserAuth<T>(this HttpContext httpContext, string userAuthKey, T userAuthInfo)
        {
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
        public static T ToUserAuth<T>(this IHttpContextAccessor contextAccessor, string userAuthKey)
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