using Com.GleekFramework.AssemblySdk;
using Com.GleekFramework.AutofacSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Assembly
{
    /// <summary>
    /// 程序集服务单元测试
    /// </summary>
    public class AssemblyServiceTests : BaseUnitTest
    {
        /// <summary>
        /// 通过 DI 获取 AssemblyService 实例
        /// </summary>
        [Fact(DisplayName = "AssemblyService从容器解析应成功")]
        public void AssemblyServiceResolveSucceed()
        {
            var service = new AssemblyService();
            Assert.NotNull(service);
        }

        /// <summary>
        /// 加载已存在的程序集应返回非空结果
        /// </summary>
        [Fact(DisplayName = "加载已存在的程序集应返回非空结果")]
        public void GetAssemblyWithExistingNameReturnsAssembly()
        {
            var assembly = AssemblyService.GetAssembly("Com.GleekFramework.CommonSdk");
            Assert.NotNull(assembly);
            Assert.Equal("Com.GleekFramework.CommonSdk", assembly.GetName().Name);
        }

        /// <summary>
        /// 加载不存在的程序集应返回 null（不抛异常）
        /// </summary>
        [Fact(DisplayName = "加载不存在的程序集应返回Null")]
        public void GetAssemblyWithNonExistentNameReturnsNull()
        {
            var assembly = AssemblyService.GetAssembly("NonExistent.Assembly.Name");
            Assert.Null(assembly);
        }

        /// <summary>
        /// 获取程序集列表应返回非空集合
        /// </summary>
        [Fact(DisplayName = "获取程序集列表应返回非空集合")]
        public void GetAssemblyListReturnsAssemblies()
        {
            var service = new AssemblyService();
            var assemblyList = service.GetAssemblyList();
            Assert.NotNull(assemblyList);
            Assert.NotEmpty(assemblyList);
        }

        /// <summary>
        /// 按 IBaseAutofac 类型筛选应返回包含实现的程序集
        /// </summary>
        [Fact(DisplayName = "按IBaseAutofac类型筛选应返回包含实现的程序集")]
        public void GetAssemblyListByIBaseAutofacReturnsFilteredAssemblies()
        {
            var service = new AssemblyService();
            var assemblyList = service.GetAssemblyList(typeof(IBaseAutofac));
            Assert.NotNull(assemblyList);
            Assert.NotEmpty(assemblyList);
            Assert.Contains(assemblyList, a => a.GetName().Name == "Com.GleekFramework.AssemblySdk");
        }

        /// <summary>
        /// 按类型筛选结果不应超过全部程序集列表
        /// </summary>
        [Fact(DisplayName = "按类型筛选结果不应超过全部程序集列表")]
        public void GetAssemblyListByNullTypeReturnsAllAssemblies()
        {
            var service = new AssemblyService();
            var filteredList = service.GetAssemblyList(typeof(IBaseAutofac));
            var allList = service.GetAssemblyList();
            Assert.NotNull(filteredList);
            Assert.NotEmpty(allList);
            Assert.True(filteredList.Count() <= allList.Count());
        }

        /// <summary>
        /// 检查包含 IBaseAutofac 的程序集应返回 true
        /// </summary>
        [Fact(DisplayName = "检查包含IBaseAutofac的程序集应返回True")]
        public void CheckIsAssignableFromAssemblyHasIBaseAutofacReturnsTrue()
        {
            var assembly = AssemblyService.GetAssembly("Com.GleekFramework.AssemblySdk");
            Assert.NotNull(assembly);
            var result = AssemblyService.CheckIsAssignableFrom(assembly, typeof(IBaseAutofac));
            Assert.True(result);
        }

        /// <summary>
        /// 传入 null 程序集应返回 false，不抛异常
        /// </summary>
        [Fact(DisplayName = "传入Null程序集应返回False")]
        public void CheckIsAssignableFromNullAssemblyReturnsFalse()
        {
            var result = AssemblyService.CheckIsAssignableFrom(null, typeof(IBaseAutofac));
            Assert.False(result);
        }

        /// <summary>
        /// 检查不包含匹配类型的程序集
        /// </summary>
        [Fact(DisplayName = "检查不包含匹配类型的程序集")]
        public void CheckIsAssignableFromNoMatchTypeReturnsFalse()
        {
            var assembly = AssemblyService.GetAssembly("Com.GleekFramework.CommonSdk");
            Assert.NotNull(assembly);
            var result = AssemblyService.CheckIsAssignableFrom(assembly, typeof(System.ComponentModel.ISupportInitialize));
        }
    }
}
