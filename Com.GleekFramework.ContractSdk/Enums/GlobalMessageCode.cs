using System.ComponentModel;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 全局消息错误码
    /// </summary>
    public enum GlobalMessageCode
    {
        /// <summary>
        /// 业务处理失败
        /// </summary>
        [Description("业务处理失败")]
        FAIL = -1,

        /// <summary>
        ///  业务处理成功
        /// </summary>
        [Description("业务处理成功")]
        SUCCESS = 200,

        /// <summary>
        /// 请求参数错误
        /// </summary>
        [Description("请求参数错误")]
        PARAM_ERROR = 100001,

        /// <summary>
        /// 签名失败
        /// </summary>
        [Description("签名失败")]
        SIGN_ERROR = 100002,

        /// <summary>
        /// 参数验证失败
        /// </summary>
        [Description("参数验证失败")]
        PARAM_VALIDATE_FAIL = 100003,

        /// <summary>
        /// 参数必填
        /// </summary>
        [Description("参数必填")]
        PARAM_REQUIRED = 100004,

        /// <summary>
        /// 超出[1,200]长度的区间限制
        /// </summary>
        [Description("超出[1,200]长度的区间限制")]
        EXCEED_PPER_LIMIT_200 = 100005,

        /// <summary>
        ///  未知错误(系统异常)
        /// </summary>
        [Description("未知错误")]
        ERROR = 999999,
    }
}