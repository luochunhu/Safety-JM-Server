using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_Ll_M;

namespace Sys.Safety.ServiceContract
{
    public interface IEmissionMonthService
    {
        BasicResponse<Jc_Ll_MInfo> AddEmissionMonth(Jc_Ll_MAddRequest jc_Ll_Mrequest);
        BasicResponse<Jc_Ll_MInfo> UpdateEmissionMonth(Jc_Ll_MUpdateRequest jc_Ll_Mrequest);
        BasicResponse DeleteEmissionMonth(Jc_Ll_MDeleteRequest jc_Ll_Mrequest);
        BasicResponse<List<Jc_Ll_MInfo>> GetEmissionMonthList(Jc_Ll_MGetListRequest jc_Ll_Mrequest);
        BasicResponse<Jc_Ll_MInfo> GetEmissionMonthById(Jc_Ll_MGetRequest jc_Ll_Mrequest);
    }
}