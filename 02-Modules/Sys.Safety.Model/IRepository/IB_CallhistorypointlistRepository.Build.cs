using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_CallhistorypointlistRepository : IRepository<B_CallhistorypointlistModel>
    {
                B_CallhistorypointlistModel AddB_Callhistorypointlist(B_CallhistorypointlistModel b_CallhistorypointlistModel);
		        void UpdateB_Callhistorypointlist(B_CallhistorypointlistModel b_CallhistorypointlistModel);
	            void DeleteB_Callhistorypointlist(string id);
		        IList<B_CallhistorypointlistModel> GetB_CallhistorypointlistList(int pageIndex, int pageSize, out int rowCount);
				B_CallhistorypointlistModel GetB_CallhistorypointlistById(string id);
    }
}
