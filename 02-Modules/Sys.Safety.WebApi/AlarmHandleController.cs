using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.AlarmHandle;
using Basic.Framework.Service;
using System.Web.Http;

namespace Sys.Safety.WebApi
{
    public class AlarmHandleController : Basic.Framework.Web.WebApi.BasicApiController, IAlarmHandleService
    {
        IAlarmHandleService alarmHandleService = ServiceFactory.Create<IAlarmHandleService>();

        [HttpPost]
        [Route("v1/AlarmHandle/AddJC_AlarmHandle")]
        public BasicResponse<JC_AlarmHandleInfo> AddJC_AlarmHandle(AlarmHandleAddRequest jC_AlarmHandlerequest)
        {
            return alarmHandleService.AddJC_AlarmHandle(jC_AlarmHandlerequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/DeleteJC_AlarmHandle")]
        public BasicResponse DeleteJC_AlarmHandle(AlarmHandleDeleteRequest jC_AlarmHandlerequest)
        {
            return alarmHandleService.DeleteJC_AlarmHandle(jC_AlarmHandlerequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/GetJC_AlarmHandleById")]
        public BasicResponse<JC_AlarmHandleInfo> GetJC_AlarmHandleById(AlarmHandleGetRequest jC_AlarmHandlerequest)
        {
            return alarmHandleService.GetJC_AlarmHandleById(jC_AlarmHandlerequest);
        }
        [HttpPost]
        [Route("v1/AlarmHandle/GetJC_AlarmHandleList")]
        public BasicResponse<List<JC_AlarmHandleInfo>> GetJC_AlarmHandleList(AlarmHandleGetListRequest jC_AlarmHandlerequest)
        {
            return alarmHandleService.GetJC_AlarmHandleList(jC_AlarmHandlerequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/GetUnclosedAlarmByAnalysisModelId")]
        public BasicResponse<JC_AlarmHandleInfo> GetUnclosedAlarmByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest alarmHandleGetByAnalysisModelIdRequest)
        {
            return alarmHandleService.GetUnclosedAlarmByAnalysisModelId(alarmHandleGetByAnalysisModelIdRequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/GetUnclosedAlarmList")]
        public BasicResponse<List<JC_AlarmHandleInfo>> GetUnclosedAlarmList(AlarmHandleWithoutSearchConditionRequest alarmHandleWithoutSearchConditionRequest)
        {
            return alarmHandleService.GetUnclosedAlarmList(alarmHandleWithoutSearchConditionRequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/GetUnhandledAlarmByAnalysisModelId")]
        public BasicResponse<JC_AlarmHandleInfo> GetUnhandledAlarmByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest alarmHandleGetByAnalysisModelIdRequest)
        {
            return alarmHandleService.GetUnhandledAlarmByAnalysisModelId(alarmHandleGetByAnalysisModelIdRequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/GetUnhandledAlarmList")]
        public BasicResponse<List<JC_AlarmHandleInfo>> GetUnhandledAlarmList(AlarmHandleWithoutSearchConditionRequest alarmHandleWithoutSearchConditionRequest)
        {
            return alarmHandleService.GetUnhandledAlarmList(alarmHandleWithoutSearchConditionRequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/UpdateAlarmHandleList")]
        public BasicResponse<List<JC_AlarmHandleInfo>> UpdateAlarmHandleList(AlarmHandleUpdateListRequest alarmHandleUpdateListRequest)
        {
            return alarmHandleService.UpdateAlarmHandleList(alarmHandleUpdateListRequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/UpdateJC_AlarmHandle")]
        public BasicResponse<JC_AlarmHandleInfo> UpdateJC_AlarmHandle(AlarmHandleUpdateRequest jC_AlarmHandlerequest)
        {
            return alarmHandleService.UpdateJC_AlarmHandle(jC_AlarmHandlerequest);
        }


        [HttpPost]
        [Route("v1/AlarmHandle/GetAlarmHandleByStimeAndEtime")]
        public BasicResponse<List<JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(AlarmHandelGetByStimeAndETime alarmHandelRequest)
        {
            return alarmHandleService.GetAlarmHandleByStimeAndEtime(alarmHandelRequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/GetAlarmHandleNoEndListByCondition")]
        public BasicResponse<List<JC_AlarmHandleNoEndInfo>> GetAlarmHandleNoEndListByCondition(AlarmHandleNoEndListByCondition alarmHandelRequest)
        {
            return alarmHandleService.GetAlarmHandleNoEndListByCondition(alarmHandelRequest);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/CloseUnclosedAlarmHandle")]
        public BasicResponse CloseUnclosedAlarmHandle(BasicRequest request)
        {
            return alarmHandleService.CloseUnclosedAlarmHandle(request);
        }

        [HttpPost]
        [Route("v1/AlarmHandle/CloseUnclosedAlarmHandleByAnalysisModelId")]
        public BasicResponse CloseUnclosedAlarmHandleByAnalysisModelId(AlarmHandleGetByAnalysisModelIdRequest getByAnalysisModelIdRequest)
        {
            return alarmHandleService.CloseUnclosedAlarmHandleByAnalysisModelId(getByAnalysisModelIdRequest);
        }
    }
}
