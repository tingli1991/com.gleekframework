using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 必填验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CustomRequiredAttribute : ValidationAttribute
    {
        /// <summary>
        /// 验证失败时的错误消息
        /// </summary>
        /// <param name="errorMessage"></param>
        public CustomRequiredAttribute(object errorMessage) : base() => ErrorMessage = errorMessage?.ToString();
    }
}