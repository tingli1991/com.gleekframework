using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 版本号扩展
    /// </summary>
    public static partial class VersionExtensions
    {
        /// <summary>
        /// 获取最大版本号
        /// </summary>
        /// <param name="versions">版本列表</param>
        /// <returns></returns>
        public static Version MaxVervion(this IEnumerable<Version> versions)
        {
            Version maxVersion = null;
            if (versions.IsNullOrEmpty())
                return maxVersion;

            foreach (var version in versions)
            {
                if (version == null)
                    continue;

                if (maxVersion == null)
                {
                    maxVersion = version;
                }
                else
                {
                    var newVersion = MaxVervion(maxVersion, version);
                    if (newVersion != null)
                    {
                        maxVersion = newVersion;
                    }
                }
            }
            return maxVersion;
        }

        /// <summary>
        /// 获取最大版本号
        /// </summary>
        /// <param name="versions">版本列表</param>
        /// <returns></returns>
        public static string MaxVervion(this IEnumerable<string> versions)
        {
            string maxVersion = null;
            if (versions.IsNullOrEmpty())
                return maxVersion;

            foreach (var version in versions)
            {
                if (string.IsNullOrEmpty(version))
                    continue;

                if (maxVersion == null)
                {
                    maxVersion = version;
                }
                else
                {
                    var newVersion = MaxVervion(maxVersion, version);
                    if (!string.IsNullOrEmpty(newVersion))
                    {
                        maxVersion = newVersion;
                    }
                }
            }
            return maxVersion;
        }

        /// <summary>
        /// 获取最小版本号
        /// </summary>
        /// <param name="versions">版本列表</param>
        /// <returns></returns>
        public static Version MinVervion(this IEnumerable<Version> versions)
        {
            Version maxVersion = null;
            if (versions.IsNullOrEmpty())
                return maxVersion;

            foreach (var version in versions)
            {
                if (version == null)
                    continue;

                if (maxVersion == null)
                {
                    maxVersion = version;
                }
                else
                {
                    var newVersion = MinVervion(maxVersion, version);
                    if (newVersion != null)
                    {
                        maxVersion = newVersion;
                    }
                }
            }
            return maxVersion;
        }

        /// <summary>
        /// 获取最小版本号
        /// </summary>
        /// <param name="versions">版本列表</param>
        /// <returns></returns>
        public static string MinVervion(this IEnumerable<string> versions)
        {
            string maxVersion = "";
            if (versions.IsNullOrEmpty())
                return maxVersion;

            foreach (var version in versions)
            {
                if (string.IsNullOrEmpty(version))
                    continue;

                if (string.IsNullOrEmpty(maxVersion))
                {
                    maxVersion = version;
                }
                else
                {
                    var newVersion = MinVervion(maxVersion, version);
                    if (!string.IsNullOrEmpty(newVersion))
                    {
                        maxVersion = newVersion;
                    }
                }
            }
            return maxVersion;
        }

        /// <summary>
        /// 获取最大的版本号
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns></returns>
        public static string MaxVervion(this string version1, string version2)
        {
            if (string.IsNullOrEmpty(version1) && string.IsNullOrEmpty(version2))
                return "";

            var isSuccess = version1.Compare(version2, VersionCompare.GTE);
            return isSuccess ? version1 : version2;
        }

        /// <summary>
        /// 获取最大的版本号
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns></returns>
        public static Version MaxVervion(this Version version1, Version version2)
        {
            if (version1 == null && version2 == null)
                return null;

            var isSuccess = version1.Compare(version2, VersionCompare.GTE);
            return isSuccess ? version1 : version2;
        }

        /// <summary>
        /// 获取最小的版本号
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns></returns>
        public static string MinVervion(this string version1, string version2)
        {
            if (string.IsNullOrEmpty(version1) && string.IsNullOrEmpty(version2))
                return "";

            var isSuccess = version1.Compare(version2, VersionCompare.LTE);
            return isSuccess ? version1 : version2;
        }

        /// <summary>
        /// 获取最小的版本号
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns></returns>
        public static Version MinVervion(this Version version1, Version version2)
        {
            if (version1 == null && version2 == null)
                return null;

            var isSuccess = version1.Compare(version2, VersionCompare.LTE);
            return isSuccess ? version1 : version2;
        }

        /// <summary>
        /// 版本比较
        /// </summary>
        /// <param name="version1">旧版本</param>
        /// <param name="version2">新版本</param>
        /// <param name="compareWay">对比方式</param>
        /// <returns></returns>
        public static bool Compare(this Version version1, Version version2, VersionCompare compareWay)
        {
            var isSuccess = false;
            if (version1 == null && version2 == null)
                return false;

            if (version1 == null)
            {
                //赋值成为默认值
                version1 = new Version("0.0.0");
            }

            if (version2 == null)
            {
                //赋值成为默认值
                version2 = new Version("0.0.0");
            }

            switch (compareWay)
            {
                case VersionCompare.EQ:
                    isSuccess = version1 == version2;
                    break;
                case VersionCompare.NQ:
                    isSuccess = version1 != version2;
                    break;
                case VersionCompare.LT:
                    isSuccess = version1 < version2;
                    break;
                case VersionCompare.GT:
                    isSuccess = version1 > version2;
                    break;
                case VersionCompare.LTE:
                    isSuccess = version1 <= version2;
                    break;
                case VersionCompare.GTE:
                    isSuccess = version1 >= version2;
                    break;
            }
            return isSuccess;
        }

        /// <summary>
        /// 版本比较
        /// </summary>
        /// <param name="version1">旧版本</param>
        /// <param name="version2">新版本</param>
        /// <param name="compareWay">对比方式</param>
        /// <returns></returns>
        public static bool Compare(this string version1, string version2, VersionCompare compareWay)
        {
            var isSuccess = false;
            if (string.IsNullOrEmpty(version1) && string.IsNullOrEmpty(version2))
                return isSuccess;

            if (string.IsNullOrEmpty(version1))
            {
                version1 = "0.0.0";
            }

            if (string.IsNullOrEmpty(version2))
            {
                version2 = "0.0.0";
            }

            if (version1.IsNumber() || version2.IsNumber())
            {
                var ver1 = int.Parse(version1);
                var ver2 = int.Parse(version2);
                switch (compareWay)
                {
                    case VersionCompare.EQ:
                        isSuccess = ver1 == ver2;
                        break;
                    case VersionCompare.NQ:
                        isSuccess = ver1 != ver2;
                        break;
                    case VersionCompare.LT:
                        isSuccess = ver1 < ver2;
                        break;
                    case VersionCompare.GT:
                        isSuccess = ver1 > ver2;
                        break;
                    case VersionCompare.LTE:
                        isSuccess = ver1 <= ver2;
                        break;
                    case VersionCompare.GTE:
                        isSuccess = ver1 >= ver2;
                        break;
                }
            }
            else
            {
                var ver1 = new Version(version1);
                var ver2 = new Version(version2);
                switch (compareWay)
                {
                    case VersionCompare.EQ:
                        isSuccess = ver1 == ver2;
                        break;
                    case VersionCompare.NQ:
                        isSuccess = ver1 != ver2;
                        break;
                    case VersionCompare.LT:
                        isSuccess = ver1 < ver2;
                        break;
                    case VersionCompare.GT:
                        isSuccess = ver1 > ver2;
                        break;
                    case VersionCompare.LTE:
                        isSuccess = ver1 <= ver2;
                        break;
                    case VersionCompare.GTE:
                        isSuccess = ver1 >= ver2;
                        break;
                }
            }
            return isSuccess;
        }
    }
}