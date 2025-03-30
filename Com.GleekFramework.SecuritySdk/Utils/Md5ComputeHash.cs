using System;
using System.Security.Cryptography;
using System.Text;

namespace Com.GleekFramework.SecuritySdk
{
    /// <summary>
    /// 跨语言兼容的MD5哈希计算工具类
    /// 注意：MD5不适用于安全敏感场景，建议优先使用SHA256等更安全的哈希算法
    /// </summary>
    public class Md5ComputeHash
    {
        /// <summary>
        /// 计算字节数组的MD5哈希值（基础方法）
        /// </summary>
        /// <param name="input">输入字节数组</param>
        /// <returns>16字节MD5哈希值</returns>
        public static byte[] ComputeHash(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return MD5.HashData(input);
        }

        /// <summary>
        /// 计算字符串的MD5哈希值（UTF8编码）
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>32字符小写十六进制字符串</returns>
        public static string ComputeHashString(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = ComputeHash(bytes);
            return BytesToHex(hashBytes);
        }

        /// <summary>
        /// 验证字符串与指定哈希值是否匹配
        /// </summary>
        public static bool Verify(string input, string hash)
        {
            string computedHash = ComputeHashString(input);
            return StringComparer.OrdinalIgnoreCase.Equals(computedHash, hash);
        }

        /// <summary>
        /// 将字节数组转换为十六进制字符串（性能优化版本）
        /// </summary>
        private static string BytesToHex(ReadOnlySpan<byte> bytes)
        {
            const string hexAlphabet = "0123456789abcdef";
            Span<char> stringBuffer = stackalloc char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var b = bytes[i];
                stringBuffer[i * 2] = hexAlphabet[b >> 4];
                stringBuffer[i * 2 + 1] = hexAlphabet[b & 0xF];
            }
            return new string(stringBuffer);
        }
    }
}