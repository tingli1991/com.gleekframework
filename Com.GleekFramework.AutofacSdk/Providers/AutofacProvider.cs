using Autofac;
using System;

namespace Com.GleekFramework.AutofacSdk
{
    /// <summary>
    /// Autofac容器实现类
    /// </summary>
    public static partial class AutofacProvider
    {
        /// <summary>
        /// 容器
        /// </summary>
        public static IContainer Container { get; set; }

        /// <summary>
        /// Ioc容器
        /// </summary>
        public static IServiceProvider Services { get; set; }

        /// <summary>
        /// 获取接口服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return GetService<T>(typeof(T));
        }

        /// <summary>
        /// 获取接口服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>(Type serviceType)
        {
            return (T)GetService(serviceType);
        }

        /// <summary>
        /// 获取接口服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object GetService(Type serviceType)
        {
            return Services.GetService(serviceType);
        }
    }
}