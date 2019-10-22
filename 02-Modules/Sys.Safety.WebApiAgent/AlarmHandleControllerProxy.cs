using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmHandle;
using Basic.Framework.Web.WebApi.Proxy;
using Basic.Framework.Common;

namespace Sys.Safety.WebApiAgent
{
    public class AlarmHandleControllerProxy : BaseProxy, IAlarmHandleService
    {
        public BasicResponse<JC_AlarmHandleInfo> AddJC_AlarmHandle(AlarmHandleAddRequest jC_AlarmHandlerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/AddJC_AlarmHandle?token=" + Token, JSONHelper.ToJSONString(jC_AlarmHandlerequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmHandleInfo>>(responseStr);
        }

        public BasicResponse DeleteJC_AlarmHandle(AlarmHandleDeleteRequest jC_AlarmHandlerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/DeleteJC_AlarmHandle?token=" + Token, JSONHelper.ToJSONString(jC_AlarmHandlerequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse<JC_AlarmHandleInfo> GetJC_AlarmHandleById(AlarmHandleGetRequest jC_AlarmHandlerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/GetJC_AlarmHandleById?token=" + Token, JSONHelper.ToJSONString(jC_AlarmHandlerequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmHandleInfo>>(responseStr);
        }

        public BasicResponse<List<JC_AlarmHandleInfo>> GetJC_AlarmHandleList(AlarmHandleGetListRequest jC_AlarmHandlerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/GetJC_AlarmHandleList?token=" + Token, JSONHelper.ToJSONString(jC_AlarmHandlerequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmHandleInfo>>>(responseStr);
        }

        public BasicResponse<JC_AlarmHandleInfo> GetUnclosedAlarmByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest alarmHandleGetByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/GetUnclosedAlarmByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(alarmHandleGetByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmHandleInfo>>(responseStr);
        }

        public BasicResponse<List<JC_AlarmHandleInfo>> GetUnclosedAlarmList(AlarmHandleWithoutSearchConditionRequest alarmHandleWithoutSearchConditionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/GetUnclosedAlarmList?token=" + Token, JSONHelper.ToJSONString(alarmHandleWithoutSearchConditionRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmHandleInfo>>>(responseStr);
        }

        public BasicResponse<JC_AlarmHandleInfo> GetUnhandledAlarmByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest alarmHandleGetByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/v1/AlarmHandle/GetUnhandledAlarmByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(alarmHandleGetByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmHandleInfo>>(responseStr);
        }

        public BasicResponse<List<JC_AlarmHandleInfo>> GetUnhandledAlarmList(AlarmHandleWithoutSearchConditionRequest alarmHandleWithoutSearchConditionRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/GetUnhandledAlarmList?token=" + Token, JSONHelper.ToJSONString(alarmHandleWithoutSearchConditionRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmHandleInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_AlarmHandleInfo>> UpdateAlarmHandleList(AlarmHandleUpdateListRequest alarmHandleUpdateListRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/UpdateAlarmHandleList?token=" + Token, JSONHelper.ToJSONString(alarmHandleUpdateListRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmHandleInfo>>>(responseStr);
        }

        public BasicResponse<JC_AlarmHandleInfo> UpdateJC_AlarmHandle(AlarmHandleUpdateRequest jC_AlarmHandlerequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/UpdateJC_AlarmHandle?token=" + Token, JSONHelper.ToJSONString(jC_AlarmHandlerequest));
            return JSONHelper.ParseJSONString<BasicResponse<JC_AlarmHandleInfo>>(responseStr);
        }

        public BasicResponse<List<JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(AlarmHandelGetByStimeAndETime alarmHandelRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/GetAlarmHandleByStimeAndEtime?token=" + Token, JSONHelper.ToJSONString(alarmHandelRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmHandleInfo>>>(responseStr);
        }

        public BasicResponse<List<JC_AlarmHandleNoEndInfo>> GetAlarmHandleNoEndListByCondition(AlarmHandleNoEndListByCondition alarmHandelRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/GetAlarmHandleNoEndListByCondition?token=" + Token, JSONHelper.ToJSONString(alarmHandelRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmHandleNoEndInfo>>>(responseStr);
        }

        public BasicResponse CloseUnclosedAlarmHandle(BasicRequest request)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/CloseUnclosedAlarmHandle?token=" + Token, JSONHelper.ToJSONString(request));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }

        public BasicResponse CloseUnclosedAlarmHandleByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest getByAnalysisModelIdRequest)
        {
            var responseStr = HttpClientHelper.Post(Webapi + "/v1/AlarmHandle/CloseUnclosedAlarmHandleByAnalysisModelId?token=" + Token, JSONHelper.ToJSONString(getByAnalysisModelIdRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responseStr);
        }
    }
}
