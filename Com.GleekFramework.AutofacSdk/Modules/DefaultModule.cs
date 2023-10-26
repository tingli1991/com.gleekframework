using Autofac;
using Com.GleekFramework.CommonSdk;
using System;
using System.Linq;
using System.Reflection;

namespace Com.GleekFramework.AutofacSdk
{
    /// <summary>
    /// 默认注入
    /// </summary>
    public class DefaultModule : Autofac.Module
    {
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            RegisterAutofacBase(builder);//注入所有的AutofacBase子类
            RegisterController(builder);//注册控制器的依赖关系
        }

        /// <summary>
        /// 注入所有的AutofacBase子类
        /// </summary>
        /// <param name="builder"></param>
        private void RegisterAutofacBase(ContainerBuilder builder)
        {
            //扫描当前执行目录下所有存在继承于IAutofacBase接口的程序集
            var assemblyList = AssemblyProvider.GetAssemblyList(AutofacConstant.BASEAUTOFAC_TYPE);
            if (assemblyList == null || !assemblyList.Any())
            {
                return;
            }

            //注入所有继承于IAutofacBase接口的实现类
            builder.RegisterAssemblyTypes(assemblyList.ToArray())
                .Where(type => type != AutofacConstant.BASEAUTOFAC_TYPE && AutofacConstant.BASEAUTOFAC_TYPE.IsAssignableFrom(type))
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }

        /// <summary>
        /// 注册控制器的依赖关系
        /// </summary>
        /// <param name="builder"></param>
        private void RegisterController(ContainerBuilder builder)
        {
            //当前项目的程序集
            var assembly = Assembly.GetEntryAssembly();

            //控制器类型
            var controllerType = AutofacConstant.CONTROLLERBASE_TYPE;

            //检查是否包含值类型
            var checkIsAssignableFrom = assembly.CheckIsAssignableFrom(controllerType);

            //如果包含值类型则进行控制器的依赖注入
            if (checkIsAssignableFrom)
            {
                bool filter(Type type) => controllerType.IsAssignableFrom(type) && type != controllerType;
                builder.RegisterAssemblyTypes(assembly).Where(filter).PropertiesAutowired();
            }
        }
    }
}