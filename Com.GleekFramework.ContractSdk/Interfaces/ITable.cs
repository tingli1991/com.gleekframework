namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 定义表的版本迁移接口
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
    }
}