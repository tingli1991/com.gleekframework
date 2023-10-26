using Dapper;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// 列的类型映射
    /// </summary>
    internal class ColumnTypeMapper : FallbackTypeMapper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ColumnTypeMapper(Type classType) : base(new SqlMapper.ITypeMap[] { new CustomPropertyTypeMap(classType, (type, columnName) =>
        type.GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))), new DefaultTypeMap(classType) })
        {

        }
    }
}