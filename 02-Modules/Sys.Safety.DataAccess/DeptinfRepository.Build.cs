using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class DeptinfRepository:RepositoryBase<DeptinfModel>,IDeptinfRepository
    {

                public DeptinfModel AddDeptinf(DeptinfModel deptinfModel)
		{
		   return base.Insert(deptinfModel);
		}
		        public void UpdateDeptinf(DeptinfModel deptinfModel)
		{
		   base.Update(deptinfModel);
		}
	            public void DeleteDeptinf(string id)
		{
		   base.Delete(id);
		}
		        public IList<DeptinfModel> GetDeptinfList(int pageIndex, int pageSize, out int rowCount)
		{
            var deptinfModelLists = base.Datas;
		   rowCount = deptinfModelLists.Count();
           return deptinfModelLists.OrderBy(p => p.DeptCode).Skip(pageIndex * pageSize).Take(pageSize).ToList();
		}
				public DeptinfModel GetDeptinfById(string id)
		{
		    DeptinfModel deptinfModel = base.Datas.FirstOrDefault(c => c.DeptID == id);
            return deptinfModel;
		}
    }
}
