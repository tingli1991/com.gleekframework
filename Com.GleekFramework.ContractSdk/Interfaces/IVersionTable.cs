using Com.GleekFramework.CommonSdk;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 版本号
    /// </summary>
    public interface IVersionTable
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [Column("version")]
        [Comment("版本号")]
        public long Version { get; set; }
    }
}