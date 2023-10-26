using Com.GleekFramework.ContractSdk;
using System.Threading.Tasks;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 消费者具体执行的实现
    /// </summary>
    public static partial class CoustomExecuteProvider
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="handler">对象实例</param>
        /// <param name="messageBody">消息内容</param>
        /// <returns></returns>
        public static async Task<ContractResult> ExecuteAsync(IHandler handler, MessageBody messageBody)
        {
            if (handler == null) return new ContractResult();
            var customAuthorizeAttributeList = handler.GetCoustomAttributeList<CustomAuthorizeAttribute>();//授权特性
            var customAuthorizationContext = CustomAuthorizationContext.CreateCustomAuthorizationContext(handler, messageBody);//授权的上下文
            var authorizeRrsponse = await customAuthorizeAttributeList.OnAuthorizationAsync(customAuthorizationContext);//调用授权的自定义函数
            if (authorizeRrsponse != null && !authorizeRrsponse.IsSuceccful())
            {
                //授权失败，直接返回授权结果
                return authorizeRrsponse;
            }
            else
            {
                var coustomAttributeList = handler.GetCoustomAttributeList<CustomActionAttribute>();//行为特性
                var customActionExecutingContext = CustomActionExecutingContext.CreateActionExecutingContext(handler, messageBody);//行为特性行为之前执行上下文
                var coustomResponse = await coustomAttributeList.OnActionExecutingAsync(customActionExecutingContext);//行为方法调用之前执行
                if (coustomResponse != null && !coustomResponse.IsSuceccful())
                {
                    //行为调用之前执行失败，直接返回授权结果
                    return coustomResponse;
                }
                else
                {
                    var executeResponse = await handler.ExecuteAsync(messageBody);//执行具体的处理方法
                    if (executeResponse != null && !executeResponse.IsSuceccful())
                    {
                        //执行具体的处理方法返回失败,返回对应的结果
                        return executeResponse;
                    }
                    else
                    {
                        //执行方法调用之后的结果
                        var customActionExecutedContext = CustomActionExecutedContext.CreateCustomActionExecutedContext(customActionExecutingContext, executeResponse);
                        var actionExecutedResponse = await coustomAttributeList.OnActionExecutedAsync(customActionExecutedContext);
                        if (actionExecutedResponse != null && !actionExecutedResponse.IsSuceccful())
                        {
                            //执行具体的处理方法返回失败,返回对应的结果
                            return actionExecutedResponse;
                        }
                        else
                        {
                            //正常返回执行方法的结果
                            return executeResponse;
                        }
                    }
                }
            }
        }
    }
}