using System.Security.Cryptography;
using System;

namespace Com.GleekFramework.SecuritySdk
{
    /// <summary>
    /// 安全随机字符串生成器（线程安全）
    /// 默认预置字符集：数字、大写字母、小写字母、常用特殊符号
    /// </summary>
    public static class RandomStringGenerator
    {
        /// <summary>
        /// 数字
        /// </summary>
        public const string NUMBERS = "0123456789";

        /// <summary>
        /// 大写字母
        /// </summary>
        public const string UPPER_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 小写字母
        /// </summary>
        public const string LOWER_LETTERS = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 特殊字符
        /// </summary>
        public const string SPECIAL_SYMBOLS = "!@#$%^&*()-_=+[]{}|;:,.<>/?";

        /// <summary>
        /// 生成应用ID（默认16位，包含数字+大写字母）
        /// </summary>
        public static string GenAppId(int length = 16)
        {
            return Generate(length, NUMBERS + UPPER_LETTERS + LOWER_LETTERS);
        }

        /// <summary>
        /// 生成应用密钥（默认32位，包含四类字符）
        /// </summary>
        public static string GenAppSecret(int length = 32)
        {
            return Generate(length, NUMBERS + UPPER_LETTERS + LOWER_LETTERS + SPECIAL_SYMBOLS);
        }

        /// <summary>
        /// 通用随机字符串生成方法
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="allowedChars">允许的字符集合</param>
        public static string Generate(int length, string allowedChars)
        {
            if (length <= 0)
            {
                throw new ArgumentException("长度必须大于0", nameof(length));
            }

            if (string.IsNullOrEmpty(allowedChars))
            {
                throw new ArgumentException("字符集不能为空", nameof(allowedChars));
            }

            // 使用加密级随机数生成器
            using var rng = RandomNumberGenerator.Create();

            // 优化内存分配
            Span<byte> randomBytes = stackalloc byte[length * 4];
            rng.GetBytes(randomBytes);

            // 生成结果缓冲区
            Span<char> result = stackalloc char[length];

            // 优化字符集访问
            ReadOnlySpan<char> allowedCharsSpan = allowedChars.AsSpan();
            int maxIndex = allowedCharsSpan.Length;

            for (int i = 0; i < length; i++)
            {
                // 使用32位随机数降低模运算偏差
                uint randomValue = BitConverter.ToUInt32(randomBytes.Slice(i * 4, 4));
                result[i] = allowedCharsSpan[(int)(randomValue % maxIndex)];
            }

            return new string(result);
        }
    }
}