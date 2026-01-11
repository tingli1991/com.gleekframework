using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ContractSdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// Table实体拓展方法
    /// </summary>
    public static class TableEntityExtensions
    {
        /// <summary>
        /// 获取数据库的字段以及字段值
        /// </summary>
        /// <param name="propertyInfoList">属性集合</param>
        /// <param name="fieldValue">字段值字典</param>
        /// <returns></returns>
        public static KeyValuePair<string, object> GetColumnFieldAndValues(this IEnumerable<PropertyInfo> propertyInfoList, KeyValuePair<string, object> fieldValue)
        {
            var keyValue = new KeyValuePair<string, object>();
            if (propertyInfoList.IsNullOrEmpty() || string.IsNullOrEmpty(fieldValue.Key))
            {
                return keyValue;
            }

            var propertyName = fieldValue.Key;//属性名称
            var propertyValue = fieldValue.Value;//属性值
            var propertyInfo = propertyInfoList.FirstOrDefault(e => e.Name == propertyName) ?? propertyInfoList.FirstOrDefault(e =>
            {
                var isSuccess = false;
                var columnAttribute = e.GetCustomAttribute<ColumnAttribute>();
                if (columnAttribute != null)
                {
                    isSuccess = columnAttribute.Name == propertyName;
                }

                if (!isSuccess)
                {
                    var jsonPropertyAttribute = e.GetCustomAttribute<JsonPropertyAttribute>();
                    if (jsonPropertyAttribute != null)
                    {
                        isSuccess = jsonPropertyAttribute.PropertyName == propertyName;
                    }
                }

                if (!isSuccess)
                {
                    var jsonPropertyNameAttribute = e.GetCustomAttribute<JsonPropertyNameAttribute>();
                    if (jsonPropertyNameAttribute != null)
                    {
                        isSuccess = jsonPropertyNameAttribute.Name == propertyName;
                    }
                }
                return isSuccess;
            });

            if (propertyInfo == null)
            {
                return keyValue;
            }
            return new KeyValuePair<string, object>(propertyInfo.Name, fieldValue.Value);
        }

        /// <summary>
        /// 获取主键属性信息
        /// </summary>
        /// <param name="propertyInfoList">属性集合</param>
        /// <returns></returns>
        public static PropertyInfo GetKeyPropertyInfo(this IEnumerable<PropertyInfo> propertyInfoList)
        {
            if (propertyInfoList.IsNullOrEmpty())
            {
                return null;
            }
            return propertyInfoList.FirstOrDefault(e => e.GetCustomAttribute<KeyAttribute>() != null);
        }

        /// <summary>
        /// 是否为自增数据库字段
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns></returns>
        public static bool IsIncrementColumn(this PropertyInfo propertyInfo)
        {
            var isSuccess = false;
            if (propertyInfo == null)
            {
                return isSuccess;
            }

            var keyAttribute = propertyInfo.GetCustomAttribute<KeyAttribute>();
            var databaseGeneratedAttribute = propertyInfo.GetCustomAttribute<DatabaseGeneratedAttribute>();
            if (keyAttribute != null && (databaseGeneratedAttribute == null || databaseGeneratedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.None))
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        /// <summary>
        /// 转换实体字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="isRefreshCreateTime">是否刷新创建时间</param>
        /// <returns></returns>
        public static void ConversionInterfaceField<T>(this T entity, bool isRefreshCreateTime = false)
        {
            if (entity == null)
            {
                return;
            }

            if (entity is IVersion versionInfo)
            {
                //绑定版本号
                versionInfo.Version = SnowflakeProvider.GetVersionNo();
            }

            if (entity is IUpdateTime updateTimeInfo)
            {
                //绑定更新时间
                updateTimeInfo.UpdateTime = DateTime.Now;
            }

            if (entity is ICreateTime createTimeInfo && isRefreshCreateTime)
            {
                //绑定创建时间
                createTimeInfo.CreateTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 转换实体字段
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldValues"></param>
        /// <param name="isRefreshCreateTime">是否刷新创建时间</param>
        /// <returns></returns>
        public static void ConversionInterfaceField(this Type type, Dictionary<string, object> fieldValues, bool isRefreshCreateTime = false)
        {
            if (type == null || fieldValues.IsNullOrEmpty())
            {
                return;
            }

            var entity = Activator.CreateInstance(type);
            if (entity is IVersion versionInfo)
            {
                //绑定版本号
                fieldValues.Add(nameof(versionInfo.Version), SnowflakeProvider.GetVersionNo());
            }

            if (entity is IUpdateTime updateTimeInfo)
            {
                //绑定更新时间
                fieldValues.Add(nameof(updateTimeInfo.UpdateTime), DateTime.Now);
            }

            if (entity is ICreateTime createTimeInfo && isRefreshCreateTime)
            {
                //绑定创建时间
                fieldValues.Add(nameof(createTimeInfo.CreateTime), DateTime.Now);
            }
        }

        /// <summary>
        /// 转换实体字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <param name="isRefreshCreateTime">是否刷新创建时间</param>
        /// <returns></returns>
        public static IEnumerable<T> ConversionInterfaceFields<T>(this IEnumerable<T> entitys, bool isRefreshCreateTime = false)
        {
            if (entitys.IsNullOrEmpty())
            {
                return [];
            }

            return entitys.Select(entity =>
            {
                if (entity is IVersion versionInfo)
                {
                    //绑定版本号
                    versionInfo.Version = SnowflakeProvider.GetVersionNo();
                }

                if (entity is IUpdateTime updateTimeInfo)
                {
                    //绑定更新时间
                    updateTimeInfo.UpdateTime = DateTime.Now;
                }

                if (entity is ICreateTime createTimeInfo && isRefreshCreateTime)
                {
                    //绑定创建时间
                    createTimeInfo.CreateTime = DateTime.Now;
                }
                return entity;
            });
        }
    }
}