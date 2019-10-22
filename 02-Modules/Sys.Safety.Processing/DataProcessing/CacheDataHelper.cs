using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.DataCollection.Common.Protocols;
using Sys.DataCollection.Common.Protocols.Devices;
using Sys.Safety.DataContract;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.NetworkModule;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.RemoteState;
using Sys.Safety.ServiceContract;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.ServiceContract.KJ237Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basic.Framework.Web;

namespace Sys.Safety.Processing.DataProcessing
{
    public class CacheDataHelper
    {

        /// <summary>
        /// 网络模块缓存RPC接口
        /// </summary>
        private static INetworkModuleCacheService networkModuleCacheService;
        /// <summary>
        /// 测点表缓存RPC接口
        /// </summary>
        private static IPointDefineCacheService pointDefineCacheService;

        private static IPointDefineService pointDefineService;

        /// <summary>
        /// 设备定义缓存RPC接口
        /// </summary>
        private static IDeviceDefineCacheService deviceDefineCacheService;

        private static ISettingCacheService settingCacheService;

        public static IAutomaticArticulatedDeviceCacheService automaticArticulatedDeviceCacheService;

        public static IAllSystemPointDefineService allSystemPointDefineService;

        public static IRemoteStateService remoteStateService;

        static CacheDataHelper()
        {
            networkModuleCacheService = ServiceFactory.Create<INetworkModuleCacheService>();
            pointDefineCacheService = ServiceFactory.Create<IPointDefineCacheService>();
            pointDefineService = ServiceFactory.Create<IPointDefineService>();
            deviceDefineCacheService = ServiceFactory.Create<IDeviceDefineCacheService>();
            settingCacheService = ServiceFactory.Create<ISettingCacheService>();
            automaticArticulatedDeviceCacheService = ServiceFactory.Create<IAutomaticArticulatedDeviceCacheService>();
            allSystemPointDefineService = ServiceFactory.Create<IAllSystemPointDefineService>();
            remoteStateService = ServiceFactory.Create<IRemoteStateService>();
        }

        public static void UpdateInspectionTime(long _InspectionTime)
        {
            UpdateInspectionTimeRequest request = new UpdateInspectionTimeRequest();
            request.InspectionTime = _InspectionTime;
            remoteStateService.UpdateInspectionTime(request);
        }


        /// <summary>
        /// 获取设备定义DEF所有数据
        /// </summary>
        /// <returns></returns>
        public static List<Jc_DefInfo> GetKJPointDefineItems()
        {
            List<Jc_DefInfo> pointDefineItems = new List<Jc_DefInfo>();
            PointDefineCacheGetAllRequest pointDefineCacheGetAllRequest = new PointDefineCacheGetAllRequest();
            pointDefineCacheGetAllRequest.IsQueryFromWriteCache = true;
            var defResponse = pointDefineCacheService.GetAllPointDefineCache(pointDefineCacheGetAllRequest);
            if (defResponse.IsSuccess)
            {
                pointDefineItems = defResponse.Data;
            }

            return pointDefineItems;
        }

