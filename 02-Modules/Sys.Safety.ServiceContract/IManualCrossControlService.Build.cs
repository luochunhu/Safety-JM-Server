using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basic.Framework.Web;
using Sys.Safety.Request.ManualCrossControl;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;

namespace Sys.Safety.ServiceContract
{
    public interface IManualCrossControlService
    {
        /// <summary>
        /// 添加手动/交叉控制
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        BasicResponse AddManualCrossControl(ManualCrossControlAddRequest ManualCrossControlRequest);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        BasicResponse AddManualCrossControls(ManualCrossControlsRequest ManualCrossControlRequest);
        /// <summary>
        /// 更新手动/交叉控制
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateManualCrossControl(ManualCrossControlUpdateRequest ManualCrossControlRequest);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        BasicResponse UpdateManualCrossControls(ManualCrossControlsRequest ManualCrossControlRequest);
        /// <summary>
        /// 删除手动/交叉控制
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteManualCrossControl(ManualCrossControlDeleteRequest ManualCrossControlRequest);
        /// <summary>
        /// 批量删除手动/交叉控制
        /// </summary>
        /// <param name="ManualCrossControlRequest"></param>
        /// <returns></returns>
        BasicResponse DeleteManualCrossControls(ManualCrossControlsRequest ManualCrossControlRequest);
        /// <summary>
        /// 批量添加\更新\删除接口
        /// </summary>
        /// <param name="NetworkModuleRequest"></param>
        /// <returns></returns>
        BasicResponse BatchOperationManualCrossControls(ManualCrossControlsRequest manualCrossControlRequest);
        BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlList(ManualCrossControlGetListRequest ManualCrossControlRequest);
        BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlList();
        BasicResponse<Jc_JcsdkzInfo> GetManualCrossControlById(ManualCrossControlGetRequest ManualCrossControlRequest);
        /// <summary>
        /// 获取所有手动/交叉控制缓存
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControl();
       /// <summary>
        /// 通过被控分站号查询手动/交叉控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByStationID(ManualCrossControlGetByStationIDRequest ManualCrossControlRequest);
         /// <summary>
        /// 通过被控分站号查询手动控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlHandCtrlByStationID(ManualCrossControlGetByStationIDRequest ManualCrossControlRequest);
        /// <summary>
        /// 通过类型、被控测点号查询手动/交叉控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeBkPoint(ManualCrossControlGetByTypeBkPointRequest ManualCrossControlRequest);
        /// <summary>
        /// 通过类型、主控测点号、被控测点号查询手动/交叉控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeZkPointBkPoint(ManualCrossControlGetByTypeZkPointBkPointRequest ManualCrossControlRequest);
        /// <summary>
        /// 通过被控测点号查询手动/交叉控制
        /// </summary>
        /// <param name="manualCrossControlCacheRequest"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByBkPoint(ManualCrossControlGetByBkPointRequest ManualCrossControlRequest);

        /// <summary>
        /// 通过类型，主控测点号查询手动/交叉控制
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetManualCrossControlByTypeZkPoint(ManualCrossControlGetByTypeZkPointRequest request);

        /// <summary>
        /// 获取所有交叉控制详细信息
        /// </summary>
        /// <returns></returns>
        BasicResponse<List<Jc_JcsdkzInfo>> GetAllManualCrossControlDetail();

        /// <summary>
        /// 重启系统时删除除交叉控制外的所有控制
        /// </summary>
        /// <returns></returns>
        BasicResponse DeleteOtherManualCrossControlFromDB();
    }
}

