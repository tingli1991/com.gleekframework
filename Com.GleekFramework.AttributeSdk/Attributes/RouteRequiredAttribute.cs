using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>  
    /// 路由必填验证
    /// </summary>  
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class RouteRequiredAttribute : ValidationAttribute, IBindingSourceMetadata, IModelNameProvider, IFromRouteMetadata
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
        /// <param name="errorMessage"></param>  
        public RouteRequiredAttribute(object errorMessage) : base()
        {
            ErrorMessage = errorMessage?.ToString();
        }

        /// <summary>  
        /// 验证失败时的错误消息
        /// </summary>  
        /// <param name="name"></param>  
        /// <param name="errorMessage"></param>  
        public RouteRequiredAttribute(string name, object errorMessage) : base()
        {
            Name = name;
            ErrorMessage = errorMessage?.ToString();
        }
    }
}
