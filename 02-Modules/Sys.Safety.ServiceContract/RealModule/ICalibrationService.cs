using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.Request.Calibration;

namespace Sys.Safety.ServiceContract
{
    public interface ICalibrationService
    {
        /// <summary>
        /// 获取标校详情
        /// </summary>
        /// <param name="calibrationRequest"></param>
        /// <returns></returns>
        BasicResponse<DataTable> GetBxDetail(GetCalibrationDetailRequest calibrationRequest);

        /// <summary>
        ///获取标校统计
        /// </summary>
        /// <returns></returns>
        BasicResponse<DataTable> GetBxStatistics(GetCalibrationStatisticsRequest calibrationRequest);
    }
}
