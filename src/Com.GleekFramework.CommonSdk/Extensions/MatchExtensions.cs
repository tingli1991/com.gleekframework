using System.Text.RegularExpressions;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// Match拓展类
    /// </summary>
    public static partial class MatchExtensions
    {
        /// <summary>
        /// 清理数据库名称
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static string ClearDatabaseName(this string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return "";
            }
            return Regex.Replace(connectionString, @"(?i)(Database=)[^;]+;", "");
        }

        /// <summary>
        /// 提取数据库名称
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string ExtractDatabaseName(this string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return "";
            }

            var pattern = @"(?i)Database=([^;]+)";
            var match = Regex.Match(connectionString, pattern);
            if (!match.Success)
            {
                return "";
            }

            if (match.Groups == null || match.Groups.Count <= 0)
            {
                return "";
            }

            return match.Groups[1].Value;
        }
    }
}