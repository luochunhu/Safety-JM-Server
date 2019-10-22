using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Pb;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IR_PbService
    {
        BasicResponse<List<R_PbInfo>> GetAlarmedDataList();
        BasicResponse<R_PbInfo> AddPb(R_PbAddRequest pbRequest);
        BasicResponse<R_PbInfo> UpdatePb(R_PbUpdateRequest pbRequest);
        BasicResponse DeletePb(R_PbDeleteRequest pbRequest);
        BasicResponse<List<R_PbInfo>> GetPbList(R_PbGetListRequest pbRequest);
        BasicResponse<R_PbInfo> GetPbById(R_PbGetRequest pbRequest);

        BasicResponse<List<R_PbInfo>> GetPbByPar(R_PbGetByParRequest pbRequest);

        BasicResponse BacthUpdateAlarmRecord(R_PBBatchUpateRequest r_PbList);

    }
}