        public static List<Jc_DefInfo> GetAllSystemPointDefineItems()
        {
            List<Jc_DefInfo> items = new List<Jc_DefInfo>();

            var result = allSystemPointDefineService.GetAllPointDefineCache();
            if (result.IsSuccess && result.Data != null)
            {
                items = result.Data.FindAll(a => a.Upflag != "1");//增加处理，下发初始化时，不下发从子系统同步的数据（通过upflag=1表示子系统同步的数据）  20180131
            }
            return items;
        }       
        public static BasicResponse QueryDeviceInfoRequest(List<DeviceInfoRequestItem> deviceInfoRequestItems)
        {
            DeviceInfoRequest request = new DeviceInfoRequest();
            request.deviceInfoRequestItems = deviceInfoRequestItems;
            return pointDefineService.QueryDeviceInfoRequest(request);
        }
        /// <summary>
        /// 获取所有分站对象
        /// </summary>
        /// <returns></returns>
        public static List<Jc_DefInfo> GetStationAllSystemPointDefineItems()
        {
            List<Jc_DefInfo> items = new List<Jc_DefInfo>();
            PointDefineGetByDevpropertIDRequest pointDefineRequest = new PointDefineGetByDevpropertIDRequest();
            pointDefineRequest.DevpropertID = 0;
            var result = allSystemPointDefineService.GetPointDefineCacheByDevpropertID(pointDefineRequest);
            if (result.IsSuccess && result.Data != null)
            {
                items = result.Data.FindAll(a => a.Upflag != "1");//增加处理，下发初始化时，不下发从子系统同步的数据（通过upflag=1表示子系统同步的数据）  20180131
            }
            return items;
        }
        /// <summary>
        /// 根据PointId获取测点
        /// </summary>
        /// <param name="PointId"></param>
        /// <returns></returns>
        public static Jc_DefInfo GetAllSystemPointDefineItemByPointId(string PointId)
        {
            Jc_DefInfo item = new Jc_DefInfo();
            PointDefineGetByPointIDRequest PointDefineRequest = new PointDefineGetByPointIDRequest();
            PointDefineRequest.PointID = PointId;
            var result = allSystemPointDefineService.GetPointDefineCacheByPointID(PointDefineRequest);
            if (result.IsSuccess && result.Data != null && result.Data.Count > 0)
            {
                item = result.Data[0];
            }
            return item;
        }

        /// <summary>
        /// 获取所有网络模块数据（JC_MAC）
        /// </summary>
        /// <returns></returns>
        public static List<Jc_MacInfo> GetAllMacItems()
        {

            List<Jc_MacInfo> macItems = new List<Jc_MacInfo>();

            var defResponse = networkModuleCacheService.GetAllNetworkModuleCache(new NetworkModuleCacheGetAllRequest());
            if (defResponse.IsSuccess)
            {
                macItems = defResponse.Data;
            }

            return macItems;
        }

        /// <summary>
        /// 获取所有设备类型dev数据
        /// </summary>
        /// <returns></returns>
        public static List<Jc_DevInfo> GetAllDevItems()
        {

            List<Jc_DevInfo> devItems = new List<Jc_DevInfo>();

            var defResponse = deviceDefineCacheService.GetAllPointDefineCache(new DeviceDefineCacheGetAllRequest());
            if (defResponse.IsSuccess)
            {
                devItems = defResponse.Data;
            }

            return devItems;
        }

        /// <summary>
        /// 更新网络模块指定字段值
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="updateItems"></param>
        public static void UpdateNetworkModeuleCacheByPropertys(string mac, Dictionary<string, object> updateItems)
        {

            NetworkModuleCacheUpdatePropertiesRequest networkModuleCacheUpdatePropertiesRequest = new NetworkModuleCacheUpdatePropertiesRequest();
            networkModuleCacheUpdatePropertiesRequest.Mac = mac;
            networkModuleCacheUpdatePropertiesRequest.UpdateItems = updateItems;
            networkModuleCacheService.UpdateNetworkInfo(networkModuleCacheUpdatePropertiesRequest);
        }

