using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.App;
using Sys.Safety.Request.App;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.Request.Jc_B;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.ServiceContract.App
{
    /// <summary>
    /// App接口
    /// </summary>
    public interface IKJ73NAppService
    {
        /// <summary> 
        /// 获取设备实时数据
        /// </summary>
        /// <param name="realDataRequest"></param>
        /// <returns></returns>
        BasicResponse<List<RealDataAppDataContract>> GetRealData(RealDataRequest realDataRequest);
        /// <summary>
        /// 实时获取所有模拟量报警、断电的ID列表，用","间隔  20170731
        /// </summary>
        /// <param name="realDataRequest"></param>
        /// <returns></returns>
        BasicResponse GetAllAnalogAlarm(RealDataRequest realDataRequest);

        /// <summary>
        /// 获取测点详细信息
        /// </summary>
        /// <param name="realDataGetDetialRequest"></param>
        /// <returns></returns>
        BasicResponse<RealDataAppDataContract> GetPointDetail(RealDataGetDetialRequest realDataGetDetialRequest);

        /// <summary>
        /// 获取分站详细信息
        /// </summary>
        /// <param name="realDataGetDetialRequest"></param>
        /// <returns></returns>
        BasicResponse<RealDataAppDataContract> GetStationDetail(RealDataGetDetialRequest realDataGetDetialRequest);

        /// <summary>
        /// 获取交换机详细信息
        /// </summary>
        /// <param name="switheRequest"></param>
        /// <returns></returns>
        BasicResponse<SwitcheAppDataContract> GetSwitcheObj(SwitcheGetRequest switheRequest);

        /// <summary>
        /// 获取网络模块详细信息
        /// </summary>
        /// <param name="networkModuleRequest"></param>
        /// <returns></returns>
        BasicResponse<NetworkModuleAppDataContract> GetNetworkModuleObj(NetworkModuleAppGetRequest networkModuleRequest);

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="logonRequest"></param>
        /// <returns></returns>
        BasicResponse<UserInfo> Logon(LogonRequest logonRequest);

        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="modifyPasswordRequest"></param>
        /// <returns></returns>
        BasicResponse ModfiyPassword(ModifyUserPasswordRequest modifyPasswordRequest);

        /// <summary>
        /// 获取基础数据
        /// </summary>
        /// <param name="basedataRequest"></param>
        /// <returns></returns>
        BasicResponse<BaseDataAppDataContract> GetBaseData(BasicRequest basedataRequest);

        /// <summary>
        /// 获取模拟量曲线
        /// </summary>
        /// <param name="analoglineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<AnalogChartAppDataContract>> GetMLLFiveLine(ChartGetRequest analoglineRequest);

        /// <summary>
        /// 获取开关量曲线
        /// </summary>
        /// <param name="deraillineRequest"></param>
        /// <returns></returns>
        BasicResponse<List<DerailChartAppDataContract>> GetKGLFiveLine(ChartGetRequest deraillineRequest);

        /// <summary>
        /// 查询运行记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        BasicResponse<GetJCRDataResponse> GetJCRData(RunLogGetRequest runlogRequest);

        /// <summary> 
        /// 查询密采记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        BasicResponse<GetJCMCDataResponse> GetJCMCData(RunLogGetRequest runlogRequest);

        /// <summary>
        /// 查询模拟量报警记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        BasicResponse<GetMLLBJDataResponse> GetMLLBJData(RunLogGetRequest runlogRequest);

        /// <summary>
        /// 查询开关量报警记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        BasicResponse<GetKGLBJDataResponse> GetKGLBJData(RunLogGetRequest runlogRequest);

        /// <summary>
        /// 查询模拟量断电记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        BasicResponse<GetMLLDDDataResponse> GetMLLDDData(RunLogGetRequest runlogRequest);

        /// <summary>
        /// 查询开关量断电记录
        /// </summary>
        /// <param name="runlogRequest"></param>
        /// <returns></returns>
        BasicResponse<GetKGLDDDataResponse> GetKGLDDData(RunLogGetRequest runlogRequest);

        /// <summary>
        /// 获取设备报警记录
        /// </summary>
        /// <param name="alarmRecordRequest"></param>
        /// <returns></returns>
        BasicResponse<List<AlarmProcessInfo>> GetAlarmRecordListByStime(AlarmRecordGetByStimeRequest alarmRecordRequest);

        /// <summary>
        /// 结束设备报警记录
        /// </summary>
        /// <param name="alarmRecordEndRequest"></param>
        /// <returns></returns>
        BasicResponse EndAlarmRecord(AlarmRecordEndRequest alarmRecordEndRequest);

        /// <summary>
        /// 获取逻辑报警记录
        /// </summary>
        /// <param name="alarmHandelRequest"></param>
        /// <returns></returns>
        BasicResponse<List<JC_AlarmHandleInfo>> GetAlarmHandleByStimeAndEtime(AlarmHandelGetByStimeAndETime alarmHandelRequest);

        /// <summary>
        /// 结束逻辑报警记录
        /// </summary>
        /// <param name="alarmHandleEndRequest"></param>
        /// <returns></returns>
        BasicResponse EndAlarmHandle(AlarmHandleEndRequest alarmHandleEndRequest);
    }
}
