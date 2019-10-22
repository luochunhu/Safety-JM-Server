using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Position;
using Basic.Framework.Common;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Config;
using Sys.Safety.Request.ManualCrossControl;

namespace Sys.Safety.WebApiAgent
{
    public class ManualCrossControlControllerProxy : BaseProxy, IManualCrossControlService
    {

        public BasicResponse AddManualCrossControl(Sys.Safety.Request.ManualCrossControl.ManualCrossControlAddRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/Add?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse AddManualCrossControls(Sys.Safety.Request.ManualCrossControl.ManualCrossControlsRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/AddManualCrossControls?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse UpdateManualCrossControl(Sys.Safety.Request.ManualCrossControl.ManualCrossControlUpdateRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/Update?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse UpdateManualCrossControls(Sys.Safety.Request.ManualCrossControl.ManualCrossControlsRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/UpdateManualCrossControls?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse DeleteManualCrossControl(Sys.Safety.Request.ManualCrossControl.ManualCrossControlDeleteRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/Delete?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse DeleteManualCrossControls(Sys.Safety.Request.ManualCrossControl.ManualCrossControlsRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/DeleteManualCrossControls?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse BatchOperationManualCrossControls(Sys.Safety.Request.ManualCrossControl.ManualCrossControlsRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/BatchOperationManualCrossControls?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlList(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetListRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetPageList?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlList()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetList?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }

        public BasicResponse<Jc_JcsdkzInfo> GetManualCrossControlById(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/Get?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse<Jc_JcsdkzInfo>>(responseStr);
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControl()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetAllManualCrossControl?token=" + Token, string.Empty);
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }

       
        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByStationID(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByStationIDRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetManualCrossControlByStationID?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlHandCtrlByStationID(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByStationIDRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetManualCrossControlHandCtrlByStationID?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeBkPoint(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByTypeBkPointRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetManualCrossControlByTypeBkPoint?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeZkPointBkPoint(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByTypeZkPointBkPointRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetManualCrossControlByTypeZkPointBkPoint?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByBkPoint(Sys.Safety.Request.ManualCrossControl.ManualCrossControlGetByBkPointRequest ManualCrossControlRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetManualCrossControlByBkPoint?token=" + Token, JSONHelper.ToJSONString(ManualCrossControlRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeZkPoint(ManualCrossControlGetByTypeZkPointRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetManualCrossControlByTypeZkPoint?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }

        public BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControlDetail()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/GetAllManualCrossControlDetail?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }


        public BasicResponse DeleteOtherManualCrossControlFromDB()
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/ManualCrossControl/DeleteOtherManualCrossControlFromDB?token=" + Token, "");
            return JSONHelper.ParseJSONString<BasicResponse<List<Jc_JcsdkzInfo>>>(responseStr);
        }
    }
}
