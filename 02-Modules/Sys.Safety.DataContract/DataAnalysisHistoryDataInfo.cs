using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    /// <summary>
    /// 大数据分析，历史数据. 如月平均值、周平均值、日最大值、日平均值、5分钟最大值、5分钟平均值
    /// </summary>
    public class DataAnalysisHistoryDataInfo : Basic.Framework.Web.BasicInfo
    {
        /// <summary>
        /// 测点Id
        /// </summary>
        public string PointId { get; set; }

        /// <summary>
        /// 测点号
        /// </summary>
        public string Point { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string Wz { get; set; }

        /// <summary>
        /// 月平均值
        /// </summary>
        public decimal MonthAverageValue { get; set; }

        /// <summary>
        /// 周平均值
        /// </summary>
        public decimal WeekAverageValue { get; set; }

        /// <summary>
        /// 日最大值
        /// </summary>
        public decimal DayMaxValue { get; set; }

        /// <summary>
        /// 日平均值
        /// </summary>
        public decimal DayAverageValue { get; set; }

        /// <summary>
        /// 5分钟最大值
        /// </summary>
        public decimal FiveMinutesMaxValue { get; set; }

        /// <summary>
        /// 5分钟平均值
        /// </summary>
        public decimal FiveMinutesAverageValue { get; set; }
    }
}
