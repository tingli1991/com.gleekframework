using Com.GleekFramework.GrpcSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Grpc
{
    /// <summary>
    /// gRPC SDK 类型单元测试
    /// </summary>
    public class GrpcTypeTests : BaseUnitTest
    {
        /// <summary>
        /// IBaseGrpcService 是接口类型
        /// </summary>
        [Fact(DisplayName = "IBaseGrpcService是接口")]
        public void IBaseGrpcServiceIsInterface() =>
            Assert.True(typeof(IBaseGrpcService).IsInterface);

        /// <summary>
        /// GlobalExceptionInterceptor 可实例化
        /// </summary>
        [Fact(DisplayName = "GlobalExceptionInterceptor可实例化")]
        public void InterceptorCanInstantiate() =>
            Assert.NotNull(new GlobalExceptionInterceptor());
    }
}
