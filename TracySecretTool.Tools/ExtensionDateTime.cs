using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TracySecretTool.Tools
{
    public static class ExtensionDateTime
    {
        /// <summary>  
        /// 将DateTime转换为10位时间戳
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static string ToUnixTime10(this DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return ((long)(time - startTime).TotalSeconds).ToString();
        }

        /// <summary>
        /// 将DateTime转换为13位时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToUnixTime13(this DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;
            return t.ToString();
        }
    }
}
