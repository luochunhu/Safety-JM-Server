using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IJc_RemarkRepository : IRepository<Jc_RemarkModel>
    {
                Jc_RemarkModel AddJc_Remark(Jc_RemarkModel jc_RemarkModel);
		        void UpdateJc_Remark(Jc_RemarkModel jc_RemarkModel);
	            void DeleteJc_Remark(string id);
		        IList<Jc_RemarkModel> GetJc_RemarkList(int pageIndex, int pageSize, out int rowCount);
				Jc_RemarkModel GetJc_RemarkById(string id);
    }
}
