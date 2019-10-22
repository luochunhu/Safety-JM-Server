using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Safety.Reports
{
    public class DateTimeManager
    {
        /// <summary>
        /// 今天
        /// </summary>
        /// <returns></returns>
        public static string GetToday()
        {          
            return GetDateTimeString(DateTime.Now.Date, DateTime.Now.Date);
        }
        /// <summary>
        /// 本周
        /// </summary>
        /// <returns></returns>
        public static string GetThisWeek()
        {
            return GetDateTimeString(DateTimeUtil.GetWeekFristDay(DateTime.Now.Date), DateTimeUtil.GetWeekEndDay(DateTime.Now.Date));
        }
        /// <summary>
        /// 本月
        /// </summary>
        /// <returns></returns>
        public static string GetThisMonth()
        {
            return GetDateTimeString(DateTimeUtil.GetMonthFristDay(DateTime.Now.Date), DateTimeUtil.GetMonthEndDay(DateTime.Now.Date));
        }
        /// <summary>
        /// 本季度
        /// </summary>
        /// <returns></returns>
        public static string GetThisSeason()
        {
            return GetDateTimeString(DateTimeUtil.GetQuarterFristDay(DateTime.Now.Date), DateTimeUtil.GetQuarterEndDay(DateTime.Now.Date));
        }
        /// <summary>
        /// 本年
        /// </summary>
        /// <returns></returns>
        public static string GetThisYear()
        {
            return GetDateTimeString(DateTimeUtil.GetYearFristDay(DateTime.Now.Date), DateTimeUtil.GetYearEndDay(DateTime.Now.Date));
        }
        /// <summary>
        /// 本周至今
        /// </summary>
        /// <returns></returns>
        public static string GetThisWeekToToday()
        {
            return GetDateTimeString(DateTimeUtil.GetWeekFristDay (DateTime.Now.Date), DateTime.Now .Date );
        }   
        /// <summary>
        /// 本月至今
        /// </summary>
        /// <returns></returns>
        public static string GetThisMonthToToday()
        {
            return GetDateTimeString(DateTimeUtil.GetMonthFristDay (DateTime.Now.Date), DateTime.Now.Date);
        }
        /// <summary>
        /// 本季度至今
        /// </summary>
        /// <returns></returns>
        public static string GetThisSeasonToToday()
        {
            return GetDateTimeString(DateTimeUtil.GetQuarterFristDay (DateTime.Now.Date), DateTime.Now.Date);
        }
        /// <summary>
        /// 本年到今
        /// </summary>
        /// <returns></returns>
        public static string GetThisYearToToday()
        {
            return GetDateTimeString(DateTimeUtil.GetYearFristDay(DateTime.Now.Date), DateTime.Now.Date);
        }
        /// <summary>
        /// 上周
        /// </summary>
        /// <returns></returns>
        public static string GetLastWeek()
        {
            return GetDateTimeString(DateTimeUtil.GetLastWeekFristDay (DateTime.Now.Date), DateTimeUtil.GetLastWeekEndDay (DateTime.Now.Date));
        }
        /// <summary>
        /// 上月
        /// </summary>
        /// <returns></returns>
        public static string GetLastMonth()
        {
            return GetDateTimeString(DateTimeUtil.GetLastWeekFristDay (DateTime.Now.Date), DateTimeUtil.GetLastWeekEndDay (DateTime.Now.Date));
        }
        /// <summary>
        /// 上季度
        /// </summary>
        /// <returns></returns>
        public static string GetLastSeason()
        {
            return GetDateTimeString(DateTimeUtil.GetLastQuarterFristDay (DateTime.Now.Date), DateTimeUtil.GetLastQuarterEndDay (DateTime.Now.Date));
        }
        

        /// <summary>
        /// 上年
        /// </summary>
        /// <returns></returns>
        public static string GetLastYear()
        {
           
            DateTime datetime = DateTime.Now.AddYears(-1);
            string str = "between '";
            str += FormatDateTime(datetime).Substring(0, 4) + "-01-01 00:00:00'";
            str += " and '" + FormatDateTime(datetime).Substring(0, 4) + "-12-31 23:59:59'";

            return str;
        }

        /// <summary>
        /// 最近一周
        /// </summary>
        /// <returns></returns>
        public static string GetLastestWeek()
        {
            DateTime datetime = DateTime.Now.AddDays(-6);
            string str = "between '";
            str += FormatDateTime(datetime) + " 00:00:00'";
            str += " and '" + FormatDateTime(DateTime.Now) + " 23:59:59'";

            return str;
        }

        /// <summary>
        /// 最近一月
        /// </summary>
        /// <returns></returns>
        public static string GetLastestMonth()
        {
            DateTime datetime = DateTime.Now.AddMonths(-1).AddDays(1);
            string str = "between '";
            str += FormatDateTime(datetime) + " 00:00:00'";
            str += " and '" + FormatDateTime(DateTime.Now) + " 23:59:59'";

            return str;
        }

        /// <summary>
        /// 最近一季度
        /// </summary>
        /// <returns></returns>
        public static string GetLastestSeason()
        {
            DateTime datetime = DateTime.Now.AddMonths(-3).AddDays(1);
            string str = "between '";
            str += FormatDateTime(datetime) + " 00:00:00'";
            str += " and '" + FormatDateTime(DateTime.Now) + " 23:59:59'";

            return str;
        }

        /// <summary>
        /// 最近一年
        /// </summary>
        /// <returns></returns>
        public static string GetLastestYear()
        {
            DateTime datetime = DateTime.Now.AddYears(-1).AddDays(1);
            string str = "between '";
            str += FormatDateTime(datetime) + " 00:00:00'";
            str += " and '" + FormatDateTime(DateTime.Now) + " 23:59:59'";

            return str;
        }



        /// <summary>
        /// 根据日期获取查询SQL的字符串
        /// </summary>
        /// <param name="datFrist">开始日期</param>
        /// <param name="datLast">结束日期</param>
        /// <returns>查询日期的字符串</returns>
        private static string GetDateTimeString(DateTime datFrist,DateTime datLast)
        {
            string strDateTime = " between '";
            strDateTime += FormatDateTime(datFrist).Substring (0,10) + " 00:00' and '";
            strDateTime += FormatDateTime(datLast).Substring(0, 10) + " 23:59:59'";
            return strDateTime;

            //string strDateTime = " between '";
            //strDateTime +=FormatDateTime(datFrist.Date) + "' and '";
            //strDateTime += datLast .Date .ToShortDateString () + " 23:59:59 '";
            //return strDateTime;
        }

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="dateTime">Date日期</param>
        /// <returns>返回的字符串格式为 年-月-日 如(  2007-01-12  )</returns>
        private static string FormatDateTime(DateTime dateTime)
        {
            string strDate = string.Format("{0:yyyy/MM/dd}", dateTime);
            return strDate;
        }

    }
}
