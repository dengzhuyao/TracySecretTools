using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TracySecretTool.Tools
{
    public static class ExtensionString
    {
        /// <summary>
        /// 10位时间戳转为C#格式时间    
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime10(this string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        } 

        /// <summary>
        /// 13位时间戳转为C#格式时间    
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>      
        public static DateTime ToDateTime13(this string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        } 
    }
}
