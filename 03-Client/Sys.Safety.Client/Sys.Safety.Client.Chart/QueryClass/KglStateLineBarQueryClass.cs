using System;
using System.Data;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Request.Chart;
using Sys.Safety.ServiceContract.Chart;

namespace Sys.Safety.Client.Chart
{
    /// <summary>
    ///     开关量曲线及柱状图查询算法
    /// </summary>
    public class KglStateLineBarQueryClass
    {
        private readonly IChartService _chartService = ServiceFactory.Create<IChartService>();

        /// <summary>
        ///     获取开关量曲线数据
        /// </summary>
        /// <param name="SzNameT"></param>
        /// <param name="SzPoint"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <param name="isPoint"></param>
        /// <returns></returns>
        public DataTable getStateLineDt(DateTime SzNameT, string CurrentPointID, string CurrentDevid, string CurrentWzid,
            bool kglztjsfs)
        {
            var dtR = new DataTable();
            //          
            dtR.Columns.Add("C");
            dtR.Columns.Add("D");
            dtR.Columns.Add("E");
            dtR.Columns.Add("sTimer", typeof(DateTime));
            dtR.Columns.Add("eTimer", typeof(DateTime));
            dtR.Columns.Add("stateName");
            try
            {
                //dtR = ServiceFactory.CreateService<IChartService>()
                //    .getStateLineDt(SzNameT, CurrentPointID, CurrentDevid, CurrentWzid, kglztjsfs);
                var req = new GetStateLineDtRequest
                {
                    SzNameT = SzNameT,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = kglztjsfs
                };
                var res = _chartService.GetStateLineDt(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dtR = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_getStateLineDt" + Ex.Message + Ex.StackTrace);
            }
            return dtR;
        }

        /// <summary>
        /// 获取控制量状态变动曲线
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="CurrentPointID"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <returns></returns>
        public DataTable getKzlStateLineDt(DateTime SzNameT, string CurrentPointID, string CurrentDevid, string CurrentWzid,
            bool kglztjsfs)
        {
            var dtR = new DataTable();
            //          
            dtR.Columns.Add("C");
            dtR.Columns.Add("D");
            dtR.Columns.Add("E");
            dtR.Columns.Add("sTimer", typeof(DateTime));
            dtR.Columns.Add("eTimer", typeof(DateTime));
            dtR.Columns.Add("stateName");
            try
            {
                //dtR = ServiceFactory.CreateService<IChartService>()
                //    .getStateLineDt(SzNameT, CurrentPointID, CurrentDevid, CurrentWzid, kglztjsfs);
                var req = new GetStateLineDtRequest
                {
                    SzNameT = SzNameT,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = kglztjsfs
                };
                var res = _chartService.GetKzlStateLineDt(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dtR = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("getKzlStateLineDt" + Ex.Message + Ex.StackTrace);
            }
            return dtR;
        }

        /// <summary>
        ///     获取模拟量报警曲线
        /// </summary>
        /// <param name="SzNameT"></param>
        /// <param name="CurrentPointID"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <param name="kglztjsfs"></param>
        /// <returns></returns>
        public DataTable getMnlBjDdLineDt(DateTime SzNameS, DateTime SzNameE, string CurrentPointID, string CurrentDevid,
            string CurrentWzid, string type)
        {
            var dtR = new DataTable();
            //          
            dtR.Columns.Add("C");
            dtR.Columns.Add("D");
            dtR.Columns.Add("E");
            dtR.Columns.Add("sTimer", typeof(DateTime));
            dtR.Columns.Add("eTimer", typeof(DateTime));
            dtR.Columns.Add("stateName");
            try
            {
                //dtR = ServiceFactory.CreateService<IChartService>()
                //    .getMnlBjLineDt(SzNameS, SzNameE, CurrentPointID, CurrentDevid, CurrentWzid, type);
                var req = new GetMnlBjLineDtRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    type = type
                };
                var res = _chartService.GetMnlBjLineDt(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dtR = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_getStateLineDt" + Ex.Message + Ex.StackTrace);
            }
            return dtR;
        }

        /// <summary>
        ///     获取控制量馈电异常曲线
        /// </summary>
        /// <param name="SzNameS"></param>
        /// <param name="SzNameE"></param>
        /// <param name="CurrentPointID"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <returns></returns>
        public DataTable getKzlLineDt(DateTime SzNameS, DateTime SzNameE, string CurrentPointID, string CurrentDevid,
            string CurrentWzid)
        {
            var dtR = new DataTable();
            //          
            dtR.Columns.Add("C");
            dtR.Columns.Add("D");
            dtR.Columns.Add("E");
            dtR.Columns.Add("sTimer");
            dtR.Columns.Add("eTimer");
            dtR.Columns.Add("stateName");
            try
            {
                //dtR = ServiceFactory.CreateService<IChartService>()
                //    .getKzlLineDt(SzNameS, SzNameE, CurrentPointID, CurrentDevid, CurrentWzid);
                var req = new GetKzlLineDtRequest
                {
                    SzNameS = SzNameS,
                    SzNameE = SzNameE,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                var res = _chartService.GetKzlLineDt(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dtR = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_getStateLineDt" + Ex.Message + Ex.StackTrace);
            }
            return dtR;
        }

        /// <summary>
        ///     查询断电范围、报警/解除、断电/复电、馈电状态、措施及时刻
        /// </summary>
        /// <param name="SzNameT"></param>
        /// <param name="SzPoint"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <param name="isPoint"></param>
        /// <returns></returns>
        public string[] GetDgView(DateTime SzNameT, string CurrentPointID, string CurrentDevid, string CurrentWzid,
            bool kglztjsfs)
        {
            var QueryStr = new string[11];
            try
            {
                //QueryStr = ServiceFactory.CreateService<IChartService>()
                //    .GetDgView(SzNameT, CurrentPointID, CurrentDevid, CurrentWzid, kglztjsfs);
                var req = new GetDgViewRequest
                {
                    SzNameT = SzNameT,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = kglztjsfs
                };
                var res = _chartService.GetDgView(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                QueryStr = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_GetDgView" + Ex.Message + Ex.StackTrace);
            }
            return QueryStr;
        }

        /// <summary>
        ///     统计每小时开机率
        /// </summary>
        /// <param name="DtStart"></param>
        /// <param name="DtEnd"></param>
        /// <param name="Point"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <param name="isPoint"></param>
        /// <param name="kglztjsfs"></param>
        /// <returns></returns>
        public string[] GetKjThings(DateTime DtStart, DateTime DtEnd, string CurrentPointID, string CurrentDevid,
            string CurrentWzid, bool kglztjsfs)
        {
            var QueryStr = new string[11];
            QueryStr[4] = "0%";
            QueryStr[5] = "0分0秒";
            QueryStr[6] = "0";
            try
            {
                //QueryStr = ServiceFactory.CreateService<IChartService>()
                //    .GetKjThings(DtStart, DtEnd, CurrentPointID, CurrentDevid, CurrentWzid, kglztjsfs);
                var req = new GetKjThingsRequest
                {
                    DtStart = DtStart,
                    DtEnd = DtEnd,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = kglztjsfs
                };
                var res = _chartService.GetKjThings(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                QueryStr = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_GetKjThings" + Ex.Message + Ex.StackTrace);
            }
            return QueryStr;
        }

        /// <summary>
        ///     柱状图查询
        /// </summary>
        /// <param name="SzNameT"></param>
        /// <param name="SzPoint"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <param name="isPoint"></param>
        /// <param name="kglztjsfs"></param>
        /// <returns></returns>
        public DataTable InitQxZhuZhuang(DateTime SzNameT, string CurrentPointID, string CurrentDevid,
            string CurrentWzid, bool kglztjsfs)
        {
            var dtR = new DataTable();
            dtR.Columns.Add("A");
            dtR.Columns.Add("B");
            dtR.Columns.Add("C");
            dtR.Columns.Add("timer");
            try
            {
                //dtR = ServiceFactory.CreateService<IChartService>()
                //    .InitQxZhuZhuang(SzNameT, CurrentPointID, CurrentDevid, CurrentWzid, kglztjsfs);
                var req = new InitQxZhuZhuangRequest
                {
                    SzNameT = SzNameT,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid,
                    kglztjsfs = kglztjsfs
                };
                var res = _chartService.InitQxZhuZhuang(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                dtR = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("KglStateLineBarQueryClass_InitQxZhuZhuang" + Ex.Message + Ex.StackTrace);
            }
            return dtR;
        }
    }
}