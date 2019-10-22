using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Jc_Ll_D;

namespace Sys.Safety.ServiceContract
{
    public interface IEmissionDayService
    {
        BasicResponse<Jc_Ll_DInfo> AddEmissionDay(Jc_Ll_DAddRequest jc_Ll_Drequest);
        BasicResponse<Jc_Ll_DInfo> UpdateEmissionDay(Jc_Ll_DUpdateRequest jc_Ll_Drequest);
        BasicResponse DeleteEmissionDay(Jc_Ll_DDeleteRequest jc_Ll_Drequest);
        BasicResponse<List<Jc_Ll_DInfo>> GetEmissionDayList(Jc_Ll_DGetListRequest jc_Ll_Drequest);
        BasicResponse<Jc_Ll_DInfo> GetEmissionDayById(Jc_Ll_DGetRequest jc_Ll_Drequest);
    }
}