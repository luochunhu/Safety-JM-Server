using Basic.Framework.Service;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System.Collections;
using System.Data;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class ReportController : BasicApiController, IReportService
    {
        private readonly IReportService reportService;
        public ReportController()
        {
            reportService = ServiceFactory.Create<IReportService>();
        }

        [HttpPost]
        [Route("v1/Report/SaveDoTest")]
        public BasicResponse<string> SaveDoTest()
        {
            return reportService.SaveDoTest();
        }

        [HttpPost]
        [Route("v1/Report/GetListData")]
        public BasicResponse<DataTable> GetListData(Sys.Safety.Request.ReportGetListDataRequest reportRequest)
        {
            return reportService.GetListData(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/Getlistdisplayex")]
        public BasicResponse<DataTable> Getlistdisplayex(Sys.Safety.Request.ReportGetlistdisplayexRequest reportRequest)
        {
            return reportService.Getlistdisplayex(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/WebGetlistdisplayex")]
        public BasicResponse<DataTable> WebGetlistdisplayex(Sys.Safety.Request.ReportWebGetlistdisplayexRequest reportRequest)
        {
            return reportService.WebGetlistdisplayex(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/GetReportData")]
        public BasicResponse<DataTable> GetReportData(Sys.Safety.Request.ReportGetReportDataRequest reportRequest)
        {
            return reportService.GetReportData(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/WebGetReportData")]
        public BasicResponse<DataTable> WebGetReportData(Sys.Safety.Request.ReportWebGetReportDataRequest reportRequest)
        {
            return reportService.WebGetReportData(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/GetFKData")]
        public BasicResponse<string> GetFKData(Sys.Safety.Request.ReportGetFKDataRequest reportRequest)
        {
            return reportService.GetFKData(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/GetListTemple")]
        public BasicResponse<ListtempleInfo> GetListTemple(Sys.Safety.Request.ReportGetListTempleRequest reportRequest)
        {
            return reportService.GetListTemple(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/GetFKDataByHashTable")]
        public BasicResponse<Hashtable> GetFKDataByHashTable(Sys.Safety.Request.ReportGetFKDataByHashTableRequest reportRequest)
        {
            return reportService.GetFKDataByHashTable(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/GetEnvirLevelDetail")]
        public BasicResponse<DataTable> GetEnvirLevelDetail(Sys.Safety.Request.ReportGetEnvirLevelDetailRequest reportRequest)
        {
            return reportService.GetEnvirLevelDetail(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/GetPointParameter")]
        public BasicResponse<DataTable> GetPointParameter(Sys.Safety.Request.ReportGetPointParameterRequest reportRequest)
        {
            return reportService.GetPointParameter(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/GetMorePointData")]
        public BasicResponse<DataSet> GetMorePointData(Sys.Safety.Request.ReportGetMorePointDataRequest reportRequest)
        {
            return reportService.GetMorePointData(reportRequest);
        }

        [HttpPost]
        [Route("v1/Report/GetReportTotalRecord")]
        public BasicResponse<int> GetReportTotalRecord(Sys.Safety.Request.ReportGetReportTotalRecordRequest reportRequest)
        {
            return reportService.GetReportTotalRecord(reportRequest);
        }
    }
}
