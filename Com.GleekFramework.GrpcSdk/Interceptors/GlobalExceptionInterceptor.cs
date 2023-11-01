using Com.GleekFramework.NLogSdk;
using Grpc.Core;
using Grpc.Core.Interceptors;
using System;
using System.Threading.Tasks;

namespace Com.GleekFramework.GrpcSdk
{
    /// <summary>
    /// 全局异常捕获类
    /// </summary>
    public class GlobalExceptionInterceptor : Interceptor
    {
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, 
            ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                var errorCode = StatusCode.Internal;//错误码
                NLogProvider.Error($"【系统异常】请求参数：{request}，错误信息：{ex}");
                throw new RpcException(new Status(errorCode, ex.Message));
            }
        }
    }
}