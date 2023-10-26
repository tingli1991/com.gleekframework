using System;
using System.Linq;
using System.Text;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// Base64拓展类
    /// </summary>
    public static partial class Base64Extensions
    {
        /// <summary>
        /// Base64代码数组
        /// </summary>
        private static readonly char[] base64CodeArray = new char[]
        {
         'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
         'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
         '0', '1', '2', '3', '4',  '5', '6', '7', '8', '9', '+', '/', '='
        };

        /// <summary>
        /// 是否base64字符串
        /// * 字符串只可能包含A-Z，a-z，0-9，+，/，=字符 
        /// * 字符串长度是4的倍数
        /// * =只会出现在字符串最后，可能没有或者一个等号或者两个等号
        /// </summary>
        /// <param name="base64Str">要判断的字符串</param>
        /// <returns></returns>
        public static bool IsBase64(this string base64Str)
        {
            base64Str = Base64StringLen(base64Str);
            try
            {
                if (string.IsNullOrEmpty(base64Str))
                {
                    return false;
                }
                else
                {
                    if (base64Str.Contains(","))
                        base64Str = base64Str.Split(',')[1];

                    if (base64Str.Any(c => !base64CodeArray.Contains(c)))
                        return false;
                }

                var bytes = Convert.FromBase64String(base64Str);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// 转换成Base64字符串
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns></returns>
        public static string ToBase64(this string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return text;
                }

                byte[] bytContent = Encoding.UTF8.GetBytes(text);
                text = Convert.ToBase64String(bytContent, 0, bytContent.Length);
            }
            catch (Exception)
            {
            }
            return text;
        }

        /// <summary>
        /// Base64转字符串
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static string Base64ToString(this string base64)
        {
            var strResult = base64;
            try
            {
                base64 = Base64StringLen(base64);
                byte[] bytContent = Convert.FromBase64String(base64);
                strResult = Encoding.UTF8.GetString(bytContent);
            }
            catch (Exception)
            {
            }
            return strResult;
        }

        /// <summary>
        /// 处理base64长度
        /// * 字符串只可能包含A-Z，a-z，0-9，+，/，=字符 
        /// * 字符串长度是4的倍数
        /// * =只会出现在字符串最后，可能没有或者一个等号或者两个等号
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public static string Base64StringLen(this string test)
        {
            if (string.IsNullOrEmpty(test))
            {
                return test;
            }

            test = test.Replace("=", "");//去掉所有的=号
            while (test.Length % 4 != 0)//判断长度是否是4的倍数，少用=填充 
            {
                test += "=";
            }
            return test;
        }
    }
}