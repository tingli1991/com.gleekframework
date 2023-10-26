using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 环境变量拓展方法
    /// </summary>
    public partial class EnvironmentService : IBaseAutofac
    {
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public string GetVersion(string prefix) => EnvironmentProvider.GetVersion(prefix);

        /// <summary>
        /// 获取环境值
        /// </summary>
        /// <returns></returns>
        public string GetEnv() => EnvironmentProvider.GetEnvironmentVariable(EnvironmentConstant.ENV);

        /// <summary>
        /// 获取项目名称
        /// </summary>
        /// <returns></returns>
        public string GetProject() => EnvironmentProvider.GetEnvironmentVariable(EnvironmentConstant.PROJECT);

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public string GetVersion() => EnvironmentProvider.GetEnvironmentVariable(EnvironmentConstant.VERSION);

        /// <summary>
        /// 获取Nacos项目主机地址
        /// </summary>
        /// <returns></returns>
        public string GetNacosUrl() => EnvironmentProvider.GetEnvironmentVariable(EnvironmentConstant.NOCOS_URL);
    }
}