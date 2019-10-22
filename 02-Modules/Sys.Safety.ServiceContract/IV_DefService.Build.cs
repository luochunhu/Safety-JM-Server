using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Def;

namespace Sys.Safety.ServiceContract
{
    public interface IV_DefService
    {
        BasicResponse<V_DefInfo> AddDef(DefAddRequest defRequest);
        BasicResponse<V_DefInfo> UpdateDef(DefUpdateRequest defRequest);
        BasicResponse DeleteDef(DefDeleteRequest defRequest);
        BasicResponse<List<V_DefInfo>> GetDefList(DefGetListRequest defRequest);
        BasicResponse<V_DefInfo> GetDefById(DefGetRequest defRequest);

        BasicResponse<List<V_DefInfo>> GetAllDef(DefGetAllRequest defRequest);

        BasicResponse<List<V_DefInfo>> GetAllVideoDefCache();

        BasicResponse<V_DefInfo> GetDefByIP(DefIPRequest defRequest);
    }
}

