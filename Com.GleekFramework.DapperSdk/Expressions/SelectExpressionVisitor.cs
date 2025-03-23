using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Com.GleekFramework.DapperSdk
{
    /// <summary>
    /// SELECT访问器
    /// </summary>
    /// <typeparam name="TEntity">查询的实体类型</typeparam>
    /// <typeparam name="TResult">返回的实体类型</typeparam>
    public class SelectExpressionVisitor<TEntity, TResult>
    {
        /// <summary>
        /// 需要查询显示的列
        /// </summary>
        private IEnumerable<string> Columns = new List<string>();

        /// <summary>
        /// 获取SELECT的显示列脚本
        /// </summary>
        /// <returns></returns>
        public string GetSelectColumns() => string.Join(",", Columns.Distinct());

        /// <summary>
        /// 访问成员表达式
        /// </summary>
        public void Visit()
        {
            var resultType = typeof(TResult);//返回的实体类型
            var entityType = typeof(TEntity);//查询的实体类型
            var allEntityColumnDic = GetAllColumnDic(entityType);//查询实体的所有列字典
            if (entityType == resultType)
            {
                // 处理匿名类型或显式指定类型
                Columns = allEntityColumnDic.Values.Distinct();
            }
            else
            {
                var propertyInfoList = resultType.GetPropertyInfoList();
                foreach (var propertyInfo in propertyInfoList)
                {
                    var columnAttribute = propertyInfo.GetCustomAttribute<ColumnAttribute>();
                    if (!string.IsNullOrEmpty(columnAttribute?.Name))
                    {
                        Columns = Columns.Add(columnAttribute.Name);
                    }
                    else
                    {
                        var propertyName = propertyInfo.Name;//当前对象的属性名称
                        if (allEntityColumnDic.ContainsKey(propertyName))
                        {
                            //使用查询实体的列名称
                            Columns = Columns.Add(allEntityColumnDic[propertyName]);
                        }
                        else
                        {
                            //使用当前对象的属性名称作为字段列
                            Columns = Columns.Add(propertyName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定类型的所有列字典
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetAllColumnDic(Type type)
        {
            var propertyInfoList = type.GetPropertyInfoList();
            return propertyInfoList.ToDictionary(k => k.Name, v => v.GetCustomAttribute<ColumnAttribute>()?.Name ?? v.Name);
        }
    }
}