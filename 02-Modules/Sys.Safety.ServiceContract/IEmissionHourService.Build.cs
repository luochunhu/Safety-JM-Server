using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_Ll_H;

namespace Sys.Safety.ServiceContract
{
    public interface IEmissionHourService
    {
        BasicResponse<Jc_Ll_HInfo> AddEmissionHour(Jc_Ll_HAddRequest jc_Ll_Hrequest);
        BasicResponse<Jc_Ll_HInfo> UpdateEmissionHour(Jc_Ll_HUpdateRequest jc_Ll_Hrequest);
        BasicResponse DeleteEmissionHour(Jc_Ll_HDeleteRequest jc_Ll_Hrequest);
        BasicResponse<List<Jc_Ll_HInfo>> GetEmissionHourList(Jc_Ll_HGetListRequest jc_Ll_Hrequest);
        BasicResponse<Jc_Ll_HInfo> GetEmissionHourById(Jc_Ll_HGetRequest jc_Ll_Hrequest);
    }
}