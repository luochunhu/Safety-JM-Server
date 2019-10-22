using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class RunlogRepository : RepositoryBase<RunlogModel>, IRunlogRepository
    {

        public RunlogModel AddRunlog(RunlogModel runlogModel)
        {
            return base.Insert(runlogModel);
        }
        public void UpdateRunlog(RunlogModel runlogModel)
        {
            base.Update(runlogModel);
        }
        public void DeleteRunlog(string id)
        {
            base.Delete(id);
        }
        public IList<RunlogModel> GetRunlogList(int pageIndex, int pageSize, out int rowCount)
        {
            var runlogModelLists = base.Datas;
            rowCount = runlogModelLists.Count();
            return runlogModelLists.OrderBy(p => p.ID).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public List<RunlogModel> GetRunlogList()
        {
            var runlogModelLists = base.Datas.ToList();
            return runlogModelLists;
        }
        public RunlogModel GetRunlogById(string id)
        {
            RunlogModel runlogModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return runlogModel;
        }
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="Etime"></param>        
        /// <returns></returns>
        public void DeleteRunlogByEtime(DateTime Etime)
        {
            base.ExecuteNonQuery("global_RunLogHelper_ClearRunLog_ByEtime", Etime);           
        }
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="Etime"></param>
        /// <param name="Stime"></param>
        /// <returns></returns>
        public void DeleteRunlogByStimeEtime(DateTime Etime, DateTime Stime)
        {
            base.ExecuteNonQuery("global_RunLogHelper_ClearRunLog_ByStimeEtime", Etime, Stime);
        }
    }
}
