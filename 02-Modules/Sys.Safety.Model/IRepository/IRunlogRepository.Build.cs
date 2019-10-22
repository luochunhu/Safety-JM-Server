using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;

namespace Sys.Safety.Model
{
    public interface IRunlogRepository : IRepository<RunlogModel>
    {
        RunlogModel AddRunlog(RunlogModel runlogModel);
        void UpdateRunlog(RunlogModel runlogModel);
        void DeleteRunlog(string id);
        IList<RunlogModel> GetRunlogList(int pageIndex, int pageSize, out int rowCount);
        List<RunlogModel> GetRunlogList();
        RunlogModel GetRunlogById(string id);
        /// <summary>
        /// 根据结束时间删除
        /// </summary>
        /// <param name="Etime"></param>
        /// <returns></returns>
        void DeleteRunlogByEtime(DateTime Etime);
        /// <summary>
        /// 根据开始时间结束时间删除指定时间段的数据
        /// </summary>
        /// <param name="Etime"></param>
        /// <param name="Stime"></param>
        /// <returns></returns>
        void DeleteRunlogByStimeEtime(DateTime Etime, DateTime Stime);
    }
}
