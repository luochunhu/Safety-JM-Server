using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IOperatelogRepository : IRepository<OperatelogModel>
    {
        OperatelogModel AddOperatelog(OperatelogModel operatelogModel);
        void UpdateOperatelog(OperatelogModel operatelogModel);
        void DeleteOperatelog(string id);
        IList<OperatelogModel> GetOperatelogList(int pageIndex, int pageSize, out int rowCount);
        List<OperatelogModel> GetOperatelogList();
        OperatelogModel GetOperatelogById(string id);
        void DeleteOperatelogByEtime(DateTime Etime);
        void DeleteOperatelogByStimeEtime(DateTime Etime, DateTime Stime);
    }
}
