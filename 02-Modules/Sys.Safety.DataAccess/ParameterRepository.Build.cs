using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class ParameterRepository : RepositoryBase<JC_ParameterModel>, IParameterRepository
    {

        public JC_ParameterModel AddJC_Parameter(JC_ParameterModel jC_ParameterModel)
        {
            return base.Insert(jC_ParameterModel);
        }
        public void UpdateJC_Parameter(JC_ParameterModel jC_ParameterModel)
        {
            base.Update(jC_ParameterModel);
        }
        public void DeleteJC_Parameter(string id)
        {
            base.Delete(id);
        }
        public IList<JC_ParameterModel> GetJC_ParameterList()
        {
            var jC_ParameterModelLists = base.Datas.ToList();
            return jC_ParameterModelLists;
        }
        public JC_ParameterModel GetJC_ParameterById(string id)
        {
            JC_ParameterModel jC_ParameterModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return jC_ParameterModel;
        }
    }
}
