using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// SQL构建器
    /// </summary>
    /// <typeparam name="E">新实体类型</typeparam>
    public class SqlBuilder<E>
    {
        /// <summary>
        /// 类型
        /// </summary>
        private readonly Type Type;

        /// <summary>
        /// 数据库名称
        /// </summary>
        private readonly string TableName;

        /// <summary>
        /// 主键属性信息
        /// </summary>
        public PropertyInfo KeyPropertyInfo;

        /// <summary>
        /// 属性列表
        /// </summary>
        private readonly IEnumerable<PropertyInfo> PropertyInfoList;

        /// <summary>
        /// 属性的映射关系列表
        /// </summary>
        private readonly Dictionary<string, string> ColumnMappingList = [];

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlBuilder()
        {
            Type = typeof(E);//初始化实体类型
            TableName = Type.GetTableName();//数据库名称
            PropertyInfoList = Type.GetPropertyInfoList();//实体属性列表
            KeyPropertyInfo = PropertyInfoList.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null);//主键属性
            ColumnMappingList = PropertyInfoList.ToDictionary(k => k.Name, v => v.GetCustomAttribute<ColumnAttribute>()?.Name ?? v.Name);//列的映射关系列表
        }

        /// <summary>
        /// 编译插入脚本
        /// </summary>
        /// <returns></returns>
        public string GenInsertSQL()
        {
            var propertieInfoList = PropertyInfoList
               .Where(e => e.GetCustomAttribute<KeyAttribute>() == null
               && e.GetCustomAttribute<DatabaseGeneratedAttribute>()?.DatabaseGeneratedOption != DatabaseGeneratedOption.Identity);

            var parameterList = propertieInfoList.Select(e => "@" + e.Name);//新增的参数列表
            var columns = propertieInfoList.Select(e => ColumnMappingList[e.Name]);//新增的参数列
            return $"insert into {TableName} ({string.Join(",", columns)}) values ({string.Join(",", parameterList)});";
        }

        /// <summary>
        /// 生成查询SQL脚本
        /// </summary>
        /// <returns></returns>
        public (string SQL, string PropertyName) GenQuerySQL()
        {
            var propertyInfo = PropertyInfoList.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null)
                ?? throw new ArgumentNullException(nameof(PropertyInfo));

            var columns = PropertyInfoList.Select(e => $"{ColumnMappingList[e.Name]}");
            var sql = $"select {string.Join(",", columns)} from {TableName} where {ColumnMappingList[propertyInfo.Name]}=@{propertyInfo.Name}";
            return (sql, propertyInfo.Name);
        }

        /// <summary>
        /// 生成更新SQL脚本
        /// </summary>
        /// <typeparam name="T">返回实体</typeparam>
        /// <param name="entity">更新实体</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public (string UpdateSQL, Dictionary<string, object> UpdateParamters) GenUpdateSQL<T>(E entity, long id) where T : class
        {
            var propertyInfo = typeof(T).GetPropertyInfoList().FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null);
            var updateSQL = GenUpdateSQL(propertyInfo);//更新脚本
            var paramters = entity.GetType().GetPropertyInfoList().ToDictionary(k => k.Name, v => entity.GetPropertyValue(v.Name));
            if (!paramters.ContainsKey(propertyInfo.Name))
            {
                paramters.Add(propertyInfo.Name, id);
            }
            else
            {
                paramters[propertyInfo.Name] = id;
            }
            return (updateSQL, paramters);
        }

        /// <summary>
        /// 生成更新SQL脚本
        /// </summary>
        /// <returns></returns>
        public string GenUpdateSQL()
        {
            var propertyInfo = PropertyInfoList.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null);
            return GenUpdateSQL(propertyInfo);
        }

        /// <summary>
        /// 生成更新SQL脚本
        /// </summary>
        /// <returns></returns>
        public string GenUpdateSQL(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(PropertyInfo));
            }

            var setClauses = PropertyInfoList
                .Where(e => e.Name != propertyInfo.Name)
                .Select(e => $"{ColumnMappingList[e.Name]} = @{e.Name}");

            return $"update {TableName} set {string.Join(",", setClauses)} where {propertyInfo.Name}=@{propertyInfo.Name}";
        }

        /// <summary>
        /// 生成删除SQL脚本
        /// </summary>
        /// <returns></returns>
        public string GenDeleteSQL()
        {
            var propertyInfo = PropertyInfoList.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null)
                ?? throw new ArgumentNullException(nameof(PropertyInfo));

            var setClauses = PropertyInfoList
                .Where(e => e.Name != propertyInfo.Name)
                .Select(e => $"{ColumnMappingList[e.Name]} = @{e.Name}");

            return $"delete from {TableName} where {ColumnMappingList[propertyInfo.Name]}=@{propertyInfo.Name};";
        }

        /// <summary>
        /// 生成获取主键Id返回值的查询脚本
        /// </summary>
        /// <param name="databaseType">数据库类型</param>
        /// <returns></returns>
        public string GetIdentitySQL(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.MsSQL:
                    return $"SELECT SCOPE_IDENTITY();";
                case DatabaseType.MySQL:
                    return $"SELECT LAST_INSERT_ID();";
                case DatabaseType.PgSQL:
                    var propertyInfo = PropertyInfoList.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null);
                    return $"RETURNING {ColumnMappingList[propertyInfo.Name]};";
                case DatabaseType.SQLite:
                    return $"SELECT last_insert_rowid();";
                default:
                    throw new Exception($"{databaseType} 暂不支持读取的元素数量");
            }
        }
    }
}