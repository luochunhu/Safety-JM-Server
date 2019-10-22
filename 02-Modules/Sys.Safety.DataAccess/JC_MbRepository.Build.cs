using Basic.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Safety.Model;
using System.Data.Entity;

namespace Sys.Safety.DataAccess
{
    public partial class JC_MbRepository : RepositoryBase<JC_MbModel>, IJC_MbRepository
    {

        public JC_MbModel AddMb(JC_MbModel mbModel)
        {
            return base.Insert(mbModel);
        }
        public void UpdateMb(JC_MbModel mbModel)
        {
            base.Update(mbModel);
        }
        public void DeleteMb(string id)
        {
            base.Delete(id);
        }
        public IList<JC_MbModel> GetMbList(int pageIndex, int pageSize, out int rowCount)
        {
            var mbModelLists = base.Datas;
            rowCount = mbModelLists.Count();
            return mbModelLists.OrderBy(p => p.Id).Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }
        public JC_MbModel GetMbById(string id)
        {
            JC_MbModel mbModel = base.Datas.FirstOrDefault(c => c.Id == id);
            return mbModel;
        }


        public void AddItemBySql(JC_MbModel jc_mb)
        {
            //Insert into {0}(Id,PointID,fzh,kh,dzh,devid,wzid,point,type,bstj,bsz,stime,etime,ssz,pjz) values({2}) 
            string tableName = "KJ_MultipleHistory" + jc_mb.Stime.ToString("yyyyMM");
            string values = "'" + jc_mb.Id + "','"
                + jc_mb.PointID + "','"
                + jc_mb.Fzh + "','"
                + jc_mb.Kh + "','" 
                + jc_mb.Dzh + "','" 
                + jc_mb.Devid + "','"
                + jc_mb.Wzid + "','"
                + jc_mb.Point + "','"
                + jc_mb.Type + "','" 
                + jc_mb.Bstj + "','" 
                + jc_mb.Bsz + "','"
                + jc_mb.Stime + "','" 
                + "1900-1-1 00:00:00" + "','" 
                + jc_mb.Ssz + "','"
                + jc_mb.Pjz + "'";
            base.ExecuteNonQuery("global_RatioAlarm_AddItem", tableName, values);
        }

        public void UpdateItemBySql(JC_MbModel jc_mb)
        {
            //Update {0} set zdz = '{1}',zdzs = '{2}' ,etime = '{3}' where point = '{4}' and stime = '{5}' and type = '{6}'
            string tableName = "KJ_MultipleHistory" + jc_mb.Stime.ToString("yyyyMM");
            base.ExecuteNonQuery("global_RatioAlarm_UpdateItem", tableName, jc_mb.Zdz, jc_mb.Zdzs, jc_mb.Etime, jc_mb.Point, jc_mb.Stime, jc_mb.Type);
        }

        public void ClearDbNoEndAlarmBySql()
        {
            string tableName = "KJ_MultipleHistory" + DateTime.Now.ToString("yyyyMM");
            base.ExecuteNonQuery("global_RatioAlarm_ClearItem", tableName, DateTime.Now);
        }
    }
}
