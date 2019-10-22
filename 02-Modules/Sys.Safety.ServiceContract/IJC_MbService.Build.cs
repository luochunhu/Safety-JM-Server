using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.JC_Mb;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IJC_MbService
    {
        BasicResponse<JC_MbInfo> AddMb(JC_MbAddRequest mbrequest);
        BasicResponse<JC_MbInfo> UpdateMb(JC_MbUpdateRequest mbrequest);
        BasicResponse DeleteMb(JC_MbDeleteRequest mbrequest);
        BasicResponse<List<JC_MbInfo>> GetMbList(JC_MbGetListRequest mbrequest);
        BasicResponse<JC_MbInfo> GetMbById(JC_MbGetRequest mbrequest);	
    }
}

