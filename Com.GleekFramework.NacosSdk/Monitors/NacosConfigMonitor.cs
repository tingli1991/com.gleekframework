using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.NLogSdk;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 配置文件客户端监听扩展类
    /// </summary>
    static class NacosConfigMonitor
    {
        /// <summary>
        /// 心跳最小的等待时长
        /// </summary>
        private static readonly int minPulllistenInterval = 3000;

        /// <summary>
        /// 心跳上线的等待时长
        /// </summary>
        private static readonly int maxPulllistenInterval = 180000;

        /// <summary>
        /// 批量初始化配置
        /// </summary>
        /// <param name="nacosConf">Nacos系统配置参数</param>
        /// <returns></returns>
        public static void BatchInitNacos(this NacosSettings nacosConf)
        {
            if (nacosConf.ConfigSettings == null)
            {
                throw new ArgumentNullException("nacosConf.ConfigSettings");
            }

            foreach (var configNode in nacosConf.ConfigSettings.ConfitOptions)
            {
                var fileName = "";//文件名称
                try
                {
                    var baseUrl = nacosConf.ServerAddresses.GetServerAddressesUrl();//随机获取服务端配置
                    var nodeSettings = configNode.ToParamConfigNode(nacosConf);//配置信息
                    fileName = configNode.GetFileName(nacosConf.ConfigSettings);//文件名称
                    var configValue = File.ReadAllText(fileName, Encoding.UTF8) ?? "";//配置内容

                    var token = baseUrl.GetAuthTokenAsync(nacosConf.UserName, nacosConf.Password).Result;
                    var responseJsonValue = baseUrl.GetConfigAsync(nodeSettings.NamespaceId, nodeSettings.GroupName, nodeSettings.DataId, token).Result;
                    if (!responseJsonValue.IsNullOrEmpty() && configValue.EncryptMd5() != responseJsonValue.EncryptMd5())
                    {
                        //写入文件
                        File.WriteAllText(fileName, responseJsonValue, Encoding.UTF8);
                    }
                }
                catch (Exception ex)
                {
                    NLogProvider.Error($@"【Nacos配置初始化】名命空间：{configNode.NamespaceId},配置分组：{nacosConf.GroupName},DataId：{configNode.DataId},ConfigName：{configNode.ConfigName}文件名称：{fileName}，发生错误：{ex}");
                }
            }
        }

        /// <summary>
        /// 批量添加配置监听
        /// </summary>
        /// <param name="nacosConf">Nacos系统配置参数</param>
        /// <returns></returns>
        public static void BatchAddListener(this NacosSettings nacosConf)
        {
            if (nacosConf.ConfigSettings == null)
            {
                throw new ArgumentNullException("nacosConf.ConfigSettings");
            }

            foreach (var configNode in nacosConf.ConfigSettings.ConfitOptions)
            {
                Task.Run(async () =>
                {
                    while (true)
                    {
                        var fileName = "";//文件名称
                        var beginTime = DateTime.Now.ToCstTime();//当前时间
                        var pulllistenInterval = nacosConf.ListenInterval;
                        try
                        {
                            if (pulllistenInterval >= maxPulllistenInterval)
                            {
                                pulllistenInterval = maxPulllistenInterval;
                            }

                            if (pulllistenInterval <= minPulllistenInterval)
                            {
                                pulllistenInterval = minPulllistenInterval;
                            }

                            var baseUrl = nacosConf.ServerAddresses.GetServerAddressesUrl();//随机获取服务端配置
                            var nodeSettings = configNode.ToParamConfigNode(nacosConf);//配置信息
                            fileName = configNode.GetFileName(nacosConf.ConfigSettings);//文件名称
                            var configValue = File.ReadAllText(fileName) ?? "";//配置内容

                            var token = baseUrl.GetAuthTokenAsync(nacosConf.UserName, nacosConf.Password).Result;
                            var responseJsonValue = await baseUrl.AddListenerAsync(
                                nodeSettings.NamespaceId,
                                nodeSettings.GroupName,
                                nodeSettings.DataId,
                                configValue,
                                pulllistenInterval,
                                token);

                            if (!string.IsNullOrEmpty(responseJsonValue))
                            {
                                configValue = await baseUrl.GetConfigAsync(nodeSettings.NamespaceId, nodeSettings.GroupName, nodeSettings.DataId, token);
                                File.WriteAllText(fileName, configValue ?? "", Encoding.UTF8);//写入文件
                            }
                        }
                        catch (Exception ex)
                        {
                            NLogProvider.Error($@"【Nacos配置监听】名命空间：{configNode.NamespaceId},配置分组：{nacosConf.GroupName},DataId：{configNode.DataId},ConfigName：{configNode.ConfigName}文件名称：{fileName}，发生错误：{ex}");
                        }
                        finally
                        {
                            var timeStampMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;//业务处理耗时(毫秒)
                            var surplusDelayMilliseconds = (int)(pulllistenInterval - timeStampMilliseconds);//剩余等待的毫秒数
                            if (surplusDelayMilliseconds > 0)
                            {
                                //延迟等待
                                await Task.Delay(surplusDelayMilliseconds);
                            }
                        }
                    }
                });
            }
        }
    }
}