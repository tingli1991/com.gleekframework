using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.ConsumerSdk
{
    /// <summary>
    /// 消息处理工厂类
    /// </summary>
    public static partial class HandlerFactory
    {
        /// <summary>
        /// 并发所
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 消息处理类字典
        /// </summary>
        private static readonly Dictionary<Type, IEnumerable<IHandler>> MessageHandlerServiceList = new Dictionary<Type, IEnumerable<IHandler>>();

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="actionKey">消息类型</param>
        /// <returns></returns>
        public static T GetInstance<T>(string actionKey) where T : IHandler
        {
            var messageHandlerServiceList = GetHandlerServiceList(typeof(T));
            return (T)messageHandlerServiceList.FirstOrDefault(e => e.ActionKey.EqualsActionKey(actionKey));
        }

        /// <summary>
        /// 获取消息类型列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> GetHandlerServiceList<T>()
        {
            var messageHandlerServiceList = GetHandlerServiceList(typeof(T));
            if (messageHandlerServiceList.IsNullOrEmpty())
            {
                return new List<T>();
            }
            return messageHandlerServiceList.Select(e => (T)e);
        }

        /// <summary>
        /// 获取事件服务列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TopicServiceModel<T>> GetTopicServiceList<T>() where T : ITopicHandler
        {
            var messageHandlerServiceList = GetHandlerServiceList<T>();
            if (messageHandlerServiceList.IsNullOrEmpty())
            {
                return new List<TopicServiceModel<T>>();
            }

            return messageHandlerServiceList
                .GroupBy(e => e.Topic)
                .Select(e => new TopicServiceModel<T>()
                {
                    Topic = e.Key,
                    ServiceList = e.OrderBy(p => p.Order).ToList()
                });
        }

        /// <summary>
        /// 获取消息类型列表
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <returns></returns>
        private static IEnumerable<IHandler> GetHandlerServiceList(Type type)
        {
            if (!MessageHandlerServiceList.ContainsKey(type))
            {
                lock (@lock)
                {
                    if (!MessageHandlerServiceList.ContainsKey(type))
                    {
                        var messageHandlerList = type.GetServiceList<IHandler>();
                        if (messageHandlerList.IsNullOrEmpty())
                        {
                            return new List<IHandler>();
                        }
                        else
                        {
                            //获取或者新增
                            MessageHandlerServiceList.Add(type, messageHandlerList);
                        }
                    }
                }
            }
            return MessageHandlerServiceList[type];
        }
    }
}