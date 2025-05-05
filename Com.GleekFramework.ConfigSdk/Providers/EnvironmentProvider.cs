using Com.GleekFramework.CommonSdk;
using System;
using System.Collections.Generic;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 环境变量
    /// </summary>
    public static class EnvironmentProvider
    {
        /// <summary>
        /// 线程对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// 环境变量缓存
        /// </summary>
        private static readonly Dictionary<string, string> CacheDic = new Dictionary<string, string>();

        /// <summary>
        /// 获取环境值
        /// </summary>
        /// <returns></returns>
        public static string GetEnv() => GetEnvironmentVariable(EnvironmentConstant.ENV);

        /// <summary>
        /// 获取项目名称
        /// </summary>
        /// <returns></returns>
        public static string GetProject() => GetEnvironmentVariable(EnvironmentConstant.PROJECT);

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public static string GetVersion() => GetEnvironmentVariable(EnvironmentConstant.VERSION);

        /// <summary>
        /// 获取Nacos项目主机地址
        /// </summary>
        /// <returns></returns>
        public static string GetNacosUrl() => GetEnvironmentVariable(EnvironmentConstant.NOCOS_URL);

        /// <summary>
        /// 获取主机的端口地址
        /// </summary>
        /// <returns></returns>
        public static string GetHost() => $"{GetScheme()}://*:{GetPort()}";

        /// <summary>
        /// 获取主机的端口
        /// </summary>
        /// <returns></returns>
        public static string GetPort() => $"{GetEnvironmentVariable(EnvironmentConstant.PORT) ?? "8080"}";

        /// <summary>
        /// 获取Http协议
        /// </summary>
        /// <returns></returns>
        public static string GetScheme() => $"{GetEnvironmentVariable(EnvironmentConstant.SCHEME) ?? "http"}";

        /// <summary>
        /// 获取Swagger开关
        /// </summary>
        /// <returns></returns>
        public static bool GetSwaggerSwitch() => GetEnvironmentVariable<bool>(EnvironmentConstant.SWAGGER_SWITCH);

        /// <summary>
        /// 获取版本迁移开关
        /// </summary>
        /// <returns></returns>
        public static bool GetMigrationSwitch() => GetEnvironmentVariable<bool>(EnvironmentConstant.MIGRATION_SWITCH);

        /// <summary>
        /// 获取版本升级开关
        /// </summary>
        /// <returns></returns>
        public static bool GetUpgrationSwitch() => GetEnvironmentVariable<bool>(EnvironmentConstant.UPGRATION_SWITCH);

        /// <summary>
        /// 获取环境变量值
        /// </summary>
        /// <param name="name">环境变量参数名称</param>
        /// <returns></returns>
        public static T GetEnvironmentVariable<T>(string name)
        {
            var environmentVariable = GetEnvironmentVariable(name);
            return environmentVariable.ToObject<T>();
        }

        /// <summary>
        /// 获取环境变量值
        /// </summary>
        /// <param name="name">环境变量参数名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T GetEnvironmentVariable<T>(string name, T defaultValue)
        {
            var environmentVariable = GetEnvironmentVariable(name);
            if (environmentVariable.IsNullOrEmpty())
            {
                // 未找到环境变量，直接给默认值
                return defaultValue;
            }
            return environmentVariable.ToObject<T>();
        }

        /// <summary>
        /// 获取环境变量值
        /// </summary>
        /// <param name="name">环境变量参数名称</param>
        /// <returns></returns>
        public static string GetEnvironmentVariable(string name)
        {
            if (!CacheDic.ContainsKey(name))
            {
                lock (@lock)
                {
                    if (!CacheDic.ContainsKey(name))
                    {
                        //获取环境变量值
                        var environmentVariable = Environment.GetEnvironmentVariable(name);

                        //绑定环境变量值
                        CacheDic.Add(name, environmentVariable ?? "");
                    }
                }
            }
            return CacheDic[name];
        }
    }
}