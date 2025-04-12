using Com.GleekFramework.CommonSdk;
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
        /// 获取主键字段名称
        /// </summary>
        /// <returns></returns>
        public static string GetPrimaryName<T>(this T source) where T : class
        {
            var type = source.GetType();
            var propertyInfoList = type.GetPropertyInfoList();
            var primaryPropertyInfo = propertyInfoList.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null);//主键属性
            return primaryPropertyInfo?.Name ?? primaryPropertyInfo.Name ?? "id";
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        public static string GetTableName<T>(this T source) where T : class
        {
            var type = source.GetType();
            var tableAttribute = ClassAttributeProvider.GetCustomAttribute<TableAttribute>(type);
            return tableAttribute?.Name ?? type.Name ?? "";
        }
    }
}