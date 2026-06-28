using Com.GleekFramework.Models;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Models
{
    /// <summary>
    /// 测试模型类型单元测试
    /// </summary>
    public class ModelsTypeTests : BaseUnitTest
    {
        /// <summary>
        /// WeatherForecastModel 可实例化
        /// </summary>
        [Fact(DisplayName = "WeatherForecastModel可实例化")]
        public void WeatherForecastModelCanInstantiate() =>
            Assert.NotNull(new WeatherForecastModel());

        /// <summary>
        /// WeatherForecastParam 可实例化
        /// </summary>
        [Fact(DisplayName = "WeatherForecastParam可实例化")]
        public void WeatherForecastParamCanInstantiate() =>
            Assert.NotNull(new WeatherForecastParam());

        /// <summary>
        /// StudentParam 可实例化
        /// </summary>
        [Fact(DisplayName = "StudentParam可实例化")]
        public void StudentParamCanInstantiate() =>
            Assert.NotNull(new StudentParam());

        /// <summary>
        /// ConfigOptions 可实例化
        /// </summary>
        [Fact(DisplayName = "ConfigOptions可实例化")]
        public void ConfigOptionsCanInstantiate() =>
            Assert.NotNull(new ConfigOptions());

        /// <summary>
        /// ComAreaModel 可实例化
        /// </summary>
        [Fact(DisplayName = "ComAreaModel可实例化")]
        public void ComAreaModelCanInstantiate() =>
            Assert.NotNull(new ComAreaModel());

        /// <summary>
        /// ComAreaUpgration 可实例化
        /// </summary>
        [Fact(DisplayName = "ComAreaUpgration可实例化")]
        public void ComAreaUpgrationCanInstantiate() =>
            Assert.NotNull(new ComAreaUpgration());

        /// <summary>
        /// ConfigConstant 配置键不为空
        /// </summary>
        [Fact(DisplayName = "ConfigConstant配置键不为空")]
        public void ConfigConstantIsNotEmpty() =>
            Assert.False(string.IsNullOrWhiteSpace(ConfigConstant.SummariesKey));

        /// <summary>
        /// DatabaseConstant 数据库连接键不为空
        /// </summary>
        [Fact(DisplayName = "DatabaseConstant数据库连接键不为空")]
        public void DatabaseConstantIsNotEmpty() =>
            Assert.False(string.IsNullOrWhiteSpace(DatabaseConstant.DefaultMySQLHostsKey));
    }
}
