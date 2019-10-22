using Basic.Framework.Service;
using Basic.Framework.Web;
using Basic.Framework.Web.WebApi;
using Sys.Safety.DataContract.App;
using Sys.Safety.Request.App;
using Sys.Safety.ServiceContract.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Sys.Safety.DataContract.App;

namespace Sys.Safety.WebApi.App
{
    /// <summary>
    /// KJ73NApp API接口
    /// </summary>
    public class KJ73NAppController : BasicApiController, IKJ73NAppService
    {
        private IKJ73NAppService _kj73nAppService;
        public KJ73NAppController()
        {
            _kj73nAppService = ServiceFactory.Create<IKJ73NAppService>();
            //_kj73nAppService=kj73nAppService
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <param name="realDataRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetRealData")]
        public BasicResponse<List<RealDataAppDataContract>> GetRealData(RealDataRequest realDataRequest)
        {
            return _kj73nAppService.GetRealData(realDataRequest);
        }
        [HttpPost]
        [Route("v1/KJ73NApp/GetAllAnalogAlarm")]
        public BasicResponse GetAllAnalogAlarm(RealDataRequest realDataRequest)
        {
            return _kj73nAppService.GetAllAnalogAlarm(realDataRequest);
        }

        /// <summary>
        /// 获取测点详细信息
        /// </summary>
        /// <param name="realDataGetDetialRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetPointDetail")]
        public BasicResponse<RealDataAppDataContract> GetPointDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            return _kj73nAppService.GetPointDetail(realDataGetDetialRequest);
        }

        /// <summary>
        /// 获取分站详细信息
        /// </summary>
        /// <param name="realDataGetDetialRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetStationDetail")]
        public BasicResponse<RealDataAppDataContract> GetStationDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            return _kj73nAppService.GetStationDetail(realDataGetDetialRequest);
        }

        /// <summary>
        /// 获取交换机详细信息
        /// </summary>
        /// <param name="switheRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetSwitcheObj")]
        public BasicResponse<SwitcheAppDataContract> GetSwitcheObj(SwitcheGetRequest switheRequest)
        {
            return _kj73nAppService.GetSwitcheObj(switheRequest);
        }

        /// <summary>
        /// 获取网络模块详细信息
        /// </summary>
        /// <param name="networkModuleRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetNetworkModuleObj")]
        public BasicResponse<NetworkModuleAppDataContract> GetNetworkModuleObj(NetworkModuleAppGetRequest networkModuleRequest)
        {
            return _kj73nAppService.GetNetworkModuleObj(networkModuleRequest);
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="logonRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/Logon")]
        public BasicResponse<Sys.Safety.DataContract.UserInfo> Logon(LogonRequest logonRequest)
        {
            return _kj73nAppService.Logon(logonRequest);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="modifyPasswordRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/ModfiyPassword")]
        public BasicResponse ModfiyPassword(ModifyUserPasswordRequest modifyPasswordRequest)
        {
            return _kj73nAppService.ModfiyPassword(modifyPasswordRequest);
        }

        /// <summary>
        /// 获取基础数据
        /// </summary>
        /// <param name="basedataRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetBaseData")]
        public BasicResponse<BaseDataAppDataContract> GetBaseData(BasicRequest basedataRequest)
        {
            return _kj73nAppService.GetBaseData(basedataRequest);
        }

        /// <summary>
        /// 获取模拟量曲线
        /// </summary>
        /// <param name="analoglineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetMLLFiveLine")]
        public BasicResponse<List<AnalogChartAppDataContract>> GetMLLFiveLine(ChartGetRequest analoglineRequest)
        {
            return _kj73nAppService.GetMLLFiveLine(analoglineRequest);
        }

        /// <summary>
        /// 获取开关量曲线
        /// </summary>
        /// <param name="deraillineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetKGLFiveLine")]
        public BasicResponse<List<DerailChartAppDataContract>> GetKGLFiveLine(ChartGetRequest deraillineRequest)
        {
            return _kj73nAppService.GetKGLFiveLine(deraillineRequest);
        }

        /// <summary>
        /// 查询运行记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetJCRData")]
        public BasicResponse<GetJCRDataResponse> GetJCRData(RunLogGetRequest runlogRequest)
        {
            return _kj73nAppService.GetJCRData(runlogRequest);
        }

        /// <summary>
        /// 查询密采记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetJCMCData")]
        public BasicResponse<GetJCMCDataResponse> GetJCMCData(RunLogGetRequest runlogRequest)
        {
            return _kj73nAppService.GetJCMCData(runlogRequest);
        }

        /// <summary>
        /// 查询模拟量报警记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetMLLBJData")]
        public BasicResponse<GetMLLBJDataResponse> GetMLLBJData(RunLogGetRequest runlogRequest)
        {
            return _kj73nAppService.GetMLLBJData(runlogRequest);
        }

        /// <summary>
        /// 查询开关量报警记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetKGLBJData")]
        public BasicResponse<GetKGLBJDataResponse> GetKGLBJData(RunLogGetRequest runlogRequest)
        {
            return _kj73nAppService.GetKGLBJData(runlogRequest);
        }

        /// <summary>
        /// 查询模拟量断电记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetMLLDDData")]
        public BasicResponse<GetMLLDDDataResponse> GetMLLDDData(RunLogGetRequest runlogRequest)
        {
            return _kj73nAppService.GetMLLDDData(runlogRequest);
        }

        /// <summary>
        /// 查询开关量断电记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetKGLDDData")]
        public BasicResponse<GetKGLDDDataResponse> GetKGLDDData(RunLogGetRequest runlogRequest)
        {
            return _kj73nAppService.GetKGLDDData(runlogRequest);
        }

        /// <summary>
        /// 获取设备报警记录
        /// </summary>
        /// <param name="alarmRecordRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetAlarmRecordListByStime")]
        public BasicResponse<List<Sys.Safety.DataContract.AlarmProcessInfo>> GetAlarmRecordListByStime(Sys.Safety.Request.Jc_B.AlarmRecordGetByStimeRequest alarmRecordRequest)
        {
            return _kj73nAppService.GetAlarmRecordListByStime(alarmRecordRequest);
        }

        /// <summary>
        /// 结束设备报警记录
        /// </summary>
        /// <param name="alarmRecordEndRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/EndAlarmRecord")]
        public BasicResponse EndAlarmRecord(AlarmRecordEndRequest alarmRecordEndRequest)
        {
            return _kj73nAppService.EndAlarmRecord(alarmRecordEndRequest);
        }

        /// <summary>
        /// 获取逻辑报警记录
        /// </summary>
        /// <param name="alarmHandelRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/GetAlarmHandleByStimeAndEtime")]
        public BasicResponse<List<Sys.Safety.DataContract.JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(Sys.Safety.Request.AlarmHandle.AlarmHandelGetByStimeAndETime alarmHandelRequest)
        {
            return _kj73nAppService.GetAlarmHandleByStimeAndEtime(alarmHandelRequest);
        }

        /// <summary>
        /// 结束逻辑报警记录
        /// </summary>
        /// <param name="alarmHandleEndRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/KJ73NApp/EndAlarmHandle")]
        public BasicResponse EndAlarmHandle(AlarmHandleEndRequest alarmHandleEndRequest)
        {
            return _kj73nAppService.EndAlarmHandle(alarmHandleEndRequest);
        }
    }
}
