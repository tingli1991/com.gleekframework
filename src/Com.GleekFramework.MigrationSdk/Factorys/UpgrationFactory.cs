using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 版本升级工厂类
    /// </summary>
    public static partial class UpgrationFactory
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
            if (serviceList.IsNullOrEmpty())
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
                        IEnumerable<IUpgration> messageHandlerList = new List<IUpgration>();
                        var assemblyList = AssemblyProvider.GetAssemblyList(type);//类型列表
                        if (assemblyList.IsNullOrEmpty())
                        {
                            return new List<IUpgration>();
                        }
                        else
                        {
                            foreach (var assembly in assemblyList)
                            {
                                var assembleTypeList = AssemblyTypeProvider.GetTypeList(assembly);
                                if (assembleTypeList.IsNullOrEmpty())
                                {
                                    continue;
                                }
                                else
                                {
                                    foreach (var assembleType in assembleTypeList)
                                    {
                                        if (assembleType == type || !assembleType.IsClass || string.IsNullOrEmpty(assembleType.Name) || string.IsNullOrEmpty(assembleType.Namespace))
                                        {
                                            continue;
                                        }

                                        if (!type.IsAssignableFrom(assembleType))
                                        {
                                            continue;
                                        }

                                        //当前对象实例
                                        var instance = ActivatorProvider.CreateInstance(assembleType);
                                        if (instance == null)
                                        {
                                            continue;
                                        }
                                        messageHandlerList = messageHandlerList.Add((IUpgration)instance);
                                    }
                                }
                            }
                        }
                        ServiceList.Add(type, messageHandlerList);
                    }
                }
            }
            return ServiceList[type];
        }
    }
}