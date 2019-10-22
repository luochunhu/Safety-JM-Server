using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_Ll_Y;

namespace Sys.Safety.ServiceContract
{
    public interface IEmissionYearService
    {
        BasicResponse<Jc_Ll_YInfo> AddEmissionYear(Jc_Ll_YAddRequest jc_Ll_Yrequest);
        BasicResponse<Jc_Ll_YInfo> UpdateEmissionYear(Jc_Ll_YUpdateRequest jc_Ll_Yrequest);
        BasicResponse DeleteEmissionYear(Jc_Ll_YDeleteRequest jc_Ll_Yrequest);
        BasicResponse<List<Jc_Ll_YInfo>> GetEmissionYearList(Jc_Ll_YGetListRequest jc_Ll_Yrequest);
        BasicResponse<Jc_Ll_YInfo> GetEmissionYearById(Jc_Ll_YGetRequest jc_Ll_Yrequest);
    }
}