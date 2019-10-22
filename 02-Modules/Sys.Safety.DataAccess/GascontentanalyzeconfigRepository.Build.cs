using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class GascontentanalyzeconfigRepository:RepositoryBase<GascontentanalyzeconfigModel>,IGascontentanalyzeconfigRepository
    {

                public GascontentanalyzeconfigModel AddGascontentanalyzeconfig(GascontentanalyzeconfigModel gascontentanalyzeconfigModel)
		{
		   return base.Insert(gascontentanalyzeconfigModel);
		}
		        public void UpdateGascontentanalyzeconfig(GascontentanalyzeconfigModel gascontentanalyzeconfigModel)
		{
		   base.Update(gascontentanalyzeconfigModel);
		}
	            public void DeleteGascontentanalyzeconfig(string id)
		{
		   base.Delete(id);
		}
		        public IList<GascontentanalyzeconfigModel> GetGascontentanalyzeconfigList(int pageIndex, int pageSize, out int rowCount)
		{	      
		   rowCount = base.Datas.Count();
           return base.Datas.Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public GascontentanalyzeconfigModel GetGascontentanalyzeconfigById(string id)
		{
		    GascontentanalyzeconfigModel gascontentanalyzeconfigModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return gascontentanalyzeconfigModel;
		}
    }
}
