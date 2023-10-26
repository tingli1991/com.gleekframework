using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 字段实现类
    /// </summary>
    public static partial class FieldProvider
    {
        /// <summary>
        /// 对象所
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 缓存列表
        /// </summary>
        private static readonly Dictionary<Type, IEnumerable<FieldInfo>> CacheList = new Dictionary<Type, IEnumerable<FieldInfo>>();

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static FieldInfo GetField(Type type, string name)
        {
            var fieldList = GetFieldList(type);
            return fieldList.FirstOrDefault(e => e.Name == name);
        }

        /// <summary>
        /// 获取字段列表
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetFieldList(Type type)
        {
            if (!CacheList.ContainsKey(type))
            {
                lock (@lock)
                {
                    if (!CacheList.ContainsKey(type))
                    {
                        var fieldList = type.GetFields();
                        CacheList.Add(type, fieldList);
                    }
                }
            }
            return CacheList[type];
        }
    }
}