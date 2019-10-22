using Basic.Framework.Data;
using Sys.Safety.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Safety.Model
{
    public interface IAreaRuleRepository : IRepository<AreaRuleModel>
    {
        AreaRuleModel AddAreaRule(AreaRuleModel areaRuleModel);
        void UpdateAreaRule(AreaRuleModel areaRuleModel);
        void DeleteAreaRule(string id);
        IList<AreaRuleModel> GetAreaRuleList(int pageIndex, int pageSize, out int rowCount);
        AreaRuleModel GetAreaRuleById(string id);

        IList<AreaRuleModel> GetAreaRuleList();

        void DeleteAreaRuleByAreaID(string id);
    }
}