using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IMsgRuleRepository : IRepository<MsgRuleModel>
    {
        MsgRuleModel AddMsgRule(MsgRuleModel msgRuleModel);
        void UpdateMsgRule(MsgRuleModel msgRuleModel);
        void DeleteMsgRule(string id);
        IList<MsgRuleModel> GetMsgRuleList(int pageIndex, int pageSize, out int rowCount);
        MsgRuleModel GetMsgRuleById(string id);

        IList<MsgRuleModel> GetAllMsgRule();
    }
}
