using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using Sys.Safety.Model;
using Basic.Framework.Data;

namespace Sys.Safety.DataAccess
{
    public class CalibrationRepository : RepositoryBase<CalibrationModel>, ICalibrationRepository
    {
        /// <summary>
        /// 获取标校详情
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable GetBxDetail(DateTime startTime, DateTime endTime)
        {
            return base.QueryTable("global_RealModule_GetBxDetail_Bytime", startTime, endTime);
        }

        /// <summary>
        ///获取标校统计
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataTable GetBxStatistics(DateTime time)
        {
            DataTable dt = new DataTable();
            dt.TableName = "BxTj";
            dt = base.QueryTable("global_RealModule_GetBx_Bytime", time);
            dt.Columns.Add("syDay", typeof(string));
            dt.Columns.Add("ybxcd", typeof(string));
            dt.Columns.Add("wbxcd", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                //剩余时间
                string sSyDay = "";
                int iSyDay = (Convert.ToDateTime(dr["time"]) - Convert.ToDateTime(DateTime.Now.ToShortDateString())).Days;
                sSyDay = iSyDay < 0 ? "已过期" : iSyDay.ToString();
                dr["syDay"] = sSyDay;

                //已标校测点
                DataTable dt2 = base.QueryTable("global_RealModule_GetYesBxPoint_Bytime", dr["time"]);
                StringBuilder sbYbxcd = new StringBuilder();
                foreach (DataRow dr2 in dt2.Rows)
                {
                    if (sbYbxcd.Length != 0)
                    {
                        sbYbxcd.Append(",");
                    }
                    sbYbxcd.Append(dr2["point"]);
                }
                dr["ybxcd"] = sbYbxcd.Length == 0 ? "无" : sbYbxcd.ToString();

                //未标校测点
                DataTable dt3 = base.QueryTable("global_RealModule_GetNoBxPoint_Bytime", dr["time"], dr["time"]);
                StringBuilder sbWbxcd = new StringBuilder();
                foreach (DataRow dr3 in dt3.Rows)
                {
                    if (sbWbxcd.Length != 0)
                    {
                        sbWbxcd.Append(",");
                    }
                    sbWbxcd.Append(dr3["point"]);
                }
                dr["wbxcd"] = sbWbxcd.Length == 0 ? "无" : sbWbxcd.ToString();
            }

            return dt;
        }
    }
}
