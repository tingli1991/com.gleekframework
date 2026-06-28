using Com.GleekFramework.MigrationSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Migration
{
    /// <summary>
    /// 数据库迁移 SDK 类型单元测试
    /// </summary>
    public class MigrationTypeTests : BaseUnitTest
    {
        /// <summary>
        /// MigrationOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "MigrationOptions可实例化")]
        public void MigrationOptionsCanInstantiate() =>
            Assert.NotNull(new MigrationOptions());

        /// <summary>
        /// IUpgration 是接口类型
        /// </summary>
        [Fact(DisplayName = "IUpgration是接口")]
        public void IUpgrationIsInterface() =>
            Assert.True(typeof(IUpgration).IsInterface);

        /// <summary>
        /// IDatabaseProvider 是接口类型
        /// </summary>
        [Fact(DisplayName = "IDatabaseProvider是接口")]
        public void IDatabaseProviderIsInterface() =>
            Assert.True(typeof(IDatabaseProvider).IsInterface);

        /// <summary>
        /// AlwaysMigrationAttribute 可实例化
        /// </summary>
        [Fact(DisplayName = "AlwaysMigrationAttribute可实例化")]
        public void AlwaysMigrationAttributeCanInstantiate() =>
            Assert.NotNull(new AlwaysMigrationAttribute());

        /// <summary>
        /// UpgrationAttribute 默认构造函数可实例化
        /// </summary>
        [Fact(DisplayName = "UpgrationAttribute可实例化")]
        public void UpgrationAttributeCanInstantiate() =>
            Assert.NotNull(new UpgrationAttribute());

        /// <summary>
        /// SchemaMigration 可实例化
        /// </summary>
        [Fact(DisplayName = "SchemaMigration可实例化")]
        public void SchemaMigrationCanInstantiate() =>
            Assert.NotNull(new SchemaMigration());

        /// <summary>
        /// Upgration 抽象类型存在
        /// </summary>
        [Fact(DisplayName = "Upgration类型存在")]
        public void UpgrationTypeExists() =>
            Assert.NotNull(typeof(Upgration));
    }
}
