using System.Collections.Generic;
using System.Data;
using Basic.Framework.Web;
using Sys.Safety.DataContract;
using Sys.Safety.Request.JC_Emergencylinkageconfig;
using Sys.Safety.Request.Listex;
using Sys.Safety.Request.SysEmergencyLinkage;

namespace Sys.Safety.ServiceContract
{
    public interface ISysEmergencyLinkageService
    {
        //BasicResponse<SysEmergencyLinkageInfo> AddSysEmergencyLinkage(
        //    SysEmergencyLinkageAddRequest sysEmergencyLinkageRequest);

        //BasicResponse<SysEmergencyLinkageInfo> UpdateSysEmergencyLinkage(
        //    SysEmergencyLinkageUpdateRequest sysEmergencyLinkageRequest);

        //BasicResponse DeleteSysEmergencyLinkage(SysEmergencyLinkageDeleteRequest sysEmergencyLinkageRequest);

        BasicResponse<List<SysEmergencyLinkageInfo>> GetSysEmergencyLinkageList(
            SysEmergencyLinkageGetListRequest sysEmergencyLinkageRequest);

        BasicResponse<SysEmergencyLinkageInfo> GetSysEmergencyLinkageById(
            SysEmergencyLinkageGetRequest sysEmergencyLinkageRequest);

        /// <summary>
        /// 新增应急联动配置表、主控信息、被控信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse AddEmergencylinkageconfigMasterInfoPassiveInfo(
            AddEmergencylinkageconfigMasterInfoPassiveInfoRequest request);

        /// <summary>获取应急联动定义信息及统计数量
        /// 
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<GetSysEmergencyLinkageListAndStatisticsResponse>> GetSysEmergencyLinkageListAndStatistics(
            StringRequest request);

        /// <summary>根据关联Id获取主控测点列表
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DefInfo>> GetMasterPointInfoByAssId(LongIdRequest request);

        /// <summary>根据关联id获取主控区域列表
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<AreaInfo>> GetMasterAreaInfoByAssId(LongIdRequest request);

        /// <summary>根据关联id获取主控设备类型列表
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_DevInfo>> GetMasterEquTypeInfoByAssId(LongIdRequest request);

        /// <summary>根据关联id获取触发数据状态列表
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<EnumcodeInfo>> GetMasterTriDataStateByAssId(LongIdRequest request);

        /// <summary>根据关联id获取被控人员列表
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<R_PersoninfInfo>> GetPassivePersonByAssId(LongIdRequest request);

        /// <summary>根据关联id获取被控测点列表
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<IdTextCheck>> GetPassivePointInfoByAssId(LongIdRequest request);

        /// <summary>根据关联id获取被控区域列表
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<AreaInfo>> GetPassiveAreaInfoByAssId(LongIdRequest request);

        /// <summary>获取所有被控测点
        /// 
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<IdTextCheck>> GetAllPassivePointInfo();

        /// <summary>软删除应急联动配置
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse SoftDeleteSysEmergencyLinkageById(LongIdRequest request);
        /// <summary>更新实时值
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse UpdateRealTimeState(UpdateRealTimeStateRequest request);

        /// <summary>
        /// 获取所有应急联动配置
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageList();

        /// <summary>
        /// 从数据库获取所有应急联动配置
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageListDb();

        /// <summary>
        /// 根据应急联动配置Id获取所有主控测点
        /// </summary>
        BasicResponse<List<Jc_DefInfo>> GetAllMasterPointsById(SysEmergencyLinkageGetRequest request);

        /// <summary>
        /// 从数据库加载所有应急联动配置
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<SysEmergencyLinkageInfo>> GetAllSysEmergencyLinkageInfo();

        BasicResponse UpdateSysEmergencyLinkage(SysEmergencyLinkageUpdateRequest request);
    }
}