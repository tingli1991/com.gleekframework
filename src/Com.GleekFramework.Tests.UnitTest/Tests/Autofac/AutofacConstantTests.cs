using Com.GleekFramework.AutofacSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Autofac
{
    /// <summary>
    /// Autofac 常量单元测试
    /// </summary>
    public class AutofacConstantTests : BaseUnitTest
    {
        /// <summary>
        /// BASEAUTOFAC_TYPE 是 IBaseAutofac 类型
        /// </summary>
        [Fact(DisplayName = "BASEAUTOFAC_TYPE是IBaseAutofac")]
        public void BaseAutofacTypeIsIBaseAutofac()
        {
            Assert.Equal(typeof(IBaseAutofac), AutofacConstant.BASEAUTOFAC_TYPE);
        }

        /// <summary>
        /// BASEAUTOFAC_GENERIC_TYPE 是 IBaseAutofac(T) 类型
        /// </summary>
        [Fact(DisplayName = "泛型BASEAUTOFAC_TYPE是IBaseAutofacT")]
        public void BaseAutofacGenericTypeIsIBaseAutofacT()
        {
            Assert.Equal(typeof(IBaseAutofac<>), AutofacConstant.BASEAUTOFAC_GENERIC_TYPE);
        }

        /// <summary>
        /// CONTROLLERBASE_TYPE 是 ControllerBase 类型
        /// </summary>
        [Fact(DisplayName = "CONTROLLERBASE_TYPE是ControllerBase")]
        public void ControllerBaseTypeIsControllerBase()
        {
            Assert.Equal(typeof(Microsoft.AspNetCore.Mvc.ControllerBase), AutofacConstant.CONTROLLERBASE_TYPE);
        }

        /// <summary>
        /// IBaseAutofac 是一个接口
        /// </summary>
        [Fact(DisplayName = "IBaseAutofac是接口")]
        public void IBaseAutofacIsInterface()
        {
            Assert.True(typeof(IBaseAutofac).IsInterface);
        }

        /// <summary>
        /// IBaseAutofac(T) 是泛型接口
        /// </summary>
        [Fact(DisplayName = "IBaseAutofacT是泛型接口")]
        public void IBaseAutofacGenericIsGenericInterface()
        {
            Assert.True(typeof(IBaseAutofac<>).IsGenericType);
        }

        /// <summary>
        /// AssemblyService 实现了 IBaseAutofac
        /// </summary>
        [Fact(DisplayName = "AssemblyService实现IBaseAutofac")]
        public void AssemblyServiceImplementsIBaseAutofac()
        {
            Assert.True(typeof(AssemblySdk.AssemblyService).IsAssignableTo(typeof(IBaseAutofac)));
        }

        /// <summary>
        /// DocumentService 实现了 IBaseAutofac
        /// </summary>
        [Fact(DisplayName = "DocumentService实现IBaseAutofac")]
        public void DocumentServiceImplementsIBaseAutofac()
        {
            Assert.True(typeof(AssemblySdk.DocumentService).IsAssignableTo(typeof(IBaseAutofac)));
        }
    }
}
