using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.ServiceContract;
using System;
using System.Collections;
using System.Data;

namespace Sys.Safety.WebApiAgent
{
    public class ReportControllerProxy : BaseProxy, IReportService
    {
        public BasicResponse<string> SaveDoTest()
        {
            throw new NotImplementedException();
        }

        public BasicResponse<DataTable> GetListData(Sys.Safety.Request.ReportGetListDataRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/GetListData?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public BasicResponse<DataTable> Getlistdisplayex(Sys.Safety.Request.ReportGetlistdisplayexRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/Getlistdisplayex?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public BasicResponse<DataTable> WebGetlistdisplayex(Sys.Safety.Request.ReportWebGetlistdisplayexRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/WebGetlistdisplayex?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public BasicResponse<DataTable> GetReportData(Sys.Safety.Request.ReportGetReportDataRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/GetReportData?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public BasicResponse<DataTable> WebGetReportData(Sys.Safety.Request.ReportWebGetReportDataRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/WebGetReportData?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public BasicResponse<string> GetFKData(Sys.Safety.Request.ReportGetFKDataRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/GetFKData?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<string>>(responseStr);
        }

        public BasicResponse<ListtempleInfo> GetListTemple(Sys.Safety.Request.ReportGetListTempleRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/GetListTemple?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<ListtempleInfo>>(responseStr);
        }

        public BasicResponse<Hashtable> GetFKDataByHashTable(Sys.Safety.Request.ReportGetFKDataByHashTableRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/GetFKDataByHashTable?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Hashtable>>(responseStr);
        }

        public BasicResponse<DataTable> GetEnvirLevelDetail(Sys.Safety.Request.ReportGetEnvirLevelDetailRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/GetEnvirLevelDetail?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public BasicResponse<DataTable> GetPointParameter(Sys.Safety.Request.ReportGetPointParameterRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/GetPointParameter?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataTable>>(responseStr);
        }

        public BasicResponse<DataSet> GetMorePointData(Sys.Safety.Request.ReportGetMorePointDataRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/GetMorePointData?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<DataSet>>(responseStr);
        }

        public BasicResponse<int> GetReportTotalRecord(Sys.Safety.Request.ReportGetReportTotalRecordRequest reportRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/Report/GetReportTotalRecord?token=" + Token, JSONHelper.ToJSONString(reportRequest));
            return JSONHelper.ParseJSONString<BasicResponse<int>>(responseStr);
        }
    }
}
