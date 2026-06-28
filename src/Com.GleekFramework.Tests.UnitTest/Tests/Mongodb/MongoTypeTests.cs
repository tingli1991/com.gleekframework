using Com.GleekFramework.MongodbSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Mongodb
{
    /// <summary>
    /// MongoDB SDK 类型单元测试
    /// </summary>
    public class MongoTypeTests : BaseUnitTest
    {
        /// <summary>
        /// MongoConstant 默认连接名不为空
        /// </summary>
        [Fact(DisplayName = "MongoConstant默认连接名不为空")]
        public void DefaultConnectionNameIsNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(MongoConstant.DEFAULT_CONNECTION_NAME));
        }

        /// <summary>
        /// IMEntity 是接口
        /// </summary>
        [Fact(DisplayName = "IMEntity是接口")]
        public void IMEntityIsInterface()
        {
            Assert.True(typeof(IMEntity).IsInterface);
        }

        /// <summary>
        /// IMEntity 接口包含 Id 属性
        /// </summary>
        [Fact(DisplayName = "IMEntity包含Id属性")]
        public void IMEntityHasIdProperty()
        {
            var prop = typeof(IMEntity).GetProperty("Id");
            Assert.NotNull(prop);
            Assert.Equal(typeof(string), prop.PropertyType);
        }

        /// <summary>
        /// IMongoRepository 是接口
        /// </summary>
        [Fact(DisplayName = "IMongoRepository是接口")]
        public void IMongoRepositoryIsInterface()
        {
            Assert.True(typeof(IMongoRepository).IsInterface);
        }

        /// <summary>
        /// IMongoRepository(T) 是泛型接口
        /// </summary>
        [Fact(DisplayName = "IMongoRepositoryT是泛型接口")]
        public void IMongoRepositoryGenericIsInterface()
        {
            Assert.True(typeof(IMongoRepository<>).IsGenericType);
            Assert.True(typeof(IMongoRepository<>).IsInterface);
        }

        /// <summary>
        /// MEntity 基类可实例化
        /// </summary>
        [Fact(DisplayName = "MEntity可实例化")]
        public void MEntityCanInstantiate()
        {
            Assert.NotNull(new MEntity());
        }

        /// <summary>
        /// MEntity 设置 Id 后读取正确
        /// </summary>
        [Fact(DisplayName = "MEntity设置Id属性")]
        public void MEntitySetId()
        {
            var entity = new MEntity { Id = "507f1f77bcf86cd799439011" };
            Assert.Equal("507f1f77bcf86cd799439011", entity.Id);
        }
    }
}
