using Com.GleekFramework.ConfigSdk;
using Xunit;

namespace Com.GleekFramework.UnitTest.Tests.Config
{
    /// <summary>
    /// 配置常量单元测试
    /// </summary>
    public class ConfigConstantsTests : BaseUnitTest
    {
        /// <summary>
        /// ConfigConstant 默认配置目录名为 Config
        /// </summary>
        [Fact(DisplayName = "默认配置目录名为Config")]
        public void DefaultConfigDirectoryIsConfig()
        {
            Assert.Equal("Config", ConfigConstant.DEFAULT_CONFIG);
        }

        /// <summary>
        /// ConfigConstant 配置文件名称常量不为空
        /// </summary>
        [Fact(DisplayName = "配置文件名称常量不为空")]
        public void ConfigFileNamesAreNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(ConfigConstant.SHARE_CONFIG_FILENAME));
            Assert.False(string.IsNullOrWhiteSpace(ConfigConstant.APP_CONFIG_FILENAME));
            Assert.False(string.IsNullOrWhiteSpace(ConfigConstant.BOOTSTRAP_CONFIG_FILENAME));
            Assert.False(string.IsNullOrWhiteSpace(ConfigConstant.SUBSCRIPTION_CONFIG_FILENAME));
        }

        /// <summary>
        /// ConfigConstant 默认配置名称不为空
        /// </summary>
        [Fact(DisplayName = "默认配置名称不为空")]
        public void DefaultConfigurationNameIsNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(ConfigConstant.DEFAULT_CONFIGURATION_NAME));
        }

        /// <summary>
        /// ConfigConstant 默认文件目录不为空
        /// </summary>
        [Fact(DisplayName = "默认文件目录不为空")]
        public void DefaultFileDirIsNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(ConfigConstant.DEFAULT_FILE_DIR));
        }

        /// <summary>
        /// ConfigConstant 过滤的程序集名称列表不为空
        /// </summary>
        [Fact(DisplayName = "过滤程序集名称列表不为空")]
        public void FilterAssemblyNameListIsNotEmpty()
        {
            Assert.NotNull(ConfigConstant.FilterAssemblyNameList);
            Assert.NotEmpty(ConfigConstant.FilterAssemblyNameList);
        }

        /// <summary>
        /// ConfigConstant 过滤列表包含已知系统库
        /// </summary>
        [Fact(DisplayName = "过滤列表包含System")]
        public void FilterAssemblyNameListContainsSystem()
        {
            Assert.Contains(ConfigConstant.FilterAssemblyNameList, name => name.Contains("System"));
        }

        /// <summary>
        /// EnvironmentConstant 环境变量键不为空
        /// </summary>
        [Fact(DisplayName = "环境变量键不为空")]
        public void EnvironmentConstantKeysAreNotEmpty()
        {
            Assert.False(string.IsNullOrWhiteSpace(EnvironmentConstant.ENV));
            Assert.False(string.IsNullOrWhiteSpace(EnvironmentConstant.PORT));
            Assert.False(string.IsNullOrWhiteSpace(EnvironmentConstant.MODULE));
            Assert.False(string.IsNullOrWhiteSpace(EnvironmentConstant.VERSION));
            Assert.False(string.IsNullOrWhiteSpace(EnvironmentConstant.NOCOS_URL));
        }

        /// <summary>
        /// EnvironmentConstant ENV 默认值为 ENV
        /// </summary>
        [Fact(DisplayName = "环境变量ENV键为ENV")]
        public void EnvironmentConstantEnvKeyIsEnv()
        {
            Assert.Equal("ENV", EnvironmentConstant.ENV);
        }
    }
}
