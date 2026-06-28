using System.Collections.Generic;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 静态测试配置模型
    /// </summary>
    public class ConfigOptions
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字符串集合
        /// </summary>
        public IEnumerable<string> Arrays { get; set; }
    }
}