using System;

namespace shadowsocks_csharp.util.Extentions
{
    public static class DateTimeExtention
    {
        /// <summary>
        /// 将Unix时间戳（格林威治时间）转换为DateTime类型
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(this long timestamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime time = startTime.AddSeconds(timestamp);
            return time;
        }

        /// <summary>
        /// 转换为Unix时间戳（格林威治时间）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(this DateTime dateTime)
        {
            TimeSpan timespan = (dateTime - new DateTime(1970, 1, 1).ToLocalTime());

            long result = Convert.ToInt64(timespan.TotalSeconds);

            return result;
        }
    }
}