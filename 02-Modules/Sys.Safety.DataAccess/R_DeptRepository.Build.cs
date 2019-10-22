using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class R_DeptRepository : RepositoryBase<R_DeptModel>, IR_DeptRepository
    {

        public R_DeptModel AddDept(R_DeptModel deptModel)
        {
            return base.Insert(deptModel);
        }
        public void UpdateDept(R_DeptModel deptModel)
        {
            base.Update(deptModel);
        }
        public void DeleteDept(string id)
        {
            base.Delete(id);
        }
        public IList<R_DeptModel> GetDeptList(int pageIndex, int pageSize, out int rowCount)
        {
            //var deptModelLists = base.Datas.ToList();
            rowCount = base.Datas.Count();
            return base.Datas.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public R_DeptModel GetDeptById(string id)
        {
            R_DeptModel deptModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return deptModel;
        }
    }
}
