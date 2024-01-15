using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 主机拓展类
    /// </summary>
    public static partial class CommonHostExtensions
    {
        /// <summary>
        /// 注册主机启动完成时间
        /// </summary>
        /// <param name="host">主机信息</param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IHostApplicationLifetime RegisterApplicationStarted(this IHost host, Action callback)
        {
            var lifeTime = host.Services.GetRequiredService<IHostApplicationLifetime>();
            lifeTime.ApplicationStarted.Register(callback);
            return lifeTime;
        }

        /// <summary>
        /// 注册主机启动完成时间
        /// </summary>
        /// <param name="app">主机信息</param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IHostApplicationLifetime RegisterApplicationStarted(this IApplicationBuilder app, Action callback)
        {
            var lifeTime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            lifeTime.ApplicationStarted.Register(callback);
            return lifeTime;
        }

        /// <summary>
        /// 注册主机停止中事件
        /// </summary>
        /// <param name="lifeTime"></param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IHostApplicationLifetime RegisterApplicationStopping(this IHostApplicationLifetime lifeTime, Action callback)
        {
            lifeTime.ApplicationStopping.Register(callback);
            return lifeTime;
        }

        /// <summary>
        /// 注册主机停止完成事件
        /// </summary>
        /// <param name="lifeTime"></param>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public static IHostApplicationLifetime RegisterApplicationStopped(this IHostApplicationLifetime lifeTime, Action callback)
        {
            lifeTime.ApplicationStopped.Register(callback);
            return lifeTime;
        }
    }
}