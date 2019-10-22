using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IParameterRepository : IRepository<JC_ParameterModel>
    {
                JC_ParameterModel AddJC_Parameter(JC_ParameterModel jC_ParameterModel);
		        void UpdateJC_Parameter(JC_ParameterModel jC_ParameterModel);
	            void DeleteJC_Parameter(string id);
		        IList<JC_ParameterModel> GetJC_ParameterList();
				JC_ParameterModel GetJC_ParameterById(string id);
    }
}
