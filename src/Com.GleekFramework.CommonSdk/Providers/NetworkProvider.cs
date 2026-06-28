using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Com.GleekFramework.CommonSdk.Extensions
{
    /// <summary>
    /// 网络实现类
    /// </summary>
    public static class NetworkProvider
    {
        /// <summary>
        /// 卡网列表
        /// </summary>
        private static IEnumerable<NetworkInterface> AllNetworkInterfaces { get; set; }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static NetworkProvider()
        {
            AllNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        }

        /// <summary>
        /// 获取Mac地址
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            var networkInterface = AllNetworkInterfaces
                .FirstOrDefault(e => e.NetworkInterfaceType == NetworkInterfaceType.Ethernet && e.OperationalStatus == OperationalStatus.Up);
            if (networkInterface == null)
            {
                return string.Empty;
            }
            else
            {
                var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
                return BitConverter.ToString(addressBytes);
            }
        }

        /// <summary>
        /// 获取内网IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            try
            {
                var ipAddress = "";
                var addressInfo = AllNetworkInterfaces
                    .Select(e => e.GetIPProperties())
                    .SelectMany(e => e.UnicastAddresses)
                    .Where(e => e.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(e.Address))
                    .FirstOrDefault();

                if (addressInfo != null && addressInfo.Address != null)
                {
                    //绑定Ip地址
                    ipAddress = $"{addressInfo.Address}";
                }

                if (string.IsNullOrEmpty(ipAddress))
                {
                    var hoostAddresses = Dns.GetHostAddresses(Dns.GetHostName())
                    .Where(e => e.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(e))
                    .FirstOrDefault();

                    if (hoostAddresses != null)
                    {
                        //绑定Ip地址
                        ipAddress = $"{hoostAddresses}";
                    }
                }
                return ipAddress;
            }
            catch
            {

            }
            return "127.0.0.1";
        }
    }
}