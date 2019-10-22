using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.B_Def;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IB_DefService
    {
        BasicResponse<Jc_DefInfo> AddDef(B_DefAddRequest defRequest);
        BasicResponse<Jc_DefInfo> UpdateDef(B_DefUpdateRequest defRequest);
        BasicResponse DeleteDef(B_DefDeleteRequest defRequest);
        BasicResponse<List<Jc_DefInfo>> GetDefList(B_DefGetListRequest defRequest);
        BasicResponse<Jc_DefInfo> GetDefById(B_DefGetRequest defRequest);

        BasicResponse<List<Jc_DefInfo>> GetAll(BasicRequest defRequest);     
    }
}

