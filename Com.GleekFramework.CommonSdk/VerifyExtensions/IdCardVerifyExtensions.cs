using System;
using System.Globalization;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 身份证号码验证
    /// </summary>
    public static partial class IdCardVerifyExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        private const string Address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="cardId">身份证号</param>
        /// <returns></returns>
        public static bool IsCardId(this string cardId)
        {
            if (cardId.Length == 18)
            {
                return CheckIDCard18(cardId);
            }
            if (cardId.Length == 15)
            {
                return CheckIDCard15(cardId);
            }
            return false;
        }

        /// <summary>
        /// 根据身份证号获取生日
        /// </summary>
        /// <param name="cardId">身份证号码</param>
        /// <returns></returns>
        public static DateTime GetBrithdayTime(this string cardId)
        {
            var birthday = DateTime.MinValue;
            if (cardId.Length == 15)
            {
                var str = cardId.Substring(6, 6).Insert(4, "-").Insert(2, "-");
                birthday = DateTime.Parse(str);
            }
            else if (cardId.Length == 18)
            {
                var str = cardId.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                birthday = DateTime.Parse(str);
            }
            return birthday;
        }

        /// <summary>
        /// 15位身份证验证
        /// </summary>
        /// <param name="cardId">身份证号</param>
        /// <returns></returns>
        private static bool CheckIDCard15(this string cardId)
        {
            if (long.TryParse(cardId, out long n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }

            if (Address.IndexOf(cardId.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }

            var birth = cardId.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            return DateTime.TryParse(birth, out _); //生日验证
        }

        /// <summary>
        /// 18位身份证验证
        /// </summary>
        /// <param name="cardId">身份证号</param>
        /// <returns></returns>
        private static bool CheckIDCard18(this string cardId)
        {
            if (long.TryParse(cardId.Remove(17), out long n) == false || n < Math.Pow(10, 16) || long.TryParse(cardId.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }

            if (Address.IndexOf(cardId.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }

            string birth = cardId.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            if (DateTime.TryParse(birth, out _) == false)
            {
                return false;//生日验证
            }

            string[] arrVarifyCode = "1,0,x,9,8,7,6,5,4,3,2".Split(',');
            string[] Wi = "7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2".Split(',');
            char[] Ai = cardId.Remove(17).ToCharArray();
            int sum = 0;
            for (var i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString(CultureInfo.InvariantCulture));
            }
            Math.DivRem(sum, 11, out int y);
            return arrVarifyCode[y] == cardId.Substring(17, 1).ToLower();//校验码验证（是否符合GB11643-1999标准）
        }
    }
}