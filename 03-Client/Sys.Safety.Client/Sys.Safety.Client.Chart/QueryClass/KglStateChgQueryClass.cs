using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Request.Chart;
using Sys.Safety.ServiceContract.Chart;

namespace Sys.Safety.Client.Chart
{
    public class KglStateChgQueryClass
    {
        private readonly IChartService _chartService = ServiceFactory.Create<IChartService>();

        /// <summary>
        /// 获取曲线、状态统计列表、柱状图的绑定数据
        /// </summary>
        /// <param name="SzName"></param>
        /// <param name="EN_Point"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <param name="kglztjsfs"></param>
        /// <param name="isPoint"></param>
        /// <returns></returns>
        public void getStateBarTable(DateTime SzNameT, string CurrentPointID, string CurrentDevid, string CurrentWzid, bool kglztjsfs,
            ref string TjTxt, ref List<DataTable> dt_R)
        {
            TjTxt = "";

            //状态变化柱状图数据
            DataTable dtBarStateChg = new DataTable();
            dtBarStateChg.Columns.Add("state");
            dtBarStateChg.Columns.Add("stateName");
            dtBarStateChg.Columns.Add("stime");
            dtBarStateChg.Columns.Add("etime");
            //状态统计列表数据
            DataTable dtTotal = new DataTable();
            dtTotal.Columns.Add("Columns1");
            dtTotal.Columns.Add("Columns2");
            dtTotal.Columns.Add("Columns3");
            dtTotal.Columns.Add("Columns4");
            dtTotal.Columns.Add("Columns5");
            dtTotal.Columns.Add("Columns6");
            dtTotal.Columns.Add("Columns7");
            dtTotal.Columns.Add("Columns8");
            dtTotal.Columns.Add("Columns9");
            //小时开机率统计柱状图数据
            DataTable dtBarHourTj = new DataTable();
            dtBarHourTj.Columns.Add("percentage1Name");
            dtBarHourTj.Columns.Add("percentage1");
            dtBarHourTj.Columns.Add("percentage2Name");
            dtBarHourTj.Columns.Add("percentage2");
            dtBarHourTj.Columns.Add("timer");

            try
            {
                //GetStateBarRequest Rvalue = ServiceFactory.CreateService<IChartService>().getStateBarTable(SzNameT, CurrentPointID, CurrentDevid, CurrentWzid, kglztjsfs);
                var req = new GetStateBarTableRequest
                {
                    SzNameT = SzNameT,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = kglztjsfs
                };
                var res = _chartService.GetStateBarTable(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                GetStateBarTableResponse Rvalue = res.Data;

                if (Rvalue!=null)
                {                  
                    TjTxt = Rvalue.TjTxt;

                    dt_R.Add(Rvalue.dtBarStateChg);
                    dt_R.Add(Rvalue.dtTotal);
                    dt_R.Add(Rvalue.dtBarHourTj);
                }
                else
                {//赋值空的dt
                    dt_R.Add(dtBarStateChg);
                    dt_R.Add(dtTotal);
                    dt_R.Add(dtBarHourTj);
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateChgQueryClass_getStateBarTable" + Ex.Message + Ex.StackTrace);
            }
        }
        /// <summary>
        /// 获取开关量状态变动明细列表
        /// </summary>
        /// <param name="SzNameT">时间</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="kglztjsfs">是否计算未知状态</param>   
        /// <returns></returns>
        public DataTable getStateChgdt(DateTime SzNameT, string CurrentPointID, string CurrentDevid, string CurrentWzid, bool kglztjsfs)
        {
            DataTable Dt = new DataTable();
            try
            {
                //Dt = ServiceFactory.CreateService<IChartService>().getStateChgdt(SzNameT, CurrentPointID, CurrentDevid, CurrentWzid, kglztjsfs);
                var req = new GetStateChgdtRequest
                {
                    SzNameT = SzNameT,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = kglztjsfs
                };
                var res = _chartService.GetStateChgdt(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                Dt = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateChgQueryClass_getStateChgdt" + Ex.Message + Ex.StackTrace);
            }
            return Dt;
        }
    }
}
