using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Net;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 模型验证过滤器
    /// 具体的验证规则请参见命名空间：System.ComponentModel.DataAnnotations
    /// 微软官方地址：https://docs.microsoft.com/zh-cn/dotnet/api/system.componentmodel.dataannotations?view=net-5.0
    /// </summary>
    public class ModelValidAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 方法调用之前出发
        /// </summary>
        /// <param name="context">上下文</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var result = new ContractResult();
                var serialNo = context.HttpContext.Request.Headers.GetSerialNo();
                try
                {
                    var modelStateErrorDic = GetModelStateErrorDic(modelState);
                    var errorInfo = modelStateErrorDic.FirstOrDefault();
                    result.SetError(errorInfo.Value, serialNo);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                catch (Exception)
                {
                    result.SetError(GlobalMessageCode.PARAM_VALIDATE_FAIL, serialNo);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                }
                finally
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = (int)HttpStatusCode.OK,//返回状态码设置为200，表示成功
                        Content = JsonConvert.SerializeObject(result),
                        ContentType = "application/json;charset=utf-8",//设置返回格式
                    };
                }
            }
        }

        /// <summary>
        /// 获取模型验证信息
        /// </summary>
        /// <param name="modelState">模型状态</param>
        /// <returns></returns>
        private static Dictionary<string, Enum> GetModelStateErrorDic(ModelStateDictionary modelState)
        {
            var dataDic = new Dictionary<string, Enum>();
            var errorFieldsAndMsgList = modelState.Where(e => e.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });
            if (errorFieldsAndMsgList != null && errorFieldsAndMsgList.Any())
            {
                foreach (var errorFieldsAndMsg in errorFieldsAndMsgList)
                {
                    var fieldKey = errorFieldsAndMsg.Key;//获取键
                    var errorInfo = errorFieldsAndMsg.Errors.FirstOrDefault();
                    dataDic.Add(fieldKey, ToEnum(ModelValidExtensions.Type, errorInfo.ErrorMessage));
                }
            }
            return dataDic;
        }

        /// <summary>
        /// 枚举转换
        /// </summary>
        /// <param name="type">错误类型</param>
        /// <param name="enumNameOrValue">枚举名称或者枚举值</param>
        /// <returns></returns>
        private static Enum ToEnum(Type type, string enumNameOrValue)
        {
            //判断是否可以转换为整型
            if (!Enum.TryParse(type, enumNameOrValue, out object number))
            {
                string[] enumNames = type.GetEnumNames();
                string nameKey = enumNames.FirstOrDefault(name => name.Equals(enumNameOrValue, StringComparison.InvariantCultureIgnoreCase));
                if (!string.IsNullOrEmpty(nameKey))
                {
                    enumNameOrValue = nameKey;
                }
            }
            return (Enum)Enum.Parse(type, enumNameOrValue);
        }
    }
}