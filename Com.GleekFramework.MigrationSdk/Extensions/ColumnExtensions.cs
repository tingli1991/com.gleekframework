using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 字段列的拓展类
    /// </summary>
    public static partial class ColumnExtensions
    {
        /// <summary>
        /// 创建主键
        /// </summary>
        /// <param name="tableWithColumnSyntax"></param>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static ICreateTableColumnOptionOrWithColumnSyntax WithPrimaryColumn(this ICreateTableWithColumnSyntax tableWithColumnSyntax, PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var comment = propertyInfo.GetComment();//字段备注
            var columnName = propertyInfo.GetColumnName();//列的名称
            if (propertyInfo.Equals<string>())
            {
                var maxLength = propertyInfo.GetMaxLength();
                return tableWithColumnSyntax
                   .WithColumn(columnName)
                   .AsString(maxLength)
                   .NotNullable()
                   .PrimaryKey()
                   .WithColumnDescription(comment);
            }
            else if (propertyInfo.Equals<long>())
            {
                var databaseGenerated = propertyInfo.GetCustomAttribute<DatabaseGeneratedAttribute>();//获取数据库生成的属性
                if (databaseGenerated != null && databaseGenerated.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                {
                    return tableWithColumnSyntax
                        .WithColumn(columnName)
                        .AsInt64()
                        .NotNullable()
                        .PrimaryKey()
                        .WithColumnDescription(comment);
                }
                else
                {
                    return tableWithColumnSyntax
                        .WithColumn(columnName)
                        .AsInt64()
                        .NotNullable()
                        .PrimaryKey()
                        .Identity()
                        .WithColumnDescription(comment);
                }
            }
            else
            {
                throw new Exception($"不支持的主键类型映射：{propertyInfo.PropertyType.FullName}");
            }
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="alterTableColumnSyntax"></param>
        /// <param name="propertyInfo">属性信息</param>
        public static void AddColumnSchema(this IAlterTableColumnAsTypeSyntax alterTableColumnSyntax, PropertyInfo propertyInfo)
        {
            var comment = propertyInfo.GetComment();//字段备注
            var isPrimaryKey = propertyInfo.IsPrimaryKey();//是否是主键
            var defaultValue = propertyInfo.GetDefaultValue();//默认值
            var databaseGenerated = propertyInfo.GetDatabaseGenerated();
            if (propertyInfo.Equals<string>())
            {
                var maxLength = propertyInfo.GetMaxLength();
                alterTableColumnSyntax.AsString(maxLength).WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<int>())
            {
                alterTableColumnSyntax.AsInt32().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.PropertyType.IsEnum)
            {
                alterTableColumnSyntax.AsInt32().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<uint>())
            {
                alterTableColumnSyntax.AsInt32().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<byte>())
            {
                alterTableColumnSyntax.AsByte().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<sbyte>())
            {
                alterTableColumnSyntax.AsByte().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<short>())
            {
                alterTableColumnSyntax.AsInt16().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<ushort>())
            {
                alterTableColumnSyntax.AsInt16().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<long>())
            {
                alterTableColumnSyntax.AsInt64().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<ulong>())
            {
                alterTableColumnSyntax.AsInt64().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<float>())
            {
                alterTableColumnSyntax.AsFloat().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<decimal>())
            {
                var (Scale, Precision) = propertyInfo.GetPrecisionInfo();//精度信息
                alterTableColumnSyntax.AsDecimal(Precision, Scale).WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<bool>())
            {
                alterTableColumnSyntax.AsBoolean().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<DateTime>())
            {
                alterTableColumnSyntax.AsDateTime2().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else if (propertyInfo.Equals<Guid>())
            {
                alterTableColumnSyntax.AsGuid().WithColumn(defaultValue, comment, isPrimaryKey, databaseGenerated);
            }
            else
            {
                throw new Exception($"不支持类型映射：{propertyInfo.PropertyType.FullName}");
            }
        }

        /// <summary>
        /// 追加列的相关属性
        /// </summary>
        /// <param name="columnSyntax"></param>
        /// <param name="defaultValue">字段默认值</param>
        /// <param name="comment">字段描述</param>
        /// <param name="isPrimaryKey">是否是主键</param>
        /// <param name="databaseGenerated"></param>
        /// <returns></returns>
        private static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax WithColumn(this IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax columnSyntax,
            object defaultValue, string comment, bool isPrimaryKey = false, DatabaseGeneratedOption databaseGenerated = DatabaseGeneratedOption.None)
        {
            if (isPrimaryKey)
            {
                columnSyntax.PrimaryKey();
            }

            switch (databaseGenerated)
            {
                case DatabaseGeneratedOption.Identity:
                    columnSyntax.Identity();
                    break;
                case DatabaseGeneratedOption.Computed:
                    break;
            }

            columnSyntax = columnSyntax.WithColumnDescription(comment);
            if (defaultValue != null)
            {
                //字段默认值
                columnSyntax = columnSyntax.WithDefaultValue(defaultValue);
            }
            return columnSyntax;
        }
    }
}