using System.Collections.Generic;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.R_Restrictedperson;

namespace Sys.Safety.ServiceContract
{
    public interface IR_RestrictedpersonService
    {
        BasicResponse<R_RestrictedpersonInfo> AddRestrictedperson(R_RestrictedpersonAddRequest restrictedpersonRequest);
        BasicResponse<R_RestrictedpersonInfo> UpdateRestrictedperson(R_RestrictedpersonUpdateRequest restrictedpersonRequest);
        BasicResponse DeleteRestrictedperson(R_RestrictedpersonDeleteRequest restrictedpersonRequest);
        /// <summary>
        /// 根据测点id删除测点的限制进入、禁止进入信息
        /// </summary>
        /// <param name="restrictedpersonRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteRestrictedpersonByPointId(R_RestrictedpersonDeleteByPointIdRequest restrictedpersonRequest);
        BasicResponse<List<R_RestrictedpersonInfo>> GetRestrictedpersonList(R_RestrictedpersonGetListRequest restrictedpersonRequest);
        BasicResponse<R_RestrictedpersonInfo> GetRestrictedpersonById(R_RestrictedpersonGetRequest restrictedpersonRequest);
    }
}

