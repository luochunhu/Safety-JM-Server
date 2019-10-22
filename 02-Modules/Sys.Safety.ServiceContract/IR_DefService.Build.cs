using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.R_Def;

namespace Sys.Safety.ServiceContract
{
    public interface IR_DefService
    {
        BasicResponse<Jc_DefInfo> AddDef(R_DefAddRequest defRequest);
        BasicResponse<Jc_DefInfo> UpdateDef(R_DefUpdateRequest defRequest);
        BasicResponse DeleteDef(R_DefDeleteRequest defRequest);
        BasicResponse<List<Jc_DefInfo>> GetDefList(R_DefGetListRequest defRequest);
        BasicResponse<Jc_DefInfo> GetDefById(R_DefGetRequest defRequest);

        BasicResponse<List<Jc_DefInfo>> GetAllDefInfo();

        /// <summary>老人员定位系统测点同步接口
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //BasicResponse OldPlsPointSync(OldPlsPointSyncRequest request);
    }
}

