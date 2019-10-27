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
    public class McLineQueryClass
    {
        private readonly IChartService _chartService = ServiceFactory.Create<IChartService>();

        /// <summary>
        /// 模拟量密采曲线数据
        /// </summary>
        /// <param name="SzNameS">开始时间</param>
        /// <param name="SzNameE">结束时间</param>
        /// <param name="flag">是否根据开始时间结束时间过滤数据</param>
        /// <param name="CurrentPointID">测点ID</param>
        /// <param name="CurrentDevid">设备类型ID</param>
        /// <param name="CurrentWzid">安装位置ID</param>
        /// <param name="TimeTick">时间间隔（密采值，1分钟，1小时）</param>
        /// <param name="MaxLC2"></param>
        /// <param name="MinLC2"></param>
        /// <returns></returns>
        public DataTable GetMcData(DateTime SzNameS, DateTime SzNameE, bool flag, string CurrentPointID, string CurrentDevid, string CurrentWzid,
              string TimeTick, ref double MaxLC2, ref double MinLC2,bool isQueryByPoint = false)
        {
            DataTable dtR = new DataTable();
            MaxLC2 = 0;
            MinLC2 = 0;
            //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
            dtR.Columns.Add("A");
            dtR.Columns.Add("B");
            dtR.Columns.Add("Timer");
            dtR.Columns.Add("state");
            dtR.Columns.Add("statetext");
            dtR.Columns.Add("type");
            dtR.Columns.Add("typetext");           
            try
            {
                //Dictionary<string, DataTable> Rvalue = ServiceFactory.CreateService<IChartService>().GetMcData(SzNameS, SzNameE, flag, CurrentPointID, CurrentDevid, CurrentWzid, TimeTick);
                var req = new GetMcDataRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    flag = flag,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    TimeTick = TimeTick,
                    IsQueryByPoint = isQueryByPoint
                };
                var res = _chartService.GetMcData(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                Dictionary<string, DataTable> Rvalue = res.Data;

                if (Rvalue.Count > 0)
                {
                    Dictionary<string, DataTable>.KeyCollection keyv = Rvalue.Keys;
                    MaxLC2 = double.Parse(keyv.ElementAt(0).Split(',')[0]);
                    MinLC2 = double.Parse(keyv.ElementAt(0).Split(',')[1]);                    

                    Dictionary<string, DataTable>.ValueCollection value = Rvalue.Values;
                    dtR = value.ElementAt(0);
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("McLineQueryClass_GetMcData" + Ex.Message + Ex.StackTrace);
            }

            return dtR;
        }
        public DataTable GetMcData(DateTime SzNameS, DateTime SzNameE, bool flag, string CurrentPointID, string CurrentDevid, string CurrentWzid,
              string TimeTick, ref double MaxLC2, ref double MinLC2, ref string maxValueTime, bool isQueryByPoint = false)
        {
            DataTable dtR = new DataTable();
            MaxLC2 = 0;
            MinLC2 = 0;
            //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
            dtR.Columns.Add("A");
            dtR.Columns.Add("B");
            dtR.Columns.Add("Timer");
            dtR.Columns.Add("state");
            dtR.Columns.Add("statetext");
            dtR.Columns.Add("type");
            dtR.Columns.Add("typetext");
          
            try
            {
                //Dictionary<string, DataTable> Rvalue = ServiceFactory.CreateService<IChartService>().GetMcData(SzNameS, SzNameE, flag, CurrentPointID, CurrentDevid, CurrentWzid, TimeTick);
                var req = new GetMcDataRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    flag = flag,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    TimeTick = TimeTick,
                    IsQueryByPoint = isQueryByPoint
                };
                var res = _chartService.GetMcData(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                Dictionary<string, DataTable> Rvalue = res.Data;

                if (Rvalue.Count > 0)
                {
                    Dictionary<string, DataTable>.KeyCollection keyv = Rvalue.Keys;
                    MaxLC2 = double.Parse(keyv.ElementAt(0).Split(',')[0]);
                    MinLC2 = double.Parse(keyv.ElementAt(0).Split(',')[1]);
                    maxValueTime = keyv.ElementAt(0).Split(',')[2];

                    Dictionary<string, DataTable>.ValueCollection value = Rvalue.Values;
                    dtR = value.ElementAt(0);
                }
            }
            catch (Exception Ex)
            {
                LogHelper.Error("McLineQueryClass_GetMcData" + Ex.Message + Ex.StackTrace);
            }

            return dtR;
        }
    }
}
