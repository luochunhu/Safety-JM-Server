using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IGascontentanalyzeconfigRepository : IRepository<GascontentanalyzeconfigModel>
    {
                GascontentanalyzeconfigModel AddGascontentanalyzeconfig(GascontentanalyzeconfigModel gascontentanalyzeconfigModel);
		        void UpdateGascontentanalyzeconfig(GascontentanalyzeconfigModel gascontentanalyzeconfigModel);
	            void DeleteGascontentanalyzeconfig(string id);
		        IList<GascontentanalyzeconfigModel> GetGascontentanalyzeconfigList(int pageIndex, int pageSize, out int rowCount);
				GascontentanalyzeconfigModel GetGascontentanalyzeconfigById(string id);
    }
}
