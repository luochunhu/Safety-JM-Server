using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_Bx;

namespace Sys.Safety.ServiceContract
{
    public interface ICalibrationDefService
    {
        BasicResponse<Jc_BxInfo> AddCalibrationDef(Jc_BxAddRequest jc_Bxrequest);
        BasicResponse<Jc_BxInfo> UpdateCalibrationDef(Jc_BxUpdateRequest jc_Bxrequest);
        BasicResponse DeleteCalibrationDef(Jc_BxDeleteRequest jc_Bxrequest);
        BasicResponse<List<Jc_BxInfo>> GetCalibrationDefList(Jc_BxGetListRequest jc_Bxrequest);
        BasicResponse<Jc_BxInfo> GetCalibrationDefById(Jc_BxGetRequest jc_Bxrequest);
    }
}