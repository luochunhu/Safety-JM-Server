using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class FactorRepository : RepositoryBase<JC_FactorModel>, IFactorRepository
    {

        public JC_FactorModel AddJC_Factor(JC_FactorModel jC_FactorModel)
        {
            return base.Insert(jC_FactorModel);
        }
        public void UpdateJC_Factor(JC_FactorModel jC_FactorModel)
        {
            base.Update(jC_FactorModel);
        }
        public void DeleteJC_Factor(string id)
        {
            base.Delete(id);
        }
        public IList<JC_FactorModel> GetJC_FactorList()
        {
            var jC_FactorModelLists = base.Datas.ToList();
            return jC_FactorModelLists;
        }
        public JC_FactorModel GetJC_FactorById(string id)
        {
            JC_FactorModel jC_FactorModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_FactorModel;
        }
    }
}
