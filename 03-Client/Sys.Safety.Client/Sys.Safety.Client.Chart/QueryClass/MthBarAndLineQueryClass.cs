using System;
using System.Data;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.Safety.Request.Chart;
using Sys.Safety.ServiceContract.Chart;

namespace Sys.Safety.Client.Chart
{
    public class MthBarAndLineQueryClass
    {
        private readonly IChartService _chartService = ServiceFactory.Create<IChartService>();

        /// <summary>
        ///     查询并返回柱状图数据
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="pointfzh"></param>
        /// <param name="pointkh"></param>
        /// <param name="CurrentDevid"></param>
        /// <param name="CurrentWzid"></param>
        /// <param name="isPoint"></param>
        /// <returns></returns>
        public DataTable getMonthBar(int year, int month, string CurrentPointID, string CurrentDevid, string CurrentWzid)
        {
            var SaveTable = new DataTable();
            SaveTable.Columns.Add("zdz", Type.GetType("System.String"));
            SaveTable.Columns.Add("pjz", Type.GetType("System.String"));
            SaveTable.Columns.Add("zxz", Type.GetType("System.String"));
            SaveTable.Columns.Add("dday", typeof(DateTime));
            try
            {
                //SaveTable = ServiceFactory.CreateService<IChartService>()
                //    .getMonthBar(year, month, CurrentPointID, CurrentDevid, CurrentWzid);
                var req = new GetMonthBarRequest
                {
                    year = year,
                    month = month,
                    CurrentPointID = CurrentPointID,
                    CurrentDevid = CurrentDevid,
                    CurrentWzid = CurrentWzid
                };
                var res = _chartService.GetMonthBar(req);
                if (!res.IsSuccess)
                {
                    throw new Exception(res.Message);
                }
                SaveTable = res.Data;
            }
            catch (Exception Ex)
            {
                LogHelper.Error("MthBarAndLineQueryClass_getMonthBar" + Ex.Message + Ex.StackTrace);
            }
            return SaveTable;
        }
    }
}