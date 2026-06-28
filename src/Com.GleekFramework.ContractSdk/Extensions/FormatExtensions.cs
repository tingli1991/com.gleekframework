using Com.GleekFramework.CommonSdk;
using System.Text.RegularExpressions;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 格式化拓展类
    /// </summary>
    public static class FormatExtensions
    {
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string SafeFormat(this string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
            {
                return format;
            }

            if (!args.AnyOf())
            {
                return format;
            }

            return Regex.Replace(format, @"{(\d+)(?:,.*?)?}", match =>
            {
                int index = int.Parse(match.Groups[1].Value);
                return index < args.Length
                    ? (args[index]?.ToString() ?? "")
                    : match.Value; // 保留无参数的占位符
            });
        }
    }
}