using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 数据模型映射拓展类
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TTarget"></typeparam>
    internal static class DataMapper<TSource, TTarget>
        where TSource : class
        where TTarget : class
    {
        /// <summary>
        /// Map带返回值的委托
        /// </summary>
        private static Func<TSource, TTarget> MapFunc { get; set; }

        /// <summary>
        /// 单模型映射方法
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget Map(TSource source)
        {
            MapFunc ??= GetMap();
            return MapFunc(source);
        }

        /// <summary>
        /// 列表模型映射
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static IEnumerable<TTarget> MapList(IEnumerable<TSource> sources)
        {
            MapFunc ??= GetMap();
            var result = new List<TTarget>();
            foreach (var source in sources)
            {
                result.Add(MapFunc(source));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static Func<TSource, TTarget> GetMap()
        {
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);

            //Func委托传入变量
            var parameter = Parameter(sourceType, "p");
            var memberBindings = new List<MemberBinding>();
            var targetTypes = targetType.GetPropertyInfoList(e => e.PropertyType.IsPublic && e.CanWrite);
            foreach (var targetItem in targetTypes)
            {
                var sourceItem = sourceType.GetPropertyInfo(targetItem.Name);
                if (sourceItem == null || !sourceItem.CanRead || sourceItem.PropertyType.IsNotPublic)
                {
                    //判断实体的读写权限
                    continue;
                }

                //标注NotMapped特性的属性忽略转换
                var notMappedAttribute = PropertyAttributeProvider.GetCustomAttribute<NotMapAttribute>(sourceItem);
                if (notMappedAttribute != null)
                {
                    continue;
                }

                //当非值类型且类型不相同时
                var sourceProperty = Property(parameter, sourceItem);
                if (!sourceItem.PropertyType.IsValueType && sourceItem.PropertyType != targetItem.PropertyType)
                {
                    //判断都是(非泛型)class
                    if (sourceItem.PropertyType.IsClass && targetItem.PropertyType.IsClass && !sourceItem.PropertyType.IsGenericType && !targetItem.PropertyType.IsGenericType)
                    {
                        var expression = GetClassExpression(sourceProperty, sourceItem.PropertyType, targetItem.PropertyType);
                        memberBindings.Add(Bind(targetItem, expression));
                    }

                    //集合数组类型的转换
                    if (typeof(IEnumerable).IsAssignableFrom(sourceItem.PropertyType) && typeof(IEnumerable).IsAssignableFrom(targetItem.PropertyType))
                    {
                        var expression = GetListExpression(sourceProperty, sourceItem.PropertyType, targetItem.PropertyType);
                        memberBindings.Add(Bind(targetItem, expression));
                    }
                    continue;
                }

                if (targetItem.PropertyType != sourceItem.PropertyType)
                {
                    continue;
                }
                memberBindings.Add(Bind(targetItem, sourceProperty));
            }

            //创建一个if条件表达式
            var test = NotEqual(parameter, Constant(null, sourceType));
            var ifTrue = MemberInit(New(targetType), memberBindings);
            var condition = Condition(test, ifTrue, Constant(null, targetType));
            var lambda = Lambda<Func<TSource, TTarget>>(condition, parameter);
            return lambda.Compile();
        }

        /// <summary>
        /// 类型是clas时赋值
        /// </summary>
        /// <param name="sourceProperty"></param>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private static Expression GetClassExpression(Expression sourceProperty, Type sourceType, Type targetType)
        {
            //条件p.Item!=null
            var testItem = NotEqual(sourceProperty, Constant(null, sourceType));

            //构造回调 Mapper<TSource, TTarget>.Map()
            var mapperType = typeof(DataMapper<,>).MakeGenericType(sourceType, targetType);
            var iftrue = Call(mapperType.GetMethod(nameof(Map), new[] { sourceType }), sourceProperty);
            var conditionItem = Condition(testItem, iftrue, Constant(null, targetType));
            return conditionItem;
        }

        /// <summary>
        /// 类型为集合时赋值
        /// </summary>
        /// <param name="sourceProperty"></param>
        /// <param name="sourceType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private static Expression GetListExpression(Expression sourceProperty, Type sourceType, Type targetType)
        {
            //条件p.Item!=null    
            var testItem = NotEqual(sourceProperty, Constant(null, sourceType));

            //构造回调 Mapper<TSource, TTarget>.MapList()
            var sourceArg = sourceType.IsArray ? sourceType.GetElementType() : sourceType.GetGenericArguments()[0];
            var targetArg = targetType.IsArray ? targetType.GetElementType() : targetType.GetGenericArguments()[0];
            var mapperType = typeof(DataMapper<,>).MakeGenericType(sourceArg, targetArg);
            var mapperExecMap = Call(mapperType.GetMethod(nameof(MapList), new[] { sourceType }), sourceProperty);
            Expression iftrue;
            if (targetType == mapperExecMap.Type)
            {
                iftrue = mapperExecMap;
            }
            else if (targetType.IsArray)//数组类型调用ToArray()方法
            {
                iftrue = Call(mapperExecMap, mapperExecMap.Type.GetMethod("ToArray"));
            }
            else if (typeof(IDictionary).IsAssignableFrom(targetType))
            {
                iftrue = Constant(null, targetType);//字典类型不转换
            }
            else
            {
                iftrue = Convert(mapperExecMap, targetType);
            }
            var conditionItem = Condition(testItem, iftrue, Constant(null, targetType));
            return conditionItem;
        }
    }
}