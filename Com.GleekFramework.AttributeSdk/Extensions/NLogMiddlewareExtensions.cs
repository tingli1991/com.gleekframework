using Microsoft.AspNetCore.Builder;
using NLog;
using System.Collections.Generic;

namespace Com.GleekFramework.AttributeSdk
{
    /// <summary>
    /// 日志收集扩展
    /// </summary>
    public static class NLogMiddlewareExtensions
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public static LogLevel Level { get; set; } = null;

        /// <summary>
        /// 是否记录Content
        /// </summary>
        public static bool IsSaveContent { get; set; } = true;

        /// <summary>
        /// 排除的接口路径配置
        /// </summary>
        public static IEnumerable<string> ExclActionList { get; set; }

        /// <summary>
        /// 接口日志收集
        /// </summary>
        /// <param name="app"></param>
        /// <param name="level">日志级别</param>
        /// <param name="isSaveContent">是否保存Content字段</param>
        /// <param name="exclActionList"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseNLogMiddleware(this IApplicationBuilder app, LogLevel level, bool isSaveContent = true, IEnumerable<string> exclActionList = null, params object[] args)
        {
            Level = level;
            IsSaveContent = isSaveContent;
            ExclActionList = exclActionList;
            app.UseMiddleware<NLogMiddleware>(args);//接口日志收集
            return app;
        }
    }
}