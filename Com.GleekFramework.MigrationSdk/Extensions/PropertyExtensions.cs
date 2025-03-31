using Com.GleekFramework.CommonSdk;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 列的属性拓展
    /// </summary>
    static partial class PropertyExtensions
    {
        /// <summary>
        /// 获取表的描述
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static string GetTableComment(this Type typeInfo)
        {
            var tableCommentAttribute = ClassAttributeProvider.GetCustomAttribute<CommentAttribute>(typeInfo);
            return tableCommentAttribute?.Comment ?? "";
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static string GetTableName(this Type typeInfo)
        {
            var tableAttribute = ClassAttributeProvider.GetCustomAttribute<TableAttribute>(typeInfo);
            return (tableAttribute?.Name ?? typeInfo.Name ?? "").ToLower();
        }

        /// <summary>
        /// 对比属性类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool Equals<T>(this PropertyInfo propertyInfo)
        {
            var type = typeof(T);
            var propertyType = propertyInfo.PropertyType;//属性类型
            var nullPropertyType = Nullable.GetUnderlyingType(propertyType);//可空的属性类型
            return type == propertyType || type == nullPropertyType;//判断是可空类型或者非可空类型
        }

        /// <summary>
        /// 获取列的名称
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns></returns>
        public static string GetColumnName(this PropertyInfo propertyInfo)
        {
            var columnAttribute = PropertyAttributeProvider.GetCustomAttribute<ColumnAttribute>(propertyInfo);
            return (columnAttribute?.Name ?? propertyInfo?.Name ?? "").ToLower();
        }

        /// <summary>
        /// 获取备注
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns></returns>
        public static string GetComment(this PropertyInfo propertyInfo)
        {
            var commentAttribute = PropertyAttributeProvider.GetCustomAttribute<CommentAttribute>(propertyInfo);
            return commentAttribute?.Comment ?? "";
        }

        /// <summary>
        /// 是否是主键
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns></returns>
        public static bool IsPrimaryKey(this PropertyInfo propertyInfo)
        {
            var keyAttribute = PropertyAttributeProvider.GetCustomAttribute<KeyAttribute>(propertyInfo);
            return keyAttribute != null;
        }

        /// <summary>
        /// 是否必填
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns></returns>
        public static bool IsRequired(this PropertyInfo propertyInfo)
        {
            var requiredAttribute = PropertyAttributeProvider.GetCustomAttribute<RequiredAttribute>(propertyInfo);
            return requiredAttribute != null;
        }

        /// <summary>
        /// 获取最大长度
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns></returns>
        public static int GetMaxLength(this PropertyInfo propertyInfo)
        {
            var maxLengthAttribute = PropertyAttributeProvider.GetCustomAttribute<MaxLengthAttribute>(propertyInfo);
            return maxLengthAttribute?.Length ?? int.MaxValue;
        }

        /// <summary>
        /// 获取精度配置
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns></returns>
        public static (byte Scale, byte Precision) GetPrecisionInfo(this PropertyInfo propertyInfo)
        {
            var precisionAttribute = PropertyAttributeProvider.GetCustomAttribute<PrecisionAttribute>(propertyInfo);
            return (precisionAttribute?.Scale ?? 0, precisionAttribute?.Precision ?? 30);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static DatabaseGeneratedOption GetDatabaseGenerated(this PropertyInfo propertyInfo)
        {
            var databaseGeneratedAttribute = PropertyAttributeProvider.GetCustomAttribute<DatabaseGeneratedAttribute>(propertyInfo);//主键的生成规则
            return databaseGeneratedAttribute?.DatabaseGeneratedOption ?? DatabaseGeneratedOption.None;
        }

        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns></returns>
        public static object GetDefaultValue(this PropertyInfo propertyInfo)
        {
            var defaultValueAttribute = PropertyAttributeProvider.GetCustomAttribute<DefaultValueAttribute>(propertyInfo);
            if (defaultValueAttribute == null)
            {
                if (propertyInfo.Equals<char>())
                {
                    return char.MinValue;
                }
                else if (propertyInfo.Equals<string>())
                {
                    return "";
                }
                else if (propertyInfo.Equals<int>())
                {
                    return 0;
                }
                else if (propertyInfo.Equals<uint>())
                {
                    return 0;
                }
                else if (propertyInfo.Equals<byte>())
                {
                    return 0;
                }
                else if (propertyInfo.Equals<sbyte>())
                {
                    return 0;
                }

                else if (propertyInfo.Equals<short>())
                {
                    return 0;
                }
                else if (propertyInfo.Equals<ushort>())
                {
                    return 0;
                }
                else if (propertyInfo.Equals<long>())
                {
                    return 0;
                }
                else if (propertyInfo.Equals<ulong>())
                {
                    return 0;
                }
                else if (propertyInfo.Equals<float>())
                {
                    return 0f;
                }
                else if (propertyInfo.Equals<decimal>())
                {
                    return 0M;
                }
                else if (propertyInfo.Equals<double>())
                {
                    return 0.0;
                }
                else if (propertyInfo.Equals<bool>())
                {
                    return false;
                }
                else if (propertyInfo.Equals<Guid>())
                {
                    return Guid.Empty;
                }
                else if (propertyInfo.Equals<DateTime>())
                {
                    return DateTime.MinValue;
                }
                else if (propertyInfo.PropertyType.IsEnum)
                {
                    return 0;
                }
                else
                {
                    return null;
                }
            }
            return defaultValueAttribute.Value;
        }
    }
}