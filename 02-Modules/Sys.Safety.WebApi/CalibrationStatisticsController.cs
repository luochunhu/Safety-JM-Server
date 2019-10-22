using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Basic.Framework.Service;
using Basic.Framework.Web.WebApi;
using Sys.Safety.ServiceContract;

namespace Sys.Safety.WebApi
{
    public class CalibrationStatisticsController : BasicApiController, ICalibrationStatisticsService
    {
        ICalibrationStatisticsService _calibrationStatisticsService = ServiceFactory.Create<ICalibrationStatisticsService>();
        
        [HttpPost]
        [Route("v1/CalibrationStatistics/AddCalibrationStatistics")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_BxexInfo> AddCalibrationStatistics(Sys.Safety.Request.Jc_Bxex.Jc_BxexAddRequest jc_Bxexrequest)
        {
            return _calibrationStatisticsService.AddCalibrationStatistics(jc_Bxexrequest);
        }

        [HttpPost]
        [Route("v1/CalibrationStatistics/UpdateCalibrationStatistics")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_BxexInfo> UpdateCalibrationStatistics(Sys.Safety.Request.Jc_Bxex.Jc_BxexUpdateRequest jc_Bxexrequest)
        {
            return _calibrationStatisticsService.UpdateCalibrationStatistics(jc_Bxexrequest);
        }

        [HttpPost]
        [Route("v1/CalibrationStatistics/DeleteCalibrationStatistics")]
        public Basic.Framework.Web.BasicResponse DeleteCalibrationStatistics(Sys.Safety.Request.Jc_Bxex.Jc_BxexDeleteRequest jc_Bxexrequest)
        {
            return _calibrationStatisticsService.DeleteCalibrationStatistics(jc_Bxexrequest);
        }

        [HttpPost]
        [Route("v1/CalibrationStatistics/GetCalibrationStatisticsList")]
        public Basic.Framework.Web.BasicResponse<List<Sys.Safety.DataContract.Jc_BxexInfo>> GetCalibrationStatisticsList(Sys.Safety.Request.Jc_Bxex.Jc_BxexGetListRequest jc_Bxexrequest)
        {
            return _calibrationStatisticsService.GetCalibrationStatisticsList(jc_Bxexrequest);
        }

        [HttpPost]
        [Route("v1/CalibrationStatistics/GetCalibrationStatisticsById")]
        public Basic.Framework.Web.BasicResponse<Sys.Safety.DataContract.Jc_BxexInfo> GetCalibrationStatisticsById(Sys.Safety.Request.Jc_Bxex.Jc_BxexGetRequest jc_Bxexrequest)
        {
            return _calibrationStatisticsService.GetCalibrationStatisticsById(jc_Bxexrequest);
        }
    }
}
