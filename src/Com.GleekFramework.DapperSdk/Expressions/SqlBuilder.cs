using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.CommonSdk.Extensions;
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
            var columnIgnoreDic = Type.GetColumnIgnoreDic();//获取被忽略的列字典
            PropertyInfoList = Type.GetPropertyInfoList().WhereIf(columnIgnoreDic.IsNotEmpty(), e => !columnIgnoreDic.ContainsKey(e.Name));//实体属性列表
            KeyPropertyInfo = PropertyInfoList.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null);//主键属性
            ColumnMappingList = PropertyInfoList.ToDictionary(k => k.Name, v => v.GetCustomAttribute<ColumnAttribute>()?.Name ?? v.Name);//列的映射关系列表
        }

        /// <summary>
        /// 编译插入脚本
        /// </summary>
        /// <returns></returns>
        public string GenInsertSQL()
        {
            var propertieInfoList = PropertyInfoList.Where(e => !e.IsIncrementColumn());
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
        /// <returns></returns>
        public string GenUpdateSQL() => GenUpdateSQL(PropertyInfoList.GetKeyPropertyInfo());

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

            var setClauses = PropertyInfoList.Where(e => e.Name != propertyInfo.Name).Select(e => $"{ColumnMappingList[e.Name]} = @{e.Name}");
            return $"update {TableName} set {string.Join(",", setClauses)} where {GetKeyColumnName(propertyInfo)}=@{propertyInfo.Name}";
        }

        /// <summary>
        /// 生成更新SQL脚本
        /// </summary>
        /// <param name="fieldValues">需要更新的字典</param>
        /// <param name="primaryValue">主键值</param>
        /// <returns></returns>
        public (string UpdateSQL, Dictionary<string, object> UpdateFieldValues) GenUpdateSQL(Dictionary<string, object> fieldValues, object primaryValue)
        {
            var type = typeof(E);
            var propertyInfoList = type.GetPropertyInfoList();
            var keyPropertyInfo = propertyInfoList.GetKeyPropertyInfo();
            if (keyPropertyInfo == null)
            {
                throw new ArgumentNullException(nameof(PropertyInfo));
            }

            var primaryKeyValue = new KeyValuePair<string, object>(keyPropertyInfo.Name, primaryValue);
            var primaryKeyFieldAndValue = propertyInfoList.GetColumnFieldAndValues(primaryKeyValue);
            if (string.IsNullOrEmpty(primaryKeyFieldAndValue.Key))
            {
                throw new ArgumentNullException(nameof(primaryKeyValue));
            }

            type.ConversionInterfaceField(fieldValues);
            var updateFieldValue = new Dictionary<string, object> { { primaryKeyFieldAndValue.Key, primaryKeyFieldAndValue.Value } };
            foreach (var fieldValue in fieldValues)
            {
                var columnFieldAndValue = propertyInfoList.GetColumnFieldAndValues(fieldValue);
                if (string.IsNullOrEmpty(columnFieldAndValue.Key))
                {
                    continue;
                }
                updateFieldValue.Add(columnFieldAndValue.Key, columnFieldAndValue.Value);
            }
            var setClauses = propertyInfoList.Where(e => e.Name != keyPropertyInfo.Name && updateFieldValue.Keys.Contains(e.Name)).Select(e => $"{ColumnMappingList[e.Name]} = @{e.Name}");
            var sql = $"update {TableName} set {string.Join(",", setClauses)} where {GetKeyColumnName(keyPropertyInfo)}=@{keyPropertyInfo.Name}";
            return (sql, updateFieldValue);
        }

        /// <summary>
        /// 生成更新SQL脚本
        /// </summary>
        /// <param name="fieldValueList">需要更新的字典集合</param>
        /// <returns></returns>
        public (string UpdateSQL, IEnumerable<Dictionary<string, object>> UpdateFieldValues) GenUpdateSQL(Dictionary<object, Dictionary<string, object>> fieldValueList)
        {
            var type = typeof(E);
            var propertyInfoList = type.GetPropertyInfoList();
            var keyPropertyInfo = propertyInfoList.GetKeyPropertyInfo();
            if (keyPropertyInfo == null)
            {
                throw new ArgumentNullException(nameof(PropertyInfo));
            }

            var updateFieldValues = new List<Dictionary<string, object>>();
            foreach (var e in fieldValueList)
            {
                var primaryValue = e.Key;//主键值
                var fieldValues = e.Value;//更新字段值
                var primaryKeyValue = new KeyValuePair<string, object>(keyPropertyInfo.Name, primaryValue);
                var primaryKeyFieldAndValue = propertyInfoList.GetColumnFieldAndValues(primaryKeyValue);
                if (string.IsNullOrEmpty(primaryKeyFieldAndValue.Key))
                {
                    throw new ArgumentNullException(nameof(primaryKeyValue));
                }

                type.ConversionInterfaceField(fieldValues);
                var updateFieldValue = new Dictionary<string, object> { { primaryKeyFieldAndValue.Key, primaryKeyFieldAndValue.Value } };
                foreach (var fieldValue in fieldValues)
                {
                    var columnFieldAndValue = propertyInfoList.GetColumnFieldAndValues(fieldValue);
                    if (string.IsNullOrEmpty(columnFieldAndValue.Key))
                    {
                        continue;
                    }
                    updateFieldValue.Add(columnFieldAndValue.Key, columnFieldAndValue.Value);
                }
                updateFieldValues.Add(updateFieldValue);
            }
            var firstFieldValue = updateFieldValues.FirstOrDefault();
            var setClauses = propertyInfoList.Where(e => e.Name != keyPropertyInfo.Name && firstFieldValue.Keys.Contains(e.Name)).Select(e => $"{ColumnMappingList[e.Name]} = @{e.Name}");
            var sql = $"update {TableName} set {string.Join(",", setClauses)} where {GetKeyColumnName(keyPropertyInfo)}=@{keyPropertyInfo.Name}";
            return (sql, updateFieldValues);
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
            var databaseGeneratedAttribute = KeyPropertyInfo?.GetCustomAttribute<DatabaseGeneratedAttribute>();
            if (databaseGeneratedAttribute == null || databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
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
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取主键列名
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private string GetKeyColumnName(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return "";
            }

            var columnName = ColumnMappingList.ContainsKey(propertyInfo.Name) ? ColumnMappingList[propertyInfo.Name] : "";
            if (string.IsNullOrEmpty(columnName))
            {
                var columnAttribute = propertyInfo.GetCustomAttribute<ColumnAttribute>();
                columnName = columnAttribute?.Name ?? propertyInfo.Name;
            }
            return columnName;
        }
    }
}