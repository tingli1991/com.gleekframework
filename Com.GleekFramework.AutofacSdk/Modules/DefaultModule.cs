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
            RegisterAutofacGenericBase(builder);//注入所有的Autofac泛型子类
            RegisterController(builder);//注册控制器的依赖关系
        }

        /// <summary>
        /// 注入所有的AutofacBase子类
        /// </summary>
        /// <param name="builder"></param>
        private void RegisterAutofacBase(ContainerBuilder builder)
        {
            //基础类型
            var autofacType = AutofacConstant.BASEAUTOFAC_TYPE;

            //扫描当前执行目录下所有存在继承于IAutofacBase接口的程序集
            var assemblyList = AssemblyProvider.GetAssemblyList(autofacType);
            if (assemblyList.IsNullOrEmpty())
            {
                return;
            }

            //注入所有继承于IAutofacBase接口的实现类
            builder.RegisterAssemblyTypes(assemblyList.ToArray())
                .Where(type => type != autofacType && (autofacType.IsAssignableFrom(type) || type.ImplementsGenericInterface(autofacType)))
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }

        /// <summary>
        /// 注入所有的Autofac泛型子类
        /// </summary>
        /// <param name="builder"></param>
        private void RegisterAutofacGenericBase(ContainerBuilder builder)
        {
            //基础类型
            var autofacType = AutofacConstant.BASEAUTOFAC_GENERIC_TYPE;

            //扫描当前执行目录下所有存在继承于IAutofacBase接口的程序集
            var assemblyList = AssemblyProvider.GetAssemblyList(autofacType);
            if (assemblyList.IsNullOrEmpty())
            {
                return;
            }

            //扫描所有的类型列表
            var typeList = assemblyList.GetTypeList().Where(type => type != autofacType && (autofacType.IsAssignableFrom(type) || type.ImplementsGenericInterface(autofacType)));
            if (typeList.IsNullOrEmpty())
            {
                return;
            }

            //循环注入泛型实现
            foreach (var type in typeList)
            {
                //注册你的MongoRepository以及其所有继承类
                builder.RegisterGeneric(type)
                .AsSelf()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();//启用属性注入
            }
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