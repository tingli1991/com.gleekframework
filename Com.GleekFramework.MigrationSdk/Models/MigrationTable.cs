using Com.GleekFramework.CommonSdk;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 基础表
    /// </summary>
    public class MigrationTable : IMigrationTable
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [Column("id")]
        [Comment("主键")]
        public long Id { get; set; }

        /// <summary>
        /// 死否删除
        /// </summary>
        [Comment("是否删除")]
        [Column("id_deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 透传字段
        /// </summary>
        [MaxLength(500)]
        [Comment("透传字段")]
        [Column("extend")]
        public string Extend { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Comment("更新时间")]
        [Column("update_time")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Comment("创建时间")]
        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        [Comment("备注")]
        [Column("remark")]
        public string Remark { get; set; }
    }
}