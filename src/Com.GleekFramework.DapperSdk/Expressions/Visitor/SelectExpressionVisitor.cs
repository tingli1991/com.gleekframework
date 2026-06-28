using Com.GleekFramework.CommonSdk;
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
            var allEntityColumnDic = entityType.GetAllColumnDic();//查询实体的所有列字典
            if (entityType == resultType)
            {
                var columnIgnoreDic = entityType.GetColumnIgnoreDic();//获取被忽略的列字典
                Columns = allEntityColumnDic.Values.Distinct();// 处理匿名类型或显式指定类型
                Columns = Columns.Except(columnIgnoreDic.Values.Distinct());//移除被忽略的列
            }
            else
            {
                var propertyInfoList = resultType.GetPropertyInfoList();
                var columnIgnoreDic = resultType.GetColumnIgnoreDic();//获取被忽略的列字典
                foreach (var propertyInfo in propertyInfoList)
                {
                    if (columnIgnoreDic.ContainsKey(propertyInfo.Name))
                    {
                        //如果当前对象的属性被忽略，则跳过
                        continue;
                    }

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
    }
}