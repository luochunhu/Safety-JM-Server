using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data;

namespace Sys.Safety.Model
{
    public interface IJc_HourRepository : IRepository<Jc_HourModel>
    {
        Jc_HourModel AddJc_Hour(Jc_HourModel jc_HourModel);
        void UpdateJc_Hour(Jc_HourModel jc_HourModel);
        void DeleteJc_Hour(string id);
        IList<Jc_HourModel> GetJc_HourList(int pageIndex, int pageSize, out int rowCount);
        Jc_HourModel GetJc_HourById(string id);

        /// <summary>
        /// 2017.7.27 by 倍数报警取分析基数用
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <param name="pointItems"></param>
        /// <returns></returns>
        DataTable GetJC_HourDataByTimer(DateTime stime, DateTime etime, List<string> pointItems);
    }
}
