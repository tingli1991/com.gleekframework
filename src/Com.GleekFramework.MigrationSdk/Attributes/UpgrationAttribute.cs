using FluentMigrator;
using System;

namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 本本升级特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class UpgrationAttribute : Attribute
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public long Version { get; } = 0L;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 事务行为
        /// </summary>
        public TransactionBehavior TransactionBehavior { get; } = TransactionBehavior.Default;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpgrationAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="version"></param>
        /// <param name="description"></param>
        public UpgrationAttribute(long version, string description)
            : this(version, TransactionBehavior.Default, description)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="version"></param>
        /// <param name="transactionBehavior"></param>
        /// <param name="description"></param>
        public UpgrationAttribute(long version, TransactionBehavior transactionBehavior = TransactionBehavior.Default, string description = null)
        {
            Version = version;
            Description = description;
            TransactionBehavior = transactionBehavior;
        }
    }
}