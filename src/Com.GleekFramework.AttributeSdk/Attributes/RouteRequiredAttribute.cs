using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>  
    /// 路由必填验证
    /// </summary>  
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class RouteRequiredAttribute : RequiredAttribute, IBindingSourceMetadata, IModelNameProvider, IFromRouteMetadata
    {
        /// <summary>  
        /// 路由名称  
        /// </summary>  
        public string Name { get; set; }

        /// <summary>  
        /// 绑定资源  
        /// </summary>  
        public BindingSource BindingSource => BindingSource.Path;

        /// <summary>  
        /// 构造函数
        /// </summary>
        /// <param name="errorMessage">错误消息</param>  
        public RouteRequiredAttribute(object errorMessage)
        {
            ErrorMessage = errorMessage?.ToString();
        }

        /// <summary>  
        /// 构造函数
        /// </summary>  
        /// <param name="name">路由名称</param>  
        /// <param name="errorMessage">错误消息</param>  
        public RouteRequiredAttribute(string name, object errorMessage)
        {
            Name = name;
            ErrorMessage = errorMessage?.ToString();
        }
    }
}
