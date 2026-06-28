using Com.GleekFramework.ConfigSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Config
{
    /// <summary>
    /// 环境变量提供器单元测试
    /// </summary>
    public class EnvironmentProviderTests : BaseUnitTest
    {
        /// <summary>
        /// GetEnv 默认返回 dev
        /// </summary>
        [Fact(DisplayName = "GetEnv默认返回dev")]
        public void GetEnvDefaultReturnsDev()
        {
            var env = EnvironmentProvider.GetEnv();
            // ENV 环境变量已在 launchSettings.json 中设置为 test
            Assert.NotNull(env);
        }

        /// <summary>
        /// GetEnvironmentVariable 泛型方法默认值正确
        /// </summary>
        [Fact(DisplayName = "泛型GetEnvironmentVariable默认值正确")]
        public void GetEnvironmentVariableGenericDefaultWorks()
        {
            var result = EnvironmentProvider.GetEnvironmentVariable("NONEXISTENT_VAR_FOR_TEST", 999);
            Assert.Equal(999, result);
        }

        /// <summary>
        /// GetEnvironmentVariable 字符串默认值为空
        /// </summary>
        [Fact(DisplayName = "字符串GetEnvironmentVariable默认值为空")]
        public void GetEnvironmentVariableStringDefaultIsEmpty()
        {
            var result = EnvironmentProvider.GetEnvironmentVariable<string>("NONEXISTENT_VAR_FOR_TEST_2");
            Assert.Null(result);
        }

        /// <summary>
        /// GetEnvironmentVariable bool 默认值为 false
        /// </summary>
        [Fact(DisplayName = "布尔GetEnvironmentVariable默认False")]
        public void GetEnvironmentVariableBoolDefaultIsFalse()
        {
            var result = EnvironmentProvider.GetEnvironmentVariable<bool>("NONEXISTENT_VAR_FOR_TEST_3");
            Assert.False(result);
        }
    }
}
