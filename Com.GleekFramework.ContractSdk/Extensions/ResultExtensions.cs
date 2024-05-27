using Com.GleekFramework.CommonSdk;
using System;

namespace Com.GleekFramework.ContractSdk
{
    /// <summary>
    /// 通用返回模型
    /// </summary>
    public static partial class ResultExtensions
    {
        /// <summary>
        /// 检查是否成功
        /// </summary>
        /// <param name="source">数据源</param>
        /// <returns></returns>
        public static bool IsSuceccful(this ContractResult source)
        {
            if (source == null)
            {
                return false;
            }
            return source.Success;
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="error">错误码</param>
        /// <param name="serialNo">流水号</param>
        public static ContractResult SetError(this ContractResult source, Enum error, string serialNo = "")
        {
            source.Success = false;
            source.Code = $"{error.GetHashCode()}";
            source.Message = error.GetDescription();
            if (!string.IsNullOrWhiteSpace(serialNo))
            {
                source.SerialNo = serialNo;
            }
            return source;
        }

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="error">错误码</param>
        /// <param name="serialNo">流水号</param>
        public static ContractResult<T> SetError<T>(this ContractResult<T> source, Enum error, string serialNo = "")
        {
            source.Success = false;
            source.Code = $"{error.GetHashCode()}";
            source.Message = error.GetDescription();
            if (!string.IsNullOrWhiteSpace(serialNo))
            {
                source.SerialNo = serialNo;
            }
            return source;
        }

        /// <summary>
        /// 设置成功返回信息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public static ContractResult SetSuceccful(this ContractResult source, string serialNo = "")
        {
            source.Success = true;
            if (!string.IsNullOrWhiteSpace(serialNo))
            {
                source.SerialNo = serialNo;
            }
            source.Message = GlobalMessageCode.SUCCESS.GetDescription();
            source.Code = GlobalMessageCode.SUCCESS.GetHashCode().ToString();
            return source;
        }

        /// <summary>
        /// 设置成功返回信息
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="data"></param>
        /// <param name="serialNo"></param>
        public static ContractResult<T> SetSuceccful<T>(this ContractResult<T> source, T data = default, string serialNo = "")
        {
            source.Success = true;
            if (data != null)
            {
                source.Data = data;
            }

            if (!string.IsNullOrWhiteSpace(serialNo))
            {
                source.SerialNo = serialNo;
            }

            source.Code = $"{GlobalMessageCode.SUCCESS.GetHashCode()}";
            source.Message = GlobalMessageCode.SUCCESS.GetDescription();
            return source;
        }
    }
}