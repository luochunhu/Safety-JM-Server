using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.DataContract;
using Sys.Safety.Request.StationUpdate;
using Sys.Safety.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Driver
{
    public class StationUpdateBusiness
    {
        static IStationUpdateService stationUpdateService;
        static StationUpdateBusiness()
        {
            stationUpdateService = ServiceFactory.Create<IStationUpdateService>();
        }

        public static void Handle(MasProtocol data)
        {
           switch (data.ProtocolType)
            {
                case ProtocolType.GetStationUpdateStateResponse://获取分站的工作状态(设备->上位机)---- 
                    GetStationUpdateStateResponseProc(data);
                    break;
                case ProtocolType.InspectionResponse:   //巡检单台分站的文件接收情况回复(设备->上位机)
                    InspectionResponseProc(data);
                    break;
                case ProtocolType.ReductionResponse:    //远程还原最近一次备份程序(设备->上位机)
                    ReductionResponseProc(data);
                    break;
                case ProtocolType.RestartResponse:      //通知分站进行重启升级回复(设备->上位机)
                    RestartResponseProc(data);
                    break;
                case ProtocolType.StationUpdateResponse:    //设备请求升级回复(设备->上位机)
                    StationUpdateResponseProc(data);
                    break;
                case ProtocolType.UpdateCancleResponse:     //异常中止升级流程(设备->上位机)
                    UpdateCancleResponseProc(data);
                    break;
            }
        }

        /// <summary>
        /// 获取分站的工作状态回复
        /// </summary>
        private static void GetStationUpdateStateResponseProc(MasProtocol data)
        {
            GetStationItemRequest getStationItemRequest = new GetStationItemRequest();
            var result = data.Deserialize<GetStationUpdateStateResponse>();
            int fzh = Convert.ToInt32(result.DeviceCode.Substring(0, 3));
            getStationItemRequest.fzh = fzh;
            var stationItem = stationUpdateService.GetStationItem(getStationItemRequest);
            if (stationItem != null && stationItem.IsSuccess)
            {
                StationUpdateItem item = stationItem.Data;
                if (item != null)
                {
                    item.stationWorkState = new StationWorkState();
                    item.stationWorkState.softVersion = result.GetSoftVersion;
                    item.stationWorkState.hardVersion = result.GetHardVersion;
                    item.stationWorkState.devTypeID = result.GetDevType;
                    item.stationWorkState.updateState = result.GetUpdateState;
                    item.Info = "获取分站的工作状态成功";
                    UpdateOrderForSysRequest updateOrderForSysRequest = new UpdateOrderForSysRequest();
                    updateOrderForSysRequest.item = item;
                    stationUpdateService.UpdateStationItemForSys(updateOrderForSysRequest);
                    LogHelper.Info("获取分站的工作状态GetStationUpdateStateResponseProc【" + fzh + "】获取成功");
                }
            }

            Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(result.DeviceCode);
            station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            updateItems.Add("ClsCommObj", station.ClsCommObj);
            SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
            LogHelper.Info("获取分站的工作状态GetStationUpdateStateResponseProc【" + fzh + "】切F命令");
        }

        /// <summary>
        /// 巡检单台分站的文件接收情况回复(设备->上位机)
        /// </summary>
        /// <param name="data"></param>
        private static void InspectionResponseProc(MasProtocol data)
        {
            GetStationItemRequest getStationItemRequest = new GetStationItemRequest();
            var result = data.Deserialize<InspectionResponse>();
            int fzh = Convert.ToInt32(result.DeviceCode.Substring(0, 3));
            getStationItemRequest.fzh = fzh;
            var stationItem = stationUpdateService.GetStationItem(getStationItemRequest);
            if (stationItem != null && stationItem.IsSuccess)
            {
                StationUpdateItem item = stationItem.Data;
                if (item != null)
                {
                    Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(result.DeviceCode);
                    getStationItemRequest.fzh = station.Fzh;

                    //Bit3~bit0：故障码
                    //=0：正常（升级文件已接收完毕）
                    //=1：升级文件不属于该类设备（不能升级）
                    //=2：请求的升级文件版本号与本地的升级文件版本号不匹配；（不能升级）
                    //=3：该设备不处于升级模式（不能升级）
                    //=4：升级文件缺失（不能升级，后续有缺失文件编号字段）
                    //=5：文件编号异常；
                    //=6：硬件版本号不匹配（不能升级）

                    if (result.ResponseCode == 0)
                    {
                        //正常（升级文件已接收完毕）
                        item.updateState = Sys.Safety.Enums.StationUpdateState.replacComplete;
                        item.Info = "升级文件已接收完毕";
                        LogHelper.Info("【" + station.Point + "】InspectionResponseProc:升级文件已接收完毕");
                    }
                    else if (result.ResponseCode == 4)
                    {
                        //升级文件缺失（不能升级，后续有缺失文件编号字段）
                        item.updateState = Sys.Safety.Enums.StationUpdateState.replacement;
                        item.nowNeedSendBuffIndex = result.LostFileNum;
                        item.Info = "升级文件缺失，文件编号：" + item.nowNeedSendBuffIndex;
                        LogHelper.Info("【" + station.Point + "】InspectionResponseProc:升级文件缺失，文件编号：" + item.nowNeedSendBuffIndex);
                    }
                    else
                    {
                        item.updateState = Sys.Safety.Enums.StationUpdateState.error;

                        switch (result.ResponseCode)
                        {
                            case 1:
                                item.Info = "升级文件不属于该类设备";
                                break;
                            case 2:
                                item.Info = "请求的升级文件版本号与本地的升级文件版本号不匹配";
                                break;
                            case 3:
                                item.Info = "该设备不处于升级模式";
                                break;
                            case 5:
                                item.Info = "文件编号异常";
                                break;
                            case 6:
                                item.Info = "硬件版本号不匹配";
                                break;
                        }

                        LogHelper.Info("【" + station.Point + "】InspectionResponseProc:回发异常状态，故障码：" + result.ResponseCode);
                    }
                    LogHelper.Info("巡检单台分站的文件接收情况InspectionResponseProc【" + fzh + "】");
                    UpdateOrderForSysRequest updateOrderForSysRequest = new UpdateOrderForSysRequest();
                    updateOrderForSysRequest.item = item;
                    stationUpdateService.UpdateStationItemForSys(updateOrderForSysRequest);

                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                    LogHelper.Info("巡检单台分站的文件接收情况InspectionResponseProc【" + fzh + "】切F命令");
                }
            }
        }

        /// <summary>
        /// 远程还原最近一次备份程序(设备->上位机)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="defInfo"></param>
        private static void ReductionResponseProc(MasProtocol data)
        {
            GetStationItemRequest getStationItemRequest = new GetStationItemRequest();
            var result = data.Deserialize<ReductionResponse>();
            int fzh = Convert.ToInt32(result.DeviceCode.Substring(0, 3));
            getStationItemRequest.fzh = fzh;
            var stationItem = stationUpdateService.GetStationItem(getStationItemRequest);
            if (stationItem != null && stationItem.IsSuccess)
            {
                StationUpdateItem item = stationItem.Data;
                if (item != null)
                {
                    Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(result.DeviceCode);
                    getStationItemRequest.fzh = station.Fzh;
                   //Bit3~bit0：故障码
                   // =0：正常（分站已准备恢复操作）
                   // =1：设备类型不匹配（未能恢复）
                   // =2：版本号不匹配（未能恢复）
                   // =3：该分站未进行备份，不能恢复；
                   // =4：分站最近因存储器不可靠而升级失败，不能恢复
                   // =5：硬件版本号不匹配（未能恢复）


                    if (result.ResponseCode == 0)
                    {
                        //正常（升级文件已接收完毕）
                        item.updateState = Sys.Safety.Enums.StationUpdateState.restoreSuccess;
                        item.Info = "分站已恢复成功";
                        LogHelper.Info("【" + station.Point + "】ReductionResponseProc:分站已恢复成功");
                    }
                    else
                    {
                        item.updateState = Sys.Safety.Enums.StationUpdateState.error;
                        switch (result.ResponseCode)
                        {
                            case 1:
                                item.Info = "设备类型不匹配";
                                break;
                            case 2:
                                item.Info = "版本号不匹配";
                                break;
                            case 3:
                                item.Info = "该分站未进行备份，不能恢复";
                                break;
                            case 4:
                                item.Info = "分站最近因存储器不可靠而升级失败,不能恢复";
                                break;
                            case 5:
                                item.Info = "硬件版本号不匹配";
                                break;
                        }
                        LogHelper.Info("【" + station.Point + "】ReductionResponseProc:回发异常状态，故障码：" + result.ResponseCode);
                    }

                    UpdateOrderForSysRequest updateOrderForSysRequest = new UpdateOrderForSysRequest();
                    updateOrderForSysRequest.item = item;
                    stationUpdateService.UpdateStationItemForSys(updateOrderForSysRequest);
                    LogHelper.Info("远程还原最近一次备份程序ReductionResponseProc【" + station.Fzh + "】");

                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                    LogHelper.Info("远程还原最近一次备份程序ReductionResponseProc【" + station.Fzh + "】切F命令");
                }
            }
        }
        /// <summary>
        /// 通知分站进行重启升级回复(设备->上位机)1
        /// </summary>
        /// <param name="data"></param>
        /// <param name="defInfo"></param>
        private static void RestartResponseProc(MasProtocol data)
        {
            GetStationItemRequest getStationItemRequest = new GetStationItemRequest();
            var result = data.Deserialize<RestartResponse>();
            int fzh = Convert.ToInt32(result.DeviceCode.Substring(0, 3));
            getStationItemRequest.fzh = fzh;
            var stationItem = stationUpdateService.GetStationItem(getStationItemRequest);
            if (stationItem != null && stationItem.IsSuccess)
            {
                StationUpdateItem item = stationItem.Data;
                if (item != null)
                {
                    Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(result.DeviceCode);
                    getStationItemRequest.fzh = station.Fzh;
                    //=0：正常（分站准备重启并升级）
                    //=1：升级文件不属于该类设备（不能升级）
                    //=2：请求的升级文件版本号与本地的升级文件版本号不匹配；（不能升级）
                    //=3：该分站不处于升级模式（不能升级）
                    //=4：该分站升级文件缺失；（不能升级）
                    //=5：文件校验失败（不能升级）
                    //=6：硬件版本号不匹配（不能升级）

                    if (result.ResponseCode == 0)
                    {
                        //正常（升级文件已接收完毕）
                        item.updateState = Sys.Safety.Enums.StationUpdateState.restartSuccess;
                        item.Info = "分站准备重启并升级";
                        LogHelper.Info("【" + station.Point + "】RestartResponseProc:分站准备重启并升级");
                    }
                    else
                    {
                        switch (result.ResponseCode)
                        {
                            case 1:
                                item.Info = "升级文件不属于该类设备";
                                break;
                            case 2:
                                item.Info = "请求的升级文件版本号与本地的升级文件版本号不匹配";
                                break;
                            case 3:
                                item.Info = "该分站不处于升级模式";
                                break;
                            case 4:
                                item.Info = "该分站升级文件缺失";
                                break;
                            case 5:
                                item.Info = "文件校验失败";
                                break;
                            case 6:
                                item.Info = "硬件版本号不匹配";
                                break;
                        }
                        item.updateState = Sys.Safety.Enums.StationUpdateState.error;
                        LogHelper.Info("【" + station.Point + "】RestartResponseProc:回发异常状态，故障码：" + result.ResponseCode);
                    }

                    UpdateOrderForSysRequest updateOrderForSysRequest = new UpdateOrderForSysRequest();
                    updateOrderForSysRequest.item = item;
                    stationUpdateService.UpdateStationItemForSys(updateOrderForSysRequest);
                    LogHelper.Info("通知分站进行重启升级RestartResponseProc【" + station.Fzh + "】");

                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                    LogHelper.Info("通知分站进行重启升级RestartResponseProc【" + station.Fzh + "】切F命令");
                }
            }
        }
        /// <summary>
        /// 设备请求升级回复
        /// </summary>
        /// <param name="data"></param>
        /// <param name="defInfo"></param>
        private static void StationUpdateResponseProc(MasProtocol data)
        {
            GetStationItemRequest getStationItemRequest = new GetStationItemRequest();
            var result = data.Deserialize<StationUpdateResponse>();
            int fzh  =Convert.ToInt32(result.DeviceCode.Substring(0,3));
            getStationItemRequest.fzh = fzh;
            var stationItem = stationUpdateService.GetStationItem(getStationItemRequest);
            if (stationItem != null && stationItem.IsSuccess)
            {
                StationUpdateItem item = stationItem.Data;
                if (item != null)
                {
                    
                    Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(result.DeviceCode);
                    getStationItemRequest.fzh = station.Fzh;
                    //Bit3~bit0：故障码
                    //=0：正常（已准备好接收升级文件）
                    //=1：升级文件不属于该类设备（不能升级）
                    //=2：升级条件不满足（不能升级）
                    //=3：本地代码存储空间过小（不能升级）
                    //=4：分站已处于远程升级模式且升级软件版本号与本次请求一致（分站将不更新已接收数据）；
                    //=5：分站已处于升级模式且升级软件版本号与本次请求不匹配（中心站若想升级该分站，应下发强制中止当前升级流程再重新下发本次升级请求。）
                    //=6：本地存储器不可靠，最近一次升级失败（不能升级）
                    //=7：分站还未做好升级准备，稍后再试；（不能升级）
                    //=8：硬件版本号与本地不匹配（不能升级）

                    if (result.ResponseCode == 0 || result.ResponseCode == 4)
                    {
                        //正常（升级文件已接收完毕）
                        item.updateState = Sys.Safety.Enums.StationUpdateState.requestSuccess;
                        item.Info = "已准备好接收升级文件";
                        LogHelper.Info("【" + station.Point + "】StationUpdateResponseProc:已准备好接收升级文件");
                    }
                    else
                    {
                        item.updateState = Sys.Safety.Enums.StationUpdateState.requestFailure;

                        switch (result.ResponseCode)
                        {
                            case 1:
                                item.Info = "升级文件不属于该类设备";
                                break;
                            case 2:
                                item.Info = "升级条件不满足";
                                break;
                            case 3:
                                item.Info = "本地代码存储空间过小";
                                break;
                            case 5:
                                item.Info = "分站已处于升级模式且升级软件版本号与本次请求不匹配";
                                break;
                            case 6:
                                item.Info = "本地存储器不可靠，最近一次升级失败";
                                break;
                            case 7:
                                item.Info = "分站还未做好升级准备，稍后再试";
                                break;
                            case 8:
                                item.Info = "硬件版本号与本地不匹配";
                                break;
                        }

                        LogHelper.Info("【" + station.Point + "】StationUpdateResponseProc:回发异常状态，故障码：" + result.ResponseCode);
                    }

                    UpdateOrderForSysRequest updateOrderForSysRequest = new UpdateOrderForSysRequest();
                    updateOrderForSysRequest.item = item;
                    stationUpdateService.UpdateStationItemForSys(updateOrderForSysRequest);
                    LogHelper.Info("设备请求升级StationUpdateResponseProc【" + station.Fzh + "】");

                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                    LogHelper.Info("设备请求升级StationUpdateResponseProc【" + station.Fzh + "】切F命令");
                }
            }
        }

        private static void UpdateCancleResponseProc(MasProtocol data)
        {
            GetStationItemRequest getStationItemRequest = new GetStationItemRequest();
            var result = data.Deserialize<UpdateCancleResponse>();
            int fzh = Convert.ToInt32(result.DeviceCode.Substring(0, 3));
            getStationItemRequest.fzh = fzh;
            var stationItem = stationUpdateService.GetStationItem(getStationItemRequest);
            if (stationItem != null && stationItem.IsSuccess)
            {
                StationUpdateItem item = stationItem.Data;
                if (item != null)
                {
                    Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(result.DeviceCode);
                    getStationItemRequest.fzh = station.Fzh;
                    //Bit3~bit0：故障码
                    //=0：正常（分站已中止该次升级）
                    //=1：设备类型不匹配（未能中止）
                    //=2：升级文件版本号不匹配（未能中止）
                    //=3：硬件版本号不匹配（未能中止）

                    if (result.ResponseCode == 0)
                    {
                        //正常（升级文件已接收完毕）
                        item.updateState = Sys.Safety.Enums.StationUpdateState.updateCancleSuccess;
                        item.Info = "分站已中止该次升级";
                        LogHelper.Info("【" + station.Point + "】UpdateCancleResponseProc:分站已中止该次升级");
                    }
                    else
                    {
                        item.updateState = Sys.Safety.Enums.StationUpdateState.updateCancleFailure;

                        switch (result.ResponseCode)
                        {
                            case 1:
                                item.Info = "设备类型不匹配";
                                break;
                            case 2:
                                item.Info = "升级文件版本号不匹配";
                                break;
                            case 3:
                                item.Info = "硬件版本号不匹配";
                                break;
                        }

                        LogHelper.Info("【" + station.Point + "】UpdateCancleResponseProc:回发异常状态，故障码：" + result.ResponseCode);
                    }

                    UpdateOrderForSysRequest updateOrderForSysRequest = new UpdateOrderForSysRequest();
                    updateOrderForSysRequest.item = item;
                    stationUpdateService.UpdateStationItemForSys(updateOrderForSysRequest);
                    LogHelper.Info("设备请求取消升级UpdateCancleResponseProc【" + station.Fzh + "】");

                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                    LogHelper.Info("设备请求取消升级UpdateCancleResponseProc【" + station.Fzh + "】切F命令");
                }
            }
        }
    }
}
