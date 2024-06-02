using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// 属性拓展
    /// </summary>
    internal static class PropertyExtensions
    {
        /// <summary>
        /// 获取主键名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static string GetPrimaryColumnName<T>(this Type type) where T : ITable
        {
            var propertyName = nameof(ITable.Id);//属性名称
            var keyPropertyInfo = type.GetPropertyInfo(propertyName);
            var keyColumnAttribute = keyPropertyInfo.GetCustomAttribute<ColumnAttribute>();
            return keyColumnAttribute?.Name ?? keyPropertyInfo.Name ?? "id";
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        public static string GetTableName<T>(this Type type) where T : ITable
        {
            var tableAttribute = ClassAttributeProvider.GetCustomAttribute<TableAttribute>(type);
            return tableAttribute?.Name ?? type.Name ?? "";
        }
    }
}