        /// <summary>
        /// 判断需要向交换机下发的命令，  修改支持命令同时发送  20171207
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="macItems"></param>
        /// <returns></returns>
        public static List<MasProtocol> GetMacSendData(string mac, List<Jc_MacInfo> macItems)
        {
            List<MasProtocol> masProtocolList = new List<MasProtocol>();

            try
            {
                Jc_MacInfo macInfo = macItems.FirstOrDefault(a => a.MAC == mac);
                if (macInfo != null)
                {
                    if ((macInfo.NCommandbz & 0x02) == 0x02)
                    {
                        #region ----下发模拟报警----

                        ResetDeviceCommandRequest resetDeviceCommandRequest = new ResetDeviceCommandRequest();
                        resetDeviceCommandRequest.DeviceCode = mac;
                        resetDeviceCommandRequest.LastAcceptFlag = (byte)macInfo.testAlarmFlag;
                        resetDeviceCommandRequest.Mac = mac;

                        MasProtocol masProtocol = null;
                        masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.ResetNetWorkDeviceRequest);
                        masProtocol.Protocol = resetDeviceCommandRequest;
                        masProtocolList.Add(masProtocol);

                        Dictionary<string, object> updateItems = new Dictionary<string, object>();
                        updateItems.Add("sendAlarmCount", macInfo.sendAlarmCount--);
                        if (macInfo.sendAlarmCount <= 0)
                        {
                            updateItems.Add("NCommandbz", (macInfo.NCommandbz & 0xFD));
                        }

                        CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(macInfo.MAC, updateItems);
                        LogHelper.Info(macInfo.MAC + "下发模拟报警，待下发" + macInfo.sendAlarmCount + "次");

                        #endregion
                    }

                    if (((macInfo.NCommandbz & 0x01) == 0x01) || macInfo.SendBatteryControlCount > 0)
                    {
                        #region ----获取智能电源箱数据----
                        if ((DateTime.Now - macInfo.SendDtime).TotalSeconds <= 15)
                        {
                            if (macInfo.Type == 0 && macInfo.Bz4 == "1")//Bz4 = 1表示带智能电源箱
                            {
                                QueryBatteryRealDataRequest queryBatteryRealDataRequest = GetQueryBatteryRealDataRequest(macInfo);

                                MasProtocol masProtocol = null;
                                masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.QueryBatteryRealDataRequest);
                                masProtocol.Protocol = queryBatteryRealDataRequest;
                                masProtocolList.Add(masProtocol);
                            }
                        }
                        #endregion
                    }

                    #region ----人员定位呼叫命令(屏蔽  20180307) ----
                    //R_CallInfo tempCallInfo;
                    //string[] startAndEnd;
                    //int tempInt = 0;
                    //string tempStr;
                    //List<R_CallInfo> callItems = KJ237CacheHelper.GetCallItems().Where(a => a.CallType != 2).OrderBy(a => a.CallTime).ToList();
                    //if (callItems.Count > 0)//下发紧急呼叫及一般呼叫  20171206
                    //{
                    //    //先查找人员的全员呼叫命令并下发(呼叫所有设备与呼叫所有人员为同一命令)（优先级最高)
                    //    List<R_CallInfo> sendCallList = callItems.FindAll(a => a.CallType == 1 && (a.CallPersonDefType == 0 || a.CallPersonDefType == 3));
                    //    if (sendCallList.Count > 0)//全员呼叫下发
                    //    {
                    //        if (sendCallList[0].SendCount > 0)//判断是否还需要下发  20171213
                    //        {
                    //            CallPersonRequest request = new CallPersonRequest();
                    //            request.HJ_Type = 0;
                    //            request.HJ_State = 2;
                    //            request.DeviceCode = mac;
                    //            request.HJ_KH = new ushort[12];

                    //            MasProtocol masProtocol = null;
                    //            masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //            masProtocol.Protocol = request;


                    //            masProtocolList.Add(masProtocol);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        #region//紧急呼叫井下设备
                    //        List<R_CallInfo> sendCallListByPoint = callItems.FindAll(a => a.CallType == 1 && (a.CallPersonDefType == 4));
                    //        if (sendCallListByPoint.Count > 0)
                    //        {
                    //            if (sendCallListByPoint[0].SendCount > 0)//判断是否还需要下发  20171213
                    //            {
                    //                CallPersonRequest request = new CallPersonRequest();
                    //                request.HJ_Type = 4;
                    //                request.HJ_State = 2;
                    //                request.DeviceCode = mac;
                    //                List<ushort> callContent = new List<ushort>();
                    //                foreach (R_CallInfo call in sendCallListByPoint)
                    //                {
                    //                    string[] callList = call.PointList.Split(',');
                    //                    foreach (string tempCall in callList)
                    //                    {
                    //                        callContent.Add(Convert.ToUInt16(((Convert.ToInt64(tempCall.Substring(0, 3)) << 8)) + Convert.ToInt64(tempCall.Substring(4, 2))));
                    //                        if (callContent.Count == 12)
                    //                        {
                    //                            request.HJ_KH = callContent.ToArray();
                    //                            MasProtocol masProtocol = null;
                    //                            masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                            masProtocol.Protocol = request;
                    //                            masProtocolList.Add(masProtocol);

                    //                            callContent = new List<ushort>();
                    //                        }
                    //                    }
                    //                }
                    //                if (callContent.Count > 0)
                    //                {
                    //                    request.HJ_KH = new ushort[12];
                    //                    for (int i = 0; i < callContent.Count; i++)
                    //                    {
                    //                        request.HJ_KH[i] = callContent[i];
                    //                    }
                    //                    MasProtocol masProtocol = null;
                    //                    masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                    masProtocol.Protocol = request;
                    //                    masProtocolList.Add(masProtocol);
                    //                }
                    //            }
                    //        }
                    //        #endregion
                    //        #region//紧急呼叫指定人员及指定卡号段下发,卡号段也按指定人员下发
                    //        List<R_CallInfo> sendCallListByPerson = callItems.FindAll(a => a.CallType == 1 && (a.CallPersonDefType == 1 || a.CallPersonDefType == 2));
                    //        if (sendCallListByPerson.Count > 0)
                    //        {
                    //            if (sendCallListByPerson[0].SendCount > 0)//判断是否还需要下发  20171213
                    //            {
                    //                CallPersonRequest request = new CallPersonRequest();
                    //                request.HJ_Type = 2;
                    //                request.HJ_State = 2;
                    //                request.DeviceCode = mac;
                    //                List<ushort> callContent = new List<ushort>();
                    //                foreach (R_CallInfo call in sendCallListByPerson)
                    //                {
                    //                    if (call.BhContent.Contains("-"))
                    //                    {//指定卡号段呼叫
                    //                        ushort sBh = ushort.Parse(call.BhContent.Split('-')[0]);
                    //                        ushort eBh = ushort.Parse(call.BhContent.Split('-')[1]);
                    //                        for (ushort i = sBh; i <= eBh; i++)
                    //                        {
                    //                            callContent.Add(i);
                    //                            if (callContent.Count == 12)
                    //                            {
                    //                                request.HJ_KH = callContent.ToArray();
                    //                                MasProtocol masProtocol = null;
                    //                                masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                                masProtocol.Protocol = request;
                    //                                masProtocolList.Add(masProtocol);

                    //                                callContent = new List<ushort>();
                    //                            }
                    //                        }
                    //                    }
                    //                    else//指定卡号呼叫
                    //                    {
                    //                        string[] callList = call.BhContent.Split(',');
                    //                        foreach (string tempCall in callList)
                    //                        {
                    //                            callContent.Add(ushort.Parse(tempCall));
                    //                            if (callContent.Count == 12)
                    //                            {
                    //                                request.HJ_KH = callContent.ToArray();
                    //                                MasProtocol masProtocol = null;
                    //                                masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                                masProtocol.Protocol = request;
                    //                                masProtocolList.Add(masProtocol);

                    //                                callContent = new List<ushort>();
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //                if (callContent.Count > 0)
                    //                {
                    //                    request.HJ_KH = new ushort[12];
                    //                    for (int i = 0; i < callContent.Count; i++)
                    //                    {
                    //                        request.HJ_KH[i] = callContent[i];
                    //                    }
                    //                    MasProtocol masProtocol = null;
                    //                    masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                    masProtocol.Protocol = request;
                    //                    masProtocolList.Add(masProtocol);
                    //                }
                    //            }
                    //        }
                    //        #endregion

                    //        if (sendCallListByPerson.Count == 0 && sendCallListByPoint.Count == 0)
                    //        {//如果不存在紧急呼叫，则判断一般呼叫并下发
                    //            List<R_CallInfo> sendGeneralCallListAll = callItems.FindAll(a => a.CallType == 0 && (a.CallPersonDefType == 0 || a.CallPersonDefType == 3));
                    //            if (sendGeneralCallListAll.Count > 0)//全员呼叫下发
                    //            {
                    //                if (sendGeneralCallListAll[0].SendCount > 0)//判断是否还需要下发  20171213
                    //                {
                    //                    CallPersonRequest request = new CallPersonRequest();
                    //                    request.HJ_Type = 0;
                    //                    request.HJ_State = 1;
                    //                    request.DeviceCode = mac;
                    //                    request.HJ_KH = new ushort[12];

                    //                    MasProtocol masProtocol = null;
                    //                    masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                    masProtocol.Protocol = request;


                    //                    masProtocolList.Add(masProtocol);
                    //                }
                    //            }
                    //            #region//一般呼叫井下设备
                    //            List<R_CallInfo> sendGeneralCallListByPoint = callItems.FindAll(a => a.CallType == 0 && (a.CallPersonDefType == 4));
                    //            if (sendGeneralCallListByPoint.Count > 0)
                    //            {
                    //                if (sendGeneralCallListByPoint[0].SendCount > 0)//判断是否还需要下发  20171213
                    //                {
                    //                    CallPersonRequest request = new CallPersonRequest();
                    //                    request.HJ_Type = 4;
                    //                    request.HJ_State = 1;
                    //                    request.DeviceCode = mac;
                    //                    List<ushort> callContent = new List<ushort>();
                    //                    foreach (R_CallInfo call in sendGeneralCallListByPoint)
                    //                    {
                    //                        string[] callList = call.PointList.Split(',');
                    //                        foreach (string tempCall in callList)
                    //                        {
                    //                            callContent.Add(Convert.ToUInt16(((Convert.ToInt64(tempCall.Substring(0, 3)) << 8)) + Convert.ToInt64(tempCall.Substring(4, 2))));
                    //                            if (callContent.Count == 12)
                    //                            {
                    //                                request.HJ_KH = callContent.ToArray();
                    //                                MasProtocol masProtocol = null;
                    //                                masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                                masProtocol.Protocol = request;
                    //                                masProtocolList.Add(masProtocol);

                    //                                callContent = new List<ushort>();
                    //                            }
                    //                        }
                    //                    }
                    //                    if (callContent.Count > 0)
                    //                    {
                    //                        request.HJ_KH = new ushort[12];
                    //                        for (int i = 0; i < callContent.Count; i++)
                    //                        {
                    //                            request.HJ_KH[i] = callContent[i];
                    //                        }
                    //                        MasProtocol masProtocol = null;
                    //                        masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                        masProtocol.Protocol = request;
                    //                        masProtocolList.Add(masProtocol);
                    //                    }
                    //                }
                    //            }
                    //            #endregion
                    //            #region//一般呼叫指定人员及指定卡号段下发,卡号段也按指定人员下发
                    //            List<R_CallInfo> sendGeneralCallListByPerson = callItems.FindAll(a => a.CallType == 0 && (a.CallPersonDefType == 1 || a.CallPersonDefType == 2));
                    //            if (sendGeneralCallListByPerson.Count > 0)
                    //            {
                    //                if (sendGeneralCallListByPerson[0].SendCount > 0)//判断是否还需要下发  20171213
                    //                {
                    //                    CallPersonRequest request = new CallPersonRequest();
                    //                    request.HJ_Type = 2;
                    //                    request.HJ_State = 1;
                    //                    request.DeviceCode = mac;
                    //                    List<ushort> callContent = new List<ushort>();
                    //                    foreach (R_CallInfo call in sendGeneralCallListByPerson)
                    //                    {
                    //                        if (call.BhContent.Contains("-"))
                    //                        {//指定卡号段呼叫
                    //                            ushort sBh = ushort.Parse(call.BhContent.Split('-')[0]);
                    //                            ushort eBh = ushort.Parse(call.BhContent.Split('-')[1]);
                    //                            for (ushort i = sBh; i <= eBh; i++)
                    //                            {
                    //                                callContent.Add(i);
                    //                                if (callContent.Count == 12)
                    //                                {
                    //                                    request.HJ_KH = callContent.ToArray();
                    //                                    MasProtocol masProtocol = null;
                    //                                    masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                                    masProtocol.Protocol = request;
                    //                                    masProtocolList.Add(masProtocol);

                    //                                    callContent = new List<ushort>();
                    //                                }
                    //                            }
                    //                        }
                    //                        else//指定卡号呼叫
                    //                        {
                    //                            string[] callList = call.BhContent.Split(',');
                    //                            foreach (string tempCall in callList)
                    //                            {
                    //                                callContent.Add(ushort.Parse(tempCall));
                    //                                if (callContent.Count == 12)
                    //                                {
                    //                                    request.HJ_KH = callContent.ToArray();
                    //                                    MasProtocol masProtocol = null;
                    //                                    masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                                    masProtocol.Protocol = request;
                    //                                    masProtocolList.Add(masProtocol);

                    //                                    callContent = new List<ushort>();
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                    if (callContent.Count > 0)
                    //                    {
                    //                        request.HJ_KH = new ushort[12];
                    //                        for (int i = 0; i < callContent.Count; i++)
                    //                        {
                    //                            request.HJ_KH[i] = callContent[i];
                    //                        }
                    //                        MasProtocol masProtocol = null;
                    //                        masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //                        masProtocol.Protocol = request;
                    //                        masProtocolList.Add(masProtocol);
                    //                    }
                    //                }
                    //            }
                    //            #endregion
                    //        }
                    //    }

                    //}
                    //else//如果不存在紧急呼叫和一般呼叫命令则下发解除呼叫命令  20171206
                    //{
                    //    callItems = KJ237CacheHelper.GetCallItems().Where(a => a.CallType == 2).OrderBy(a => a.CallTime).ToList();
                    //    if (callItems.Count > 0)
                    //    {
                    //        CallPersonRequest request = new CallPersonRequest();
                    //        request.HJ_Type = 0;
                    //        request.HJ_State = 0;
                    //        request.DeviceCode = mac;
                    //        request.HJ_KH = new ushort[12];

                    //        MasProtocol masProtocol = null;
                    //        masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Down, ProtocolType.CallPersonRequest);
                    //        masProtocol.Protocol = request;

                    //        masProtocolList.Add(masProtocol);
                    //    }
                    //}
                    #endregion

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取向交换机下发的命令失败，原因：" + ex);
            }

