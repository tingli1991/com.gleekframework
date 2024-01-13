using Com.GleekFramework.CommonSdk;
using FluentMigrator;
using FluentMigrator.Builders.Alter;
using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Create.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 数据库架构迁移程序
    /// </summary>
    [AlwaysMigration()]
    public class SchemaMigration : Migration
    {
        /// <summary>
        /// 降级
        /// </summary>
        public override void Down()
        {

        }

        /// <summary>
        /// 升级
        /// </summary>
        public override void Up()
        {
            var databaseProvider = Execute.GetDatabaseProvider();//数据库实现类
            var excludeClassNames = new List<string>() { "IMigrationTable", "MigrationTable" };//需要排除的类名称集合
            var assemblyTypeInfoList = typeof(IMigrationTable).GetTypeList().Where(e => !excludeClassNames.ContainsIgnoreCases(e.Name));//类型列表
            if (assemblyTypeInfoList.IsNullOrEmpty())
            {
                return;
            }

            var databaseName = databaseProvider.GetDatabaseName();//数据库名称
            var databaseIndexSchemaList = databaseProvider.GetIndexSchemaList(databaseName);//数据库索引列表
            var databaseTableSchemaList = databaseProvider.GetTableSchemaList(databaseName);//数据库表信息列表
            foreach (var typeInfo in assemblyTypeInfoList)
            {
                var tableName = typeInfo.GetTableName();//表名称
                var propertyInfoList = PropertyProvider.GetPropertyInfoList(typeInfo);//对象的属性列表
                var databaseTableSchemaColumnList = databaseTableSchemaList.Where(e => e.TableName.EqualIgnoreCases(tableName));//当前表下面的所有列
                var isFirstCreateTableSchema = FirstCreateTableSchema(Create, typeInfo, propertyInfoList, databaseTableSchemaColumnList);//首次创建表结构
                AlertTableOrdinaryColumns(Alter, tableName, propertyInfoList, databaseTableSchemaColumnList);//构建表的普通列(排除Id主键和基础列)
                AlertTableBaseColumns(Alter, isFirstCreateTableSchema, tableName, propertyInfoList);//构建表的基础字段
                AlertTableIndexs(Create, typeInfo, propertyInfoList, databaseIndexSchemaList);//构建表的索引
            }
        }

        /// <summary>
        /// 构建表的基础字段
        /// </summary>
        /// <param name="alter"></param>
        /// <param name="isFirstCreateTableSchema">是否首次创建架构</param>
        /// <param name="tableName">表名称</param>
        /// <param name="propertyInfoList">属性列表</param>
        private static void AlertTableBaseColumns(IAlterExpressionRoot alter, bool isFirstCreateTableSchema, string tableName, IEnumerable<PropertyInfo> propertyInfoList)
        {
            if (!isFirstCreateTableSchema)
            {
                return;
            }

            var basePropertyInfoList = propertyInfoList.Where(e => MigrationConstant.BaseColumns.ContainsIgnoreCases(e.Name));
            if (basePropertyInfoList.IsNullOrEmpty())
            {
                return;
            }

            foreach (var propertyName in MigrationConstant.BaseColumns)
            {
                var basePropertyInfo = basePropertyInfoList.FirstOrDefault(e => e.Name.EqualIgnoreCases(propertyName));
                if (basePropertyInfo == null)
                {
                    continue;
                }

                var columnName = basePropertyInfo.GetColumnName();//列的名称
                alter.Table(tableName).AddColumn(columnName).AddColumnSchema(basePropertyInfo);//调整列
            }
        }

        /// <summary>
        /// 构建表的普通列(排除Id主键和基础列)
        /// </summary>
        /// <param name="alter"></param>
        /// <param name="tableName">表名称</param>
        /// <param name="propertyInfoList">属性列表</param>
        /// <param name="databaseTableSchemaColumnList">已存在的表结构列表</param>
        private static void AlertTableOrdinaryColumns(IAlterExpressionRoot alter, string tableName, IEnumerable<PropertyInfo> propertyInfoList, IEnumerable<TableSchemaModel> databaseTableSchemaColumnList)
        {
            var ordinaryColumnPropertyInfoList = propertyInfoList.Where(e => !e.Name.EqualIgnoreCases(MigrationConstant.Id) && !MigrationConstant.BaseColumns.ContainsIgnoreCases(e.Name));
            if (ordinaryColumnPropertyInfoList.IsNullOrEmpty())
            {
                return;
            }

            foreach (var propertyInfo in ordinaryColumnPropertyInfoList)
            {
                var databaseTableColumnSchemaInfo = databaseTableSchemaColumnList.FirstOrDefault(e => e.ColumnName.EqualIgnoreCases(propertyInfo.Name));
                if (databaseTableColumnSchemaInfo != null)
                {
                    //列已经存在的情况下
                    return;
                }

                var columnName = propertyInfo.GetColumnName();//列的名称
                alter.Table(tableName).AddColumn(columnName).AddColumnSchema(propertyInfo);//调整列
            }
        }

        /// <summary>
        /// 首次构建表架构
        /// </summary>
        /// <param name="create"></param>
        /// <param name="typeInfo">表模型的的类型</param>
        /// <param name="propertyInfoList">属性列表</param>
        /// <param name="databaseTableSchemaColumnList">已存在的表结构列表</param>
        /// <returns></returns>
        private static bool FirstCreateTableSchema(ICreateExpressionRoot create, Type typeInfo, IEnumerable<PropertyInfo> propertyInfoList, IEnumerable<TableSchemaModel> databaseTableSchemaColumnList)
        {
            var isFirstCreateTableSchema = databaseTableSchemaColumnList.IsNullOrEmpty();
            if (!isFirstCreateTableSchema)
            {
                return false;
            }

            var tableName = typeInfo.GetTableName();//表名称
            var tableComment = typeInfo.GetTableComment();//表的描述
            var primaryPropertyInfo = propertyInfoList.FirstOrDefault(e => e.Name.EqualIgnoreCases(MigrationConstant.Id));
            create.Table(tableName).WithDescription(tableComment).WithIdColumn(primaryPropertyInfo);
            return isFirstCreateTableSchema;
        }

        /// <summary>
        /// 构建表的索引
        /// </summary>
        /// <param name="create"></param>
        /// <param name="typeInfo">表模型的的类型</param>
        /// <param name="propertyInfoList">属性列表</param>
        /// <param name="databaseIndexSchemaList">数据库所有的索引</param>
        private static void AlertTableIndexs(ICreateExpressionRoot create, Type typeInfo, IEnumerable<PropertyInfo> propertyInfoList, IEnumerable<IndexSchemaModel> databaseIndexSchemaList)
        {
            var tableName = typeInfo.GetTableName();//表名称
            var tableComment = typeInfo.GetTableComment();//表的描述
            var tableIndexAttributeList = ClassAttributeExtensions.GetCustomAttributeList<IndexAttribute>(typeInfo);
            if (tableIndexAttributeList.IsNullOrEmpty())
            {
                return;
            }

            foreach (var tableIndexAttribute in tableIndexAttributeList)
            {
                var indexName = tableIndexAttribute.Name.ToLower();//索引名称
                var propertyNames = tableIndexAttribute.PropertyNames.ToList();
                if (propertyNames.IsNullOrEmpty())
                {
                    throw new ArgumentNullException(nameof(tableIndexAttribute.PropertyNames));
                }

                if (databaseIndexSchemaList.Any(e => e.TableName.EqualIgnoreCases(tableName) && e.IndexName.EqualIgnoreCases(tableIndexAttribute.Name)))
                {
                    //存在索引，直接跳过
                    continue;
                }

                ICreateIndexOnColumnSyntax indexSyntax = create.Index(indexName).OnTable(tableName);//索引构造器
                foreach (var propertyName in propertyNames)
                {
                    var propertyInfo = propertyInfoList.FirstOrDefault(e => e.Name.EqualIgnoreCases(propertyName));//属性信息
                    var columnName = propertyInfo.GetColumnName();//属性对应的列名称
                    if (string.IsNullOrEmpty(columnName))
                    {
                        //如果没取到值，则世界使用索引的属性名称
                        columnName = propertyName.ToLower();
                    }

                    if (tableIndexAttribute.IsAsc)
                    {
                        indexSyntax = indexSyntax.OnColumn(columnName).Ascending();
                    }
                    else
                    {
                        indexSyntax = indexSyntax.OnColumn(columnName).Descending();
                    }

                    if (tableIndexAttribute.IsUnique)
                    {
                        indexSyntax.WithOptions().Unique();
                    }
                }
            }
        }
    }
}