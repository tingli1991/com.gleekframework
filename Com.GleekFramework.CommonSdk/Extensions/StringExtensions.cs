using System;
using System.Collections.Generic;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 字符串基础拓展类
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 去掉开始的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        public static string TrimStart(this string input, string trimStr)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(trimStr))
            {
                return input;
            }

            if (input.StartsWith(trimStr))
            {
                input = input.Substring(trimStr.Length);
            }
            return input;
        }

        /// <summary>
        /// 去掉结尾的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="trimStr"></param>
        /// <returns></returns>
        public static string TrimEnd(this string input, string trimStr)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(trimStr))
            {
                return input;
            }

            if (input.EndsWith(trimStr))
            {
                input = input.Substring(0, input.Length - trimStr.Length);
            }
            return input;
        }

        /// <summary>
        /// 字符串转换成对象
        /// </summary>
        /// <typeparam name="T">返回的泛型对象</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string value)
        {
            T result = default;
            try
            {
                if (string.IsNullOrEmpty(value))
                {
                    return result;
                }
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 转换成列表
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static List<string> ToList(this string input)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(input))
            {
                list.Add(input);
            }
            return list;
        }

        /// <summary>
        /// 转换成列表
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns></returns>
        public static string[] ToArray(this string input)
        {
            var list = new string[] { };
            if (!string.IsNullOrEmpty(input))
            {
                list = new string[] { input };
            }
            return list;
        }

        /// <summary>
        /// 截取字符串并返回指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">需要截取的字符串</param>
        /// <param name="startIndex">开始位置</param>
        /// <returns></returns>
        public static T Substring<T>(this string content, int startIndex)
        {
            var value = content.Substring(startIndex);
            return value.ToObject<T>();
        }

        /// <summary>
        /// 截取字符串并返回指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">需要截取的字符串</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static T Substring<T>(this string content, int startIndex, int length)
        {
            var value = content.Substring(startIndex, length);
            return value.ToObject<T>();
        }

        /// <summary>
        /// 截取字符串中指定标签内的内容
        /// </summary>
        /// <param name="content">需要截取的字符串</param>
        /// <param name="start">开始标签</param>
        /// <param name="end">结束标签</param>
        /// <returns></returns>
        public static T Substring<T>(this string content, string start, string end)
        {
            var value = content.Substring(start, end);
            return value.ToObject<T>();
        }

        /// <summary>
        /// 截取字符串中指定标签内的内容
        /// </summary>
        /// <param name="content">需要截取的字符串</param>
        /// <param name="start">开始标签</param>
        /// <param name="end">结束标签</param>
        /// <returns></returns>
        public static string Substring(this string content, string start, string end)
        {
            if (string.IsNullOrEmpty(content))
            {
                return "";
            }

            var leftIndex = content.IndexOf(start);
            if (leftIndex > -1)
            {
                content = content.Substring(leftIndex + start.Length);
                int rightIndex = content.IndexOf(end);
                if (rightIndex > -1)
                {
                    return content.Substring(0, rightIndex);
                }
            }
            return "";
        }

        /// <summary>
        /// 验证字符串最大长度,返回指定长度的字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>         
        public static string HashMaxLength(this string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }

            input = input.Trim();
            if (input.Length > maxLength)//按最大长度截取字符串
            {
                input = input.Substring(0, maxLength);
            }
            return input;
        }

        /// <summary>
        /// 验证字符串的最小长度,返回指定长度的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="minLength"></param>
        /// <returns></returns>
        public static string HasMinLength(this string input, int minLength)
        {
            if (!string.IsNullOrEmpty(input))
            {
                input = input.Trim();
                if (input.Length < minLength)
                {
                    input = input.Substring(0, minLength);
                }
            }
            return input;
        }
    }
}