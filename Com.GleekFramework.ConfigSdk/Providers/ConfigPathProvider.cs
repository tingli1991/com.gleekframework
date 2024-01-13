using Com.GleekFramework.CommonSdk;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Com.GleekFramework.ConfigSdk
{
    /// <summary>
    /// 配置文件路径实现类
    /// </summary>
    public static class ConfigPathProvider
    {
        /// <summary>
        /// 获取指定环境变量下的配置文件名称
        /// </summary>
        /// <param name="fileNames">配置文件名称</param>
        /// <returns></returns>
        public static IEnumerable<string> GetEnvironmentFileNames(params string[] fileNames)
        {
            var fileNameList = new List<string>();
            if (fileNames.IsNullOrEmpty())
            {
                return fileNameList;
            }

            foreach (var name in fileNames)
            {
                var fileName = name;//文件名称
                if (string.IsNullOrEmpty(name))
                {
                    //使用默认配置文件名称
                    fileName = ConfigConstant.APP_CONFIG_FILENAME;
                }

                var envVariable = EnvironmentProvider.GetEnv();
                if (!string.IsNullOrEmpty(envVariable))
                {
                    //绑定环境
                    fileName = $"{fileName.Replace(".json", "")}.{envVariable}.json";
                }

                var projectVariable = EnvironmentProvider.GetProject();
                if (!string.IsNullOrEmpty(projectVariable))
                {
                    //绑定项目名称
                    fileName = $"{fileName.Replace(".json", "")}.{projectVariable}.json";
                }
                fileNameList.Add(Path.Combine(ConfigConstant.DEFAULT_CONFIG, fileName));
            }
            return fileNameList.Distinct();
        }
    }
}