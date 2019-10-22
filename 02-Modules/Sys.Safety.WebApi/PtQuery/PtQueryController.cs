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
    /// 外部查询 API接口
    /// </summary>
    public class PtQueryController : BasicApiController, IPtQueryService
    {
        private IPtQueryService _PtQueryService;
        public PtQueryController()
        {
            _PtQueryService = ServiceFactory.Create<IPtQueryService>();            
        }

        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <param name="realDataRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetRealData")]
        public BasicResponse<List<RealDataAppDataContract>> GetRealData(RealDataRequest realDataRequest)
        {
            return _PtQueryService.GetRealData(realDataRequest);
        }
        [HttpPost]
        [Route("v1/PtQuery/GetAllAnalogAlarm")]
        public BasicResponse GetAllAnalogAlarm(RealDataRequest realDataRequest)
        {
            return _PtQueryService.GetAllAnalogAlarm(realDataRequest);
        }

        /// <summary>
        /// 获取测点详细信息
        /// </summary>
        /// <param name="realDataGetDetialRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetPointDetail")]
        public BasicResponse<RealDataAppDataContract> GetPointDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            return _PtQueryService.GetPointDetail(realDataGetDetialRequest);
        }

        /// <summary>
        /// 获取分站详细信息
        /// </summary>
        /// <param name="realDataGetDetialRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetStationDetail")]
        public BasicResponse<RealDataAppDataContract> GetStationDetail(RealDataGetDetialRequest realDataGetDetialRequest)
        {
            return _PtQueryService.GetStationDetail(realDataGetDetialRequest);
        }

        /// <summary>
        /// 获取交换机详细信息
        /// </summary>
        /// <param name="switheRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetSwitcheObj")]
        public BasicResponse<SwitcheAppDataContract> GetSwitcheObj(SwitcheGetRequest switheRequest)
        {
            return _PtQueryService.GetSwitcheObj(switheRequest);
        }

        /// <summary>
        /// 获取网络模块详细信息
        /// </summary>
        /// <param name="networkModuleRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetNetworkModuleObj")]
        public BasicResponse<NetworkModuleAppDataContract> GetNetworkModuleObj(NetworkModuleAppGetRequest networkModuleRequest)
        {
            return _PtQueryService.GetNetworkModuleObj(networkModuleRequest);
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="logonRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/Logon")]
        public BasicResponse<Sys.Safety.DataContract.UserInfo> Logon(LogonRequest logonRequest)
        {
            return _PtQueryService.Logon(logonRequest);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="modifyPasswordRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/ModfiyPassword")]
        public BasicResponse ModfiyPassword(ModifyUserPasswordRequest modifyPasswordRequest)
        {
            return _PtQueryService.ModfiyPassword(modifyPasswordRequest);
        }

        /// <summary>
        /// 获取基础数据
        /// </summary>
        /// <param name="basedataRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetBaseData")]
        public BasicResponse<BaseDataAppDataContract> GetBaseData(BasicRequest basedataRequest)
        {
            return _PtQueryService.GetBaseData(basedataRequest);
        }

        /// <summary>
        /// 获取模拟量曲线
        /// </summary>
        /// <param name="analoglineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetMLLFiveLine")]
        public BasicResponse<List<AnalogChartAppDataContract>> GetMLLFiveLine(ChartGetRequest analoglineRequest)
        {
            return _PtQueryService.GetMLLFiveLine(analoglineRequest);
        }

        /// <summary>
        /// 获取开关量曲线
        /// </summary>
        /// <param name="deraillineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetKGLFiveLine")]
        public BasicResponse<List<DerailChartAppDataContract>> GetKGLFiveLine(ChartGetRequest deraillineRequest)
        {
            return _PtQueryService.GetKGLFiveLine(deraillineRequest);
        }
        /// <summary>
        /// 获取开关量柱状图
        /// </summary>
        /// <param name="deraillineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetKGLStateLine")]
        public BasicResponse<List<DataContract.App.DerailChartStateLineDataContract>> GetKGLStateLine(ChartGetRequest deraillineRequest)
        {
            return _PtQueryService.GetKGLStateLine(deraillineRequest);
        }
        /// <summary>
        /// 获取模拟量日班报表
        /// </summary>
        /// <param name="analoglineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetMLLDayReport")]
        public BasicResponse<List<DataContract.App.MnlDayReportDataContract>> GetMLLDayReport(GetDayReportRequest analoglineRequest)
        {
            return _PtQueryService.GetMLLDayReport(analoglineRequest);
        }
        /// <summary>
        /// 查询运行记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetJCRData")]
        public BasicResponse<GetJCRDataResponse> GetJCRData(PointRunLogGetRequest runlogRequest)
        {
            return _PtQueryService.GetJCRData(runlogRequest);
        }

        /// <summary>
        /// 查询密采记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetJCMCData")]
        public BasicResponse<GetJCMCDataResponse> GetJCMCData(RunLogGetRequest runlogRequest)
        {
            return _PtQueryService.GetJCMCData(runlogRequest);
        }

        /// <summary>
        /// 查询模拟量报警记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetMLLBJData")]
        public BasicResponse<GetMLLBJDataResponse> GetMLLBJData(RunLogGetRequest runlogRequest)
        {
            return _PtQueryService.GetMLLBJData(runlogRequest);
        }

        /// <summary>
        /// 查询开关量报警记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetKGLBJData")]
        public BasicResponse<GetKGLBJDataResponse> GetKGLBJData(RunLogGetRequest runlogRequest)
        {
            return _PtQueryService.GetKGLBJData(runlogRequest);
        }

        /// <summary>
        /// 查询模拟量断电记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetMLLDDData")]
        public BasicResponse<GetMLLDDDataResponse> GetMLLDDData(RunLogGetRequest runlogRequest)
        {
            return _PtQueryService.GetMLLDDData(runlogRequest);
        }

        /// <summary>
        /// 查询开关量断电记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetKGLDDData")]
        public BasicResponse<GetKGLDDDataResponse> GetKGLDDData(RunLogGetRequest runlogRequest)
        {
            return _PtQueryService.GetKGLDDData(runlogRequest);
        }

        /// <summary>
        /// 获取设备报警记录
        /// </summary>
        /// <param name="alarmRecordRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetAlarmRecordListByStime")]
        public BasicResponse<List<Sys.Safety.DataContract.AlarmProcessInfo>> GetAlarmRecordListByStime(Sys.Safety.Request.Jc_B.AlarmRecordGetByStimeRequest alarmRecordRequest)
        {
            return _PtQueryService.GetAlarmRecordListByStime(alarmRecordRequest);
        }

        /// <summary>
        /// 结束设备报警记录
        /// </summary>
        /// <param name="alarmRecordEndRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/EndAlarmRecord")]
        public BasicResponse EndAlarmRecord(AlarmRecordEndRequest alarmRecordEndRequest)
        {
            return _PtQueryService.EndAlarmRecord(alarmRecordEndRequest);
        }

        /// <summary>
        /// 获取逻辑报警记录
        /// </summary>
        /// <param name="alarmHandelRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetAlarmHandleByStimeAndEtime")]
        public BasicResponse<List<Sys.Safety.DataContract.JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(Sys.Safety.Request.AlarmHandle.AlarmHandelGetByStimeAndETime alarmHandelRequest)
        {
            return _PtQueryService.GetAlarmHandleByStimeAndEtime(alarmHandelRequest);
        }

        /// <summary>
        /// 结束逻辑报警记录
        /// </summary>
        /// <param name="alarmHandleEndRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/EndAlarmHandle")]
        public BasicResponse EndAlarmHandle(AlarmHandleEndRequest alarmHandleEndRequest)
        {
            return _PtQueryService.EndAlarmHandle(alarmHandleEndRequest);
        }
        /// <summary>
        /// 获取所有测点定义信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/PtQuery/GetAllPointDefineData")]
        public BasicResponse<List<DataContract.App.PointInfoDataContract>> GetAllPointDefineData()
        {
            return _PtQueryService.GetAllPointDefineData();
        }






        
    }
}
