using System;

namespace Com.GleekFramework.NLogSdk
{
    /// <summary>
    /// 日志拓展
    /// </summary>
    internal static class NLogExtensions
    {
        /// <summary>
        /// 时间格式化,false = 精确到毫秒。
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Format(this DateTime content)
        {
            if (content != null)
            {
                return content.ToString("yyyy-MM-dd HH:mm:ss.ffffff");
            }
            return string.Empty;
        }
    }
}