using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IR_DeptRepository : IRepository<R_DeptModel>
    {
        R_DeptModel AddDept(R_DeptModel deptModel);
        void UpdateDept(R_DeptModel deptModel);
        void DeleteDept(string id);
        IList<R_DeptModel> GetDeptList(int pageIndex, int pageSize, out int rowCount);
        R_DeptModel GetDeptById(string id);
    }
}
