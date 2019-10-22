using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class OperatelogRepository : RepositoryBase<OperatelogModel>, IOperatelogRepository
    {

        public OperatelogModel AddOperatelog(OperatelogModel operatelogModel)
        {
            return base.Insert(operatelogModel);
        }
        public void UpdateOperatelog(OperatelogModel operatelogModel)
        {
            base.Update(operatelogModel);
        }
        public void DeleteOperatelog(string id)
        {
            base.Delete(id);
        }
        public IList<OperatelogModel> GetOperatelogList(int pageIndex, int pageSize, out int rowCount)
        {
            var operatelogModelLists = base.Datas;
            rowCount = operatelogModelLists.Count();
            return operatelogModelLists.OrderBy(p => p.CreateTime).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<OperatelogModel> GetOperatelogList()
        {
            var operatelogModelLists = base.Datas.ToList();           
            return operatelogModelLists;
        }
        public OperatelogModel GetOperatelogById(string id)
        {
            OperatelogModel operatelogModel = base.Datas.FirstOrDefault(c => c.OperateLogID == id);
            return operatelogModel;
        }

        public void DeleteOperatelogByEtime(DateTime Etime)
        {
            base.ExecuteNonQuery("global_OperateLogHelper_DelOperateLog_ByEtime", Etime);            
        }
        public void DeleteOperatelogByStimeEtime(DateTime Etime,DateTime Stime)
        {
            base.ExecuteNonQuery("global_OperateLogHelper_DelOperateLog_ByStimeEtime", Etime,Stime);
        }
    }
}
