using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IFactorRepository : IRepository<JC_FactorModel>
    {
                JC_FactorModel AddJC_Factor(JC_FactorModel jC_FactorModel);
		        void UpdateJC_Factor(JC_FactorModel jC_FactorModel);
	            void DeleteJC_Factor(string id);
		        IList<JC_FactorModel> GetJC_FactorList();
				JC_FactorModel GetJC_FactorById(string id);
    }
}
