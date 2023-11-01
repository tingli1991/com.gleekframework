namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 定义表的版本迁移接口
    /// </summary>
    public interface IMigrationTable
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
    }
}