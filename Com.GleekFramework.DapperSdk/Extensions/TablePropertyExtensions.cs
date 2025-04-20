using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// 表的属性拓展
    /// </summary>
    public static class TablePropertyExtensions
    {
        /// <summary>
        /// 获取所有被忽略的列字典
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetColumnIgnoreDic(this Type type)
        {
            var propertyInfoList = type.GetPropertyInfoList();
            var ignorgePropertyInfoList = propertyInfoList.Where(e => e.GetCustomAttribute<ColumnIgnoreAttribute>() != null);
            if (ignorgePropertyInfoList.IsNotEmpty())
            {
                return ignorgePropertyInfoList.ToDictionary(k => k.Name, v => v.GetCustomAttribute<ColumnAttribute>()?.Name ?? v.Name);
            }
            return [];
        }

        /// <summary>
        /// 获取指定类型的所有列字典
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllColumnDic(this Type type)
        {
            var propertyInfoList = type.GetPropertyInfoList();
            return propertyInfoList.ToDictionary(k => k.Name, v => v.GetCustomAttribute<ColumnAttribute>()?.Name ?? v.Name);
        }

        /// <summary>
        /// 获取主键字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetPrimaryName(this Type type)
        {
            var propertyInfoList = type.GetPropertyInfoList();
            var primaryPropertyInfo = propertyInfoList.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null);//主键属性
            return primaryPropertyInfo?.Name ?? primaryPropertyInfo.Name ?? "id";
        }

        /// <summary>
        /// 获取主键字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetPrimaryName<T>(this T source) where T : class
        {
            return source.GetType().GetPrimaryName();
        }

        /// <summary>
        /// 实际应根据自定义属性获取表名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetTableName(this Type type)
        {
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            return tableAttribute?.Name ?? type.Name ?? "";
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        public static string GetTableName<T>(this T source) where T : class
        {
            return source.GetType().GetTableName();
        }
    }
}