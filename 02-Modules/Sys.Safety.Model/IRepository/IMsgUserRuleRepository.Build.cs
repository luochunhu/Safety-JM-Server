using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IMsgUserRuleRepository : IRepository<MsgUserRuleModel>
    {
                MsgUserRuleModel AddMsgUserRule(MsgUserRuleModel msgUserRuleModel);
		        void UpdateMsgUserRule(MsgUserRuleModel msgUserRuleModel);
	            void DeleteMsgUserRule(string id);
		        IList<MsgUserRuleModel> GetMsgUserRuleList(int pageIndex, int pageSize, out int rowCount);
				MsgUserRuleModel GetMsgUserRuleById(string id);
    }
}
