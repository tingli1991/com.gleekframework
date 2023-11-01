using Com.GleekFramework.CommonSdk;
using Com.GleekFramework.MigrationSdk;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.GleekFramework.Models
{
    /// <summary>
    /// 版本信息表
    /// </summary>
    [Comment("版本信息表")]
    [Table("migration_version")]
    [Index("idx_migration_version_update_time", nameof(UpdateTime))]
    [Index("idx_migration_version_version", nameof(Version), nameof(UpdateTime))]
    public class MigrationVersion : MigrationTable
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [MaxLength(50)]
        [Comment("版本号")]
        [Column("version")]
        public string Version { get; set; }
    }
}