using System;
using System.Data;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Request.Chart;
using Sys.Safety.Request.Listex;
using Sys.Safety.ServiceContract.Chart;

namespace Sys.Safety.Client.Chart
{
    /// <summary>
    ///     模拟量5分钟曲线
    /// </summary>
    public class FiveMiniteLineQueryClass
    {
        private readonly IChartService _chartService = ServiceFactory.Create<IChartService>();

        /// <summary>
        ///     查询模拟量月曲线
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="CurrentPointID"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <returns></returns>
        public DataTable getMonthLine(DateTime SzNameS, DateTime SzNameE, string CurrentPointID, string CurrentDevid,
            string CurrentWzid)
        {
            var dtR = new DataTable();
            dtR.Columns.Add("HourMax");
            dtR.Columns.Add("HourAvg");
            dtR.Columns.Add("HourMin");
            dtR.Columns.Add("Timer");
            try
            {
                //dtR = ServiceFactory.CreateService<IChartService>().getMonthLine(SzNameS, SzNameE, CurrentPointID, CurrentDevid, CurrentWzid);
                var req = new GetMonthLineRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                var res = _chartService.GetMonthLine(req);
                if (!res.IsSuccess)
                    throw new Exception(res.Message);
                dtR = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_getMonthLine" + Ex.Message + Ex.StackTrace);
            }
            return dtR;
        }

        /// <summary>
        ///     查询模拟量5分钟曲线
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="pointfzh"></param>
        /// <param name="pointkh"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <param name="isPoint"></param>
        /// <returns></returns>
        public DataTable getFiveMiniteLine(DateTime SzNameS, DateTime SzNameE, string CurrentPointID,
            string CurrentDevid, string CurrentWzid, bool isQueryByPoint = false)
        {
            var dtR = new DataTable();
            //Av监测值,Bv最大值,Cv最小,Dv平均,Ev移动值
            dtR.Columns.Add("Av");
            dtR.Columns.Add("Bv");
            dtR.Columns.Add("Cv");
            dtR.Columns.Add("Dv");
            dtR.Columns.Add("Ev");
            dtR.Columns.Add("type");
            dtR.Columns.Add("typetext");
            dtR.Columns.Add("Timer");
            try
            {
                //dtR = ServiceFactory.CreateService<IChartService>().getFiveMiniteLine(SzNameS, SzNameE, CurrentPointID, CurrentDevid, CurrentWzid);
                var req = new GetFiveMiniteLineRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    IsQueryByPoint = isQueryByPoint
                };
                var res = _chartService.GetFiveMiniteLine(req);
                if (!res.IsSuccess)
                    throw new Exception(res.Message);
                dtR = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_getFiveMiniteLine" + Ex.Message + Ex.StackTrace);
            }
            return dtR;
        }

        /// <summary>
        ///     获取测点的单位信息
        /// </summary>
        /// <param name="CurrentDevid"></param>
        /// <returns></returns>
        public string getPointDw(string CurrentDevid)
        {
            var dw = "";
            try
            {
                //dw = ServiceFactory.CreateService<IChartService>().getPointDw(CurrentDevid);
                var req = new IdRequest
                {
                    Id = Convert.ToInt32(CurrentDevid)
                };
                var res = _chartService.GetPointDw(req);
                if (!res.IsSuccess)
                    throw new Exception(res.Message);
                dw = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_getPointDw" + Ex.Message + Ex.StackTrace);
            }

            return dw;
        }

        /// <summary>
        ///     获取测点的基本信息
        /// </summary>
        /// <param name="showpoint"></param>
        /// <param name="showdate"></param>
        public string[] ShowPointInf(string CurrentWzid, string CurrentPointId)
        {
            var QueryStr = new string[13];
            for (var i = 0; i <= 12; i++)
                QueryStr[i] = "";
            try
            {
                //QueryStr = ServiceFactory.CreateService<IChartService>().ShowPointInf(CurrentWzid, CurrentDevid);
                var req = new ShowPointInfRequest
                {
                    CurrentWzid = CurrentWzid,
                    CurrentPointId = CurrentPointId
                };
                var res = _chartService.ShowPointInf(req);
                if (!res.IsSuccess)
                    throw new Exception(res.Message);
                QueryStr = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_ShowPointInf" + Ex.Message + Ex.StackTrace);
            }
            return QueryStr;
        }

        /// <summary>
        ///     获取某一时刻的最大值、最小值、平均值
        /// </summary>
        /// <param name="QxDate"></param>
        /// <param name="DtStart"></param>
        /// <param name="CurrentPointID"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <returns></returns>
        public string[] GetDataVale(DateTime QxDate, DateTime DtStart, string CurrentPointID, string CurrentDevid,
            string CurrentWzid)
        {
            var QueryStr = new string[14];
            for (var i = 0; i <= 13; i++)
                QueryStr[i] = "";
            try
            {
                //QueryStr = ServiceFactory.CreateService<IChartService>().GetDataVale(QxDate, DtStart, CurrentPointID, CurrentDevid, CurrentWzid);
                var req = new GetDataValeRequest
                {
                    QxDate = QxDate,
                    DtStart = DtStart,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                var res = _chartService.GetDataVale(req);
                if (!res.IsSuccess)
                    throw new Exception(res.Message);
                QueryStr = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_GetDataVale" + Ex.Message + Ex.StackTrace);
            }
            return QueryStr;
        }

        /// <summary>
        ///     查询断电范围、报警/解除、断电/复电、馈电状态、措施及时刻
        /// </summary>
        /// <param name="QxDate"></param>
        /// <param name="DtStart"></param>
        /// <param name="DtEnd"></param>
        /// <param name="CurrentPointID"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <returns></returns>
        public string[] GetValue(DateTime QxDate, DateTime DtStart, DateTime DtEnd, string CurrentPointID,
            string CurrentDevid, string CurrentWzid)
        {
            var QueryStr = new string[14];
            for (var i = 0; i <= 13; i++)
                QueryStr[i] = "";
            try
            {
                //QueryStr = ServiceFactory.CreateService<IChartService>().GetValue(QxDate, DtStart, DtEnd, CurrentPointID, CurrentDevid, CurrentWzid);
                var req = new GetValueRequest
                {
                    QxDate = QxDate,
                    DtStart = DtStart,
                    DtEnd = DtEnd,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                var res = _chartService.GetValue(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                QueryStr = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("FiveMiniteLineQueryClass_GetValue" + Ex.Message + Ex.StackTrace);
            }
            return QueryStr;
        }
    }
}