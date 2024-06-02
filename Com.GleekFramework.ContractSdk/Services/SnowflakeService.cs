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
        /// <param name="suffix">流水号前缀</param>
        /// <returns></returns>
        public string GetSerialNo(int suffix = 100)
        {
            return SnowflakeProvider.GetSerialNo(suffix);
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="suffix">流水号前缀</param>
        /// <returns></returns>
        public long GetVersionNo(int suffix = 100)
        {
            return long.Parse(GetSerialNo(suffix));
        }
    }
}