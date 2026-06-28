using System;

namespace Com.GleekFramework.AutofacSdk
{
    /// <summary>
    /// 服务实现拓展类
    /// </summary>
    public static partial class AutofacProviderExtensions
    {
        /// <summary>
        /// 获取接口服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetService<T>(this Type type)
        {
            return AutofacProvider.GetService<T>(type);
        }
    }
}