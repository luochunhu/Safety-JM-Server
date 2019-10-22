using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    public enum Recognizer
    {
        /*
         * 0.未定义;
         * 1.井下普通站;
         * 02.井下入口站;
         * 03.井下出口站;
         * 04.井下出入站;
         * 05.地面普通站;
         * 06.地面入口站;
         * 07.地面出口站;
         * 08.地面出入站;
         * 9.考勤机入口站;
         * 10.考勤机出口站;
         * 11.考勤机出入站
         */
        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义")]
        Undefine = 0,
        /// <summary>
        /// 井下普通站
        /// </summary>
        [Description("井下普通站")]
        InWellNomalStation = 1,
        /// <summary>
        /// 井下入口站
        /// </summary>
        [Description("井下入口站")]
        InWellInStation = 2,
        /// <summary>
        /// 井下出口站
        /// </summary>
        [Description("井下出口站")]
        InWellOutStation = 3,
        /// <summary>
        /// 井下出入站
        /// </summary>
        [Description("井下出入站")]
        InWellInOutStation = 4,
        /// <summary>
        /// 地面普通站
        /// </summary>
        [Description("地面普通站")]
        GroundNomalStation = 5,
        /// <summary>
        /// 地面入口站
        /// </summary>
        [Description("地面入口站")]
        GroundInStation = 6,
        /// <summary>
        /// 地面出口站
        /// </summary>
        [Description("地面出口站")]
        GroundOutStation = 7,
        /// <summary>
        /// 地面出入站
        /// </summary>
        [Description("地面出入站")]
        GroundInOutStation = 8,
        /// <summary>
        /// 考勤机入口站
        /// </summary>
        [Description("考勤机入口站")]
        AttendanceInStation = 9,
        /// <summary>
        /// 考勤机出口站
        /// </summary>
        [Description("考勤机出口站")]
        AttendanceOutStation = 10,
        /// <summary>
        /// 考勤机出入站
        /// </summary>
        [Description("考勤机出入站")]
        AttendanceInOutStation = 11
    }
}
