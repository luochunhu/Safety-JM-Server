using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_AnalysistemplatealarmlevelRepository : RepositoryBase<Jc_AnalysistemplatealarmlevelModel>, IJc_AnalysistemplatealarmlevelRepository
    {

        public Jc_AnalysistemplatealarmlevelModel AddAnalysistemplatealarmlevel(Jc_AnalysistemplatealarmlevelModel analysistemplatealarmlevelModel)
        {
            return base.Insert(analysistemplatealarmlevelModel);
        }
        public void UpdateAnalysistemplatealarmlevel(Jc_AnalysistemplatealarmlevelModel analysistemplatealarmlevelModel)
        {
            base.Update(analysistemplatealarmlevelModel);
        }
        public void DeleteAnalysistemplatealarmlevel(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_AnalysistemplatealarmlevelModel> GetAnalysistemplatealarmlevelList(int pageIndex, int pageSize, out int rowCount)
        {
            rowCount = base.Datas.Count();
            return base.Datas.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public Jc_AnalysistemplatealarmlevelModel GetAnalysistemplatealarmlevelById(string id)
        {
            Jc_AnalysistemplatealarmlevelModel analysistemplatealarmlevelModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return analysistemplatealarmlevelModel;
        }
    }
}
