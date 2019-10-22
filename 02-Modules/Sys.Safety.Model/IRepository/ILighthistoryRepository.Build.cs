using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface ILighthistoryRepository : IRepository<LighthistoryModel>
    {
                LighthistoryModel AddLighthistory(LighthistoryModel lighthistoryModel);
		        void UpdateLighthistory(LighthistoryModel lighthistoryModel);
	            void DeleteLighthistory(string id);
		        IList<LighthistoryModel> GetLighthistoryList(int pageIndex, int pageSize, out int rowCount);
				LighthistoryModel GetLighthistoryById(string id);
    }
}
