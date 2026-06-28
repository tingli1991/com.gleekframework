using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>  
    /// Query必填验证
    /// </summary>  
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class QueryRequiredAttribute : RequiredAttribute, IBindingSourceMetadata, IModelNameProvider, IFromQueryMetadata
    {
        /// <summary>  
        /// Query名称
        /// </summary>  
        public string Name { get; set; }

        /// <summary>  
        /// 绑定资源  
        /// </summary>  
        public BindingSource BindingSource => BindingSource.Query;

        /// <summary>  
        /// 构造函数  
        /// </summary>
        /// <param name="errorMessage">错误消息</param>  
        public QueryRequiredAttribute(object errorMessage) : base()
        {
            ErrorMessage = errorMessage?.ToString();
        }

        /// <summary>  
        /// 构造函数  
        /// </summary>  
        /// <param name="name">Query名称</param>  
        /// <param name="errorMessage">错误消息</param>  
        public QueryRequiredAttribute(string name, object errorMessage) : base()
        {
            Name = name;
            ErrorMessage = errorMessage?.ToString();
        }
    }
}
