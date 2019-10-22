using Basic.Framework.Common;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi.Proxy;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.App;
using Sys.Safety.Request.App;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.Request.Jc_B;
using Sys.Safety.ServiceContract.App;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.WebApiAgent.App
{
    public class KJ73NAppControllerProxy : BaseProxy, IKJ73NAppService
    {
        public BasicResponse<List<RealDataAppDataContract>> GetRealData(RealDataRequest realDataRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetRealData?token=" + Token, JSONHelper.ToJSONString(realDataRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<RealDataAppDataContract>>>(responsestr);
        }

        public BasicResponse GetAllAnalogAlarm(RealDataRequest realDataRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetAllAnalogAlarm?token=" + Token, JSONHelper.ToJSONString(realDataRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public BasicResponse<RealDataAppDataContract> GetPointDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetPointDetail?token=" + Token, JSONHelper.ToJSONString(realDataGetDetialRequest));
            return JSONHelper.ParseJSONString<BasicResponse<RealDataAppDataContract>>(responsestr);
        }

        public BasicResponse<RealDataAppDataContract> GetStationDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetStationDetail?token=" + Token, JSONHelper.ToJSONString(realDataGetDetialRequest));
            return JSONHelper.ParseJSONString<BasicResponse<RealDataAppDataContract>>(responsestr);
        }

        public BasicResponse<SwitcheAppDataContract> GetSwitcheObj(SwitcheGetRequest switheRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetSwitcheObj?token=" + Token, JSONHelper.ToJSONString(switheRequest));
            return JSONHelper.ParseJSONString<BasicResponse<SwitcheAppDataContract>>(responsestr);
        }

        public BasicResponse<NetworkModuleAppDataContract> GetNetworkModuleObj(NetworkModuleAppGetRequest networkModuleRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetNetworkModuleObj?token=" + Token, JSONHelper.ToJSONString(networkModuleRequest));
            return JSONHelper.ParseJSONString<BasicResponse<NetworkModuleAppDataContract>>(responsestr);
        }

        public BasicResponse<UserInfo> Logon(LogonRequest logonRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/Logon?token=" + Token, JSONHelper.ToJSONString(logonRequest));
            return JSONHelper.ParseJSONString<BasicResponse<UserInfo>>(responsestr);
        }

        public BasicResponse ModfiyPassword(ModifyUserPasswordRequest modifyPasswordRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/ModfiyPassword?token=" + Token, JSONHelper.ToJSONString(modifyPasswordRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public BasicResponse<BaseDataAppDataContract> GetBaseData(BasicRequest basedataRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetBaseData?token=" + Token, JSONHelper.ToJSONString(basedataRequest));
            return JSONHelper.ParseJSONString<BasicResponse<BaseDataAppDataContract>>(responsestr);
        }

        public BasicResponse<List<AnalogChartAppDataContract>> GetMLLFiveLine(ChartGetRequest analoglineRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetMLLFiveLine?token=" + Token, JSONHelper.ToJSONString(analoglineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AnalogChartAppDataContract>>>(responsestr);
        }

        public BasicResponse<List<DerailChartAppDataContract>> GetKGLFiveLine(ChartGetRequest deraillineRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetKGLFiveLine?token=" + Token, JSONHelper.ToJSONString(deraillineRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<DerailChartAppDataContract>>>(responsestr);
        }

        public BasicResponse<GetJCRDataResponse> GetJCRData(RunLogGetRequest runlogRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetJCRData?token=" + Token, JSONHelper.ToJSONString(runlogRequest));
            return JSONHelper.ParseJSONString<BasicResponse<GetJCRDataResponse>>(responsestr);
        }

        public BasicResponse<GetJCMCDataResponse> GetJCMCData(RunLogGetRequest runlogRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetJCMCData?token=" + Token, JSONHelper.ToJSONString(runlogRequest));
            return JSONHelper.ParseJSONString<BasicResponse<GetJCMCDataResponse>>(responsestr);
        }

        public BasicResponse<GetMLLBJDataResponse> GetMLLBJData(RunLogGetRequest runlogRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetMLLBJData?token=" + Token, JSONHelper.ToJSONString(runlogRequest));
            return JSONHelper.ParseJSONString<BasicResponse<GetMLLBJDataResponse>>(responsestr);
        }

        public BasicResponse<GetKGLBJDataResponse> GetKGLBJData(RunLogGetRequest runlogRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetKGLBJData?token=" + Token, JSONHelper.ToJSONString(runlogRequest));
            return JSONHelper.ParseJSONString<BasicResponse<GetKGLBJDataResponse>>(responsestr);
        }

        public BasicResponse<GetMLLDDDataResponse> GetMLLDDData(RunLogGetRequest runlogRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetMLLDDData?token=" + Token, JSONHelper.ToJSONString(runlogRequest));
            return JSONHelper.ParseJSONString<BasicResponse<GetMLLDDDataResponse>>(responsestr);
        }

        public BasicResponse<GetKGLDDDataResponse> GetKGLDDData(RunLogGetRequest runlogRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetKGLDDData?token=" + Token, JSONHelper.ToJSONString(runlogRequest));
            return JSONHelper.ParseJSONString<BasicResponse<GetKGLDDDataResponse>>(responsestr);
        }

        public BasicResponse<List<AlarmProcessInfo>> GetAlarmRecordListByStime(AlarmRecordGetByStimeRequest alarmRecordRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetAlarmRecordListByStime?token=" + Token, JSONHelper.ToJSONString(alarmRecordRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<AlarmProcessInfo>>>(responsestr);
        }

        public BasicResponse EndAlarmRecord(AlarmRecordEndRequest alarmRecordEndRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/EndAlarmRecord?token=" + Token, JSONHelper.ToJSONString(alarmRecordEndRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }

        public BasicResponse<List<JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(AlarmHandelGetByStimeAndETime alarmHandelRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/GetAlarmHandleByStimeAndEtime?token=" + Token, JSONHelper.ToJSONString(alarmHandelRequest));
            return JSONHelper.ParseJSONString<BasicResponse<List<JC_AlarmHandleInfo>>>(responsestr);
        }

        public BasicResponse EndAlarmHandle(AlarmHandleEndRequest alarmHandleEndRequest)
        {
            var responsestr = HttpClientHelper.Post(Webapi + "/v1/KJ73NApp/EndAlarmHandle?token=" + Token, JSONHelper.ToJSONString(alarmHandleEndRequest));
            return JSONHelper.ParseJSONString<BasicResponse>(responsestr);
        }
    }
}
