using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_CallhistoryRepository : IRepository<B_CallhistoryModel>
    {
                B_CallhistoryModel AddB_Callhistory(B_CallhistoryModel b_CallhistoryModel);
		        void UpdateB_Callhistory(B_CallhistoryModel b_CallhistoryModel);
	            void DeleteB_Callhistory(string id);
		        IList<B_CallhistoryModel> GetB_CallhistoryList(int pageIndex, int pageSize, out int rowCount);
				B_CallhistoryModel GetB_CallhistoryById(string id);
    }
}
