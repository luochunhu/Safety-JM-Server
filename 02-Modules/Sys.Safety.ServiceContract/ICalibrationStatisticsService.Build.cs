using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_Bxex;

namespace Sys.Safety.ServiceContract
{
    public interface ICalibrationStatisticsService
    {
        BasicResponse<Jc_BxexInfo> AddCalibrationStatistics(Jc_BxexAddRequest jc_Bxexrequest);
        BasicResponse<Jc_BxexInfo> UpdateCalibrationStatistics(Jc_BxexUpdateRequest jc_Bxexrequest);
        BasicResponse DeleteCalibrationStatistics(Jc_BxexDeleteRequest jc_Bxexrequest);
        BasicResponse<List<Jc_BxexInfo>> GetCalibrationStatisticsList(Jc_BxexGetListRequest jc_Bxexrequest);
        BasicResponse<Jc_BxexInfo> GetCalibrationStatisticsById(Jc_BxexGetRequest jc_Bxexrequest);
    }
}