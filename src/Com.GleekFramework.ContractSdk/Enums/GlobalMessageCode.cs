using System.ComponentModel;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 全局消息错误码
    /// 【1到3位 表示系统】【4到6位 表示模块】【7到10位 表示具体错误标志】
    /// </summary>
    public enum GlobalMessageCode
    {
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        FAIL = -1,

        /// <summary>
        ///  成功
        /// </summary>
        [Description("成功")]
        SUCCESS = 200,

        /// <summary>
        /// 暂无数据
        /// </summary>
        [Description("暂无数据")]
        NO_DATA_AVAILABLE = 204,

        /// <summary>
        /// 未经授权
        /// </summary>
        [Description("未经授权")]
        UNAUTHORIZED = 401,

        /// <summary>
        /// 授权被拒绝
        /// </summary>
        [Description("授权被拒绝")]
        REJECTED_AUTHORIZED = 403,

        /// <summary>
        ///  未知错误
        /// </summary>
        [Description("未知错误")]
        ERROR = 500,

        /// <summary>
        /// 签名失败
        /// </summary>
        [Description("签名失败")]
        SIGN_ERROR = 1001001001,

        /// <summary>
        /// 操作频繁
        /// </summary>
        [Description("操作频繁")]
        FREQUENT_OPERATION = 1001001002,

        /// <summary>
        /// 非法请求
        /// </summary>
        [Description("非法请求")]
        ILLEGAL_REQUEST = 1001001003,

        /// <summary>
        /// 您的账号已被禁用
        /// </summary>
        [Description("您的账号已被禁用")]
        ACCOUNT_DISABLED = 1001001004,

        /// <summary>
        /// 参数必填
        /// </summary>
        [Description("参数必填")]
        PARAM_REQUIRED = 1001001005,

        /// <summary>
        /// 请求参数错误
        /// </summary>
        [Description("请求参数错误")]
        PARAM_ERROR = 1001001006,

        /// <summary>
        /// 参数验证失败
        /// </summary>
        [Description("参数验证失败")]
        PARAM_VALIDATE_FAIL = 1001001007,

        /// <summary>
        /// 超出[1,200]长度的区间限制
        /// </summary>
        [Description("超出[1,200]长度的区间限制")]
        EXCEED_PPER_LIMIT_200 = 1001001008,
    }
}