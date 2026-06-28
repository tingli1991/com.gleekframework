using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.NLogSdk;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 文件文件监听
    /// </summary>
    internal static class JsonConfigWatcher
    {
        /// <summary>
        /// 配置文件发生变动事件
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="configurationName">配置文件名称</param>
        /// <param name="fileNames">文件名称</param>
        public static void OnChangeConfigCallback(IConfiguration configuration, string configurationName, IEnumerable<string> fileNames)
        {
            Task.Run(() =>
            {
                var beginTime = DateTime.Now.ToCstTime();
                try
                {
                    //注册(或刷新)配置特性对照的配置值
                    DependencyProvider.RefreshConfigAttribute();
                }
                catch (Exception ex)
                {
                    var totalMilliseconds = (long)(DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;
                    NLogProvider.Error($"【配置文件变更】配置文件分组名称：{configurationName}，文件名称：{string.Join(',', fileNames)}，错误：{ex}", totalMilliseconds: totalMilliseconds);
                }
            });
            configuration.GetReloadToken().RegisterChangeCallback(config => OnChangeConfigCallback((IConfiguration)config, configurationName, fileNames), configuration);
        }
    }
}