            return masProtocolList;
        }

        private static QueryBatteryRealDataRequest GetQueryBatteryRealDataRequest(Jc_MacInfo macInfo)
        {
            QueryBatteryRealDataRequest queryBatteryRealDataRequest = new QueryBatteryRealDataRequest();
            try
            {
                queryBatteryRealDataRequest.DeviceCode = macInfo.MAC;
                queryBatteryRealDataRequest.BatteryControl = macInfo.BatteryControl;
                queryBatteryRealDataRequest.DevProperty = ItemDevProperty.Switches;
                queryBatteryRealDataRequest.PowerPercentum = Convert.ToByte(GetSettingByKeyStr("DevicePowerPercentum").StrValue);
            }
            catch (Exception ex)
            {
                queryBatteryRealDataRequest.PowerPercentum = 20;
                LogHelper.Error("配置项【DevicePowerPercentum】值错误！" + ex.Message);
            }
            queryBatteryRealDataRequest.DevProperty = ItemDevProperty.Switches;
            queryBatteryRealDataRequest.BatteryControl = macInfo.BatteryControl;
            return queryBatteryRealDataRequest;
        }

        /// <summary>
        /// 更新JC_DEF的部分属性
        /// </summary>
        /// <param name="point">分站测点号</param>
        /// <param name="paramater">key = 属性名称(区分大小写),value = 值</param>
        public static void UpdatePointDefineInfoByProperties(string pointID, Dictionary<string, object> paramater)
        {

            DefineCacheUpdatePropertiesRequest defineCacheUpdatePropertiesRequest = new DefineCacheUpdatePropertiesRequest();
            defineCacheUpdatePropertiesRequest.PointID = pointID;
            defineCacheUpdatePropertiesRequest.UpdateItems = paramater;
            pointDefineCacheService.UpdatePointDefineInfo(defineCacheUpdatePropertiesRequest);
        }

