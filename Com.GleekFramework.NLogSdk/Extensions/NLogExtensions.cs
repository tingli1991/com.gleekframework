﻿using System;

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
        /// <param name="toMillisecond"></param>
        /// <returns></returns>
        public static string Format(this DateTime content, bool toMillisecond)
        {
            if (content != null)
            {
                if (!toMillisecond)
                {
                    return content.ToString("yyyy-MM-dd HH:mm:ss.ffff");
                }
                else
                {
                    return content.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            return string.Empty;
        }
    }
}