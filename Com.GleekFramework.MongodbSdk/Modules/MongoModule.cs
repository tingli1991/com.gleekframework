using Autofac;
using Com.GleekFramework.AutofacSdk;
using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.GleekFramework.MongodbSdk.Modules
{
    /// <summary>
    /// Mongo 注入
    /// </summary>
    public class MongoModule : Autofac.Module
    {
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //扫描当前执行目录下所有存在继承于IAutofacBase接口的程序集
            var assemblyList = AssemblyProvider.GetAssemblyList(MongoConstant.BASEAUTOFAC_TYPE);
            if (assemblyList.IsNullOrEmpty())
            {
                return;
            }

            //扫描所有的类型列表

            var typeList = assemblyList.GetTypeList().Where(type => type != MongoConstant.BASEAUTOFAC_TYPE && (MongoConstant.BASEAUTOFAC_TYPE.IsAssignableFrom(type) || type.ImplementsGenericInterface(MongoConstant.BASEAUTOFAC_TYPE)));
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
    }
}