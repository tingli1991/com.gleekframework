using Com.GleekFramework.ObjectSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.ObjectSdk
{
    /// <summary>
    /// 对象存储 SDK 类型单元测试
    /// </summary>
    public class ObjectSdkTypeTests : BaseUnitTest
    {
        /// <summary>
        /// ObjectStorageOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "ObjectStorageOptions可实例化")]
        public void OptionsCanInstantiate() =>
            Assert.NotNull(new ObjectStorageOptions());

        /// <summary>
        /// AliyunProvider 类型存在
        /// </summary>
        [Fact(DisplayName = "AliyunProvider类型存在")]
        public void AliyunProviderTypeExists() =>
            Assert.NotNull(typeof(AliyunProvider));

        /// <summary>
        /// S3ProtocolProvider 类型存在
        /// </summary>
        [Fact(DisplayName = "S3ProtocolProvider类型存在")]
        public void S3ProviderTypeExists() =>
            Assert.NotNull(typeof(S3ProtocolProvider));

        /// <summary>
        /// AliyunOSSService 可实例化
        /// </summary>
        [Fact(DisplayName = "AliyunOSSService可实例化")]
        public void AliyunOssServiceCanInstantiate() =>
            Assert.NotNull(new AliyunOSSService());

        /// <summary>
        /// S3ProtocolService 可实例化
        /// </summary>
        [Fact(DisplayName = "S3ProtocolService可实例化")]
        public void S3ProtocolServiceCanInstantiate() =>
            Assert.NotNull(new S3ProtocolService());
    }
}
