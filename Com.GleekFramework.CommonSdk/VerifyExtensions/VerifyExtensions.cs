using System;
using System.Text.RegularExpressions;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 数据验证基础拓展类
    /// </summary>
    public static partial class VerifyExtensions
    {
        /// <summary>
        /// 是否是字符串类型
        /// </summary>
        /// <param name="source">类型</param>
        /// <returns></returns>
        public static bool IsStringType<T>(this T source)
        {
            return source.GetType().IsStringType();
        }

        /// <summary>
        /// 是否是字符串类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsStringType(this Type type)
        {
            return type == typeof(string);
        }

        /// <summary>
        /// 是否是基础类型
        /// 注意：不包括结构体
        /// </summary>
        /// <param name="source">类型</param>
        /// <returns></returns>
        public static bool IsPrimitiveType<T>(this T source)
        {
            return source.GetType().IsPrimitiveType();
        }

        /// <summary>
        /// 是否是基础类型
        /// 注意：不包括结构体
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsPrimitiveType(this Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type.IsEnum;
        }

        /// <summary>
        /// 检查IP地址的格式是否正确
        /// </summary>
        /// <param name="input"></param>
        public static bool IsIPV4(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return Regex.IsMatch(input, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$");
        }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this string obj)
        {
            return string.IsNullOrWhiteSpace(obj);
        }

        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return !obj.IsNull();
        }

        /// <summary>
        /// 是否是Bool值
        /// </summary>
        public static bool IsBool(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            input = input.ToLower();//转小写
            return input == "true" || input == "false";
        }

        /// <summary>
        /// 验证字符串是否整数
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsInt32(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regexString = @"^-?\\d+$";
            return Regex.IsMatch(input, regexString);
        }

        /// <summary>
        /// 验证字符串是否浮点数字
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsDouble(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regexString = @"^(-?\\d+)(\\.\\d+)?$";
            return Regex.IsMatch(input, regexString);
        }

        /// <summary>
        /// 验证字符串Email地址
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsEmail(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regexString = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return Regex.IsMatch(input, regexString);
        }

        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regexString = @"^([0-9]+)$";
            return Regex.IsMatch(input, regexString);
        }

        /// <summary>
        /// 验证字符串是否日期
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsDate(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regexString = @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$";
            return Regex.IsMatch(input, regexString);
        }

        /// <summary>
        /// 验证字符串是否 ANSI SQL date format
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsAnsiSqlDate(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regexString = @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578]
									)|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[4
									69])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\
									s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([1
									3579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((
									0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((
									0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9]
									)|(2[0-8]))))))(\s(((0?[1-9])|(1[0-2]))\:([0-5][0-9])((\s)|(
									\:([0-5][0-9])\s))([AM|PM|am|pm]{2,2})))?$";
            return Regex.IsMatch(input, regexString);
        }

        /// <summary>
        /// 验证字符串是否TXT文件名(全名)
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsTxtFileName(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regexString = @"^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w ]*))+\.(txt|TXT)$";
            return Regex.IsMatch(input, regexString);
        }

        /// <summary>
        /// 验证电话号码
        /// </summary>
        /// <param name="input">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsTelephone(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string regex = @"^(\d{3,4}-)?\d{6,8}$";
            return Regex.IsMatch(input, regex);
        }

        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidMobile(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            Regex rx = new Regex(@"^(1[3-9])\d{9}$", RegexOptions.None);
            Match m = rx.Match(input);
            return m.Success;
        }

        /// <summary>
        /// 电话有效性（固话和手机 ）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsValidPhoneAndMobile(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            Regex rx = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$|^(13|15)\d{9}$", RegexOptions.None);
            Match m = rx.Match(input);
            return m.Success;
        }

        /// <summary>
        /// 验证是否为字母和数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsLetterAndNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            string regex = @"^[a-zA-Z0-9]+$";
            return Regex.IsMatch(input, regex);
        }

        /// <summary>
        /// 判断是否在制定长度范围内的数字
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="minLength">最小长度</param>
        /// <returns></returns>
        public static bool IsBetweenWithNumber(this string input, int maxLength, int minLength)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            string regex = string.Format(@"^\d{{0},{1}}$", minLength, maxLength);
            return Regex.IsMatch(input, regex);
        }

        /// <summary>
        /// 验证提现密码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool IsWithdrawalPassword(this string input, int length = 6)
        {
            bool result = false;
            if (string.IsNullOrEmpty(input))
                return result;

            if (input.IsNumber() && input.Length == length)
                result = true;

            return result;
        }

        /// <summary>
        /// 验证用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool IsUserPassword(this string input, int minLength = 6, int maxLength = 25)
        {
            bool result = false;
            if (string.IsNullOrEmpty(input))
                return result;

            int length = input.Length;
            if (input.IsLetterAndNumber() && length >= minLength && length <= maxLength)
                result = true;

            return result;
        }
    }
}