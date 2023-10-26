using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.NLogSdk;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Com.GleekFramework.NacosSdk
{
    /// <summary>
    /// Nacos服务注册扩展
    /// </summary>
    static class NacosServiceMonitor
    {
        /// <summary>
        /// 定时器
        /// </summary>
        private static Timer timer = null;

        /// <summary>
        /// 启动心跳服务健康检查
        /// </summary>
        /// <param name="nacosConf">Nacos系统配置参数</param>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
        public static async Task StartServiceInstanceBeatCheckAsync(this NacosSettings nacosConf, string serverName)
        {
            timer = new Timer(async e => await nacosConf.SendServiceInstanceBeatAsync(serverName), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(nacosConf.ListenInterval));//发送服务实例心跳
            await Task.CompletedTask;
        }

        /// <summary>
        /// 注册服务实例
        /// </summary>
        /// <param name="serverName">服务名称</param>
        /// <param name="nacosConf"></param>
        public static async Task RegisterInstanceAsync(this NacosSettings nacosConf, string serverName)
        {
            try
            {
                if (nacosConf?.ServiceSettings == null)
                    return;

                var baseUrl = nacosConf.ServerAddresses.GetServerAddressesUrl();//随机获取服务端配置
                var token = baseUrl.GetAuthTokenAsync(nacosConf.UserName, nacosConf.Password).Result;//获取授权的token
                var param = nacosConf.ToRegisterInstanceRequest(serverName, token);
                var isSuccess = await baseUrl.RegisterInstanceAsync(param);
                var isSuccessMessageText = isSuccess ? "注册成功！" : "注册失败！";
                NLogProvider.Trace($"【Nacos服务实例注册】名命空间：{param.NamespaceId},配置分组：{param.GroupName},服务名称：{param.ServiceName},主机:{param.Ip}:{param.Port},注册结果：{isSuccessMessageText}");
            }
            catch (Exception ex)
            {
                NLogProvider.Trace($"【Nacos服务实例注册】发生未经处理的异常：{ex}");
            }
        }

        /// <summary>
        /// 注销服务实例
        /// </summary>
        /// <param name="nacosConf">配置信息</param>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
        public static async Task RemoveServiceInstanceAsync(this NacosSettings nacosConf, string serverName)
        {
            try
            {
                var baseUrl = nacosConf.ServerAddresses.GetServerAddressesUrl();//随机获取服务端配置
                var token = baseUrl.GetAuthTokenAsync(nacosConf.UserName, nacosConf.Password).Result;//获取授权的token
                var param = nacosConf.ToRemoveInstanceRequest(serverName, token);
                var isSuccess = await baseUrl.RemoveServiceInstanceAsync(param);
                timer.Change(Timeout.Infinite, 0);
                timer.Dispose();
                var isSuccessMessageText = isSuccess ? "注销成功！" : "注销失败！";
                NLogProvider.Trace($"【Nacos注销服务实例】名命空间：{param.NamespaceId},配置分组：{param.GroupName},服务名称：{param.ServiceName},主机:{param.Ip}:{param.Port},注销结果：{isSuccessMessageText}");
            }
            catch (Exception ex)
            {
                NLogProvider.Trace($"【Nacos注销服务实例】发生未经处理的异常：{ex}");
            }
        }

        /// <summary>
        /// 发送心跳间隔
        /// </summary>
        /// <param name="nacosConf">配置信息</param>
        /// <param name="serverName">服务名称</param>
        /// <returns></returns>
        private static async Task SendServiceInstanceBeatAsync(this NacosSettings nacosConf, string serverName)
        {
            try
            {
                if (nacosConf?.ServiceSettings == null)
                {
                    return;
                }
                var beginTime = DateTime.Now.ToCstTime();//当前时间
                NLogProvider.Warn($"【心跳时间】{beginTime}");
                var baseUrl = nacosConf.ServerAddresses.GetServerAddressesUrl();//随机获取服务端配置
                var token = baseUrl.GetAuthTokenAsync(nacosConf.UserName, nacosConf.Password).Result;//获取授权的token
                var param = nacosConf.ToSendHeartbeatRequest(serverName, token);
                var isSuccess = await baseUrl.SendHeartbeatAsync(param);
                if (!isSuccess)
                {
                    //提示警告
                    NLogProvider.Warn($"【Nacos发送心跳间隔】名命空间：{param.NamespaceId},配置分组：{param.GroupName},服务名称：{param.ServiceName},主机:{param.Ip}:{param.Port}，心跳发送失败！");
                }

                var timeStampMilliseconds = (DateTime.Now.ToCstTime() - beginTime).TotalMilliseconds;//业务处理耗时(毫秒)
                var surplusDelayMilliseconds = (int)(nacosConf.ListenInterval - timeStampMilliseconds);//剩余等待的毫秒数
                if (surplusDelayMilliseconds > 0)
                {
                    //延迟等待
                    await Task.Delay(surplusDelayMilliseconds);
                }
            }
            catch (Exception ex)
            {
                NLogProvider.Trace($"【Nacos发送心跳间隔】发生未经处理的异常：{ex}");
            }
        }
    }
}