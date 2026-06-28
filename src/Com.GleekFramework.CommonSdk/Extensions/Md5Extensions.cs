using System.Security.Cryptography;
using System.Text;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// MD5加密拓展类
    /// </summary>
    public static partial class Md5Extensions
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncryptMd5(this string value)
        {
            var text = string.Empty;
            if (string.IsNullOrEmpty(value))
            {
                return text;
            }

            using (var md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                var sBuilder = new StringBuilder();
                foreach (byte t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }
                text = sBuilder.ToString();
            }
            return text;
        }
    }
}
