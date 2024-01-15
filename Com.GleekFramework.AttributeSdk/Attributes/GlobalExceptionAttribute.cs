using Com.GleekFramework.ContractSdk;
using Com.GleekFramework.HttpSdk;
using Com.GleekFramework.NLogSdk;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionAttribute : IAsyncExceptionFilter
    {
        /// <summary>
        /// 发生异常时调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            var result = new ContractResult();
            var headers = context.HttpContext.Request.Headers;
            var serialNo = headers.GetSerialNo();
            result.SetError(GlobalMessageCode.ERROR, serialNo);
            context.Result = new ContentResult()
            {
                StatusCode = (int)HttpStatusCode.OK,//返回状态码设置为200，表示成功
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json;charset=utf-8",//设置返回格式
            };
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            NLogProvider.Error($"{context.Exception}", serialNo, context.HttpContext.Request.Path);
            await Task.CompletedTask;
        }
    }
}