        /// <summary>
        /// 根据主键获取配置信息
        /// </summary>
        /// <param name="StrKey"></param>
        /// <returns></returns>
        public static SettingInfo GetSettingByKeyStr(string StrKey)
        {

            SettingCacheGetByKeyRequest settingCacheGetByKeyRequest = new SettingCacheGetByKeyRequest();
            settingCacheGetByKeyRequest.StrKey = StrKey;
            var result = settingCacheService.GetSettingCacheByKey(settingCacheGetByKeyRequest);
            if (result.Data != null && result.IsSuccess)
            {
                return result.Data;
            }

            return null;
        }

        /// <summary>
        /// 查询所有未定义测点
        /// </summary>
        /// <returns></returns>
        public static List<AutomaticArticulatedDeviceInfo> GetAllUnDefinePoint()
        {

            List<AutomaticArticulatedDeviceInfo> unDefineItems = new List<AutomaticArticulatedDeviceInfo>();
            AutomaticArticulatedDeviceCacheGetByConditionRequest automaticArticulatedDeviceCacheGetByConditionRequest = new AutomaticArticulatedDeviceCacheGetByConditionRequest();
            automaticArticulatedDeviceCacheGetByConditionRequest.Pridicate = a => 1 == 1;
            var result = automaticArticulatedDeviceCacheService.GetAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheGetByConditionRequest);
            if (result.IsSuccess)
            {
                unDefineItems = result.Data;
            }
            return unDefineItems;
        }
        /// <summary>
        /// 从未定义缓存移除未定义设备
        /// </summary>
        /// <param name="unDefineItems"></param>
        public static void RemoveUnDefineItems(List<AutomaticArticulatedDeviceInfo> unDefineItems)
        {

            AutomaticArticulatedDeviceCacheDeleteRequest automaticArticulatedDeviceCacheDeleteRequest;
            unDefineItems.ForEach(item =>
            {
                automaticArticulatedDeviceCacheDeleteRequest = new AutomaticArticulatedDeviceCacheDeleteRequest();
                automaticArticulatedDeviceCacheDeleteRequest.AutomaticArticulatedDeviceInfo = item;
                automaticArticulatedDeviceCacheService.DeleteAutomaticArticulatedDeviceCache(automaticArticulatedDeviceCacheDeleteRequest);
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="controlType">0不进行操作，1取消维护性放电，2维护性放电</param>
        public static void SendPowerControl(List<ushort> fzhs, List<byte> controls)
        {
            try
            {
                StationDControlRequest request = new StationDControlRequest();
                request.controlItems = new List<StationControlItem>();
                StationControlItem controlItem;
                for (int i = 0; i < fzhs.Count; i++)
                {
                    controlItem = new StationControlItem();
                    controlItem.fzh = fzhs[i];
                    controlItem.controlType = controls[i];
                    request.controlItems.Add(controlItem);
                }
                pointDefineService.SendStationDControl(request);
            }
            catch (Exception ex)
            {
                LogHelper.Error("SendPowerControl Error:" + ex.Message);
            }
        }
    }
}
