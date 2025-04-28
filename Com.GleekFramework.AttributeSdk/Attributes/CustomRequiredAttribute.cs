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
        /// <param name="errorEnum"></param>
        public CustomRequiredAttribute(Enum errorEnum) : base() => ErrorMessage = nameof(errorEnum);
    }
}