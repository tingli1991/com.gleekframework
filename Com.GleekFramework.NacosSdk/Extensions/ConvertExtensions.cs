using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.ConfigSdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// 数据转换扩展
    /// </summary>
    internal static partial class ConvertExtensions
    {
        /// <summary>
        /// 获取服务地址
        /// </summary>
        /// <param name="serverAddresses"></param>
        /// <returns></returns>
        public static string GetServerAddressesUrl(this IEnumerable<string> serverAddresses)
        {
            var nacosUrl = EnvironmentProvider.GetNacosUrl();//Nacos环境变量传入的参数
            if (!string.IsNullOrEmpty(nacosUrl))
            {
                return nacosUrl;
            }
            else
            {
                if (serverAddresses.IsNullOrEmpty())
                {
                    throw new ArgumentNullException("serverAddresses");
                }
                return serverAddresses.Next();
            }
        }

        /// <summary>
        /// 获取服务节点配置
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static ServiceClientOptions GetServiceOptions(this string serviceName)
        {
            var serviceSettings = NacosProvider.Settings.ServiceSettings;//服务配置列表
            var serviceOptions = serviceSettings.ClientOptions.FirstOrDefault(e => e.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase));
            var groupName = string.IsNullOrEmpty(serviceOptions?.GroupName) ? serviceSettings.GroupName : serviceOptions.GroupName;
            var namespaceId = string.IsNullOrEmpty(serviceOptions?.NamespaceId) ? serviceSettings.NamespaceId : serviceOptions.NamespaceId;
            if (string.IsNullOrEmpty(groupName))
            {
                groupName = NacosProvider.Settings.GroupName;
            }
            if (string.IsNullOrEmpty(namespaceId))
            {
                namespaceId = NacosProvider.Settings.NamespaceId;
            }

            return new ServiceClientOptions()
            {
                GroupName = groupName,
                Clusters = serviceOptions.Clusters,
                ServiceName = serviceOptions.ServiceName,
                NamespaceId = namespaceId.TrimStart(ConfigConstant.REMOVESTARTSTR)
            };
        }

        /// <summary>
        /// 转换服务删除接口请求参数
        /// </summary>
        /// <param name="nacosConf">配置信息</param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static RemoveInstanceRequestParam ToRemoveInstanceRequest(this NacosSettings nacosConf, string serviceName, string token)
        {
            var serviceConf = nacosConf.ServiceSettings;
            var rui = serviceConf.GetUri();
            var param = new RemoveInstanceRequestParam()
            {
                Ip = rui.Host,
                Port = rui.Port,
                Ephemeral = true,
                Token = token,
                ServiceName = serviceName,
                ClusterName = serviceConf.ClusterName,
                GroupName = nacosConf.GroupName ?? ConfigConstant.DEFAULT_GROUP,
                NamespaceId = nacosConf.NamespaceId.TrimStart(ConfigConstant.REMOVESTARTSTR)
            };

            if (serviceConf.PrivateService)
            {
                param.GroupName = serviceConf.GroupName ?? ConfigConstant.DEFAULT_GROUP;
                param.NamespaceId = serviceConf.NamespaceId.TrimStart(ConfigConstant.REMOVESTARTSTR);
            }
            return param;
        }

        /// <summary>
        /// 转换服务端发送心跳请求参数
        /// </summary>
        /// <param name="nacosConf"></param>
        /// <param name="serviceName">服务名称</param>
        /// <param name="token">token</param>
        public static SendHeartbeatRequestParam ToSendHeartbeatRequest(this NacosSettings nacosConf, string serviceName, string token)
        {
            var serviceConf = nacosConf.ServiceSettings;
            var addressInfo = nacosConf.ServiceSettings.GetUri();
            var param = new SendHeartbeatRequestParam()
            {
                Metadata = "",
                Token = token,
                Ephemeral = true,
                Ip = addressInfo.Host,
                Port = addressInfo.Port,
                ServiceName = serviceName,
                Weight = serviceConf.Weight,
                ClusterName = serviceConf.ClusterName,
                GroupName = nacosConf.GroupName ?? ConfigConstant.DEFAULT_GROUP,
                NamespaceId = nacosConf.NamespaceId.TrimStart(ConfigConstant.REMOVESTARTSTR)
            };

            if (serviceConf.PrivateService)
            {
                param.GroupName = serviceConf.GroupName ?? ConfigConstant.DEFAULT_GROUP;
                param.NamespaceId = serviceConf.NamespaceId.TrimStart(ConfigConstant.REMOVESTARTSTR);
            }
            return param;
        }

        /// <summary>
        /// 转换服务端请求参数
        /// </summary>
        /// <param name="nacosConf">配置信息</param>
        /// <param name="serverName">服务名称</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static RegisterInstanceRequestParam ToRegisterInstanceRequest(this NacosSettings nacosConf, string serverName, string token)
        {
            var serviceConf = nacosConf.ServiceSettings;
            Uri rui = serviceConf.GetUri();
            var param = new RegisterInstanceRequestParam()
            {
                Enable = true,
                Metadata = "",
                Ip = rui.Host,
                Port = rui.Port,
                Healthy = true,
                Ephemeral = true,
                Token = token,
                ServiceName = serverName,
                Weight = serviceConf.Weight,
                ClusterName = serviceConf.ClusterName,
                GroupName = nacosConf.GroupName ?? ConfigConstant.DEFAULT_GROUP,
                NamespaceId = nacosConf.NamespaceId.TrimStart(ConfigConstant.REMOVESTARTSTR)
            };

            if (serviceConf.PrivateService)
            {
                param.GroupName = serviceConf.GroupName ?? ConfigConstant.DEFAULT_GROUP;
                param.NamespaceId = serviceConf.NamespaceId.TrimStart(ConfigConstant.REMOVESTARTSTR);
            }
            return param;
        }

        /// <summary>
        /// 配置转换
        /// </summary>
        /// <param name="configNode"></param>
        /// <param name="nacosConf"></param>
        /// <returns></returns>
        public static ConfigOptionSettings ToParamConfigNode(this ConfigOptionSettings configNode, NacosSettings nacosConf)
        {
            var nodeSettings = new ConfigOptionSettings()
            {
                DataId = configNode.DataId,
                ConfigName = configNode.ConfigName,
                ConfigPath = configNode.ConfigPath,
                GroupName = nacosConf.GroupName ?? ConfigConstant.DEFAULT_GROUP,
                NamespaceId = nacosConf.NamespaceId.TrimStart(ConfigConstant.REMOVESTARTSTR)
            };

            if (!string.IsNullOrEmpty(configNode.NamespaceId))
            {
                //绑定节点自己的命名空间
                nodeSettings.NamespaceId = configNode.NamespaceId.TrimStart(ConfigConstant.REMOVESTARTSTR);
            }

            if (!string.IsNullOrEmpty(configNode.GroupName))
            {
                //绑定节点自己的分组名称
                nodeSettings.GroupName = configNode.GroupName ?? ConfigConstant.DEFAULT_GROUP;
            }
            return nodeSettings;
        }

        /// <summary>
        /// 转换文件名称
        /// </summary>
        /// <param name="configNode">配置节点</param>
        /// <param name="configSettings">配置信息</param>
        /// <returns></returns>
        public static string GetFileName(this ConfigOptionSettings configNode, ConfigSettings configSettings)
        {
            string configPath;
            if (configNode.PrivatePath)
            {
                //自定义路径&绝对路径
                configPath = configNode.ConfigPath ?? "";
            }
            else
            {
                //非相对路径&绝对路径
                configPath = configSettings.ConfigPath ?? "";
            }

            //自定义路径&相对路径
            if (configSettings.RelativePath)
            {
                //拼接目录
                configPath = configPath.TrimEnd('/').TrimEnd('\\');//处理掉后面的斜杠
                configPath = configPath.TrimStart('/').TrimStart('\\');//处理掉前面的斜杠
                configPath = Path.Combine(AppContext.BaseDirectory, configPath);
            }

            if (!string.IsNullOrEmpty(configPath) && !Directory.Exists(configPath))
            {
                //目录如果不存在则直接创建
                Directory.CreateDirectory(configPath);
            }

            var fileName = string.IsNullOrEmpty(configNode.ConfigName) ? configNode.DataId : configNode.ConfigName;
            return Path.Combine(configPath, fileName).CheckAndCreateDefaultFile();
        }

        /// <summary>
        /// 检查并创建默认文件
        /// </summary>
        /// <param name="fileName">文件名称(全路径)</param>
        public static string CheckAndCreateDefaultFile(this string fileName)
        {
            if (File.Exists(fileName))
                return fileName;

            using FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            byte[] bs = Encoding.UTF8.GetBytes("{}");
            fs.Write(bs, 0, bs.Length);
            fs.Close();
            return fileName;
        }
    }
}