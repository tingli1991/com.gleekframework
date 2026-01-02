using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 环境变量拓展方法
    /// </summary>
    public partial class EnvironmentService : IBaseAutofac
    {
        /// <summary>
        /// 获取环境值
        /// </summary>
        /// <returns></returns>
        public string GetEnv() => EnvironmentProvider.GetEnv();

        /// <summary>
        /// 获取项目名称
        /// </summary>
        /// <returns></returns>
        public string GetModule() => EnvironmentProvider.GetModule();

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public string GetVersion() => EnvironmentProvider.GetVersion();

        /// <summary>
        /// 获取Nacos项目主机地址
        /// </summary>
        /// <returns></returns>
        public string GetNacosUrl() => EnvironmentProvider.GetNacosUrl();

        /// <summary>
        /// 获取主机的端口地址
        /// </summary>
        /// <returns></returns>
        public static string GetHost() => EnvironmentProvider.GetHost();

        /// <summary>
        /// 获取主机的端口
        /// </summary>
        /// <returns></returns>
        public static int GetPort() => EnvironmentProvider.GetPort();

        /// <summary>
        /// 获取Http协议
        /// </summary>
        /// <returns></returns>
        public static string GetScheme() => EnvironmentProvider.GetScheme();

        /// <summary>
        /// 获取Swagger开关
        /// </summary>
        /// <returns></returns>
        public static bool GetSwaggerSwitch() => EnvironmentProvider.GetSwaggerSwitch();

        /// <summary>
        /// 获取版本迁移开关
        /// </summary>
        /// <returns></returns>
        public static bool GetMigrationSwitch() => EnvironmentProvider.GetMigrationSwitch();

        /// <summary>
        /// 获取版本升级开关
        /// </summary>
        /// <returns></returns>
        public static bool GetUpgrationSwitch() => EnvironmentProvider.GetUpgrationSwitch();

        /// <summary>
        /// 获取环境变量值
        /// </summary>
        /// <param name="name">环境变量参数名称</param>
        /// <returns></returns>
        public static T GetEnvironmentVariable<T>(string name) => EnvironmentProvider.GetEnvironmentVariable<T>(name);

        /// <summary>
        /// 获取环境变量值
        /// </summary>
        /// <param name="name">环境变量参数名称</param>
        /// <returns></returns>
        public static string GetEnvironmentVariable(string name) => EnvironmentProvider.GetEnvironmentVariable(name);

        /// <summary>
        /// 获取环境变量值
        /// </summary>
        /// <param name="name">环境变量参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T GetEnvironmentVariable<T>(string name, T defaultValue) => EnvironmentProvider.GetEnvironmentVariable<T>(name, defaultValue);
    }
}