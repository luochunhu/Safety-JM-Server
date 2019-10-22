using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Model
{
    public interface ICalibrationRepository
    {
        /// <summary>
        /// 获取标校详情
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable GetBxDetail(DateTime startTime, DateTime endTime);

        /// <summary>
        ///获取标校统计
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        DataTable GetBxStatistics(DateTime time);
    }
}
