using Com.GleekFramework.MongodbSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Mongodb
{
    /// <summary>
    /// MongoDB 实体单元测试
    /// </summary>
    public class MongoEntityTests : BaseUnitTest
    {
        /// <summary>
        /// MEntity 默认 Id 不为空（默认赋值为新 Guid）
        /// </summary>
        [Fact(DisplayName = "MEntity默认Id不为空")]
        public void DefaultIdIsNotEmpty()
        {
            var entity = new MEntity();
            Assert.NotNull(entity.Id);
            Assert.False(string.IsNullOrWhiteSpace(entity.Id));
        }

        /// <summary>
        /// MEntity 设置 Id 后正确读取
        /// </summary>
        [Fact(DisplayName = "MEntity设置Id后正确读取")]
        public void SetIdReturnsCorrectValue()
        {
            var entity = new MEntity { Id = "507f1f77bcf86cd799439011" };
            Assert.Equal("507f1f77bcf86cd799439011", entity.Id);
        }

        /// <summary>
        /// MEntity Id 支持 ObjectId 格式的字符串
        /// </summary>
        [Fact(DisplayName = "MEntity支持ObjectId格式")]
        public void SupportsObjectIdFormat()
        {
            var entity = new MEntity { Id = "507f191e810c19729de860ea" };
            Assert.Equal(24, entity.Id.Length);
            Assert.Matches(@"^[0-9a-fA-F]{24}$", entity.Id);
        }

        /// <summary>
        /// MEntity 多次设置 Id 会覆盖
        /// </summary>
        [Fact(DisplayName = "MEntity多次设置Id覆盖")]
        public void SetIdMultipleTimes()
        {
            var entity = new MEntity { Id = "first_id" };
            entity.Id = "second_id";
            Assert.Equal("second_id", entity.Id);
        }
    }
}
