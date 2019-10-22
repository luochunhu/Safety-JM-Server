using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Area;

namespace Sys.Safety.ServiceContract
{
    public interface IAreaRuleService
    {
        BasicResponse<AreaRuleInfo> AddAreaRule(AreaRuleAddRequest areaRuleRequest);
        BasicResponse<AreaRuleInfo> UpdateAreaRule(AreaRuleUpdateRequest areaRuleRequest);
        BasicResponse DeleteAreaRule(AreaRuleDeleteRequest areaRuleRequest);
        BasicResponse<List<AreaRuleInfo>> GetAreaRuleList(AreaRuleGetListRequest areaRuleRequest);
        BasicResponse<AreaRuleInfo> GetAreaRuleById(AreaRuleGetRequest areaRuleRequest);

        BasicResponse<List<AreaRuleInfo>> GetAreaRuleListByAreaID(GetAreaRuleListByAreaIDRequest areaRuleRequest);

        BasicResponse DeleteAreaRuleByAreaID(AreaRuleDeleteRequest areaRuleRequest);
    }
}

