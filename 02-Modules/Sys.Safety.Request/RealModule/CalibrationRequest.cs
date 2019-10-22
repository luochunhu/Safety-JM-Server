using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Request.Calibration
{
    /// <summary>
    /// 获取获取标校详情Request
    /// </summary>
    public partial class GetCalibrationDetailRequest : Basic.Framework.Web.BasicRequest
    { 
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime Time { get; set; }
    }


    /// <summary>
    /// 获取标校统计Request
    /// </summary>
    public partial class GetCalibrationStatisticsRequest : Basic.Framework.Web.BasicRequest
    {
        /// <summary>
        /// 标校时间
        /// </summary>
        public DateTime Time { get; set; }
    }



}
