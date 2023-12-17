using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.NLogSdk;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 配置文件依赖注入实现类
    /// </summary>
    public static partial class DependencyProvider
    {
        /// <summary>
        /// 开关
        /// </summary>
        public static bool Switch { get; set; }

        /// <summary>
        /// 当前进程数量
        /// </summary>
        private static int CurrentProcessorCount = 0;

        /// <summary>
        /// 最后一次执行事件
        /// </summary>
        private static DateTime? LastExecuteTime { get; set; }

        /// <summary>
        /// 注册(或刷新)配置特性对照的配置值
        /// </summary>
        public static void RefreshConfigAttribute()
        {
            var beginTime = DateTime.Now.ToCstTime();
            try
            {
                if (!Switch)
                {
                    //关闭直接结束
                    return;
                }

                //设置最近一次更新事件(防止并发，因为重载设计方式会导致，重复给重载事件)
                if (LastExecuteTime.HasValue && (beginTime - LastExecuteTime.Value).TotalSeconds <= 3)
                {
                    //5秒内不允许重复执行配置属性动作
                    return;
                }

                //递增线程数量(防止并发，因为重载设计方式会导致，重复给重载事件)
                LastExecuteTime = DateTime.Now.ToCstTime();
                Interlocked.Increment(ref CurrentProcessorCount);
                if (CurrentProcessorCount > 1)
                {
                    //前面有程序正在执行
                    return;
                }

                var configAttributeType = typeof(ConfigAttribute);
                var assemblyList = AssemblyProvider.GetAssemblyList();//类型列表
                if (assemblyList == null || !assemblyList.Any())
                {
                    return;
                }

                //遍历程序集
                Parallel.ForEach(assemblyList, (assembly) =>
                {
                    if (ConfigConstant.FilterAssemblyNameList.Any(e => assembly.FullName.StartsWith(e)))
                    {
                        //排除以清单名字开头的程序集
                        return;
                    }

                    var assembleTypeList = AssemblyTypeProvider.GetTypeList(assembly);
                    if (assembleTypeList == null || !assembleTypeList.Any())
                    {
                        return;
                    }

                    //便利程序集对照的类型
                    Parallel.ForEach(assembleTypeList, (assembleType) =>
                    {
                        if (!assembleType.IsClass || string.IsNullOrEmpty(assembleType.Name) || string.IsNullOrEmpty(assembleType.Namespace))
                        {
                            return;
                        }

                        //当前类型的属性列表
                        bool filter(PropertyInfo e) => e.CustomAttributes != null && e.CustomAttributes.Any(p => configAttributeType.IsAssignableFrom(p.AttributeType) || p.AttributeType == configAttributeType);
                        var propertyInfoList = PropertyProvider.GetPropertyInfoList(assembleType, filter);
                        if (propertyInfoList == null || !propertyInfoList.Any())
                        {
                            return;
                        }

                        //当前对象实例
                        var instance = ActivatorProvider.CreateInstance(assembleType);
                        if (instance == null)
                        {
                            return;
                        }

                        //遍历属性列表
                        Parallel.ForEach(propertyInfoList, (propertyInfo) =>
                        {
                            var configAttribute = PropertyAttributeProvider.GetCustomAttribute<ConfigAttribute>(propertyInfo);//配置文件特性
                            var configurationValue = GetConfigurationValue(configAttribute, propertyInfo.PropertyType);//配置文件值
                            instance.SetPropertyValue(propertyInfo.Name, configurationValue);//设置对象的属性值
                        });
                    });
                });
                var totalMilliseconds = (long)(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
                NLogProvider.Trace(@$"【刷新属性配置】耗时：{totalMilliseconds} 毫秒！", totalMilliseconds: totalMilliseconds);
            }
            catch (Exception ex)
            {
                var totalMilliseconds = (long)(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
                NLogProvider.Error(@$"【刷新属性配置】耗时：{totalMilliseconds} 毫秒，错误：{ex}", totalMilliseconds: totalMilliseconds);
            }
            finally
            {
                if (CurrentProcessorCount > 0)
                {
                    //递减
                    Interlocked.Decrement(ref CurrentProcessorCount);
                }
            }
        }

        /// <summary>
        /// 获取配置值
        /// </summary>
        /// <param name="configAttribute"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        private static object GetConfigurationValue(ConfigAttribute configAttribute, Type propertyType)
        {
            object configurationValue;
            if (configAttribute == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(configAttribute.Key))
            {
                //获取整个配置文件节点
                configurationValue = configAttribute.Configuration.GetConfiguration(propertyType);
            }
            else
            {
                //获取指定的配置
                configurationValue = configAttribute.Configuration.GetValue(propertyType, configAttribute.Key);
            }

            return configurationValue;
        }
    }
}