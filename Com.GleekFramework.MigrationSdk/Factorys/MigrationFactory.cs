using Com.GleekFramework.AutofacSdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本变更工厂
    /// </summary>
    public static partial class MigrationFactory
    {
        /// <summary>
        /// 并发所
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 升级服务
        /// </summary>
        private static readonly Dictionary<Type, IEnumerable<IUpgration>> ServiceList = new Dictionary<Type, IEnumerable<IUpgration>>();

        /// <summary>
        /// 服务列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetServiceList<T>()
        {
            var serviceList = GetServiceCacheList(typeof(T));
            if (serviceList == null || !serviceList.Any())
            {
                return new List<T>();
            }
            return serviceList.Select(e => (T)e);
        }

        /// <summary>
        /// 获取消息类型列表
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <returns></returns>
        private static IEnumerable<IUpgration> GetServiceCacheList(Type type)
        {
            if (!ServiceList.ContainsKey(type))
            {
                lock (@lock)
                {
                    if (!ServiceList.ContainsKey(type))
                    {
                        var messageHandlerList = type.GetServiceList<IUpgration>();
                        if (messageHandlerList == null || !messageHandlerList.Any())
                        {
                            return new List<IUpgration>();
                        }
                        else
                        {
                            //获取或者新增
                            ServiceList.Add(type, messageHandlerList);
                        }
                    }
                }
            }
            return ServiceList[type];
        }
    }
}