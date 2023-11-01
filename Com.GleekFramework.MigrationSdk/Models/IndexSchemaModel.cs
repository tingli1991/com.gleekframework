namespace Com.GleekFramework.MigrationSdk
{
    /// <summary>
    /// 数据库索引摘要信息
    /// </summary>
    public class IndexSchemaModel
    {
        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 索引名称
        /// </summary>
        public string IndexName { get; set; }
    }
}