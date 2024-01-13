using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.GrpcSdk
{
    /// <summary>
    /// 主机拓展类
    /// </summary>
    public static partial class HostingExtensions
    {
        /// <summary>
        /// 获取Grpc服务列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IBaseGrpcService> GetGrpcServiceList()
        {
            Type imessageType = typeof(IBaseGrpcService);
            var assemblyList = AssemblyProvider.GetAssemblyList(imessageType);
            IEnumerable<Type> messageAssemblyTypes = assemblyList.SelectMany(e => e.GetTypes());
            IEnumerable<Type> imessageTypeList = messageAssemblyTypes.Where(type => type.IsClass && type != imessageType && type.GetInterfaces().Any(e => imessageType.Name.Equals(e.Name)));
            return imessageTypeList.Select(type => type.GetService<IBaseGrpcService>()).Where(type => type != null);
        }

        /// <summary>
        /// 注入GRPC
        /// </summary>
        /// <param name="endpoints"></param>
        /// <returns></returns>
        public static GrpcServiceEndpointConventionBuilder MapGrpcServices(this IEndpointRouteBuilder endpoints)
        {
            var serviceList = GetGrpcServiceList();
            return endpoints.MapGrpcServices(serviceList);
        }

        /// <summary>
        /// 注入GRPC
        /// </summary>
        /// <param name="endpoints"></param>
        /// <param name="grpcServiceList">服务列表</param>
        /// <returns></returns>
        public static GrpcServiceEndpointConventionBuilder MapGrpcServices(this IEndpointRouteBuilder endpoints, IEnumerable<IBaseGrpcService> grpcServiceList)
        {
            if (grpcServiceList.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(grpcServiceList));
            }


            GrpcServiceEndpointConventionBuilder builder = null;
            var mt = typeof(GrpcEndpointRouteBuilderExtensions).GetMethod("MapGrpcService");
            foreach (var grpcService in grpcServiceList)
            {
                var mt2 = mt.MakeGenericMethod(grpcService.GetType());
                builder = (GrpcServiceEndpointConventionBuilder)mt2.Invoke(null, new object[] { endpoints });
            }
            return builder;
        }
    }
}