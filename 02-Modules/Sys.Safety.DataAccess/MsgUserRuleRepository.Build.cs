using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class MsgUserRuleRepository : RepositoryBase<MsgUserRuleModel>, IMsgUserRuleRepository
    {

        public MsgUserRuleModel AddMsgUserRule(MsgUserRuleModel msgUserRuleModel)
        {
            return base.Insert(msgUserRuleModel);
        }
        public void UpdateMsgUserRule(MsgUserRuleModel msgUserRuleModel)
        {
            base.Update(msgUserRuleModel);
        }
        public void DeleteMsgUserRule(string id)
        {
            base.Delete(id);
        }
        public IList<MsgUserRuleModel> GetMsgUserRuleList(int pageIndex, int pageSize, out int rowCount)
        {
            // var msgUserRuleModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return base.Datas.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public MsgUserRuleModel GetMsgUserRuleById(string id)
        {
            MsgUserRuleModel msgUserRuleModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return msgUserRuleModel;
        }
    }
}
