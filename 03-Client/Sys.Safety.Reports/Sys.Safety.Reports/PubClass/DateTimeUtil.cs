using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Collections;


namespace Sys.Safety.Reports
{
    public class DateTimeUtil
    {
        /// <summary>        
        /// 返回指定日期当前周的第一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetWeekFristDay(DateTime dateTime)
        {
            string temp = "-" + DayInWeek(dateTime);
            int days = Convert.ToInt32(temp);
            return dateTime.AddDays(days);
        }
        /// <summary>
        /// 返回指定日期当前周的最一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetWeekEndDay(DateTime dateTime)
        {
            string temp = "-" + DayInWeek(dateTime);
            int days = Convert.ToInt32(temp);
            return dateTime.AddDays(6 + days);
        }
        /// <summary>
        /// 返回指定日期当前月的第一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetMonthFristDay(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            string date = year + "-" + month + "-" + "1";
            return Convert.ToDateTime(date);
        }
        /// <summary>
        /// 返回指定日期当前月的最后一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetMonthEndDay(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            int date = DateTime.DaysInMonth(year, month);
            string strDate = year + "-" + month + "-" + date;
            return Convert.ToDateTime(strDate);
        }
        /// <summary>
        /// 返回指定日期当前季度的第一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetQuarterFristDay(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            string date = year + "-" + GetQuarterFristMonth(month) + "-" + "1";
            return Convert.ToDateTime(date);
        }
        /// <summary>
        /// 返回指定日期当前季度的最后一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetQuarterEndDay(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            int lastMonth = GetQuarterEndMonth(month);
            string date = year + "-" + lastMonth + "-" + DateTime.DaysInMonth(year, lastMonth);
            return Convert.ToDateTime(date);
        }
        /// <summary>
        /// 返回指定日期当前年度的第一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetYearFristDay(DateTime dateTime)
        {
            int year = dateTime.Year;
            string date = year + "-" + "1" + "-" + "1";
            return Convert.ToDateTime(date);
        }
        /// <summary>
        /// 返回指定日期当前年度的最后一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetYearEndDay(DateTime dateTime)
        {
            int year = dateTime.Year;
            int date = DateTime.DaysInMonth(year, 12);
            string strDate = year + "-" + "12" + "-" + date;
            return Convert.ToDateTime(strDate); ;
        }
        /// <summary>
        /// 返回指定日期上周的第一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastWeekFristDay(DateTime dateTime)
        {
            return GetWeekFristDay(dateTime.AddDays(-7));
        }
        /// <summary>
        /// 返回指定日期上周的最后一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastWeekEndDay(DateTime dateTime)
        {
            return GetWeekEndDay(dateTime.AddDays(-7));
        }
        /// <summary>
        /// 返回指定日期上月的第一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastMonthFristDay(DateTime dateTime)
        {
            return GetMonthFristDay(dateTime.AddMonths(-1));
        }
        /// <summary>
        /// 返回指定日期上月的最后一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastMonthEndDay(DateTime dateTime)
        {
            return GetMonthEndDay(dateTime.AddMonths(-1));
        }
        /// <summary>
        /// 返回指定日期上季度的第一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastQuarterFristDay(DateTime dateTime)
        {
            return GetQuarterFristDay(dateTime.AddMonths(-3));
        }
        /// <summary>
        /// 返回批定日期上季度的最后一天日期

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastQuarterEndDay(DateTime dateTime)
        {
            return GetQuarterEndDay(dateTime.AddMonths(-3));
        }

        /// <summary>
        /// 获取日期为这周的第几天

        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int DayInWeek(DateTime dateTime)
        {

            if (dateTime.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                return 0;
            }

            if (dateTime.DayOfWeek.Equals(DayOfWeek.Monday))
            {
                return 1;
            }
            if (dateTime.DayOfWeek.Equals(DayOfWeek.Tuesday))
            {
                return 2;
            }
            if (dateTime.DayOfWeek.Equals(DayOfWeek.Wednesday))
            {
                return 3;
            }
            if (dateTime.DayOfWeek.Equals(DayOfWeek.Thursday))
            {
                return 4;
            }
            if (dateTime.DayOfWeek.Equals(DayOfWeek.Friday))
            {
                return 5;
            }

            return 6;
        }

        /// <summary>
        /// 获取日期所在季度的第一月

        /// </summary>
        /// <param name="currentMonth"></param>
        /// <returns></returns>
        private static int GetQuarterFristMonth(int currentMonth)
        {
            if (currentMonth > 9)
            {
                return 10;
            }
            if (currentMonth > 6)
            {
                return 7;
            }
            if (currentMonth > 3)
            {
                return 4;
            }
            return 1;
        }

        /// <summary>
        /// 获取日期所在季度的最后一月

        /// </summary>
        /// <param name="currentMonth"></param>
        /// <returns></returns>
        private static int GetQuarterEndMonth(int currentMonth)
        {
            if (currentMonth > 9)
            {
                return 12;
            }
            if (currentMonth > 6)
            {
                return 9;
            }
            if (currentMonth > 3)
            {
                return 6;
            }

            return 3;
        }


        public static List<string> SetDateString(string datFrom, string datTo)
        {
            List<string> li = new List<string>();
            DateTime time1 = TypeUtil.ToDateTime(datFrom);
            DateTime time2 = TypeUtil.ToDateTime(datTo);
            if (datTo.Contains("9999")) return null;
            TimeSpan span = time2 - time1;
            int Day = span.Days + 1;
            for (int i = 0; i < Day; i++)
            {
                li.Add(time1.AddDays(i).ToString("yyyyMMdd"));
            }
            return li;
           
        }
    }
}
