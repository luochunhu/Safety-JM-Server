using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Sys.Safety.Model;

namespace Sys.Safety.DataAccess
{
    public partial class AreaRuleRepository : RepositoryBase<AreaRuleModel>, IAreaRuleRepository
    {

        public AreaRuleModel AddAreaRule(AreaRuleModel areaRuleModel)
        {
            return base.Insert(areaRuleModel);
        }
        public void UpdateAreaRule(AreaRuleModel areaRuleModel)
        {
            base.Update(areaRuleModel);
        }
        public void DeleteAreaRule(string id)
        {
            base.Delete(id);
        }
        public IList<AreaRuleModel> GetAreaRuleList(int pageIndex, int pageSize, out int rowCount)
        {
            var areaRuleModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return areaRuleModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public AreaRuleModel GetAreaRuleById(string id)
        {
            AreaRuleModel areaRuleModel = base.Datas.FirstOrDefault(c => c.RuleID == id);
            return areaRuleModel;
        }
        public IList<AreaRuleModel> GetAreaRuleList()
        {
            var areaRuleModelLists = base.Datas.ToList();
            return areaRuleModelLists;
        }


        public void DeleteAreaRuleByAreaID(string id)
        {
            base.QueryTable("global_DeleteAreaRulesByAreaID_Data", id);
        }
    }
}
