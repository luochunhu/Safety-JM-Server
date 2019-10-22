using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;
using System.Data;

namespace Sys.Safety.DataAccess
{
    public partial class Jc_HourRepository : RepositoryBase<Jc_HourModel>, IJc_HourRepository
    {

        public Jc_HourModel AddJc_Hour(Jc_HourModel jc_HourModel)
        {
            return base.Insert(jc_HourModel);
        }
        public void UpdateJc_Hour(Jc_HourModel jc_HourModel)
        {
            base.Update(jc_HourModel);
        }
        public void DeleteJc_Hour(string id)
        {
            base.Delete(id);
        }
        public IList<Jc_HourModel> GetJc_HourList(int pageIndex, int pageSize, out int rowCount)
        {
            var jc_HourModelLists = base.Datas.ToList();
            rowCount = jc_HourModelLists.Count();
            return jc_HourModelLists.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public Jc_HourModel GetJc_HourById(string id)
        {
            Jc_HourModel jc_HourModel = base.Datas.FirstOrDefault(c => c.ID == id);
            return jc_HourModel;
        }

        public DataTable GetJC_HourDataByTimer(DateTime stime, DateTime etime, List<string> pointItems)
        {
            string tableName = "KJ_Hour" + stime.ToString("yyyyMM");
            string pointStr = "'";
            for (int i = 0; i < pointItems.Count; i++)
            {
                if (i == pointItems.Count - 1)
                {
                    pointStr += pointItems[i] + "'";
                }
                else
                {
                    pointStr += pointItems[i] + "','";
                }
            }

            return base.QueryTable("global_GetHour_Data", tableName, stime.ToString("yyyy-MM-dd HH:mm:ss"), etime.ToString("yyyy-MM-dd HH:mm:ss"), pointStr);
        }
    }
}
