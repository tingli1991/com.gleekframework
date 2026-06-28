using Com.GleekFramework.CommonSdk;
using System;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 每次都会执行的迁移特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AlwaysUpgrationAttribute : UpgrationAttribute
    {
        /// <summary>
        /// /构造函数
        /// </summary>
        public AlwaysUpgrationAttribute() : base(long.Parse(DateTime.Now.ToCstTime().ToString("yyyyMMddHHmmssfff")))
        {

        }
    }
}