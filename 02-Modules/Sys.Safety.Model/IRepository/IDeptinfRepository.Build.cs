using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IDeptinfRepository : IRepository<DeptinfModel>
    {
                DeptinfModel AddDeptinf(DeptinfModel deptinfModel);
		        void UpdateDeptinf(DeptinfModel deptinfModel);
	            void DeleteDeptinf(string id);
		        IList<DeptinfModel> GetDeptinfList(int pageIndex, int pageSize, out int rowCount);
				DeptinfModel GetDeptinfById(string id);
    }
}
