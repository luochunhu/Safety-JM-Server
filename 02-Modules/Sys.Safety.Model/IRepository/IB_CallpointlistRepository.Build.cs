using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IB_CallpointlistRepository : IRepository<B_CallpointlistModel>
    {
                B_CallpointlistModel AddB_Callpointlist(B_CallpointlistModel b_CallpointlistModel);
		        void UpdateB_Callpointlist(B_CallpointlistModel b_CallpointlistModel);
	            void DeleteB_Callpointlist(string id);
		        IList<B_CallpointlistModel> GetB_CallpointlistList(int pageIndex, int pageSize, out int rowCount);
				B_CallpointlistModel GetB_CallpointlistById(string id);
    }
}
