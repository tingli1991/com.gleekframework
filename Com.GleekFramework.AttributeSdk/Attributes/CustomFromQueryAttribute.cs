using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 从查询字符串中获取参数，并且该参数为必填项
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class CustomFromQueryAttribute : ValidationAttribute, IBindingSourceMetadata, IModelNameProvider, IFromQueryMetadata
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 绑定资源
        /// </summary>
        public BindingSource BindingSource { get; }

        /// <summary>
        /// 验证失败时的错误消息
        /// </summary>
        /// <param name="errorEnum"></param>
        public CustomFromQueryAttribute(Enum errorEnum) : base() => ErrorMessage = nameof(errorEnum);
    }
}
