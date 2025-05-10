using Com.GleekFramework.CommonSdk;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 认证拓展类
    /// </summary>
    public static class OAuthExtensions
    {
        /// <summary>
        /// 令牌键字段名
        /// </summary>
        public const string TOKEN_KEY_FIELD = "token_key";

        /// <summary>
        /// 授权模式字段名
        /// </summary>
        public const string GRANT_TYPE_FIELD = "grant_type";

        /// <summary>
        /// 访问令牌前缀
        /// </summary>
        private const string START_WITH_TOKEN_KEY = "Bearer";

        /// <summary>
        /// 生成访问令牌
        /// </summary>
        /// <param name="gigningCredentials"></param>
        /// <param name="tokenKey">令牌键</param>
        /// <param name="grantType">授权模式</param>
        /// <param name="expireSeconds">过期时间</param>
        /// <param name="claims">声明集合</param>
        /// <returns></returns>
        public static string CreateAccessToken(this SigningCredentials gigningCredentials, string tokenKey, GrantType grantType, int expireSeconds, IEnumerable<Claim> claims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var header = new JwtHeader(gigningCredentials)
            {
                { TOKEN_KEY_FIELD, tokenKey },
                { GRANT_TYPE_FIELD, $"{grantType}".ToLower() }
            };
            var payload = new JwtPayload(null, null, claims ?? [], null, DateTime.UtcNow.AddSeconds(expireSeconds));
            return tokenHandler.WriteToken(new JwtSecurityToken(header, payload));
        }

        /// <summary>
        /// 获取访问令牌的头部信息
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <returns>包含 TokenKey 和 GrantType 的元组</returns>
        public static (string TokenKey, GrantType GrantType) GetAccessTokenHeaders(this string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(accessToken);
            jwtSecurityToken.Header.TryGetValue(TOKEN_KEY_FIELD, out var tokenKeyObj);
            jwtSecurityToken.Header.TryGetValue(GRANT_TYPE_FIELD, out var grantTypeObj);
            var grantType = $"{grantTypeObj}" switch
            {
                "password" => GrantType.PASSWORD,
                "authorization_code" => GrantType.AUTHORIZATION_CODE,
                "client_credentials" => GrantType.CLIENT_CREDENTIALS,
                _ => throw new NotImplementedException()
            };
            return ($"{tokenKeyObj}", grantType);
        }

        /// <summary>
        /// 获取访问令牌的过期时间
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <returns></returns>
        public static DateTime GetTokenExpiryTime(this string accessToken)
        {
            if (accessToken.IsNullOrEmpty())
            {
                return DateTime.MinValue;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(accessToken);
            return GetTokenExpiryTime(jwtSecurityToken);
        }

        /// <summary>
        /// 获取访问令牌的过期时间
        /// </summary>
        /// <param name="securityToken">访问令牌</param>
        /// <returns></returns>
        public static DateTime GetTokenExpiryTime(this SecurityToken securityToken)
        {
            if (securityToken == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                var jwtSecurityToken = (JwtSecurityToken)securityToken;
                var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return startTime.AddSeconds((jwtSecurityToken.Payload.Expiration ?? 0)).ToLocalTime();
            }
        }

        /// <summary>
        /// 从上下文获取访问令牌
        /// </summary>
        /// <param name="contextAccessor">上下文</param>
        /// <returns></returns>
        public static string GetAccessToken(this IHttpContextAccessor contextAccessor)
        {
            return contextAccessor?.HttpContext.GetAccessToken();
        }

        /// <summary>
        /// 从上下文获取访问令牌
        /// </summary>
        /// <param name="httpContext">上下文</param>
        /// <returns></returns>
        public static string GetAccessToken(this HttpContext httpContext)
        {
            if (httpContext == null)
            {
                return "";
            }

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
        /// 转换授权对象
        /// </summary>
        /// <param name="contextAccessor">Http上下文</param>
        /// <param name="userInfo">授权信息</param>
        /// <returns></returns>
        public static void SetUserInfo(this IHttpContextAccessor contextAccessor, ClaimsPrincipal userInfo = default)
        {
            if (contextAccessor == null)
            {
                return;
            }
            contextAccessor.HttpContext.SetUserInfo();
        }

        /// <summary>
        /// 设置授权对象
        /// </summary>
        /// <param name="httpContext">Http上下文</param>
        /// <param name="userInfo">授权信息</param>
        public static void SetUserInfo(this HttpContext httpContext, ClaimsPrincipal userInfo = default)
        {
            if (httpContext == null)
            {
                return;
            }
            httpContext.User = userInfo;
        }

        /// <summary>
        /// 转换授权对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contextAccessor">Http上下文</param>
        /// <returns></returns>
        public static T GetUserInfo<T>(this IHttpContextAccessor contextAccessor)
        {
            return contextAccessor.HttpContext.GetUserInfo<T>();
        }

        /// <summary>
        /// 转换授权对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpContext">Http上下文</param>
        /// <returns></returns>
        public static T GetUserInfo<T>(this HttpContext httpContext)
        {
            T result = default;
            if (httpContext?.User?.Claims == null || !httpContext.User.Claims.Any())
            {
                return result;
            }

            var claimsInfoDic = httpContext.User.Claims.ToDictionary(k => k.Type, v => v.Value);
            if (claimsInfoDic == null || !claimsInfoDic.Any())
            {
                return result;
            }

            var jsonValue = JsonConvert.SerializeObject(claimsInfoDic);
            if (!jsonValue.IsNullOrEmpty())
            {
                result = JsonConvert.DeserializeObject<T>(jsonValue);
            }
            return result;
        }

        /// <summary>
        /// 转换为声明集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userInfo">用户西西模型</param>
        /// <returns></returns>
        public static List<Claim> ToAccessTokenClaims<T>(this T userInfo) where T : class
        {
            var claims = new List<Claim>();
            if (userInfo == null)
            {
                return claims;
            }

            var propertyInfoList = userInfo.GetPropertyInfoList();
            if (!propertyInfoList.IsNotNull())
            {
                return claims;
            }

            foreach (var propertyInfo in propertyInfoList)
            {
                var propertyName = propertyInfo.Name;//属性名称
                var propertyValue = userInfo.GetPropertyValue(propertyName);//属性值
                var customAttribute = propertyInfo.GetCustomAttribute<JsonPropertyAttribute>();
                if (customAttribute != null)
                {
                    //绑定自定义属性名称
                    propertyName = customAttribute.PropertyName;
                }
                claims.Add(new Claim(propertyName, $"{propertyValue}"));
            }
            return claims;
        }

        /// <summary>
        /// 获取允许用户授权范围特性
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AllowAuthorizeAttribute GetAllowAuthorizeAttribute(this AuthorizationFilterContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;//获取当前Action和Controller的描述信息
            var methodAllowAuthorizeAttribute = actionDescriptor.MethodInfo.GetCustomAttribute<AllowAuthorizeAttribute>();//方法上的匿名函数特性
            var controllerAllowAuthorizeAttribute = actionDescriptor.ControllerTypeInfo.GetCustomAttribute<AllowAuthorizeAttribute>();//控制器上的匿名函数特性
            var allowAuthorizeAttribute = methodAllowAuthorizeAttribute ?? controllerAllowAuthorizeAttribute ?? new AllowAuthorizeAttribute();
            allowAuthorizeAttribute.VerifyStatus = allowAuthorizeAttribute.VerifyToken && allowAuthorizeAttribute.VerifyStatus;//修正状态验证的逻辑
            return allowAuthorizeAttribute;
        }
    }
}