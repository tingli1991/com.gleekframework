using System.ComponentModel;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 授权模式
    /// </summary>
    public enum GrantType
    {
        /// <summary>
        /// 密码模式
        /// </summary>
        [Description("密码模式")]
        PASSWORD = 1,

        /// <summary>
        /// 授权码模式
        /// </summary>
        [Description("授权码模式")]
        AUTHORIZATION_CODE = 2,

        /// <summary>
        /// 客户端凭证模式
        /// </summary>
        [Description("客户端凭证模式")]
        CLIENT_CREDENTIALS = 3
    }
}