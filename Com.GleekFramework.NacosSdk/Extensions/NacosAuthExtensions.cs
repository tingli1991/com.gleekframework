using Com.GleekFramework.CommonSdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 登录授权拓展
    /// </summary>
    internal static partial class NacosAuthExtensions
    {
        /// <summary>
        /// token字段
        /// </summary>
        private static string Token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        private static DateTime? ExpireTime { get; set; }

        /// <summary>
        /// 随机因子
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// 注册服务实例
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWrod">密码</param>
        /// <returns></returns>
        public static async Task<string> GetAuthTokenAsync(this string baseUrl, string userName, string passWrod)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWrod))
            {
                //帐号密码不存在
                return "";
            }

            var difSeconds = Random.Next(5, 20);//误差秒数
            if (string.IsNullOrEmpty(Token) || !ExpireTime.HasValue || ExpireTime <= DateTime.Now.ToCstTime().AddSeconds(-difSeconds))
            {
                var tokenObject = await baseUrl.GetAuthTokenInfoAsync(userName, passWrod);//获取token
                if (!string.IsNullOrEmpty(tokenObject.Token))
                {
                    Token = tokenObject.Token;
                    if (tokenObject.ExpireSeconds > 0)
                    {
                        //更新下一次过期时间
                        ExpireTime = DateTime.Now.ToCstTime().AddSeconds(tokenObject.ExpireSeconds - difSeconds);
                    }
                }
            }
            return Token;
        }

        /// <summary>
        /// 注册服务实例
        /// </summary>
        /// <param name="baseUrl">基地址</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWrod">密码</param>
        /// <returns></returns>
        public static async Task<(string Token, int ExpireSeconds)> GetAuthTokenInfoAsync(this string baseUrl, string userName, string passWrod)
        {
            var paramters = new Dictionary<string, string>()
            {
                { "username", $"{userName??""}" },
                { "password", $"{passWrod ?? ""}" }
            };

            var responseContent = await paramters.PostAsync(RestConstant.GetAuthTokenUrl(baseUrl));
            var isSuccess = !string.IsNullOrEmpty(responseContent) && (responseContent.Contains("200") || responseContent.Contains("accessToken"));
            if (!isSuccess)
            {
                //调取接口异常，返回结果为空
                return ("", 0);
            }
            else
            {
                var dataToken = JsonConvert.DeserializeObject<dynamic>(responseContent);
                var token = dataToken.data ?? dataToken.accessToken ?? "";//返回token结果
                int expireSeconds = dataToken.tokenTtl > 0 ? dataToken.tokenTtl : 0;//过期时间
                return (token, expireSeconds);
            }
        }
    }
}