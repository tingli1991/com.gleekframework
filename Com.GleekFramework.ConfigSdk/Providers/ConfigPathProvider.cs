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
            if (fileNames == null || !fileNames.Any())
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

                var environmentVariable = EnvironmentProvider.GetEnvironmentVariable(EnvironmentConstant.ENV);
                if (!string.IsNullOrEmpty(environmentVariable))
                {
                    fileName = $"{fileName.Replace(".json", "")}.{environmentVariable}.json";
                }
                fileNameList.Add(Path.Combine(ConfigConstant.DEFAULT_CONFIG, fileName));
            }
            return fileNameList.Distinct();
        }
    }
}