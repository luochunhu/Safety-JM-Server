using Basic.Framework.Service;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.App;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.Request.App;
using Sys.Safety.Request.Jc_B;
using Sys.Safety.ServiceContract.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mas.KJ73N.AppApi.Controllers
{
    public class AppController : ApiController
    {
        IKJ73NAppService kj73nappService;
        public AppController()
        {
            kj73nappService = ServiceFactory.Create<IKJ73NAppService>();
        }

        [HttpGet]
        [Route("v1/App/test")]
        public string Test() 
        {
            return "success";
        }

        [HttpPost]
        [Route("v1/App/GetRealData")]
        public BasicResponse<List<RealDataAppDataContract>> GetRealData(RealDataRequest realDataRequest)
        {
            return kj73nappService.GetRealData(realDataRequest);
        }

        [HttpPost]
        [Route("v1/App/GetAllAnalogAlarm")]
        public BasicResponse GetAllAnalogAlarm(RealDataRequest realDataRequest)
        {
            return kj73nappService.GetAllAnalogAlarm(realDataRequest);
        }
        
        [HttpPost]
        [Route("v1/App/GetPointDetail")]
        public BasicResponse<RealDataAppDataContract> GetPointDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            return kj73nappService.GetPointDetail(realDataGetDetialRequest);
        }

        [HttpPost]
        [Route("v1/App/GetStationDetail")]
        public BasicResponse<RealDataAppDataContract> GetStationDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            return kj73nappService.GetStationDetail(realDataGetDetialRequest);
        }

        [HttpPost]
        [Route("v1/App/GetSwitcheObj")]
        public BasicResponse<SwitcheAppDataContract> GetSwitcheObj(SwitcheGetRequest switheRequest)
        {
            return kj73nappService.GetSwitcheObj(switheRequest);
        }

        [HttpPost]
        [Route("v1/App/GetNetworkModuleObj")]
        public BasicResponse<NetworkModuleAppDataContract> GetNetworkModuleObj(NetworkModuleAppGetRequest networkModuleRequest)
        {
            return kj73nappService.GetNetworkModuleObj(networkModuleRequest);
        }

        [HttpPost]
        [Route("v1/App/Logon")]
        public BasicResponse<UserInfo> Logon(LogonRequest logonRequest)
        {
            return kj73nappService.Logon(logonRequest);
        }

        [HttpPost]
        [Route("v1/App/ModfiyPassword")]
        public BasicResponse ModfiyPassword(ModifyUserPasswordRequest modifyPasswordRequest)
        {
            return kj73nappService.ModfiyPassword(modifyPasswordRequest);
        }

        [HttpPost]
        [Route("v1/App/GetBaseData")]
        public BasicResponse<BaseDataAppDataContract> GetBaseData(BasicRequest basedataRequest)
        {
            return kj73nappService.GetBaseData(basedataRequest);
        }

        [HttpPost]
        [Route("v1/App/GetMLLFiveLine")]
        public BasicResponse<List<AnalogChartAppDataContract>> GetMLLFiveLine(ChartGetRequest analoglineRequest)
        {
            return kj73nappService.GetMLLFiveLine(analoglineRequest);
        }

        [HttpPost]
        [Route("v1/App/GetKGLFiveLine")]
        public BasicResponse<List<DerailChartAppDataContract>> GetKGLFiveLine(ChartGetRequest deraillineRequest)
        {
            return kj73nappService.GetKGLFiveLine(deraillineRequest);
        }

        [HttpPost]
        [Route("v1/App/GetJCRData")]
        public BasicResponse<GetJCRDataResponse> GetJCRData(RunLogGetRequest runlogRequest)
        {
            return kj73nappService.GetJCRData(runlogRequest);
        }

        [HttpPost]
        [Route("v1/App/GetJCMCData")]
        public BasicResponse<GetJCMCDataResponse> GetJCMCData(RunLogGetRequest runlogRequest)
        {
            return kj73nappService.GetJCMCData(runlogRequest);
        }

        [HttpPost]
        [Route("v1/App/GetMLLBJData")]
        public BasicResponse<GetMLLBJDataResponse> GetMLLBJData(RunLogGetRequest runlogRequest)
        {
            return kj73nappService.GetMLLBJData(runlogRequest);
        }

        [HttpPost]
        [Route("v1/App/GetKGLBJData")]
        public BasicResponse<GetKGLBJDataResponse> GetKGLBJData(RunLogGetRequest runlogRequest)
        {
            return kj73nappService.GetKGLBJData(runlogRequest);
        }

        [HttpPost]
        [Route("v1/App/GetMLLDDData")]
        public BasicResponse<GetMLLDDDataResponse> GetMLLDDData(RunLogGetRequest runlogRequest)
        {
            return kj73nappService.GetMLLDDData(runlogRequest);
        }

        [HttpPost]
        [Route("v1/App/GetKGLDDData")]
        public BasicResponse<GetKGLDDDataResponse> GetKGLDDData(RunLogGetRequest runlogRequest)
        {
            return kj73nappService.GetKGLDDData(runlogRequest);
        }

        [HttpPost]
        [Route("v1/App/GetAlarmRecordListByStime")]
        public BasicResponse<List<AlarmProcessInfo>> GetAlarmRecordListByStime(AlarmRecordGetByStimeRequest alarmRecordRequest)
        {
            return kj73nappService.GetAlarmRecordListByStime(alarmRecordRequest);
        }

        [HttpPost]
        [Route("v1/App/EndAlarmRecord")]
        public BasicResponse EndAlarmRecord(AlarmRecordEndRequest alarmRecordEndRequest)
        {
            return kj73nappService.EndAlarmRecord(alarmRecordEndRequest);
        }

        [HttpPost]
        [Route("v1/App/GetAlarmHandleByStimeAndEtime")]
        public BasicResponse<List<JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(AlarmHandelGetByStimeAndETime alarmHandelRequest)
        {
            return kj73nappService.GetAlarmHandleByStimeAndEtime(alarmHandelRequest);
        }

        [HttpPost]
        [Route("v1/App/EndAlarmHandle")]
        public BasicResponse EndAlarmHandle(AlarmHandleEndRequest alarmHandleEndRequest)
        {
            return kj73nappService.EndAlarmHandle(alarmHandleEndRequest);
        }
    }
}
