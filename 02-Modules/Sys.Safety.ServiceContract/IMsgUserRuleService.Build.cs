using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.MsgUserRule;
using Sys.Safety.DataContract;

namespace Sys.Safety.ServiceContract
{
    public interface IMsgUserRuleService
    {
        BasicResponse<MsgUserRuleInfo> AddMsgUserRule(MsgUserRuleAddRequest msgUserRuleRequest);
        BasicResponse<MsgUserRuleInfo> UpdateMsgUserRule(MsgUserRuleUpdateRequest msgUserRuleRequest);
        BasicResponse DeleteMsgUserRule(MsgUserRuleDeleteRequest msgUserRuleRequest);
        BasicResponse<List<MsgUserRuleInfo>> GetMsgUserRuleList(MsgUserRuleGetListRequest msgUserRuleRequest);
        BasicResponse<MsgUserRuleInfo> GetMsgUserRuleById(MsgUserRuleGetRequest msgUserRuleRequest);

        BasicResponse<List<MsgUserRuleInfo>> GetAllMsgUserInfo(BasicRequest msgUserRuleRequest);

        BasicResponse<bool> BatchInsert(MsgUserRuleBatchRequest msgUserRuleRequest);

        BasicResponse<MsgUserRuleInfo> GetMsgUserRuleByUserInfoAndRuleId(MsgUserRuleGetByUserInfoAndRuleIdRequest msgUserRuleRequest);
    }
}

