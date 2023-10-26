using MongoDB.Driver;

namespace Com.GleekFramework.MongodbSdk
{
    /// <summary>
    /// Mongodb常量
    /// </summary>
    public static class MongoConstant
    {
        /// <summary>
        /// 默认的链接字符串名称
        /// </summary>
        public const string DEFAULT_CONNECTION_NAME = "ConnectionStrings:MongoConnectionHost";

        /// <summary>
        /// 批量新增默认选项
        /// </summary>
        /// <param name="isOrdered">是否有序的插入</param>
        /// <returns></returns>
        public static InsertManyOptions GetInsertManyOptions(bool isOrdered) => new InsertManyOptions() { IsOrdered = isOrdered };
    }
}