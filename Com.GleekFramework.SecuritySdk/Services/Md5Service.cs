using Com.GleekFramework.AutofacSdk;
using System.Security.Cryptography;
using System.Text;

namespace Com.GleekFramework.SecuritySdk
{
    /// <summary>
    /// MD5加密服务
    /// </summary>
    public class Md5Service : IBaseAutofac
    {
        /// 计算字符串的MD5哈希值（不加盐）
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <returns>32字符小写的MD5哈希值</returns>
        public static string ComputeMD5(string input)
        {
            // 处理空值（可根据需求抛异常）
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            using (MD5 md5 = MD5.Create())
            {
                // UTF-8编码
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 转换为十六进制字符串
                return BytesToHex(hashBytes);
            }
        }

        /// <summary>
        /// 计算字符串的MD5哈希值（加盐）
        /// 盐值将追加到原始字符串末尾
        /// </summary>
        /// <param name="input">原始字符串</param>
        /// <param name="salt">盐值</param>
        /// <returns>32字符小写的MD5哈希值</returns>
        public static string ComputeMD5WithSalt(string input, string salt)
        {
            // 处理空值（可根据需求调整）
            if (string.IsNullOrEmpty(input) && string.IsNullOrEmpty(salt))
            {
                return null;
            }

            // 拼接原始字符串和盐值
            string saltedInput = input + salt;
            return ComputeMD5(saltedInput);
        }

        /// <summary>
        /// 将字节数组转换为十六进制字符串（小写）
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string BytesToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2")); // "x2"表示小写两位十六进制
            }
            return sb.ToString();
        }
    }
}
