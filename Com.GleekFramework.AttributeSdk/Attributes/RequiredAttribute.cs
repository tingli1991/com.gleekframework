using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 必填验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequiredAttribute : ValidationAttribute
    {
        /// <summary>
        /// 验证失败时的错误消息
        /// </summary>
        /// <param name="errorMessage"></param>
        public RequiredAttribute(object errorMessage) : base() => ErrorMessage = errorMessage?.ToString();
    }
}