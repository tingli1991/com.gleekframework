using Com.GleekFramework.CommonSdk;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// JSON配置文件实现类
    /// </summary>
    public static partial class JsonConfigProvider
    {
        /// <summary>
        /// 线程对象锁
        /// </summary>
        private static readonly object @lock = new object();

        /// <summary>
        /// Json配置缓存信息
        /// </summary>
        private static readonly ConcurrentDictionary<string, IConfiguration> JsonConfigCache = new ConcurrentDictionary<string, IConfiguration>();

        /// <summary>
        /// Json配置缓存信息
        /// </summary>
        private static readonly Dictionary<string, IConfigurationBuilder> ConfigurationBuilderCache = new Dictionary<string, IConfigurationBuilder>();

        /// <summary>
        /// 从缓存中获取配置对象
        /// </summary>
        /// <returns></returns>
        public static IConfiguration GetConfiguration(string configurationName = ConfigConstant.DEFAULT_CONFIGURATION_NAME)
        {
            if (!JsonConfigCache.ContainsKey(configurationName))
            {
                throw new ArgumentNullException(nameof(configurationName));
            }
            return JsonConfigCache[configurationName];
        }

        /// <summary>
        /// 获取JSON配置信息
        /// </summary>
        /// <param name="configurationName">配置文件名称</param>
        /// <param name="fileNames">文件名称</param>
        /// <returns></returns>
        public static IConfiguration GetJsonConfiguration(string configurationName, string[] fileNames)
        {
            if (fileNames.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(fileNames));
            }

            var configuration = CreateConfigurationRoot(configurationName, fileNames);
            configuration.GetReloadToken().RegisterChangeCallback(config => JsonConfigWatcher.OnChangeConfigCallback((IConfiguration)config, configurationName, fileNames), configuration);
            return configuration;
        }

        /// <summary>
        /// 创建JSON配置信息
        /// </summary>
        /// <param name="configurationName">配置文件名称</param>
        /// <param name="fileNames">文件名称</param>
        /// <returns></returns>
        private static IConfiguration CreateConfigurationRoot(string configurationName, string[] fileNames)
        {
            if (string.IsNullOrEmpty(configurationName))
            {
                //归类到默认的配置文件组
                configurationName = ConfigConstant.DEFAULT_CONFIGURATION_NAME;
            }

            var configBuilder = GetConfigurationBuilder(configurationName);//获取配置生成器
            var configurationRoot = CreateConfigurationRoot(configBuilder, fileNames);//构建配置文件对象
            return JsonConfigCache.AddOrUpdate(configurationName, configurationRoot, (key, oldValue) => oldValue = configurationRoot);
        }

        /// <summary>
        /// 构建配置文件对象
        /// </summary>
        /// <param name="configBuilder"></param>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        private static IConfigurationRoot CreateConfigurationRoot(IConfigurationBuilder configBuilder, string[] fileNames)
        {
            IConfigurationBuilder configurationBuilder = null;
            foreach (var fileName in fileNames)
            {
                configurationBuilder = JsonConfigurationExtensions
                    .AddJsonFile(configBuilder, fileName, true, true);
            }

            if (configurationBuilder == null)
            {
                throw new NullReferenceException(nameof(configurationBuilder));
            }
            var configuration = configurationBuilder.Build();//编译配置文件
            return configuration;
        }

        /// <summary>
        /// 获取配置生成器
        /// </summary>
        /// <param name="configurationName">配置文件名称</param>
        /// <returns></returns>
        private static IConfigurationBuilder GetConfigurationBuilder(string configurationName)
        {
            if (!ConfigurationBuilderCache.ContainsKey(configurationName))
            {
                lock (@lock)
                {
                    if (!ConfigurationBuilderCache.ContainsKey(configurationName))
                    {
                        var builder = new ConfigurationBuilder();
                        var configBuilder = FileConfigurationExtensions.SetBasePath(builder, ConfigConstant.DEFAULT_FILE_DIR);//设置配置文件基础目录
                        ConfigurationBuilderCache.Add(configurationName, configBuilder);
                    }
                }
            }
            return ConfigurationBuilderCache[configurationName];
        }
    }
}