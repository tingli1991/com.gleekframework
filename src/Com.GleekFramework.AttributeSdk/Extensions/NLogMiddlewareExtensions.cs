using Microsoft.AspNetCore.Builder;
using NLog;

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
        public static LogLevel Level { get; set; }

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
        public static IApplicationBuilder UseNLogMiddleware(this IApplicationBuilder app, LogLevel level = null, bool isSaveContent = true, IEnumerable<string> exclActionList = null, params object[] args)
        {
            ExclActionList = exclActionList;
            Level = level ?? LogLevel.Trace;//记录日志的级别
            app.UseMiddleware<NLogMiddleware>(args);//接口日志收集
            return app;
        }
    }
}