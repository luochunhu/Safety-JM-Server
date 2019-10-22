using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_AnalysistemplatealarmlevelRepository : IRepository<Jc_AnalysistemplatealarmlevelModel>
    {
        Jc_AnalysistemplatealarmlevelModel AddAnalysistemplatealarmlevel(Jc_AnalysistemplatealarmlevelModel analysistemplatealarmlevelModel);
        void UpdateAnalysistemplatealarmlevel(Jc_AnalysistemplatealarmlevelModel analysistemplatealarmlevelModel);
        void DeleteAnalysistemplatealarmlevel(string id);
        IList<Jc_AnalysistemplatealarmlevelModel> GetAnalysistemplatealarmlevelList(int pageIndex, int pageSize, out int rowCount);
        Jc_AnalysistemplatealarmlevelModel GetAnalysistemplatealarmlevelById(string id);
    }
}
