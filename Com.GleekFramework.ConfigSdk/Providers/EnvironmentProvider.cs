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
        /// 环境变量缓存
        /// </summary>
        private static readonly Dictionary<string, string> CacheDic = new Dictionary<string, string>();

        /// <summary>
        /// 获取swagger开关配置
        /// </summary>
        /// <returns></returns>
        public static string GetSwaggerSwitch() => GetEnvironmentVariable(EnvironmentConstant.SWAGGER_SWITCH);

        /// <summary>
        /// 获取主机的端口地址
        /// </summary>
        /// <returns></returns>
        public static string GetHost() => $"http://*:{GetEnvironmentVariable(EnvironmentConstant.PORT) ?? "8080"}";

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public static string GetVersion(string prefix) => $"{prefix}_{GetEnvironmentVariable(EnvironmentConstant.VERSION)}".TrimEnd('_');

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