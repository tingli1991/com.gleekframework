using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.GleekFramework.CommonSdk
{
    /// <summary>
    /// 时间拓展类
    /// </summary>
    public static partial class DateTimeExtensions
    {
        /// <summary>
        /// 随机因子
        /// </summary>
        private static readonly Random Random = new Random((int)DateTime.Now.ToCstTime().Ticks);

        /// <summary>
        /// 转换时间格式
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DateTime? dateTime)
        {
            return dateTime ?? DateTime.Now.ToCstTime();
        }

        /// <summary>
        /// 时间戳转为C#格式时间10位
        /// </summary>
        /// <param name="unixTime">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime ToDateTime(this decimal unixTime)
        {
            return ((long)unixTime).ToDateTime();
        }

        /// <summary>
        /// 时间戳转为C#格式时间10位
        /// </summary>
        /// <param name="unixTime">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime ToDateTime(this double unixTime)
        {
            return ((long)unixTime).ToDateTime();
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="unixTime">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime ToDateTime(this long unixTime)
        {
            DateTime? dateTime;
            var timeStamp = unixTime.ToString();
            var startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(8);
            switch (timeStamp.Length)
            {
                case 10:
                    dateTime = startTime.AddSeconds(unixTime);
                    break;
                case 13:
                    dateTime = startTime.AddMilliseconds(unixTime);
                    break;
                default:
                    throw new ArgumentException("Invalid length");
            }
            return dateTime.Value;
        }

        /// <summary>
        /// 解决Windows和linux系统市区问题
        /// </summary>
        /// <param name="dateTime">时间对象</param>
        /// <returns></returns>
        public static DateTime ToCstToday(this DateTime dateTime)
        {
            var time = dateTime.ToCstTime();
            return new DateTime(time.Year, time.Month, time.Day);
        }

        /// <summary>
        /// 解决Windows和linux系统市区问题
        /// </summary>
        /// <param name="dateTime">时间对象</param>
        /// <returns></returns>
        public static DateTime ToCstTime(this DateTime dateTime)
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            var shanghaiZone = DateTimeZoneProviders.Tzdb["Asia/Shanghai"];
            return now.InZone(shanghaiZone).ToDateTimeUnspecified();
        }

        /// <summary>
        /// 转换为时间戳
        /// 精确到秒（长度10）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long ToUnixTimeForSeconds(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// 转换为时间戳
        /// 精确到毫秒（长度13）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long ToUnixTimeForMilliseconds(this DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }

        /// <summary>
        /// 获取两个时间段内的随机时间
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static DateTime GetRandomTime(this DateTime beginTime, DateTime endTime)
        {
            if (beginTime == endTime)
                return beginTime;//如果相等直接取一个时间

            if (beginTime > endTime)
            {
                var tmpTime = endTime;
                endTime = beginTime;
                beginTime = tmpTime;
            }

            var minValue = beginTime.ToUnixTimeForMilliseconds();//开始时间的时间戳
            var maxValue = endTime.ToUnixTimeForMilliseconds();//结束时间的时间戳
            var randomTimes = Random.NextLong(minValue, maxValue);
            return randomTimes.ToDateTime();//当前的时间戳
        }

        /// <summary>
        /// 在制定的时间范围内生成随机时间
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="intervalSeconds">间隔时间(秒数)</param>
        /// <returns></returns>
        public static DateTime? GetRandomTime(this DateTime beginTime, DateTime endTime, int intervalSeconds)
        {
            DateTime? randomTime = null;
            if (beginTime == endTime)
            {
                return randomTime;
            }

            if (beginTime > endTime)
            {
                (beginTime, endTime) = (endTime, beginTime);
            }

            if ((int)Math.Abs(endTime.Subtract(beginTime).TotalSeconds) <= intervalSeconds)
            {
                //开始时间间和结束时间的时间差
                return randomTime;
            }

            var activeBeginTime = beginTime.AddSeconds(intervalSeconds + 1);//有效的开始时间
            var activeEndTime = endTime.AddSeconds(-intervalSeconds - 1);//有效的结束时间
            if (activeBeginTime > activeEndTime)
            {
                return randomTime;
            }
            return activeBeginTime.GetRandomTime(activeEndTime);//生成随机时间
        }

        /// <summary>
        /// 在指定的时间抽上面生成随机时间
        /// </summary>
        /// <param name="timeaxis">时间抽(开始时间到结束时间之内的时间轴有效)</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="intervalSeconds">间隔时间(秒数)</param>
        /// <returns></returns>
        public static DateTime? GetRandomTime(this IEnumerable<DateTime> timeaxis, DateTime beginTime, DateTime endTime, int intervalSeconds)
        {
            DateTime? randomTime = null;
            if (beginTime > endTime)
            {
                (beginTime, endTime) = (endTime, beginTime);
            }

            if (timeaxis.IsNullOrEmpty())
            {
                //没有时间轴，直接按照开始和结束时间去随机
                return beginTime.GetRandomTime(endTime);
            }

            var orderTimeAxisList = timeaxis.Where(time => time >= beginTime.AddSeconds(-intervalSeconds - 2) && time <= endTime.AddSeconds(intervalSeconds + 2));
            if (orderTimeAxisList.IsNullOrEmpty())
            {
                //没有时间轴，直接按照开始和结束时间去随机
                return beginTime.GetRandomTime(endTime);
            }
            else
            {
                DateTime? oldExecuteTime = null;//旧时间
                DateTime? currentExecuteTime = null;//当前时间
                if (orderTimeAxisList.Count() <= 1)
                {
                    orderTimeAxisList = orderTimeAxisList.Add(endTime);//追加结束时间
                    orderTimeAxisList = orderTimeAxisList.Add(beginTime);//追加开始时间
                }
                else
                {
                    //最小时间的逻辑判断
                    var minTime = orderTimeAxisList.Min();
                    if (minTime > beginTime)
                    {
                        //追加开始时间
                        orderTimeAxisList = orderTimeAxisList.Add(beginTime);
                    }

                    //最大时间的逻辑判断
                    var maxTime = orderTimeAxisList.Max();
                    if (maxTime < endTime)
                    {
                        //追加结束时间
                        orderTimeAxisList = orderTimeAxisList.Add(endTime);
                    }
                }
                orderTimeAxisList = orderTimeAxisList.Distinct().OrderBy(time => time);//重新去重排序
                foreach (var orderTimeAxis in orderTimeAxisList)
                {
                    currentExecuteTime = orderTimeAxis;//当前时间
                    if (!oldExecuteTime.HasValue)
                    {
                        oldExecuteTime = currentExecuteTime;
                        continue;
                    }

                    randomTime = oldExecuteTime.Value.GetRandomTime(currentExecuteTime.Value, intervalSeconds);
                    if (randomTime >= beginTime && randomTime <= endTime)
                    {
                        //已经去到随机时间或者遍历到最后一次的时候退出程序
                        break;
                    }
                    else
                    {
                        //不满足时间区间的条件，直接赋空
                        randomTime = null;
                    }
                    oldExecuteTime = currentExecuteTime;//赋值旧时间
                }
            }
            return randomTime;
        }
    }
}