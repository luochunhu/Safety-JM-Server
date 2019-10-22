using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.Arearestrictedperson;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IR_ArearestrictedpersonService
    {
        BasicResponse<R_ArearestrictedpersonInfo> AddArearestrictedperson(R_ArearestrictedpersonAddRequest arearestrictedpersonRequest);
        BasicResponse<R_ArearestrictedpersonInfo> UpdateArearestrictedperson(R_ArearestrictedpersonUpdateRequest arearestrictedpersonRequest);
        BasicResponse DeleteArearestrictedperson(R_ArearestrictedpersonDeleteRequest arearestrictedpersonRequest);
        BasicResponse<List<R_ArearestrictedpersonInfo>> GetArearestrictedpersonList(R_ArearestrictedpersonGetListRequest arearestrictedpersonRequest);
        BasicResponse<R_ArearestrictedpersonInfo> GetArearestrictedpersonById(R_ArearestrictedpersonGetRequest arearestrictedpersonRequest);
        /// <summary>
        /// 根据区域ID删除区域限制进入、禁止进入人员信息
        /// </summary>
        /// <param name="arearestrictedpersonRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteArearestrictedpersonByAreaId(R_ArearestrictedpersonDeleteByAreaIdRequest arearestrictedpersonRequest);
    }
}

