using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Http;
using Basic.Framework.Web;
using Basic.Framework.Service;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Calibration;

namespace Sys.Safety.WebApi
{
    public class CalibrationController : Basic.Framework.Web.WebApi.BasicApiController, ICalibrationService
    {

        private ICalibrationService calibrationService = ServiceFactory.Create<ICalibrationService>();
        /// <summary>
        /// 获取标校详情
        /// </summary>
        /// <param name="calibrationRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/calibration/getbxdetail")]
        public BasicResponse<DataTable> GetBxDetail(GetCalibrationDetailRequest calibrationRequest)
        {
            return calibrationService.GetBxDetail(calibrationRequest);
        }

        /// <summary>
        /// 获取标校统计详情
        /// </summary>
        /// <param name="calibrationRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/calibration/getbxstatistics")]
        public BasicResponse<DataTable> GetBxStatistics(GetCalibrationStatisticsRequest calibrationRequest)
        {
            return calibrationService.GetBxStatistics(calibrationRequest);
        }
    }
}
