using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sys.Safety.ServiceContract;
using Sys.Safety.Services;
using Basic.Framework.Common;
using Basic.Framework.Web;
using Sys.Safety.Request.Calibration;
using System.Data;
using Sys.Safety.Model;


namespace Sys.Safety.Services
{
    public partial class CalibrationService : ICalibrationService
    {
        private ICalibrationRepository _Repository;
        public CalibrationService(ICalibrationRepository _Repository)
        {
            this._Repository = _Repository;
        }

        /// <summary>
        /// 获取标校详情
        /// </summary>
        /// <param name="calibrationRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetBxDetail(GetCalibrationDetailRequest calibrationRequest)
        {
            var calibrationResponse = new BasicResponse<DataTable>();
            var sStart = Convert.ToDateTime(calibrationRequest.Time.ToString("yyyy-MM-dd 00:00:00"));
            var sStop = Convert.ToDateTime(calibrationRequest.Time.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
            var dataTable = _Repository.GetBxDetail(sStart, sStop);
            dataTable.Columns.Add("cxText", typeof(string));

            //时间转换为时分秒
            foreach (DataRow item in dataTable.Rows)
            {
                var ts = new TimeSpan(0,0,Convert.ToInt32(item["cx"]));
                item["cxText"] = ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds;
            }

            calibrationResponse.Data = dataTable;
            calibrationResponse.Code = 100;
            calibrationResponse.Message = "获取成功！";

            return calibrationResponse;
        }

        /// <summary>
        ///获取标校统计
        /// </summary>
        /// <param name="calibrationRequest"></param>
        /// <returns></returns>
        public BasicResponse<DataTable> GetBxStatistics(GetCalibrationStatisticsRequest calibrationRequest)
        {
            var calibrationResponse = new BasicResponse<DataTable>();
            var dataTable = _Repository.GetBxStatistics(calibrationRequest.Time);
            calibrationResponse.Data = dataTable;
            calibrationResponse.Code = 100;
            calibrationResponse.Message = "获取成功！";

            return calibrationResponse;
        }
    }
}
