using Com.GleekFramework.AutofacSdk;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 雪花算法实现类
    /// </summary>
    public partial class SnowflakeService : IBaseAutofac
    {
        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <returns></returns>
        public string GetSerialNo()
        {
            return SnowflakeProvider.GetSerialNo();
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        public decimal GetVersionNo()
        {
            return SnowflakeProvider.GetVersionNo();
        }
    }
}