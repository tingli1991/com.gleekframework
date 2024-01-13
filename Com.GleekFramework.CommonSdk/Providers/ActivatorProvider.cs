using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 实例激活实现类
    /// </summary>
    public static class ActivatorProvider
    {
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 对象类型缓存列表
        /// </summary>
        private static readonly Dictionary<Type, object> CacheList = new Dictionary<Type, object>();

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="typeList">对象类型列表</param>
        /// <returns></returns>
        public static IEnumerable<object> CreateInstances(IEnumerable<Type> typeList)
        {
            if (typeList.IsNullOrEmpty())
            {
                return new List<object>();
            }
            return typeList.Select(type => CreateInstance(type));
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static T CreateInstance<T>(Type type) where T : class, new()
        {
            var instance = CreateInstance(type);
            if (instance == null)
            {
                return default;
            }
            return (T)instance;
        }

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static object CreateInstance(Type type)
        {
            if (!CacheList.ContainsKey(type))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(type))
                    {
                        var instance = GetInstance(type);//当前对象实例
                        CacheList.Add(type, instance);
                    }
                }
            }
            return CacheList[type];
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        private static object GetInstance(Type type)
        {
            object instance = null;
            try
            {
                instance = Activator.CreateInstance(type);
            }
            catch (Exception)
            {

            }
            return instance;
        }
    }
}