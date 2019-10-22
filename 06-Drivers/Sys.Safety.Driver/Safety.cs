using Sys.Safety.Interface;
using Basic.Framework.Logging;
using Basic.Framework.Service;
using Sys.DataCollection.Common.Protocols;
using Sys.DataCollection.Common.Protocols.Devices;
using Sys.Safety.DataContract;
using Sys.Safety.DataContract.CommunicateExtend;

using Sys.Safety.Driver.RealDataHandle;
using Sys.Safety.Enums;
using Sys.Safety.ClientFramework.CBFCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using Sys.Safety.Model;
using System.Threading;
using System.Collections.Concurrent;


namespace Sys.Safety.Driver
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-26
    /// 描述:数据处理驱动
    /// 修改记录：
    /// 2017-06-05
    /// </summary>
    public class Safety : IDriver
    {
        /// <summary>
        ///上次处理的分站号
        /// </summary>
        string preInfoPoint = "";
        /// <summary>
        /// 容错次数倍数
        /// </summary>
        int ErrorMul = 1;

        /*  电源箱地址号对应关系如下：
            30表示设备地址为250的24（B）电源箱唯一编码；
            29表示设备地址为249的25（B）电池箱唯一编码；
            28表示设备地址为248的25（B）电池箱唯一编码；
            27表示设备地址为247的25（B）电池箱唯一编码；
            26表示设备地址为246的25（B）电池箱唯一编码
         */

        public event DriverEventHandler OnDriverSendDataEventHandler;
        /// <summary>
        /// 历史数据处理线程
        /// </summary>
        private Thread HisDataProThread = null;
        /// <summary>
        /// 历史数据异步处理队列
        /// </summary>
        private ConcurrentQueue<StaionHistoryDataInfo> updateStaionHistoryDataItems = new ConcurrentQueue<StaionHistoryDataInfo>();

        ///// <summary>
        ///// 定义委托
        ///// </summary>
        ///// <param name="objFrm"></param>
        //public delegate void ShowToMainForm(Form objFrm, RequestInfo dto, bool isSystemDesktop);
        ///// <summary>
        ///// 添加窗体触发
        ///// </summary>
        //public static ShowToMainForm OnShowToMainForm;

        #region 属性定义
        private string driverName = "安全监控系统";
        /// <summary>
        /// 驱动名称
        /// </summary>
        public string Drv_StrDriverName
        {
            get
            {
                return driverName;
            }
        }

        private string driverSource = "Sys.Safety.Driver.dll";
        /// <summary>
        /// 驱动源文件名称
        /// </summary>
        public string Drv_StrDriverSource
        {
            get
            {
                return driverSource;
            }
        }

        private string driverVersion = "1.0.0.0";
        /// <summary>
        /// 驱动版本号
        /// </summary>
        public string Drv_StrDriverVersion
        {
            get
            {
                return driverVersion;
            }
        }

        private decimal driverID = (int)SystemEnum.Security;
        /// <summary>
        /// 驱动编号
        /// </summary>
        public decimal Drv_ID
        {
            get
            {
                return this.driverID;
            }
        }

        private DateTime driverUpdateTime = DateTime.Parse("2017-06-14");
        /// <summary>
        /// 驱动更新时间
        /// </summary>
        public DateTime Drv_DttDriverVersionTime
        {
            get
            {
                return driverUpdateTime;
            }
        }

        #endregion

        #region 驱动接口

        /// <summary>
        /// 获取下发数据
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <returns></returns>
        public void GetSendData(string stationID, Jc_DevInfo devInfo, ref int sendTime)
        {
            MasProtocol masProtocol = null;
            //List<Jc_DefInfo> JCDEFItems = SafetyHelper.GetPointDefinesByStationID(stationID);//hdw:20170719
            Jc_DefInfo stationItem = SafetyHelper.GetPointDefinesByPoint(stationID);
            //Jc_DefInfo stationItem = JCDEFItems.FirstOrDefault(a => a.Fzh == stationID && a.DevPropertyID == (int)DeviceProperty.Substation);
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            if (stationItem != null)
            {
                try
                {

                    #region 下发命令判断
                    sendTime = 500;
                    //如果没有初始化,则下发初始化命令（开机、修改等状态）
                    if (!stationItem.ClsCommObj.BInit)
                    {
                        ////初始化命令
                        //if (stationItem.Bz19 == "1")
                        //{
                        //    //才下发初始化
                        //    SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendInitializeRequest");
                        //    masProtocol = GetInitializeRequest(stationItem);
                        //}
                        //else
                        //{
                        //    //更新分站内存为已下发初始化
                        //    stationItem.ClsCommObj.BInit = true;
                        //    updateItems.Add("ClsCommObj", stationItem.ClsCommObj);
                        //    //不下发初始化时，直接下发F命令
                        //    SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】不需要下发初始化，直接SendQueryRealDataRequest(" + stationItem .GetDeviceSoleCoding+ ")");
                        //    masProtocol = GetQueryRealDataRequest(stationItem);
                        //}

                        //SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendInitializeRequest");
                        //masProtocol = GetInitializeRequest(stationItem);
                        //sendTime = 2500;

                        //修改为发F命令
                        SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendQueryRealDataRequest(" + stationItem.GetDeviceSoleCoding + ")");
                        masProtocol = GetQueryRealDataRequest(stationItem);
                        sendTime = 850;
                    }
                    else
                    {
                        #region 按命令优先级获取本次发送命令
                        if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_InitializeRequest) == CommunicationCommandValue.Comm_InitializeRequest || stationItem.sendIniCount > 0)
                        {
                            //初始化命令
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendInitializeRequest");
                            //stationItem.ClsCommObj.BInit = true;
                            masProtocol = GetInitializeRequest(stationItem);
                            //updateItems.Add("ClsCommObj", stationItem.ClsCommObj);  
                            sendTime = 2500;
                        }
                        else if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_DeviceControlRequest) == CommunicationCommandValue.Comm_DeviceControlRequest || stationItem.realControlCount > 0)
                        {
                            //控制命令
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendDeviceControlRequest");
                            masProtocol = GetDeviceControlRequest(stationItem);
                            sendTime = 850;
                        }
                        else if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_SetSensorGradingAlarmRequest) == CommunicationCommandValue.Comm_SetSensorGradingAlarmRequest)
                        {
                            //分级报警
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SetSensorGradingAlarmRequest");
                            masProtocol = SetSensorGradingAlarmRequest(stationItem);
                            LogHelper.Debug("下发分级报警：" + DateTime.Now + ":" + stationItem.GradingAlarmTime);
                            if ((DateTime.Now - stationItem.GradingAlarmTime).TotalSeconds > 15)   //2018.3.13 by
                            {
                                stationItem.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_SetSensorGradingAlarmRequest);  //置下发分级报警成功标记 不再下发分级报警命令
                                stationItem.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;  //置下发F命令标记
                                stationItem.IsSendFCommand = false;

                                masProtocol = GetQueryRealDataRequest(stationItem);
                                updateItems.Add("IsSendFCommand", stationItem.IsSendFCommand);
                                updateItems.Add("ClsCommObj", stationItem.ClsCommObj);
                            }
                            sendTime = 500;
                        }
                        else if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryBatteryRealDataRequest) == CommunicationCommandValue.Comm_QueryBatteryRealDataRequest)
                        {
                            //D命令(获取电源箱信息)
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendQueryBatteryRealDataRequest");
                            masProtocol = GetQueryBatteryRealDataRequest(stationItem);
                            if ((DateTime.Now - stationItem.sendDTime).TotalSeconds > 10)
                            {
                                stationItem.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_QueryBatteryRealDataRequest);  //置获取电源箱数据成功标记 不再下发D命令
                                stationItem.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;  //置下发F命令标记
                                masProtocol = GetQueryRealDataRequest(stationItem);
                                updateItems.Add("ClsCommObj", stationItem.ClsCommObj);
                            }
                            sendTime = 500;
                        }
                        else if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_ModificationDeviceAddressRequest) == CommunicationCommandValue.Comm_ModificationDeviceAddressRequest)
                        {
                            //修改传感器地址号
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendModificationDeviceAddressRequest");
                            masProtocol = GetModificationDeviceAddressRequest(stationItem);
                            sendTime = 500;
                        }
                        else if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryDeviceInfoRequest) == CommunicationCommandValue.Comm_QueryDeviceInfoRequest)
                        {
                            //获取设备基础信息
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendQueryDeviceInfoRequest");
                            masProtocol = GetQueryDeviceInfoRequest(stationItem);
                            if ((DateTime.Now - stationItem.sendDTime).TotalSeconds > 15)   //2017.8.11 by
                            {
                                stationItem.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_QueryDeviceInfoRequest);  //置获取电源箱数据成功标记 不再下发D命令
                                stationItem.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;  //置下发F命令标记
                                stationItem.IsSendFCommand = false;

                                masProtocol = GetQueryRealDataRequest(stationItem);
                                updateItems.Add("IsSendFCommand", stationItem.IsSendFCommand);
                                updateItems.Add("ClsCommObj", stationItem.ClsCommObj);
                            }
                            sendTime = 850;
                        }
                        else if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryHistoryControlRequest) == CommunicationCommandValue.Comm_QueryHistoryControlRequest)
                        {
                            //获取分站历史控制命令数据
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendQueryHistoryControlRequest");
                            masProtocol = GetQueryHistoryControlRequest(stationItem);
                            sendTime = 850;
                        }
                        else if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryHistoryRealDataRequest) == CommunicationCommandValue.Comm_QueryHistoryRealDataRequest)
                        {
                            //获取分站历史五分钟数据
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendQueryHistoryRealDataRequest");
                            masProtocol = GetQueryHistoryRealDataRequest(stationItem);
                            sendTime = 850;
                        }
                        else if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_ResetDeviceCommandRequest) == CommunicationCommandValue.Comm_ResetDeviceCommandRequest)
                        {
                            //R命令  
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendResetDeviceCommandRequest");
                            masProtocol = GetResetDeviceCommandRequest(stationItem);
                            sendTime = 500;
                        }
                        else if ((stationItem.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryRealDataRequest) == CommunicationCommandValue.Comm_QueryRealDataRequest)
                        {
                            //if (stationItem.GetDeviceSoleCoding == 1 && (DateTime.Now - stationItem.GetDeviceSoleCodingTime).TotalSeconds > 20)
                            //{
                            //    //获取唯一编码20秒都没有回复，不再继续获取，切换为正常的F命令 2018.329 by
                            //    stationItem.GetDeviceSoleCoding = 0;
                            //    stationItem.IsSendFCommand = true;
                            //    updateItems.Add("GetDeviceSoleCoding", stationItem.GetDeviceSoleCoding);
                            //    updateItems.Add("IsSendFCommand", stationItem.IsSendFCommand);
                            //}
                            //F命令  取数
                            SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】SendQueryRealDataRequest(" + stationItem.GetDeviceSoleCoding + ")");
                            masProtocol = GetQueryRealDataRequest(stationItem);
                            sendTime = 850;
                        }
                        else
                        {
                            #region ----远程升级相关命令----
                            StationUpdateItem stationUpdateItem = SafetyHelper.GetStationItem((int)stationItem.Fzh);
                            if (stationUpdateItem != null)
                            {
                                if (stationUpdateItem.isSendBuffer)
                                {
                                    SafetyHelper.WriteLogInfo("【" + stationItem.Point + "】下发远程升级命令：" + stationUpdateItem.protocolType.ToString());
                                    masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, stationUpdateItem.protocolType);
                                    masProtocol.Protocol = stationUpdateItem.protocol;
                                }
                            }
                            #endregion
                        }
                        #endregion
                        if (updateItems.Count > 0)
                        {
                            SafetyHelper.UpdatePointDefineInfoByProperties(stationItem.PointID, updateItems);
                        }
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    LogHelper.Error("获取下发命令出错，原因：" + ex.Message + ex.StackTrace + ex.StackTrace);
                }
            }

            if (masProtocol != null)
            {
                if (OnDriverSendDataEventHandler != null)
                {
                    OnDriverSendDataEventHandler(masProtocol);
                }
            }
        }

        /// <summary>
        /// 数据解析
        /// </summary>
        /// <param name="data">分站号</param>
        public void DataProc(MasProtocol data)
        {
            if (HisDataProThread == null)
            {
                HisDataProThread = new Thread(HisDataProc);
                HisDataProThread.IsBackground = true;
                HisDataProThread.Start();
            }
            DeviceProtocol deviceProtocol = data.Deserialize<DeviceProtocol>();
            switch (data.ProtocolType)
            {
                ////通讯测试命令回发
                //case ProtocolType.CommunicationTestResponse:
                //    CommunicationTestResponseProc(data);
                //    break;
                //控制命令回复
                case ProtocolType.DeviceControlResponse:
                    DeviceControlResponseProc(data);
                    break;
                //设备请求初始化
                case ProtocolType.DeviceInitializeRequest:
                    DeviceInitializeRequestProc(data);
                    break;
                //初始化回发
                case ProtocolType.InitializeResponse:
                    InitializeResponseProc(data);
                    break;
                //  电源箱实时数据
                case ProtocolType.QueryBatteryRealDataResponse:
                    QueryBatteryRealDataResponseProc(data);
                    break;
                //设备基础信息处理
                case ProtocolType.QueryDeviceInfoResponse:
                    QueryDeviceInfoResponseProc(data);
                    break;
                //实时数据回发
                case ProtocolType.QueryRealDataResponse:
                    QueryRealDataResponseProc(data);
                    break;
                //复位命令回复
                case ProtocolType.ResetDeviceCommandResponse:
                    ResetDeviceCommandResponseProc(data);
                    break;
                //历史控制数据处理
                case ProtocolType.QueryHistoryControlResponse:
                    QueryHistoryControlResponseProc(data);
                    break;
                //历史五分钟数据处理
                case ProtocolType.QueryHistoryRealDataResponse:
                    QueryHistoryRealDataResponseProc(data);
                    break;
                case ProtocolType.ModificationDeviceAdressResponse:
                    ModificationDeviceAddressResponseProc(data);
                    break;
                case ProtocolType.SetSensorGradingAlarmResponse:
                    SetSensorGradingAlarmResponseProc(data);
                    break;
                case ProtocolType.GetStationUpdateStateResponse://获取分站的工作状态(设备->上位机)----                                              
                case ProtocolType.InspectionResponse:   //巡检单台分站的文件接收情况回复(设备->上位机)
                case ProtocolType.ReductionResponse:    //远程还原最近一次备份程序(设备->上位机)
                case ProtocolType.RestartResponse:      //通知分站进行重启升级回复(设备->上位机)
                case ProtocolType.StationUpdateResponse:    //设备请求升级回复(设备->上位机)
                case ProtocolType.UpdateCancleResponse:     //异常中止升级流程(设备->上位机)
                    StationUpdateBusiness.Handle(data);
                    break;
            }
            //2018.8.20 修改为根据容错次数判断设备中断
            if (deviceProtocol != null)
            {
                string point = deviceProtocol.DeviceCode;
                Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
                if (station != null)
                {
                    station.NErrCount = 0;
                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("NErrCount", station.NErrCount);
                    //如果收到数据，将容错次数改回定义时的容错次数
                    int ErrCount = (station.K4 == 0 ? 5 : station.K4);
                    if (station.StationFaultCount != ErrCount)
                    {
                        updateItems.Add("StationFaultCount", ErrCount);
                    }
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                }
                else
                {
                    LogHelper.Error("DataProc 未找到设备" + point + " " + deviceProtocol);
                }
            }
        }

        /// <summary>
        /// 设备中断处理
        /// </summary>
        /// <param name="stationID">分站号</param>
        public void Drv_InterruptPro(short stationID)
        {
            //if (stationID > 0)
            //{
            //    KJ73NHelper.DeviceInterruptPro(stationID, DateTime.Now, DeviceRunState.EquipmentInterrupted);
            //}
            if (stationID > 0)
            {
                //2018.2.8 by  设备中断时，结束下级传感器的报警时间、写本分站的通讯中断时间均写最后一次采集到数据的真实时间
                Jc_DefInfo stationInfo = SafetyHelper.GetPointDefinesByPoint(stationID.ToString().PadLeft(3, '0') + "0000");

                if (stationInfo != null)
                {
                    int ErrCount = (stationInfo.K4 == 0 ? 5 : stationInfo.K4);
                    ErrCount = ErrCount * ErrorMul;

                    if (stationInfo.NErrCount >= ErrCount && stationInfo.NErrCount >= stationInfo.StationFaultCount)//2018.8.20 修改为根据容错次数判断设备中断
                    {
                        DateTime time = stationInfo.DttStateTime > new DateTime(2000, 1, 1) ? stationInfo.DttStateTime : DateTime.Now;
                        SafetyHelper.DeviceInterruptPro(stationID, time, DeviceRunState.EquipmentInterrupted);

                        //将上一帧接收正确标记置成0
                        Dictionary<string, object> updateItems = new Dictionary<string, object>();
                        stationInfo.LastAcceptFlag = 0;
                        updateItems.Add("LastAcceptFlag", stationInfo.LastAcceptFlag);
                        SafetyHelper.UpdatePointDefineInfoByProperties(stationInfo.PointID, updateItems);
                    }
                    else
                    {
                        stationInfo.NErrCount += 1;
                        Dictionary<string, object> updateItems = new Dictionary<string, object>();
                        updateItems.Add("NErrCount", stationInfo.NErrCount);
                        SafetyHelper.UpdatePointDefineInfoByProperties(stationInfo.PointID, updateItems);
                    }
                }
            }
        }

        /// <summary>
        /// 五分钟数据处理
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <param name="time">当前时间</param>
        public void Drv_FiveMinPro(short stationID, DateTime time)
        {
            try
            {
                List<Jc_DefInfo> pointDefineItems = SafetyHelper.GetPointDefinesByStationID(stationID);
                Jc_DefInfo stationInfo = pointDefineItems.FirstOrDefault(a => a.DevPropertyID == (int)ItemDevProperty.Substation);
                Dictionary<string, Dictionary<string, object>> UpdateItemsList = new Dictionary<string, Dictionary<string, object>>();//需要更新缓存的数据集合。
                Dictionary<string, object> upitem = new Dictionary<string, object>();//每个测点要更新的缓存对像
                if (stationInfo != null)
                {
                    //分站不休眠时，处理5分钟数据
                    if ((stationInfo.Bz4 & 0x02) != 0x02)
                    {
                        Jc_DefInfo analogInfo;
                        for (int j = 0; j < pointDefineItems.Count; j++)
                        {
                            analogInfo = pointDefineItems[j];
                            //5分钟数据只处理模拟量
                            if (analogInfo.DevPropertyID == (int)ItemDevProperty.Analog && analogInfo.ClsFiveMinObj != null)
                            {
                                //测点不休眠时写五分钟记录(设备异常不写JC_M记录)
                                if (((analogInfo.Bz4 & 0x02) != 0x02) && (analogInfo.ClsFiveMinObj.m_nAllCount > 0))
                                {
                                    upitem = new Dictionary<string, object>();
                                    FiveMinBusiness.CreateFiviMinInfo(pointDefineItems[j], DateTime.Now);
                                    pointDefineItems[j].ClsFiveMinObj.m_nAllCount = 0; //重新计数
                                    upitem.Add("ClsFiveMinObj", pointDefineItems[j].ClsFiveMinObj);
                                    upitem.Add("DoFiveMinData", true);
                                    UpdateItemsList.Add(analogInfo.PointID, upitem);
                                    //FiveMinBusiness.CreateFiviMinInfo(analogInfo, time);
                                }
                                //清空五分钟累计次数，重新计数
                                //analogInfo.ClsFiveMinObj.m_nAllCount = 0;
                                //updateItems = new Dictionary<string, object>();
                                //updateItems.Add("ClsFiveMinObj", analogInfo.ClsFiveMinObj);
                                //SafetyHelper.UpdatePointDefineInfoByProperties(analogInfo.PointID, updateItems);
                            }
                        }
                        SafetyHelper.BatchUpdatePointDefineInfoByProperties(UpdateItemsList);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Drv_FiveMinPro Error" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 跨天处理
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <param name="today">今天跨天时间（一般为当天23:59:59）</param>
        /// <param name="tomorrow">跨天生成新记录时间（一般为第二天00:00:00）</param>
        public void Drv_CrossDayPro(short stationID, DateTime today, DateTime tomorrow)
        {
            List<Jc_DefInfo> pointDefineItems = SafetyHelper.GetPointDefinesByStationID(stationID);
            List<Jc_DefInfo> tempItems;
            Jc_DefInfo defInfo = pointDefineItems.FirstOrDefault(a => a.DevPropertyID == (int)ItemDevProperty.Substation);
            if (defInfo != null)
            {
                #region ----跨天处理（休眠设备跨天不进行处理）----

                //分站处理
                AlarmBusiness.ReStartSubstationAlarm(defInfo, today, tomorrow);
                //控制量处理  
                tempItems = pointDefineItems.Where(a => a.DevPropertyID == (int)ItemDevProperty.Control).ToList();
                AlarmBusiness.ReStartControlAlarm(tempItems, today, tomorrow);
                //模拟量处理
                tempItems = pointDefineItems.Where(a => a.DevPropertyID == (int)ItemDevProperty.Analog).ToList();
                AlarmBusiness.ReStartAnalogAlarm(tempItems, today, tomorrow);
                //开关量处理
                tempItems = pointDefineItems.Where(a => a.DevPropertyID == (int)ItemDevProperty.Derail).ToList();
                AlarmBusiness.ReStartDerailAlarm(tempItems, today, tomorrow);

                #endregion
            }
        }

        /// <summary>
        /// 系统退出处理
        /// </summary>
        /// <param name="stationID">分站号</param>
        /// <param name="time">系统退出时间</param>
        public void Drv_SystemExistPro(short stationID, DateTime time)
        {
            List<Jc_DefInfo> pointDefineInfo = SafetyHelper.GetPointDefinesByStationID(stationID);
            List<Jc_DefInfo> updataPointDefineItems = new List<Jc_DefInfo>();
            Jc_DefInfo tempPointDefine;
            try
            {
                #region 更新分站及下级设备状态及其实时值
                //分站
                tempPointDefine = pointDefineInfo.FirstOrDefault(a => a.DevPropertyID == (int)ItemDevProperty.Substation);
                AlarmBusiness.EndSubstationAlarm(tempPointDefine, time);
                tempPointDefine.Ssz = EnumHelper.GetEnumDescription(DeviceDataState.EquipmentInterrupted);
                tempPointDefine.DataState = (int)DeviceDataState.EquipmentInterrupted;
                tempPointDefine.State = (int)DeviceDataState.EquipmentInterrupted;
                tempPointDefine.Voltage = 0;
                tempPointDefine.Alarm = 1;
                tempPointDefine.Zts = time;
                updataPointDefineItems.Add(tempPointDefine);

                //模拟量
                AlarmBusiness.BatchEndAnalogAlarm(pointDefineInfo.Where(a => a.DevPropertyID == (int)ItemDevProperty.Analog).ToList(), time, false);
                //开关量
                AlarmBusiness.BatchEndDerailAlarm(pointDefineInfo.Where(a => a.DevPropertyID == (int)ItemDevProperty.Derail).ToList(), time, false);
                //控制量
                AlarmBusiness.BatchEndControlAlarm(pointDefineInfo.Where(a => a.DevPropertyID == (int)ItemDevProperty.Control).ToList(), time);
                ////更新实时值
                // Dictionary<string, object> updateItems = new Dictionary<string, object>();
                //foreach (Jc_DefInfo defInfo in pointDefineInfo)
                //{
                //    if (defInfo.DevPropertyID == (int)DeviceProperty.Substation)
                //    {
                //        continue;
                //    }

                //    defInfo.State = (int)DeviceDataState.EquipmentStateUnknow;
                //    defInfo.DataState = (int)DeviceDataState.EquipmentStateUnknow;
                //    defInfo.DttRunStateTime = time;
                //    defInfo.Alarm = 0;
                //    defInfo.Ssz = "";
                //    defInfo.Zts = time;
                //    defInfo.Voltage = 0;
                //    //updataPointDefineItems.Add(defInfo);

                //    updateItems = new Dictionary<string,object>();
                //    updateItems.Add("State", defInfo.State);
                //    updateItems.Add("DataState", defInfo.DataState);
                //    updateItems.Add("DttRunStateTime", defInfo.DttRunStateTime);
                //    updateItems.Add("Alarm", defInfo.Alarm);
                //    updateItems.Add("Ssz", defInfo.Ssz);
                //    updateItems.Add("Zts", defInfo.Zts);
                //    updateItems.Add("Voltage", defInfo.Voltage);
                //    SafetyHelper.UpdatePointDefineInfoByProperties(defInfo.PointID, updateItems);

                //}


                //SafetyHelper.BatchUpdatePointDefineInfo(updataPointDefineItems, 1);

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message + ex.StackTrace + "[" + driverSource + "]-Drv_SystemExistPro-系统退出处理");
            }

        }

        /// <summary>
        /// 驱动预处理
        /// </summary>
        /// <param name="defInfo">被处理过的设备链表</param>
        public void Drv_Pretreatment(List<Jc_DefInfo> pointDefineItems)
        {
            if (pointDefineItems != null && pointDefineItems.Any())
            {
                //分站下发初始化标记
                bool isSendIniOrder = false;
                short fzh = 0;
                Dictionary<string, object> updateItems;
                foreach (Jc_DefInfo pointDefineItem in pointDefineItems)
                {
                    PointHandle realdataHandle = PointHandleFactory.CreateRealDataHandle(pointDefineItem.DevPropertyID);
                    if (realdataHandle == null) { continue; }
                    realdataHandle.IniData(pointDefineItem);
                    try
                    {
                        isSendIniOrder |= realdataHandle.PretreatmentHandle(pointDefineItem);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("Drv_Pretreatment Error【" + pointDefineItem.Point + "】" + ex.Message + ex.StackTrace);
                    }
                    fzh = pointDefineItem.Fzh;
                }
                //更新内存
                if (isSendIniOrder)
                {
                    //置分站下发初始化标记
                    Jc_DefInfo station = SafetyHelper.GetPointDefinesByStationID(fzh).FirstOrDefault(a => a.DevPropertyID == (int)DeviceProperty.Substation);
                    if (station != null)
                    {
                        //station.ClsCommObj.BInit = false; 2018.3.26 by 屏蔽，系统首次启动使用此参数判断是否下发初始化，之后都根据NCommandbz 来判断 
                        station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_InitializeRequest;
                        station.sendIniCount++;
                        //station.Bz19 = "1";
                        updateItems = new Dictionary<string, object>();
                        updateItems.Add("ClsCommObj", station.ClsCommObj);
                        updateItems.Add("sendIniCount", station.sendIniCount);
                        //updateItems.Add("Bz19", station.Bz19);
                        //SafetyHelper.UpdateJC_DEF_Bz19(station);//2018.3.26 by
                        SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                        //SafetyHelper.UpdatePointDefineInfo(station, 3);

                    }
                }
            }
        }
        #endregion

        #region 回发数据处理

        /// <summary>
        /// 通讯测试回发数据处理
        /// </summary>
        private void CommunicationTestResponseProc(MasProtocol data)
        {
            try
            {//todo:hdw
                //CommunicationTestResponse communicationTestResponse = data.Deserialize<CommunicationTestResponse>();
                //string point = communicationTestResponse.DeviceCode;
                //Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
                //Dictionary<string, object> updateItems = new Dictionary<string, object>();
                //if (station != null)
                //{
                //    SafetyHelper.WriteLogInfo("【" + station.Point + "】CommunicationTestResponseProc");
                //    station.ClsCommObj.NCommCount++;
                //    updateItems.Add("ClsCommObj", station.ClsCommObj);
                //    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("CommunicationTestResponseProc Error:" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 控制命令回发数据处理
        /// </summary>
        private void DeviceControlResponseProc(MasProtocol data)
        {
            try
            {
                DeviceControlResponse deviceControlResponse = data.Deserialize<DeviceControlResponse>();

                string point = deviceControlResponse.DeviceCode;
                Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (station != null)
                {
                    SafetyHelper.WriteLogInfo("【" + station.Point + "】DeviceControlResponseProc");
                    if (station.realControlCount > 0)
                    {
                        station.realControlCount--;
                        if (station.realControlCount > 1)
                        {
                            //此时只需要下次下发最新的控制字即可
                            station.realControlCount = 1;
                        }
                        updateItems.Add("realControlCount", station.realControlCount);
                    }
                    else if (station.realControlCount <= 0) //2018.4.15 by  下发一次命令，交换机可能回8次确认（1个模块只绑定了一个分站）
                    {
                        station.realControlCount = 0;
                    }

                    if (station.realControlCount > 0)
                    {
                        station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_DeviceControlRequest; //继续下发控制命令标记
                        station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_QueryRealDataRequest);//置不下发F命令标记
                    }
                    else
                    {
                        station.ClsCommObj.BSendControlCommand = false;
                        station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_DeviceControlRequest); //不继续下发控制命令标记
                        station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//置下发F命令标记
                    }

                    //if (station.GetDeviceSoleCoding == 1)
                    //{
                    //    station.GetDeviceSoleCoding = 0;
                    //    //置下发F命令标记
                    //    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;
                    //    station.IsSendFCommand = false;//2018.3.26 by

                    //    updateItems.Add("GetDeviceSoleCoding", station.GetDeviceSoleCoding);
                    //    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    //    updateItems.Add("IsSendFCommand", station.IsSendFCommand);
                    //    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);//更新到分站内存
                    //    LogHelper.Info("【" + station.Point + "】分站首次通讯后收到唯一编码，切换F命令");
                    //}
                    //else
                    //{
                    //    LogHelper.Info("【" + station.Point + "】分站正常通讯后收到唯一编码，不切换F命令");
                    //}

                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                    //SafetyHelper.UpdatePointDefineInfo(station, 2);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DeviceControlResponseProc Error:" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 设备请求初始化数据处理
        /// </summary>
        private void DeviceInitializeRequestProc(MasProtocol data)
        {
            try
            {
                DeviceInitializeRequest deviceInitializeRequest = data.Deserialize<DeviceInitializeRequest>();
                string point = deviceInitializeRequest.DeviceCode;
                Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
                DateTime createdTime = data.CreatedTime;//接受数据时间 2017.10.13 by
                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (station != null)
                {
                    SafetyHelper.WriteLogInfo("【" + station.Point + "】DeviceInitializeRequestProc");
                    //station.ClsCommObj.BInit = false;
                    station.DttStateTime = DateTime.Now;
                    if (station.sendIniCount == 0)//2018.3.26 by
                    {
                        station.sendIniCount++;
                    }

                    if (station.DataState != (short)DeviceDataState.EquipmentIniting)
                    {
                        //SafetyHelper.DeviceInterruptPro(station.Fzh, DateTime.Now, DeviceRunState.EquipmentIniting);
                        SafetyHelper.DeviceInterruptPro(station.Fzh, createdTime, DeviceRunState.EquipmentIniting);
                    }
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_InitializeRequest;
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    updateItems.Add("DttStateTime", station.DttStateTime);
                    updateItems.Add("sendIniCount", station.sendIniCount);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                    //SafetyHelper.UpdatePointDefineInfo(station, 2);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DeviceInitializeRequestProc Error:" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 初始化回发数据处理
        /// </summary>
        private void InitializeResponseProc(MasProtocol data)
        {
            try
            {
                InitializeResponse initializeResponse = data.Deserialize<InitializeResponse>();
                string point = initializeResponse.DeviceCode;
                Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);

                ushort stationCrc = initializeResponse.StationCrc;
                if (station.Bz19 != initializeResponse.StationCrc.ToString())//判断如果上行数据的Crc与上一次的不一致时，重新切换到I命令下发
                {
                    Basic.Framework.Logging.LogHelper.Warn(station.Point + "初始命令回发，检测到分站上行的CRC与中心站不一致，分站上行：" + initializeResponse.StationCrc.ToString() + ",中心站缓存：" + station.Bz19);
                    station.Bz19 = initializeResponse.StationCrc.ToString();
                    Dictionary<string, object> tempupdateItems = new Dictionary<string, object>();
                    tempupdateItems.Add("Bz19", station.Bz19);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, tempupdateItems);//更新缓存
                    SafetyHelper.UpdateStation_Crc(station);//更新数据库
                    return;
                }

                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (station != null)
                {
                    station.sendIniCount--;
                    station.DttStateTime = DateTime.Now;
                    if (station.sendIniCount > 1)
                    {
                        //有初始化要下发，只需要下发最新的初始化即可
                        station.sendIniCount = 1;

                        station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_InitializeRequest;//下发I命令

                        LogHelper.Info("【FCommand-" + station.Point + "】收到初始化回复命令，剩余下发次数：" + station.sendIniCount);
                    }
                    else
                    {
                        //不需要下发初始化，置回标记
                        station.sendIniCount = 0;
                        station.ClsCommObj.BInit = true;

                        station.IsSendFCommand = false;
                        LogHelper.Info("【FCommand-" + station.Point + "】收到初始化回复命令，置下发F标记为False。");
                        //station.ClsCommObj.NCommandbz &= 0xFFFE;
                        station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_InitializeRequest);
                        station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                        station.IsSendFCommand = false;//2018.3.26 by
                        UpdatePointEditState(station);

                        //station.Bz19 = "0"; //2018.3.26 by
                        //更新bz19到数据库
                        //SafetyHelper.UpdateJC_DEF_Bz19(station);

                        //#region//分站初始化回发时，对分站网络模块进行时间同步
                        //Jc_MacInfo mac = SafetyHelper.GetMacItemByMac(station.Jckz1);
                        //if (mac != null)
                        //{
                        //    mac.TimeSynchronizationcount = 2;
                        //    mac.TimeSynchronization = true;

                        //    //更新网络模块内存
                        //    updateItems = new Dictionary<string, object>();
                        //    updateItems.Add("TimeSynchronizationcount", mac.TimeSynchronizationcount);
                        //    updateItems.Add("TimeSynchronization", mac.TimeSynchronization);
                        //    SafetyHelper.UpdateNetworkModeuleCacheByPropertys(mac.MAC, updateItems);
                        //    LogHelper.Info("分站初始化回发，下发分站时间同步命令，分站：" + station.Point + ",Mac地址：" + mac.MAC + "下发时间同步，待下发" + mac.TimeSynchronizationcount + "次");
                        //}
                        //#endregion

                    }
                    station.GradingAlarmTime = DateTime.Now; //以此保证在正常通讯后，能够将每一次从数据库加载出来的分级报警控制下发 2018.3.13 by
                    SafetyHelper.WriteLogInfo("【" + station.Point + "】InitializeResponseProc " + station.sendIniCount);
                    updateItems.Add("DttStateTime", station.DttStateTime);
                    updateItems.Add("sendIniCount", station.sendIniCount);
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    updateItems.Add("IsSendFCommand", station.IsSendFCommand);
                    updateItems.Add("GradingAlarmTime", station.GradingAlarmTime);
                    //updateItems.Add("Bz19", station.Bz19);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                }
                else
                {
                    LogHelper.Warn("收到初始化回复命令，未找到分站定义：" + point);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("InitializeResponseProc Error:" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 电源箱回发数据处理
        /// </summary>
        private void QueryBatteryRealDataResponseProc(MasProtocol data)
        {
            try
            {
                QueryBatteryRealDataResponse queryBatteryRealDataResponse = data.Deserialize<QueryBatteryRealDataResponse>();
                string point = queryBatteryRealDataResponse.DeviceCode;
                Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (station != null)
                {
                    SafetyHelper.WriteLogInfo("【" + station.Point + "】QueryBatteryRealDataResponseProc");
                    List<BatteryItem> batteryItems = new List<BatteryItem>();
                    BatteryItem batteryItem;
                    List<BatteryRealDataItem> batteryDataaItems = queryBatteryRealDataResponse.BatteryRealDataItems;

                    //if (station.BatteryItems == null)
                    //{
                    station.BatteryItems = new List<BatteryItem>();
                    //}

                    int batteryIndex = 0;
                    if (batteryDataaItems != null)
                    {
                        foreach (BatteryRealDataItem batteryDataaItem in batteryDataaItems)
                        {
                            if (batteryDataaItem.Channel == "0")
                            {

                            }
                            batteryIndex = station.BatteryItems.FindIndex(a => a.Channel == batteryDataaItem.Channel);
                            if (batteryIndex >= 0)
                            {
                                station.BatteryItems[batteryIndex].Channel = batteryDataaItem.Channel;
                                station.BatteryItems[batteryIndex].BatteryAddress = batteryDataaItem.Address;
                                //station.BatteryItems[batteryIndex].BatteryState = batteryDataaItem.BatteryState;
                                //station.BatteryItems[batteryIndex].BatteryTooHot = batteryDataaItem.BatteryTooHot;
                                station.BatteryItems[batteryIndex].BatteryACDC = batteryDataaItem.BatteryACDC;
                                //station.BatteryItems[batteryIndex].BatteryUndervoltage = batteryDataaItem.BatteryUndervoltage;
                                //station.BatteryItems[batteryIndex].BatteryOverCharge = batteryDataaItem.BatteryOverCharge;
                                //station.BatteryItems[batteryIndex].BatteryPackStateCd = batteryDataaItem.BatteryPackStateCd;
                                //station.BatteryItems[batteryIndex].BatteryPackStateJh = batteryDataaItem.BatteryPackStateJh;
                                //station.BatteryItems[batteryIndex].BatteryPackStateFd = batteryDataaItem.BatteryPackStateFd;
                                station.BatteryItems[batteryIndex].BatteryVOL = batteryDataaItem.BatteryVOL;
                                //station.BatteryItems[batteryIndex].PowerPackMA = batteryDataaItem.BatteryPackMA;
                                //station.BatteryItems[batteryIndex].PowerPackVOL = batteryDataaItem.BatteryPackVOL;
                                station.BatteryItems[batteryIndex].DeviceTemperature1 = batteryDataaItem.DeviceTemperature1;
                                station.BatteryItems[batteryIndex].DeviceTemperature2 = batteryDataaItem.DeviceTemperature2;
                                station.BatteryItems[batteryIndex].TotalVoltage = batteryDataaItem.TotalVoltage;
                            }
                            else
                            {
                                batteryItem = new BatteryItem();
                                batteryItem.Channel = batteryDataaItem.Channel;
                                batteryItem.BatteryAddress = batteryDataaItem.Address;
                                //batteryItem.BatteryState = batteryDataaItem.BatteryState;
                                //batteryItem.BatteryTooHot = batteryDataaItem.BatteryTooHot;
                                batteryItem.BatteryACDC = batteryDataaItem.BatteryACDC;
                                //batteryItem.BatteryUndervoltage = batteryDataaItem.BatteryUndervoltage;
                                //batteryItem.BatteryOverCharge = batteryDataaItem.BatteryOverCharge;
                                //batteryItem.BatteryPackStateCd = batteryDataaItem.BatteryPackStateCd;
                                //batteryItem.BatteryPackStateJh = batteryDataaItem.BatteryPackStateJh;
                                //batteryItem.BatteryPackStateFd = batteryDataaItem.BatteryPackStateFd;
                                batteryItem.BatteryVOL = batteryDataaItem.BatteryVOL;
                                //batteryItem.PowerPackMA = batteryDataaItem.BatteryPackMA;
                                //batteryItem.PowerPackVOL = batteryDataaItem.BatteryPackVOL;
                                batteryItem.DeviceTemperature1 = batteryDataaItem.DeviceTemperature1;
                                batteryItem.DeviceTemperature2 = batteryDataaItem.DeviceTemperature2;
                                batteryItem.TotalVoltage = batteryDataaItem.TotalVoltage;
                                station.BatteryItems.Add(batteryItem);
                            }
                        }
                        station.PowerDateTime = queryBatteryRealDataResponse.BatteryDateTime;
                        station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_QueryBatteryRealDataRequest);   //置获取电源箱数据成功标记 不再下发D命令
                        station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令

                        station.IsSendFCommand = false;
                        station.DttStateTime = DateTime.Now;

                        updateItems.Add("BatteryItems", station.BatteryItems);
                        updateItems.Add("PowerDateTime", station.PowerDateTime);
                        updateItems.Add("ClsCommObj", station.ClsCommObj);
                        updateItems.Add("IsSendFCommand", station.IsSendFCommand);
                        updateItems.Add("DttStateTime", station.DttStateTime);
                        SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);

                        station = SafetyHelper.GetPointDefinesByPoint(point);
                        ////更新初始化标记
                        //SafetyHelper.UpdatePointDefineInfo(station, 3);
                        ////更新电源箱数据
                        //SafetyHelper.UpdatePointDefineInfo(station, 5);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("QueryBatteryRealDataResponseProc Error:" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 设备唯一编码数据处理
        /// </summary>
        private void QueryDeviceInfoResponseProc(MasProtocol data)
        {
            try
            {
                GetDeviceInformationResponse queryDeviceInfoResponse = data.Deserialize<GetDeviceInformationResponse>();
                string point = queryDeviceInfoResponse.DeviceCode;
                Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
                DateTime createdTime = data.CreatedTime;//接受数据时间
                if (station != null)
                {
                    List<Jc_DefInfo> sensors = SafetyHelper.GetPointDefinesByStationID(station.Fzh);

                    SafetyHelper.WriteLogInfo("【" + station.Point + "】QueryDeviceInfoResponseProc");

                    //更新获取到的数据到缓存
                    //station.DeviceDetailDtatalstStation = queryDeviceInfoResponse.lstStation;
                    //station.DeviceDetailDtatalstSensor = queryDeviceInfoResponse.lstSensor;
                    //更新分站数据到缓存及数据库
                    foreach (StationInfo item in queryDeviceInfoResponse.lstStation)
                    {
                        station.Bz17 = item.RestartNum.ToString();
                        station.Bz18 = item.TimeNow.ToString("yyyy-MM-dd HH:mm:ss");
                        station.Voltage = float.Parse(item.Voltage);
                        station.Bz15 = item.ProductionTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    //更新命令标记
                    station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_QueryDeviceInfoRequest); //不继续下发本命令标记
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//置下发F命令标记
                    station.IsSendFCommand = false;//2018.3.26 by
                    station.DttStateTime = DateTime.Now;

                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    updateItems.Add("DttStateTime", station.DttStateTime);
                    updateItems.Add("Bz17", station.Bz17);
                    updateItems.Add("Bz18", station.Bz18);
                    updateItems.Add("Voltage", station.Voltage);
                    updateItems.Add("Bz15", station.Bz15);

                    //updateItems.Add("DeviceDetailDtatalstStation", station.DeviceDetailDtatalstStation);
                    //updateItems.Add("DeviceDetailDtatalstSensor", station.DeviceDetailDtatalstSensor);
                    updateItems.Add("IsSendFCommand", station.IsSendFCommand);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                    List<Jc_DefInfo> stations = new List<Jc_DefInfo>();
                    stations.Add(station);
                    SafetyHelper.UpdateStationDetailData(stations);
                    //更新传感器的基础信息
                    List<Jc_DefInfo> addsensers = new List<Jc_DefInfo>();
                    foreach (SensorInfo item in queryDeviceInfoResponse.lstSensor)
                    {
                        Jc_DefInfo senser = sensors.Find(a => a.Fzh == station.Fzh && a.Kh == short.Parse(item.Channel) && a.Dzh == short.Parse(item.Address));
                        if (senser != null)
                        {
                            senser.Voltage = float.Parse(item.Voltage);//电压
                            senser.Bz17 = item.RestartNum.ToString();
                            senser.Bz16 = item.SoleCoding.ToString();
                            senser.Bz15 = item.ProductionTime.ToString("yyyy-MM-dd HH:mm:ss");
                            senser.Bz13 = item.AlarmNum.ToString();
                            senser.Bz14 = item.TimeNow.ToString("yyyy-MM-dd HH:mm:ss");

                            updateItems = new Dictionary<string, object>();
                            updateItems.Add("DttStateTime", DateTime.Now);
                            updateItems.Add("Bz17", senser.Bz17);
                            updateItems.Add("Bz16", senser.Bz16);
                            updateItems.Add("Bz13", senser.Bz13);
                            updateItems.Add("Voltage", senser.Voltage);
                            updateItems.Add("Bz15", senser.Bz15);
                            updateItems.Add("Bz14", senser.Bz14);//传感器当前时间
                            SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);

                            addsensers.Add(senser);

                        }
                    }
                    SafetyHelper.UpdateSensorDetailData(addsensers);
                    //处理自动挂接设备  20181013
                    DeviceUnDefineNewProc(queryDeviceInfoResponse.lstSensor, sensors, station, createdTime);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("QueryDeviceInfoResponseProc Error:" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 实时数据应答累计量处理。
        /// </summary>
        /// <param name="realItems"></param>
        /// <param name="queryRealDataResponse"></param>
        /// <param name="defItems"></param>
        /// <param name="station"></param>
        private void AccumulationDataHandle(List<RealDataItem> realItems, QueryRealDataResponse queryRealDataResponse, List<Jc_DefInfo> defItems, Jc_DefInfo station)
        {
            #region ----累积量数据入库统一先处理----
            if (station.Point == "0800000" || station.Point == "0820000")
            {

            }
            List<RealDataItem> accumulationDataItems = realItems.FindAll(item => (int)item.DeviceProperty == (int)DeviceProperty.Accumulation);
            if (accumulationDataItems != null && accumulationDataItems.Any())
            {
                //当判断到网关回发的时间或抽放时间与当前天日期不相等，直接按照网关或抽放时间建表 2017.7.6 by
                if (queryRealDataResponse.CumulantTime.Date == DateTime.Now.Date ||
                    ((queryRealDataResponse.CumulantTime < DateTime.Now) && ((DateTime.Now - queryRealDataResponse.CumulantTime).TotalHours <= 2)))     //解决跨天时可能存在丢弃数据问题(跨天时，一小时内的数据仍可继续处理)。 2017.7.23 by
                {
                    AccumulationHandle accumulationHandle = new AccumulationHandle();
                    accumulationHandle.DataHandle(station, accumulationDataItems, queryRealDataResponse.CumulantTime, defItems);
                }
                else
                {
                    LogHelper.Error("QueryRealDataResponseProc：Device【" + station.Point + "-" + queryRealDataResponse.CumulantTime.ToString() + "】-Now【" + DateTime.Now.ToString() + "】");
                }
            }
            #endregion
        }
        /// <summary>
        /// 实时数据应答
        /// </summary>
        /// <param name="realItems"></param>
        /// <param name="station"></param>
        private void BatteryDataHandle(List<RealDataItem> realItems, Jc_DefInfo station)
        {
            #region ----电源箱唯一编码优先处理----
            //todo:唯一编码没有带到这里面。
            //同时，D命令获取唯一编码和F命令带唯一编码确认是分开不同的。
            List<RealDataItem> batteryDataItems = realItems.FindAll(item => (int)item.DeviceProperty == (int)DeviceProperty.PowerStation);
            if (batteryDataItems != null && batteryDataItems.Any())
            {
                BatteryHandle batteryHandle = new BatteryHandle();
                batteryHandle.DataHandle(station, batteryDataItems);
            }
            #endregion
        }
        /// <summary>
        /// 实时数据处理
        /// </summary>
        private void QueryRealDataResponseProc(MasProtocol data)
        {
            try
            {
                Dictionary<string, Dictionary<string, object>> UpdateItemsList = new Dictionary<string, Dictionary<string, object>>();//需要更新缓存的数据集合。
                QueryRealDataResponse queryRealDataResponse = data.Deserialize<QueryRealDataResponse>();
                if (queryRealDataResponse == null)
                {
                    return;
                }
                ////模拟数据测试代码
                //bool isMn = true;
                //if (isMn && queryRealDataResponse.DeviceCode == "0110000")
                //{
                //    //queryRealDataResponse.RealDataItems[0].State = ItemState.StationStart;
                //    //queryRealDataResponse.RealDataItems[1].ChangeSenior = 0;
                //    RealDataItem real = new RealDataItem();
                //    real.Address = "1";
                //    real.Channel = "13";
                //    real.DeviceProperty = ItemDevProperty.Derail;
                //    real.BranchNumber = 4;
                //    real.BatteryState = 0;
                //    real.ChangeSenior = 0;
                //    real.DeviceTypeCode = 14;
                //    real.FeedBackState = "";
                //    real.FeedState = "";
                //    real.RealData = "1";
                //    real.SeniorGradeAlarm = 0;
                //    real.SoleCoding = "";
                //    real.State = ItemState.EquipmentCommOK;
                //    real.Voltage = "";
                //    queryRealDataResponse.RealDataItems.Add(real);
                //    real = new RealDataItem();
                //    real.Address = "2";
                //    real.Channel = "13";
                //    real.DeviceProperty = ItemDevProperty.Derail;
                //    real.BranchNumber = 4;
                //    real.BatteryState = 0;
                //    real.ChangeSenior = 0;
                //    real.DeviceTypeCode = 14;
                //    real.FeedBackState = "";
                //    real.FeedState = "";
                //    real.RealData = "1";
                //    real.SeniorGradeAlarm = 0;
                //    real.SoleCoding = "";
                //    real.State = ItemState.EquipmentCommOK;
                //    real.Voltage = "";
                //    queryRealDataResponse.RealDataItems.Add(real);
                //}     


                string point = queryRealDataResponse.DeviceCode;
                List<Jc_DefInfo> defItems = SafetyHelper.GetPointDefinesByStationID(Convert.ToInt16(point.Substring(0, 3)));//hdw:20170719--只获取一次，不反复获取
                Jc_DefInfo station = defItems.Find(a => a.Point == point);// Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);    


                ushort stationCrc = queryRealDataResponse.StationCrc;
                if (station.Bz19 != queryRealDataResponse.StationCrc.ToString())//判断如果上行数据的Crc与上一次的不一致时，重新切换到I命令下发
                {
                    Basic.Framework.Logging.LogHelper.Warn(station.Point + "F命令回发，检测到分站上行的CRC与中心站不一致，分站上行：" + queryRealDataResponse.StationCrc.ToString() + ",中心站缓存：" + station.Bz19);
                    Dictionary<string, object> tempupdateItems = new Dictionary<string, object>();
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_InitializeRequest;//下发I命令
                    station.sendIniCount = 1;
                    station.Bz19 = queryRealDataResponse.StationCrc.ToString();
                    tempupdateItems.Add("Bz19", station.Bz19);
                    //station.ClsCommObj.BInit = true;
                    tempupdateItems.Add("ClsCommObj", station.ClsCommObj);
                    tempupdateItems.Add("sendIniCount", station.sendIniCount);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, tempupdateItems);//更新缓存
                    SafetyHelper.UpdateStation_Crc(station);//更新数据库
                    return;
                }

                #region//分站数据回发时，判断分站时间与计算机时间差，如果超过30秒，则下发时间同步命令，对分站网络模块进行时间同步
                TimeSpan ts = data.CreatedTime - queryRealDataResponse.DeviceTime;
                if (Math.Abs(ts.TotalSeconds) > 30)
                {
                    Jc_MacInfo mac = SafetyHelper.GetMacItemByMac(station.Jckz1);
                    if (mac != null && mac.TimeSynchronizationcount < 1)//如果没有下发命令时，才下发，避免重复下发
                    {
                        mac.TimeSynchronizationcount = 2;
                        mac.TimeSynchronization = true;

                        //更新网络模块内存
                        Dictionary<string, object> updateItems = new Dictionary<string, object>();
                        updateItems.Add("TimeSynchronizationcount", mac.TimeSynchronizationcount);
                        updateItems.Add("TimeSynchronization", mac.TimeSynchronization);
                        SafetyHelper.UpdateNetworkModeuleCacheByPropertys(mac.MAC, updateItems);
                        LogHelper.Info("分站回发时间与网关时间间隔大于30秒，下发分站时间同步命令，分站：" + station.Point + ",Mac地址：" + mac.MAC + "下发时间同步，待下发" + mac.TimeSynchronizationcount + "次");
                    }
                }
                #endregion

                #region//处理分站设备电源箱通讯状态
                Dictionary<string, object> upitem = new Dictionary<string, object>();
                upitem.Add("StationDyType", queryRealDataResponse.StationDyType);
                SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, upitem);//更新缓存      
                #endregion

                DateTime createdTime = data.CreatedTime;//接受数据时间
                List<short> chanels = new List<short>();//唯一编码确认通道链表
                List<Jc_DevInfo> devItems = SafetyHelper.GetDeviceInfoByDevType(1);  //模拟量变值存储时用
                byte deviceCommperType = queryRealDataResponse.DeviceCommperType;
                if (station != null)
                {
                    List<RealDataItem> realDataProcItems;
                    //获取设备回发的数据
                    List<RealDataItem> realItems = queryRealDataResponse.RealDataItems;
                    //累计量数据处理
                    if ((createdTime - station.DttCfUpTime).TotalSeconds >= 30)//2018.8.22 by
                    {
                        AccumulationDataHandle(realItems, queryRealDataResponse, defItems, station);
                        Dictionary<string, object> updateItems = new Dictionary<string, object>();
                        updateItems.Add("DttCfUpTime", createdTime);
                        SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);//更新到分站内存
                    }

                    //实时数据处理（累计量除外）
                    realDataProcItems = realItems.Where(a => a.DeviceProperty != ItemDevProperty.SoleCoding).ToList();
                    //Basic.Framework.Logging.LogHelper.Debug(station.Point + "收到实时数据，共：" + realDataProcItems.Count);
                    DeviceRealDataProc(realDataProcItems, defItems, devItems, station, createdTime, deviceCommperType);

                    //Basic.Framework.Logging.LogHelper.Debug(station.Point + "收到实时数据1，共：" + realDataProcItems.Count);
                    ////唯一编码处理
                    //realDataProcItems = realItems.Where(a => a.DeviceProperty == ItemDevProperty.SoleCoding).ToList();
                    //if (realDataProcItems != null && realDataProcItems.Count > 0)
                    //{
                    //    DeviceSoloCodingProc(realDataProcItems, defItems, station, ref chanels);
                    //}
                    ////未定义设备处理
                    //realDataProcItems = realItems.Where(a => a.DeviceProperty == ItemDevProperty.SoleCoding
                    //    //&& a.SoleCoding != "0"   2017.12.15 by 唯一编码为0要进行处理
                    //    && a.Channel != "27"
                    //    && a.Channel != "28"
                    //    && a.Channel != "29"
                    //    && a.Channel != "30"
                    //    && a.Channel != "31").ToList();// 没有唯一编码的设备不进行未定义设备处理； 电源箱不进行未定义设备处理
                    //DeviceUnDefineProc(realDataProcItems, defItems, station, createdTime);                    
                    //分站历史数据处理  
                    DeviceHisDataProc(queryRealDataResponse, defItems, station, createdTime);
                    //处理传感器基础信息
                    SenorBaseInfoProc(queryRealDataResponse, defItems, station, createdTime);

                    if (chanels.Count > 0)//lch20180504
                        SafetyHelper.UpdateDeviceOnlyCodeConfirmToStation(station.Point, chanels);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("QueryRealDataResponseProc Error:" + ex.Message + ex.StackTrace + ex.StackTrace);
            }
        }
        /// <summary>
        /// 处理历史数据 
        /// </summary>
        /// <param name="realDataProcItems"></param>
        /// <param name="defItems"></param>
        /// <param name="station"></param>
        /// <param name="createTime"></param>
        private void DeviceHisDataProc(QueryRealDataResponse realDataProcItems, List<Jc_DefInfo> defItems, Jc_DefInfo station, DateTime createTime)
        {

            if (realDataProcItems.HistoryRealDataItems != null)
            {
                if (realDataProcItems.HistoryRealDataItems.Count > 0)
                {
                    Basic.Framework.Logging.LogHelper.Debug("收到分站历史数据，分站号：" + station.Point + ",总记录数：" + realDataProcItems.HistoryRealDataItems.Count);
                    DoDeviceHistoryRealData(station, defItems, realDataProcItems.HistoryRealDataItems, createTime);

                    SetStationLastAcceptFlag(station);

                }
                //else
                //{
                //    SetStationHisDataClear(station, 0);
                //}
            }
            //else
            //{
            //    SetStationHisDataClear(station, 0);
            //}          
        }
        /// <summary>
        /// 历史数据处理线程(接收到数据只入队列，由此线程来处理历史数据，提高效率，减少对实时数据处理的影响)
        /// </summary>
        private void HisDataProc()
        {
            while (true)
            {
                try
                {
                    if (updateStaionHistoryDataItems.Count > 0)
                    {
                        List<StaionHistoryDataInfo> tempstationHis = new List<StaionHistoryDataInfo>();
                        for (int i = 0; i < updateStaionHistoryDataItems.Count; i++)
                        {
                            StaionHistoryDataInfo temphis;
                            updateStaionHistoryDataItems.TryDequeue(out temphis);
                            if (temphis != null)
                            {
                                tempstationHis.Add(temphis);
                            }
                        }
                        if (tempstationHis.Count > 0)
                        {
                            FiveMinBusiness.StaionHistoryDataToDB(tempstationHis);
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.ToString());
                }
                Thread.Sleep(20);
            }
        }
        /// <summary>
        /// 置清除历史数据标记
        /// </summary>
        /// <param name="station"></param>
        /// <param name="value"></param>
        private void SetStationLastAcceptFlag(Jc_DefInfo station)
        {
            //置清除历史数据标记
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            //updateItems.Add("StationHisDataClear", value);
            //SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);//更新到分站内存
            station.LastAcceptFlag = 1;
            updateItems.Add("LastAcceptFlag", station.LastAcceptFlag);
            SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);//更新到分站内存

        }
        /// <summary>
        /// 传感器基础信息处理
        /// </summary>
        /// <param name="realDataProcItems"></param>
        /// <param name="defItems"></param>
        /// <param name="station"></param>
        /// <param name="createTime"></param>
        private void SenorBaseInfoProc(QueryRealDataResponse realDataProcItems, List<Jc_DefInfo> defItems, Jc_DefInfo station, DateTime createTime)
        {
            //foreach (RealDataItem item in realDataProcItems)
            //{
            if (realDataProcItems.DeviceInfoItems != null)
            {
                if (realDataProcItems.DeviceInfoItems.Count > 0)
                {
                    foreach (Sys.DataCollection.Common.Protocols.DeviceInfoMation subitem in realDataProcItems.DeviceInfoItems)
                    {
                        if (subitem != null)
                        {
                            Jc_DefInfo tempdef = defItems.Find(a => a.Kh == short.Parse(subitem.Channel) &&
                                a.Dzh == short.Parse(subitem.Address) && a.DevModelID == subitem.DeviceTypeCode);
                            if (tempdef != null)
                            {
                                tempdef.DeviceInfoItem = new DataContract.CommunicateExtend.DeviceInfoMation();
                                DataContract.CommunicateExtend.DeviceInfoMation inf = new DataContract.CommunicateExtend.DeviceInfoMation();
                                inf.Channel = subitem.Channel;
                                inf.Address = subitem.Address;
                                inf.DeviceTypeCode = subitem.DeviceTypeCode;
                                inf.UpAarmValue = subitem.UpAarmValue;
                                inf.DownAarmValue = subitem.DownAarmValue;
                                inf.UpDdValue = subitem.UpDdValue;
                                inf.DownDdValue = subitem.DownDdValue;
                                inf.UpHfValue = subitem.UpHfValue;
                                inf.DownHfValue = subitem.DownHfValue;
                                inf.LC1 = subitem.LC1;
                                inf.LC2 = subitem.LC2;
                                inf.SeniorGradeAlarmValue1 = subitem.SeniorGradeAlarmValue1;
                                inf.SeniorGradeAlarmValue2 = subitem.SeniorGradeAlarmValue2;
                                inf.SeniorGradeAlarmValue3 = subitem.SeniorGradeAlarmValue3;
                                inf.SeniorGradeAlarmValue4 = subitem.SeniorGradeAlarmValue4;
                                inf.SeniorGradeTimeValue1 = subitem.SeniorGradeTimeValue1;
                                inf.SeniorGradeTimeValue2 = subitem.SeniorGradeTimeValue2;
                                inf.SeniorGradeTimeValue3 = subitem.SeniorGradeTimeValue3;
                                inf.SeniorGradeTimeValue4 = subitem.SeniorGradeTimeValue4;
                                tempdef.DeviceInfoItem = inf;
                                //更新传感器详细信息对象到缓存 
                                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                                updateItems.Add("DeviceInfoItem", tempdef.DeviceInfoItem);
                                SafetyHelper.UpdatePointDefineInfoByProperties(tempdef.PointID, updateItems);
                            }
                        }
                    }
                }
            }
            //}
        }
        /// <summary>
        /// 实时数据处理
        /// </summary>
        /// <param name="realDataItems"></param>
        /// <param name="defItems"></param>
        /// <param name="devItems"></param>
        /// <param name="station"></param>
        /// <param name="createdTime"></param>
        private void DeviceRealDataProc(List<RealDataItem> realDataItems, List<Jc_DefInfo> defItems, List<Jc_DevInfo> devItems, Jc_DefInfo station, DateTime createdTime, byte deviceCommperType)
        {
            try
            {
                Dictionary<string, Dictionary<string, object>> UpdateItemsList = new Dictionary<string, Dictionary<string, object>>();//需要更新缓存的数据集合。
                Dictionary<string, object> upitem = new Dictionary<string, object>();//每个测点要更新的缓存对像
                if (preInfoPoint != station.Point)
                {
                    preInfoPoint = station.Point;   //减少一部分日志输出
                    SafetyHelper.WriteLogInfo("【" + station.Point + "】QueryRealDataResponseProc");
                }
                Jc_DefInfo defInfo;
                bool stationDown = false;

                bool IsPorcData = false;    //是否进行峰值过滤
                int myAbnormalCount = 2;
                int parameterCount = 0; //传感器参数个数
                SettingInfo settingItem = SafetyHelper.GetSettingByKeyStr("IsPorcData");
                if (settingItem != null)
                {
                    if (settingItem.StrValue == "1")
                    {
                        IsPorcData = true;
                        myAbnormalCount = GetMyAbnormalCount(station);
                    }
                }
                bool IsCorrectionCoefficientFlag = false;
                SettingInfo setInfo = SafetyHelper.GetSettingByKeyStr("IsCorrectionCoefficient");
                if (setInfo != null)
                {
                    if (setInfo.StrValue == "1")
                    {
                        IsCorrectionCoefficientFlag = true;
                    }
                }
                bool IsShieldNegData = false;
                setInfo = SafetyHelper.GetSettingByKeyStr("IsShieldNegData");
                if (setInfo != null)
                {
                    if (setInfo.StrValue == "1")
                    {
                        IsShieldNegData = true;
                    }
                }

                float xs = 0;
                if (station.Point == "0100000")
                {

                }

                #region//过滤重复数据处理
                realDataItems = realDataItems.OrderBy(a => a.Channel).OrderBy(a => a.Address).ToList();
                string tempChannel = "";
                for (int i = realDataItems.Count - 1; i >= 0; i--)
                {
                    if (tempChannel != realDataItems[i].Channel + "-" + realDataItems[i].Address + "-" + realDataItems[i].DeviceProperty)
                    {
                        tempChannel = realDataItems[i].Channel + "-" + realDataItems[i].Address + "-" + realDataItems[i].DeviceProperty;
                    }
                    else
                    {//移除同一分站下地址号相同的重复数据
                        realDataItems.RemoveAt(i);
                        Basic.Framework.Logging.LogHelper.Info("分站：" + station.Point + "下存在重复数据，口号：" + realDataItems[i].Channel + "，值：" + realDataItems[i].RealData);
                    }
                }
                #endregion

                realDataItems.ForEach(item =>
                {
                    //if (IsRightBranchNumber(Convert.ToInt32(item.Channel), item.BranchNumber))//2017.9.9 by 分支号错误，数据不解析 
                    {
                        stationDown = false;
                        defInfo = defItems.FirstOrDefault(a => a.DevPropertyID == (int)item.DeviceProperty && a.Kh.ToString() == item.Channel && a.Dzh.ToString() == item.Address);
                        //Test(ref item, ref defInfo);//测试
                        //Basic.Framework.Logging.LogHelper.Debug("处理数据0：" + (int)item.DeviceProperty + "-" + item.Channel + "-" + item.Address);
                        if (defInfo != null)
                        {
                            //Basic.Framework.Logging.LogHelper.Debug("处理数据1：" + defInfo.Point);
                            PointHandle realdataHandle = PointHandleFactory.CreateRealDataHandle(defInfo.DevPropertyID);
                            if (realdataHandle != null)
                            {
                                try
                                {

                                    //Test(ref item, defInfo);//测试
                                    #region ----是否启动用模拟量修正系数 2018.1.13 by

                                    if (IsCorrectionCoefficientFlag)
                                    {
                                        if (defInfo.DevPropertyID == 1)
                                        {
                                            try
                                            {
                                                Jc_DevInfo dev = devItems.FirstOrDefault(a => a.Devid == defInfo.Devid);
                                                if (dev != null)
                                                {
                                                    float.TryParse(dev.Xzxs.ToString(), out xs);
                                                    if (xs > 0.0001)
                                                    {
                                                        double realValue;
                                                        if (double.TryParse(item.RealData, out realValue))
                                                        {
                                                            realValue = realValue * xs;
                                                            //对值进行小数位数处理  20180113
                                                            if (realValue < 10)
                                                            {
                                                                if (realValue == 0)
                                                                {
                                                                    item.RealData = (realValue).ToString("f0");//如果值为0，则不保留小数  20180115
                                                                }
                                                                else
                                                                {
                                                                    item.RealData = (realValue).ToString("f2");
                                                                }
                                                            }
                                                            else if (realValue < 100)
                                                            {
                                                                item.RealData = (realValue).ToString("f1");
                                                            }
                                                            else if (realValue > 100)
                                                            {
                                                                item.RealData = (realValue).ToString("f0");
                                                            }
                                                            else if (realValue < 0)
                                                            {
                                                                item.RealData = (realValue).ToString("f2");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception eex)
                                            {
                                                LogHelper.Error("CorrectionCoefficient Error:" + eex);
                                            }
                                        }
                                    }

                                    #endregion

                                    //峰值过滤 2017.12.18 by 屏蔽峰值过滤
                                    //if (IsPorcData)
                                    //{
                                    //    if (defInfo.DevPropertyID == 1)
                                    //    {
                                    //        //模拟量峰值过滤
                                    //        AnalogPeakFilter(defInfo, devItems, ref item, myAbnormalCount);
                                    //    }
                                    //    else if (defInfo.DevPropertyID == 2)
                                    //    {
                                    //        //开关量峰值过滤
                                    //        DerailPeakFilter(defInfo, ref item, myAbnormalCount);
                                    //    }
                                    //}

                                    //传感器欠压处理
                                    UnderVoltageAlarm(defInfo, ref item);

                                    //线性突变 2018.2.8 by
                                    AnalogMutation(defInfo, devItems, ref item, myAbnormalCount);

                                    //传感器更换状态处理
                                    SensorChange(defInfo, ref item);


                                    if (IsShieldNegData)
                                    {
                                        #region ----负数屏蔽  2018.3.28 by----
                                        double realValue;
                                        if (double.TryParse(item.RealData, out realValue))
                                        {
                                            if (realValue < 0)
                                            {
                                                item.RealData = "0";
                                            }
                                        }
                                        #endregion
                                    }

                                    //Basic.Framework.Logging.LogHelper.Debug("处理数据2：" + defInfo.Point);
                                    upitem = realdataHandle.DataHandleFlow(item, defInfo, createdTime, defItems, devItems, deviceCommperType);

                                    #region ----多参传感器特殊处理 2017.10.16 by----
                                    /*
                                     * 多参数传感器断线、头子断线、通讯误码时，将只传第一个传感器的状态，将然导致后面的参数显示值不更新。
                                     */

                                    if (int.TryParse(defInfo.Bz12, out parameterCount))
                                    {
                                        if (parameterCount > 1 && (item.State == ItemState.EquipmentDown || item.State == ItemState.EquipmentBiterror))
                                        {
                                            RealDataItem tempDataItem = new RealDataItem();

                                            tempDataItem.Address = item.Address;
                                            tempDataItem.BatteryState = item.BatteryState;
                                            tempDataItem.BranchNumber = item.BranchNumber;
                                            tempDataItem.Channel = item.Channel;
                                            tempDataItem.DeviceProperty = item.DeviceProperty;
                                            tempDataItem.DeviceTypeCode = 0;
                                            tempDataItem.FeedBackState = item.FeedBackState;
                                            tempDataItem.FeedState = item.FeedState;
                                            tempDataItem.RealData = item.RealData;
                                            tempDataItem.SoleCoding = "0";
                                            tempDataItem.State = item.State;
                                            tempDataItem.Voltage = item.Voltage;

                                            Dictionary<string, object> tempUpItem = new Dictionary<string, object>();
                                            for (int i = 2; i <= parameterCount; i++)
                                            {
                                                tempDataItem.Address = i.ToString();
                                                Jc_DefInfo tempDefInfo = defItems.FirstOrDefault(a => a.Kh.ToString() == tempDataItem.Channel && a.Dzh.ToString() == tempDataItem.Address && a.Dzh > 0);
                                                if (tempDefInfo != null)
                                                {
                                                    //Basic.Framework.Logging.LogHelper.Debug("处理数据：" + tempDefInfo.Point);
                                                    tempDataItem.DeviceProperty = (ItemDevProperty)tempDefInfo.DevPropertyID;
                                                    if (realDataItems.FirstOrDefault(a => a.Channel == tempDataItem.Channel && a.Address == tempDataItem.Address && a.DeviceProperty == (ItemDevProperty)tempDefInfo.DevPropertyID) == null)
                                                    {
                                                        PointHandle tempRealdataHandle = PointHandleFactory.CreateRealDataHandle(tempDefInfo.DevPropertyID);
                                                        tempUpItem = tempRealdataHandle.DataHandleFlow(tempDataItem, tempDefInfo, createdTime, defItems, devItems, deviceCommperType);
                                                        if (tempUpItem != null)
                                                        {
                                                            UpdateItemsList.Add(tempDefInfo.PointID, tempUpItem);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    #endregion

                                    if (IsPorcData)
                                    {
                                        if (defInfo.DevPropertyID == 1 || defInfo.DevPropertyID == 2)
                                        {
                                            if (upitem == null) { upitem = new Dictionary<string, object>(); }
                                            upitem.Add("AbnormalCount", defInfo.AbnormalCount);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Error("QueryRealDataResponseProc:" + defInfo.Point + "  --" + ex.Message + ex.StackTrace);
                                }
                                if (upitem != null)//hdw1
                                {
                                    if (UpdateItemsList.ContainsKey(defInfo.PointID))
                                    {
                                        LogHelper.Error(defInfo.Point + "在UpdateItemsList中已存在" + item.DeviceProperty + "," + item.Channel + "," + item.Address + "," + item.RealData);
                                    }
                                    else
                                    {
                                        UpdateItemsList.Add(defInfo.PointID, upitem);
                                    }
                                }
                                if (defInfo.DevPropertyID == (int)DeviceProperty.Substation)
                                {
                                    if (item.State == ItemState.EquipmentInterrupted)   //通讯中断不在此处处理，由下发线程判断延时后置中断 2017.7，21 by
                                    {
                                        stationDown = true;
                                    }
                                }
                            }
                        }
                        else//把自定义测点以及电源、分站的唯一编码放进去进行处理。
                        {
                            //未定义测点处理,将未定义测点添加到自动挂接列表中  20181127
                            SafetyHelper.AddUknownPoint(station.Fzh, item, createdTime);
                        }
                    }
                });
                ushort oldNCommandbz = station.ClsCommObj.NCommandbz;
                //置不再下发F命令标记
                if (stationDown && !((station.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryRealDataRequest) == CommunicationCommandValue.Comm_QueryRealDataRequest))
                {
                    //如果为中断,无F命令标记
                    station.ClsCommObj.NCommandbz |= (ushort)(CommunicationCommandValue.Comm_QueryRealDataRequest);
                    upitem = new Dictionary<string, object>();//
                    upitem.Add("ClsCommObj", station.ClsCommObj);
                    UpdateItemsList.Add(station.PointID, upitem);
                }
                //else if (!stationDown && ((station.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryRealDataRequest) == CommunicationCommandValue.Comm_QueryRealDataRequest) && chanels.Count == 0)
                else if (!stationDown && ((station.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryRealDataRequest) == CommunicationCommandValue.Comm_QueryRealDataRequest))
                {
                    //不清除F命令，一直下发  20181123

                    LogHelper.Info("【FCommand-" + station.Point + "】收到F命令，下发标记为True，清除F命令下发标记。");//hdw-20180425                  
                    station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_DeviceControlRequest);
                    station.realControlCount = 0;
                    //station.LastAcceptFlag = 1;//置F命令接收正确标记  20181222

                    upitem = new Dictionary<string, object>();//
                    upitem.Add("ClsCommObj", station.ClsCommObj);
                    upitem.Add("realControlCount", station.realControlCount);
                    //upitem.Add("LastAcceptFlag", station.LastAcceptFlag);
                    if (UpdateItemsList.ContainsKey(station.PointID))
                    {
                        UpdateItemsList.Remove(station.PointID);
                    }
                    UpdateItemsList.Add(station.PointID, upitem);

                    //LogHelper.Info("【FCommand-" + station.Point + "】收到F命令回发");//hdw-20180425
                    //if (!station.ClsCommObj.BInit)
                    //{
                    //    station.ClsCommObj.BInit = true;
                    //    //station.realControlCount = 0;
                    //    upitem = new Dictionary<string, object>();//
                    //    upitem.Add("ClsCommObj", station.ClsCommObj);
                    //    //upitem.Add("realControlCount", station.realControlCount);
                    //    UpdateItemsList.Add(station.PointID, upitem);
                    //}

                }
                if (UpdateItemsList.Count > 0)
                {
                    SafetyHelper.BatchUpdatePointDefineInfoByProperties(UpdateItemsList);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DeviceRealDataProc Error:" + ex.Message + ex.StackTrace);
            }
        }
        /// <summary>
        /// 唯一编码处理
        /// </summary>
        /// <param name="soloCodingItems"></param>
        /// <param name="defItems"></param>
        /// <param name="station"></param>
        private void DeviceSoloCodingProc(List<RealDataItem> soloCodingItems, List<Jc_DefInfo> defItems, Jc_DefInfo station, ref List<short> chanels)
        {
            WriteReciveSoloCodeing(station.Fzh, soloCodingItems);

            List<short> responseChanels = new List<short>();//唯一编码确认回复链表
            List<RealDataItem> tempItem;
            bool isUpdateSoleCoding = false;
            List<Jc_DefInfo> UpdateSoleCodingItems = new List<Jc_DefInfo>();
            foreach (Jc_DefInfo defInfo in defItems)
            {
                if (defInfo.Dzh > 1) { continue; }//2017.10.18 by 多参传感器将第一个传感器作为唯一编码载体

                isUpdateSoleCoding = false;
                if (defInfo.DevPropertyID == (int)ItemDevProperty.Substation)
                {
                    #region ----处理分站电源箱唯一编码----
                    //27-31 电源唯一编码   
                    tempItem = soloCodingItems.Where(a => a.DeviceProperty == ItemDevProperty.SoleCoding && (a.Channel == "27" || a.Channel == "28" || a.Channel == "29" || a.Channel == "30" || a.Channel == "31")).OrderBy(a => a.Channel).ToList();
                    if (tempItem != null && tempItem.Count > 0)
                    {
                        string bz14 = "";
                        for (int i = 0; i < tempItem.Count; i++)
                        {
                            if (tempItem[i].SoleCoding != "0")
                            {
                                bz14 += tempItem[i].Channel + "," + tempItem[i].SoleCoding + "|";
                            }
                            //加入唯一编码确认队列
                            responseChanels.Add(Convert.ToInt16(tempItem[i].Channel));
                        }
                        if (bz14 != "")
                        {
                            bz14 = bz14.Substring(0, bz14.Length - 1);
                        }
                        if (defInfo.Bz14 != bz14)
                        {
                            defInfo.Bz14 = bz14;
                            /*  电源箱地址号对应关系如下：
                              31表示设备地址为250的24（B）电源箱唯一编码；
                              30表示设备地址为249的25（B）电池箱唯一编码；
                              29表示设备地址为248的25（B）电池箱唯一编码；
                              28表示设备地址为247的25（B）电池箱唯一编码；
                              27表示设备地址为246的25（B）电池箱唯一编码
                           */
                            //更新分站电源箱唯一编码队列 todo  根据转换关系，有则更新，无则添加，多则删除
                            //List<BatteryItem> batteryItems = new List<BatteryItem>();
                            //BatteryItem batteryItem;
                            if (defInfo.BatteryItems == null)
                            {
                                defInfo.BatteryItems = new List<BatteryItem>();
                            }
                            int batteryIndex = 0;
                            string realChannel = "";
                            BatteryItem batteryItem;
                            foreach (RealDataItem rItem in tempItem)
                            {
                                realChannel = "0";
                                if (rItem.Channel == "26")
                                {
                                    realChannel = "246";
                                }
                                else if (rItem.Channel == "27")
                                {
                                    realChannel = "247";
                                }
                                else if (rItem.Channel == "28")
                                {
                                    realChannel = "248";
                                }
                                else if (rItem.Channel == "29")
                                {
                                    realChannel = "249";
                                }
                                else if (rItem.Channel == "30")
                                {
                                    realChannel = "250";
                                }
                                if (realChannel != "0")
                                {
                                    batteryIndex = defInfo.BatteryItems.FindIndex(a => a.Channel == realChannel);
                                    if (batteryIndex >= 0)
                                    {
                                        defInfo.BatteryItems[batteryIndex].DeviceOnlyCode = rItem.SoleCoding;
                                    }
                                    else
                                    {
                                        batteryItem = new BatteryItem();
                                        batteryItem.Channel = realChannel;
                                        batteryItem.BatteryAddress = rItem.Address;
                                        batteryItem.DeviceOnlyCode = rItem.SoleCoding;
                                        defInfo.BatteryItems.Add(batteryItem);
                                    }
                                }
                            }
                            Dictionary<string, object> batteryUpdateItems = new Dictionary<string, object>();
                            batteryUpdateItems.Add("BatteryItems", defInfo.BatteryItems);
                            SafetyHelper.UpdatePointDefineInfoByProperties(defInfo.PointID, batteryUpdateItems);
                        }
                    }

                    #endregion
                }

                //tempItem = soloCodingItems.Where(a => a.Channel == defInfo.Kh.ToString() && a.Address == defInfo.Dzh.ToString()).ToList();
                #region ----更新设备唯一编码----
                //if (defInfo.DevPropertyID == (int)ItemDevProperty.Control) { continue; } //普通控制量没有唯一编码
                //32 分站唯一编码
                if (defInfo.DevPropertyID == (int)ItemDevProperty.Substation)
                {
                    tempItem = soloCodingItems.Where(a => a.DeviceProperty == ItemDevProperty.SoleCoding && a.Channel == "31").ToList();
                }
                else
                {
                    //tempItem = soloCodingItems.Where(a => a.DeviceProperty == ItemDevProperty.SoleCoding && a.Channel == defInfo.Kh.ToString() && a.Address == defInfo.Dzh.ToString()).ToList();
                    //tempItem = soloCodingItems.Where(a =>a.DeviceTypeCode == defInfo.DevModelID && a.DeviceProperty == ItemDevProperty.SoleCoding && a.Channel == defInfo.Kh.ToString()).ToList();
                    tempItem = soloCodingItems.Where(a => a.DeviceProperty == ItemDevProperty.SoleCoding && a.Channel == defInfo.Kh.ToString()).ToList();
                }
                if (tempItem != null && tempItem.Count > 0)
                {
                    if (defInfo.DevPropertyID != (int)ItemDevProperty.Substation)   //分站不进行分支号错误判断
                    {
                        if (!IsRightBranchNumber(Convert.ToInt32(tempItem[0].Channel), tempItem[0].BranchNumber))
                        {
                            continue;   //2017.9.9 by 传感器分支号错误，不进行唯一编码处理
                        }
                    }
                    if (!defInfo.BCommDevTypeMatching)
                    {
                        defInfo.RealTypeInfo = SafetyHelper.GetRealTypeInfoStr(tempItem[0].DeviceTypeCode, tempItem[0].RealData);
                        LogHelper.Error("【" + defInfo.Point + "】类型定义错误（0）,当前定义类型： " + defInfo.DevName + "应更新为：" + defInfo.RealTypeInfo);
                    }
                    else
                    {
                        if (tempItem[0].SoleCoding != null && tempItem[0].SoleCoding.Trim() != "")//&& tempItem[0].SoleCoding.Trim() != "0")
                        {
                            string bz16 = tempItem[0].SoleCoding;
                            if (defInfo.Bz16 != bz16)
                            {
                                defInfo.Bz16 = bz16;
                                if (bz16.Length > 8)
                                {
                                    defInfo.Bz15 = bz16.Substring(0, 4) + "-" + bz16.Substring(4, 2) + "-" + bz16.Substring(6, 2);
                                }
                                //置更新唯一编码标记
                                //isUpdateSoleCoding = true;
                            }
                            isUpdateSoleCoding = true;
                        }
                    }
                    responseChanels.Add(Convert.ToInt16(tempItem[0].Channel));
                }

                #endregion
                if (isUpdateSoleCoding)
                {
                    UpdateSoleCodingItems.Add(defInfo);
                }
            }

            if (UpdateSoleCodingItems.Count > 0)
            {
                SafetyHelper.UpdateSoleCodings(UpdateSoleCodingItems);
            }

            if (responseChanels.Count > 0)
            {
                ////接收到分站发回的唯一编码后,且当前状态为获取唯一编码，置为不再继续获取唯一编码 2018.3.26 by
                //if (station.GetDeviceSoleCoding == 1)
                //{
                //    station.GetDeviceSoleCoding = 0;
                //    //置下发F命令标记
                //    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;
                //    station.IsSendFCommand = false;//2018.3.26 by

                //    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                //    updateItems.Add("GetDeviceSoleCoding", station.GetDeviceSoleCoding);
                //    updateItems.Add("ClsCommObj", station.ClsCommObj);
                //    updateItems.Add("IsSendFCommand", station.IsSendFCommand);
                //    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);//更新到分站内存
                //    LogHelper.Info("【" + station.Point + "】分站首次通讯后收到唯一编码，切换F命令");
                //}
                //else
                //{
                //    LogHelper.Info("【" + station.Point + "】分站正常通讯后收到唯一编码，不切换F命令");
                //}
                chanels = responseChanels;
                //SafetyHelper.UpdateDeviceOnlyCodeConfirmToStation(station.Point, responseChanels);//更新唯一编码
                LogHelper.Info("【" + station.Point + "】分站正常通讯后收到唯一编码");
            }
        }
        /// <summary>
        /// 2017.10.2 by 输出接收到的唯一编码，查看自动挂接的问题
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="soloCodingItems"></param>
        private void WriteReciveSoloCodeing(int fzh, List<RealDataItem> soloCodingItems)
        {
            try
            {
                string info = "分站：" + fzh + "【(分支号-口号-地址号-唯一编码;)";
                foreach (RealDataItem item in soloCodingItems)
                {
                    info += item.BranchNumber + "-" + item.Channel + "-" + item.Address + "-" + item.SoleCoding + ";";
                }
                info += "】";
                LogHelper.Info("【WriteReciveSoloCodeing】" + info);
            }
            catch (Exception ex)
            {
                LogHelper.Error("WriteReciveSoloCodeing Error:" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 未定义设备处理
        /// </summary>
        /// <param name="realDataProcItems"></param>
        /// <param name="defItems"></param>
        /// <param name="station"></param>
        private void DeviceUnDefineProc(List<RealDataItem> realDataProcItems, List<Jc_DefInfo> defItems, Jc_DefInfo station, DateTime createTime)
        {
            Jc_DefInfo defInfo;
            //获取当前缓存中所有未定义的测点信息
            List<AutomaticArticulatedDeviceInfo> unDefineItems = SafetyHelper.GetAllUnDefinePoint();
            int channel = 0;
            realDataProcItems.ForEach(item =>
            {

                if (item.DeviceTypeCode != 0)
                {
                    channel = Convert.ToInt32(item.Channel);

                    //唯一编码(不能根据地址号区分，多参传感器上传address = 0,服务端dzh从1开始)
                    defInfo = defItems.FirstOrDefault(a => a.Kh.ToString() == item.Channel
                                && ((a.Dzh >= 0 && a.DevPropertyID != (int)ItemDevProperty.Control) //除控制量外的所有传感器
                                || (a.Dzh > 0 && a.DevPropertyID == (int)ItemDevProperty.Control))); //智能断电器

                    if (!IsRightBranchNumber(channel, (int)item.BranchNumber))
                    {
                        //分支号错误，默认未定义
                        defInfo = null;
                        if (channel == 7)
                        {

                        }
                    }

                    if (defInfo == null)
                    {
                        List<AutomaticArticulatedDeviceInfo> removeItems;
                        AutomaticArticulatedDeviceInfo autoItem = unDefineItems.FirstOrDefault(a => a.DeviceOnlyCode == item.SoleCoding);
                        if (autoItem == null)
                        {

                            if (item.SoleCoding == "0")
                            {
                                item.SoleCoding = CreateSoleCoding(station.Fzh.ToString(), item.BranchNumber.ToString(), channel.ToString()); //2017.12.15 by 没有唯一编码的自动创建一个特殊唯一编码
                            }
                            //需要新增加
                            //有其他设备占用了本设备的地址信息，把其他设备清除掉 2017.11.3 by
                            removeItems = unDefineItems.Where(a => a.ChanelNumber.ToString() == item.Channel && a.AddressNumber.ToString() == item.Address).ToList();
                            if (removeItems.Count > 0)
                            {
                                SafetyHelper.RemoveUnDefineItems(removeItems);
                                LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，有其他设备占用了本设备的地址信息，把其他设备清除掉,【删除未定义设备】");
                            }

                            //将新定义到未定义缓存中的设备加到当前链表，避免一包数据中存在多个一样的唯一编码的设备 重复定义
                            unDefineItems.Add(SafetyHelper.DoUnknownPoint(station.Fzh, item, createTime));
                            LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，【新增未定义设备1】");
                        }
                        else
                        {
                            //原来有
                            removeItems = unDefineItems.Where(a => a.DeviceOnlyCode == item.SoleCoding //该设备未定义，但设备地址信息发生了变化，要重析添加到内存中 2017.11.3 by
                            && (a.BranchNumber != item.BranchNumber
                            || a.ChanelNumber.ToString() != item.Channel
                            || a.AddressNumber.ToString() != item.Address
                            || a.DeviceModel != item.DeviceTypeCode)).ToList();
                            if (removeItems.Count > 0)
                            {
                                //原来有，但设备地址信息发生了变化，把之前的在清空重加入
                                LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，原来有，但设备地址信息发生了变化，把之前的在清空重加入,【删除未定义设备】");
                                SafetyHelper.RemoveUnDefineItems(removeItems);
                                //将新定义到未定义缓存中的设备加到当前链表，避免一包数据中存在多个一样的唯一编码的设备 重复定义
                                unDefineItems.Add(SafetyHelper.DoUnknownPoint(station.Fzh, item, createTime));
                                LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，【设备未定义，无设备类型，无法进行自定义】");
                            }
                            else
                            {
                                //原来有，地址信息也没发生变化，不处理
                            }
                        }
                    }
                    else
                    {
                        List<AutomaticArticulatedDeviceInfo> removeItems = unDefineItems.Where(a => a.DeviceOnlyCode == item.SoleCoding).ToList();
                        //唯一编码存在，直接把之前的唯一编码设备清除
                        if (removeItems.Count != 0)
                        {
                            SafetyHelper.RemoveUnDefineItems(removeItems);
                            LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，【已定义，删除+" + item.SoleCoding + "】");
                        }
                    }
                }
                else
                {
                    LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，【设备型号不存在" + item.DeviceTypeCode + "】");//20183.27 by 
                }
            });
        }
        /// <summary>
        /// 根据获取的传感器详细信息处理自动挂接设备
        /// </summary>
        /// <param name="realDataProcItems"></param>
        /// <param name="defItems"></param>
        /// <param name="station"></param>
        /// <param name="createTime"></param>
        private void DeviceUnDefineNewProc(List<SensorInfo> realDataProcItems, List<Jc_DefInfo> defItems, Jc_DefInfo station, DateTime createTime)
        {
            Jc_DefInfo defInfo;
            //获取当前缓存中所有未定义的测点信息
            List<AutomaticArticulatedDeviceInfo> unDefineItems = SafetyHelper.GetAllUnDefinePoint();
            int channel = 0;
            realDataProcItems.ForEach(item =>
            {
                int deviceTypeCode = (byte)(Int32.Parse(item.SoleCoding) >> 24);//设备型号
                if (deviceTypeCode != 0)
                {
                    channel = Convert.ToInt32(item.Channel);

                    //唯一编码(不能根据地址号区分，多参传感器上传address = 0,服务端dzh从1开始)
                    defInfo = defItems.FirstOrDefault(a => a.Kh.ToString() == item.Channel
                                && ((a.Dzh >= 0 && a.DevPropertyID != (int)ItemDevProperty.Control) //除控制量外的所有传感器
                                || (a.Dzh > 0 && a.DevPropertyID == (int)ItemDevProperty.Control))); //智能断电器

                    if (!IsRightBranchNumber(channel, (int)item.BranchNumber))
                    {
                        //分支号错误，默认未定义
                        defInfo = null;
                        if (channel == 7)
                        {

                        }
                    }

                    if (defInfo == null)
                    {
                        List<AutomaticArticulatedDeviceInfo> removeItems;
                        AutomaticArticulatedDeviceInfo autoItem = unDefineItems.FirstOrDefault(a => a.DeviceOnlyCode == item.SoleCoding);
                        if (autoItem == null)
                        {

                            if (item.SoleCoding == "0")
                            {
                                item.SoleCoding = CreateSoleCoding(station.Fzh.ToString(), item.BranchNumber.ToString(), channel.ToString()); //2017.12.15 by 没有唯一编码的自动创建一个特殊唯一编码
                            }
                            //需要新增加
                            //有其他设备占用了本设备的地址信息，把其他设备清除掉 2017.11.3 by
                            removeItems = unDefineItems.Where(a => a.ChanelNumber.ToString() == item.Channel && a.AddressNumber.ToString() == item.Address).ToList();
                            if (removeItems.Count > 0)
                            {
                                SafetyHelper.RemoveUnDefineItems(removeItems);
                                LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，有其他设备占用了本设备的地址信息，把其他设备清除掉,【删除未定义设备】");
                            }

                            //将新定义到未定义缓存中的设备加到当前链表，避免一包数据中存在多个一样的唯一编码的设备 重复定义
                            unDefineItems.Add(SafetyHelper.DoUnknownPointNew(station.Fzh, item, createTime));
                            LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，【新增未定义设备1】");
                        }
                        else
                        {
                            //原来有
                            removeItems = unDefineItems.Where(a => a.DeviceOnlyCode == item.SoleCoding //该设备未定义，但设备地址信息发生了变化，要重析添加到内存中 2017.11.3 by
                            && (a.BranchNumber != item.BranchNumber
                            || a.ChanelNumber.ToString() != item.Channel
                            || a.AddressNumber.ToString() != item.Address
                            || a.DeviceModel != deviceTypeCode)).ToList();
                            if (removeItems.Count > 0)
                            {
                                //原来有，但设备地址信息发生了变化，把之前的在清空重加入
                                LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，原来有，但设备地址信息发生了变化，把之前的在清空重加入,【删除未定义设备】");
                                SafetyHelper.RemoveUnDefineItems(removeItems);
                                //将新定义到未定义缓存中的设备加到当前链表，避免一包数据中存在多个一样的唯一编码的设备 重复定义
                                unDefineItems.Add(SafetyHelper.DoUnknownPointNew(station.Fzh, item, createTime));
                                LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，【设备未定义，无设备类型，无法进行自定义】");
                            }
                            else
                            {
                                //原来有，地址信息也没发生变化，不处理
                            }
                        }
                    }
                    else
                    {
                        List<AutomaticArticulatedDeviceInfo> removeItems = unDefineItems.Where(a => a.DeviceOnlyCode == item.SoleCoding).ToList();
                        //唯一编码存在，直接把之前的唯一编码设备清除
                        if (removeItems.Count != 0)
                        {
                            SafetyHelper.RemoveUnDefineItems(removeItems);
                            LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，【已定义，删除+" + item.SoleCoding + "】");
                        }
                    }
                }
                else
                {
                    LogHelper.Info("设备信息：fzh = '" + station.Fzh + "',kh = " + item.Channel + " ,dzh = " + item.Address + "，【设备型号不存在" + deviceTypeCode + "】");//20183.27 by 
                }
            });
        }

        //private void QueryRealDataResponseProc(MasProtocol data)
        //{
        //    try
        //    {
        //        Dictionary<string, Dictionary<string, object>> UpdateItemsList = new Dictionary<string, Dictionary<string, object>>();//需要更新缓存的数据集合。
        //        Dictionary<string, object> upitem = new Dictionary<string, object>();//每个测点要更新的缓存对像
        //        QueryRealDataResponse queryRealDataResponse = data.Deserialize<QueryRealDataResponse>();
        //        if (queryRealDataResponse == null)
        //        {
        //            return;
        //        }
        //        string point = queryRealDataResponse.DeviceCode;
        //        List<Jc_DefInfo> defItems = SafetyHelper.GetPointDefinesByStationID(Convert.ToInt16(point.Substring(0, 3)));//hdw:20170719--只获取一次，不反复获取
        //        Jc_DefInfo station = defItems.Find(a => a.Point == point);// Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
        //        DateTime createdTime = data.CreatedTime;//接受数据时间
        //        List<short> chanels = new List<short>();//唯一编码确认通道链表
        //        List<Jc_DevInfo> devItems = SafetyHelper.GetDeviceInfoByDevType(1);  //模拟量变值存储时用

        //        short chanel = 0;//唯一编码通道号
        //        bool stationDown = false;
        //        if (station != null)
        //        {
        //            SafetyHelper.WriteLogInfo("【" + station.Point + "】QueryRealDataResponseProc");
        //            List<RealDataItem> realItems = queryRealDataResponse.RealDataItems;
        //            AccumulationDataHandle(realItems, queryRealDataResponse, defItems, station, UpdateItemsList);//处理累计量
        //            BatteryDataHandle(realItems, station);//处理电源箱数据
        //            Jc_DefInfo defInfo;
        //            List<Jc_DefInfo> UpdateSoleCodingItems = new List<Jc_DefInfo>();
        //            realItems.ForEach(item =>
        //            {
        //                defInfo = defItems.FirstOrDefault(a => a.DevPropertyID == (int)item.DeviceProperty && a.Kh.ToString() == item.Channel && a.Dzh.ToString() == item.Address);
        //                if (defInfo != null)
        //                {
        //                    if (item.SoleCoding != null && item.SoleCoding.Trim() != "")
        //                    {
        //                        if (short.TryParse(item.Channel, out chanel))
        //                        {
        //                            chanels.Add(chanel);
        //                        }
        //                    }
        //                    if (defInfo.DevPropertyID != (int)DeviceProperty.PowerStation)
        //                    {
        //                        if (defInfo.Bz13 != item.SoleCoding && item.SoleCoding != null)
        //                        {
        //                            if (item.SoleCoding.Trim() != "")
        //                            {
        //                                defInfo.Bz13 = item.SoleCoding;
        //                                UpdateSoleCodingItems.Add(defInfo);
        //                            }
        //                        }
        //                        RealDataHandle.PointHandle realdataHandle = PointHandleFactory.CreateRealDataHandle(defInfo.DevPropertyID);
        //                        if (realdataHandle != null)
        //                        {
        //                            try
        //                            {
        //                                Test(ref item, defInfo);//测试
        //                                upitem = realdataHandle.DataHandleFlow(item, defInfo, createdTime, defItems, devItems);
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                LogHelper.Error("QueryRealDataResponseProc:" + defInfo.Point + "  --" + ex.Message+ex.StackTrace);
        //                            }
        //                            if (upitem != null)//hdw1
        //                            {
        //                                UpdateItemsList.Add(defInfo.PointID, upitem);
        //                            }
        //                            if (defInfo.DevPropertyID == (int)DeviceProperty.Substation)
        //                            {
        //                                if (item.State == ItemState.EquipmentInterrupted)   //通讯中断不在此处处理，由下发线程判断延时后置中断 2017.7，21 by
        //                                {
        //                                    stationDown = true;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                if (defInfo == null || item.DeviceAutoDefine)//把自定义测点以及电源、分站的唯一编码放进去进行处理。
        //                {
        //                    //未定义测点处理
        //                    SafetyHelper.DoUnknownPoint(station.Fzh, item);
        //                }
        //                if (UpdateSoleCodingItems.Count > 0)
        //                {
        //                    SafetyHelper.UpdateSoleCodings(UpdateSoleCodingItems);
        //                }
        //            });
        //            if (chanels.Count > 0)
        //            {
        //                SafetyHelper.UpdateDeviceOnlyCodeConfirmToStation(point, chanels);
        //            }
        //            ushort oldNCommandbz = station.ClsCommObj.NCommandbz;
        //            //置不再下发F命令标记
        //            if (stationDown && !((station.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryRealDataRequest) == CommunicationCommandValue.Comm_QueryRealDataRequest))
        //            {
        //                //如果为中断,无F命令标记
        //                station.ClsCommObj.NCommandbz |= (ushort)(CommunicationCommandValue.Comm_QueryRealDataRequest);
        //                upitem = new Dictionary<string, object>();//
        //                upitem.Add("ClsCommObj", station.ClsCommObj);
        //                UpdateItemsList.Add(station.PointID, upitem);
        //            }
        //            else if (!stationDown && ((station.ClsCommObj.NCommandbz & CommunicationCommandValue.Comm_QueryRealDataRequest) == CommunicationCommandValue.Comm_QueryRealDataRequest))
        //            {
        //                //如果不为中断,有F命令标记
        //                station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_QueryRealDataRequest);
        //                upitem = new Dictionary<string, object>();//
        //                upitem.Add("ClsCommObj", station.ClsCommObj);
        //                UpdateItemsList.Add(station.PointID, upitem);
        //            }
        //            if (UpdateItemsList.Count > 0)
        //            {
        //                SafetyHelper.BatchUpdatePointDefineInfoByProperties(UpdateItemsList);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error("QueryRealDataResponseProc Error:" + ex.Message+ex.StackTrace + ex.StackTrace);
        //    }
        //}

        /// <summary>
        /// 复位命令处理
        /// </summary>
        private void ResetDeviceCommandResponseProc(MasProtocol data)
        {
            try
            {
                ResetDeviceCommandResponse resetDeviceCommandResponse = data.Deserialize<ResetDeviceCommandResponse>();
                string point = resetDeviceCommandResponse.DeviceCode;
                Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (station != null)
                {
                    SafetyHelper.WriteLogInfo("【" + station.Point + "】ResetDeviceCommandResponseProc");
                    //复位命令确认
                    station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_ResetDeviceCommandRequest);
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest; ;//下发F命令
                    station.DttStateTime = DateTime.Now;
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    updateItems.Add("DttStateTime", station.DttStateTime);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                    //SafetyHelper.UpdatePointDefineInfo(station, 2);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ResetDeviceCommandResponseProc Error:" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 修改设备地址处理
        /// </summary>
        /// <param name="data"></param>
        private void ModificationDeviceAddressResponseProc(MasProtocol data)
        {
            try
            {
                ModificationDeviceAdressRequest modificationDeviceAdressRequest = data.Deserialize<ModificationDeviceAdressRequest>();
                string point = modificationDeviceAdressRequest.DeviceCode;
                Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
                Dictionary<string, object> updateItems = new Dictionary<string, object>();
                if (station != null)
                {
                    station.ModificationItems = new List<EditDeviceAddressItem>();
                    station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_ModificationDeviceAddressRequest);
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                    station.IsSendFCommand = false;//2018.3.26 by

                    updateItems.Add("ModificationItems", station.ModificationItems);
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    updateItems.Add("IsSendFCommand", station.IsSendFCommand);
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                }

                //if (station.ModificationItems != null)
                //{
                //    SafetyHelper.WriteLogInfo("【" + station.Point + "】ModificationDeviceAddressResponseProc");
                //    List<EditDeviceAddressItem> editDeviceAddressItems = station.ModificationItems.FindAll(a => a.RandomCode == modificationDeviceAdressRequest.RandomCode);
                //    if (editDeviceAddressItems != null)
                //    {
                //        for (int i = editDeviceAddressItems.Count - 1; i >= 0; i--)
                //        {
                //            station.ModificationItems.Remove(editDeviceAddressItems[i]);
                //        }

                //        if (station.ModificationItems.Count > 0)
                //        {
                //            station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_ModificationDeviceAddressRequest;
                //        }
                //        else
                //        {
                //            station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_ModificationDeviceAddressRequest);
                //            station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                //        }
                //        updateItems.Add("ModificationItems", station.ModificationItems);
                //        updateItems.Add("ClsCommObj", station.ClsCommObj);
                //        SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("EditDeviceAdressResponseProc Error:" + ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 历史控制数据回发处理
        /// </summary>
        /// <param name="data"></param>
        private void QueryHistoryControlResponseProc(MasProtocol data)
        {
            QueryHistoryControlResponse queryHistoryControlResponse = data.Deserialize<QueryHistoryControlResponse>();
            string point = queryHistoryControlResponse.DeviceCode;
            Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            DateTime time = DateTime.Now;
            if (station != null)
            {
                station.HistoryControlLegacyCount = queryHistoryControlResponse.AlarmTotal;
                if (station.HistoryControlLegacyCount == 0)
                {
                    station.HistoryControlState = 3;    //获取完成
                    station.HistoryControlLegacyCount = 0;
                    station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_QueryHistoryControlRequest);
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                    station.DttStateTime = DateTime.Now;
                    station.IsSendFCommand = false;//2018.3.26 by

                    updateItems.Add("HistoryControlState", station.HistoryControlState);
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    updateItems.Add("DttStateTime", station.DttStateTime);
                    updateItems.Add("HistoryControlLegacyCount", station.HistoryControlLegacyCount);
                    updateItems.Add("IsSendFCommand", station.IsSendFCommand);
                }
                else
                {
                    station.HistoryControlState = 2;    //还要继续获取
                    station.HistoryControlLegacyCount = queryHistoryControlResponse.AlarmTotal;
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryHistoryControlRequest;
                    station.DttStateTime = DateTime.Now;
                    updateItems.Add("HistoryControlState", station.HistoryControlState);
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    updateItems.Add("DttStateTime", station.DttStateTime);
                    updateItems.Add("HistoryControlLegacyCount", station.HistoryControlLegacyCount);
                }
                if (updateItems.Count > 0)
                {
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                }
                DoDeviceHistoryControlData(station, queryHistoryControlResponse.HistoryControlItems, time);
            }
        }
        /// <summary>
        /// 历史五分钟数据回发处理     
        /// </summary>
        /// <param name="data"></param>
        private void QueryHistoryRealDataResponseProc(MasProtocol data)
        {
            QueryHistoryRealDataResponse queryHistoryRealDataResponse = data.Deserialize<QueryHistoryRealDataResponse>();
            string point = queryHistoryRealDataResponse.DeviceCode;
            Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
            List<Jc_DefInfo> defItems = SafetyHelper.GetPointDefinesByStationID(Convert.ToInt16(point.Substring(0, 3)));
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            DateTime time = data.CreatedTime;
            if (station != null)
            {
                //if (station.SerialNumber != queryHistoryRealDataResponse.)
                station.HistoryRealDataLegacyCount = queryHistoryRealDataResponse.MinuteDataTotal;
                if (station.HistoryRealDataLegacyCount == 0)
                {
                    station.HistoryRealDataState = 3; //获取完成
                    station.HistoryRealDataLegacyCount = 0;
                    station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_QueryHistoryRealDataRequest);
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令
                    station.IsSendFCommand = false;//2018.3.26 by
                    station.DttStateTime = DateTime.Now;

                    updateItems.Add("HistoryControlState", station.HistoryControlState);
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    updateItems.Add("DttStateTime", station.DttStateTime);
                    updateItems.Add("HistoryRealDataLegacyCount", station.HistoryRealDataLegacyCount);
                    updateItems.Add("IsSendFCommand", station.IsSendFCommand);
                }
                else
                {
                    station.HistoryRealDataState = 2;//还要继续获取
                    station.HistoryRealDataLegacyCount = queryHistoryRealDataResponse.MinuteDataTotal;
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryHistoryRealDataRequest;
                    station.DttStateTime = DateTime.Now;
                    updateItems.Add("HistoryControlState", station.HistoryControlState);
                    updateItems.Add("ClsCommObj", station.ClsCommObj);
                    updateItems.Add("DttStateTime", station.DttStateTime);
                    updateItems.Add("HistoryRealDataLegacyCount", station.HistoryRealDataLegacyCount);
                }
                if (updateItems.Count > 0)
                {
                    SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);
                }
                DoDeviceHistoryRealData(station, defItems, queryHistoryRealDataResponse.HistoryRealDataItems, time);
            }
        }

        private void SetSensorGradingAlarmResponseProc(MasProtocol data)
        {
            SetSensorGradingAlarmResponse response = data.Deserialize<SetSensorGradingAlarmResponse>();
            string point = response.DeviceCode;
            Jc_DefInfo station = SafetyHelper.GetPointDefinesByPoint(point);
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            DateTime time = data.CreatedTime;
            if (station != null)
            {
                station.GradingAlarmCount--;
                if (station.GradingAlarmCount > 1)
                {
                    //此时只需要下次下发最新的控制字即可
                    station.GradingAlarmCount = 1;
                }
                else if (station.GradingAlarmCount <= 0) //2018.4.15 by  下发一次命令，交换机可能回8次确认（1个模块只绑定了一个分站）
                {
                    station.GradingAlarmCount = 0;
                }
                updateItems.Add("GradingAlarmCount", station.GradingAlarmCount);
                if (station.GradingAlarmCount > 0)
                {
                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_SetSensorGradingAlarmRequest; //继续下发控制命令标记
                }
                else
                {
                    station.ClsCommObj.NCommandbz &= (ushort)(0xFFFF - CommunicationCommandValue.Comm_SetSensorGradingAlarmRequest); //不继续下发控制命令标记
                    //清除交叉控制表中upflag = 0 && type = 12 && fzh 的记录  2018.3.12 by
                    ControlBus.DeleteManualCrossControlByZkpointByFzhTypeUpflag(station.Fzh.ToString().PadLeft(3, '0'), 12, "0");

                    station.ClsCommObj.NCommandbz |= CommunicationCommandValue.Comm_QueryRealDataRequest;//下发F命令

                    station.IsSendFCommand = false;
                }
                updateItems.Add("ClsCommObj", station.ClsCommObj);
                updateItems.Add("IsSendFCommand", station.IsSendFCommand);
                SafetyHelper.UpdatePointDefineInfoByProperties(station.PointID, updateItems);



            }
        }

        #endregion

        #region 生成驱动下发结构体

        /// <summary>
        /// 生成通讯测试结构体
        /// </summary>
        /// <param name="def">分站信息</param>
        /// <returns></returns>
        private MasProtocol GetCommunicationTestRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.CommunicationTestRequest);
            //CommunicationTestRequest protocol = new CommunicationTestRequest();//todo:删除不要
            //protocol.DeviceCode = def.Point;
            //masProtocol.Protocol = protocol;
            return masProtocol;
        }
        /// <summary>
        /// 生成设备控制命令
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private MasProtocol GetDeviceControlRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.DeviceControlRequest);

            DeviceControlRequest protocol = new DeviceControlRequest();
            protocol.DeviceCode = def.Point.ToString();
            #region 结构体赋值
            //赋值控制列表
            protocol.ControlChanels = new List<DeviceControlItem>();
            DeviceControlItem deviceControlItem;
            if (def.DeviceControlItems != null)
            {
                foreach (ControlItem item in def.DeviceControlItems)
                {
                    deviceControlItem = new DeviceControlItem();
                    deviceControlItem.Channel = item.Channel;
                    deviceControlItem.ControlType = item.ControlType;
                    protocol.ControlChanels.Add(deviceControlItem);
                }
            }
            //赋值唯一编码确认列表
            protocol.SoleCodingChanels = new List<DeviceControlItem>();
            if (def.SoleCodingChanels != null)
            {
                foreach (ControlItem item in def.SoleCodingChanels)
                {
                    deviceControlItem = new DeviceControlItem();
                    deviceControlItem.Channel = item.Channel;
                    deviceControlItem.ControlType = item.ControlType;
                    protocol.SoleCodingChanels.Add(deviceControlItem);
                }
            }
            // 表示控制分站下发报警、断电、复电值至传感器 (暂不启用，默认0)
            byte SensorParaControl = 0;
            SettingInfo settingItem = SafetyHelper.GetSettingByKeyStr("SensorParaControl");
            if (settingItem != null)
            {
                if (!byte.TryParse(settingItem.StrValue, out SensorParaControl))
                {
                    LogHelper.Error("未找到配置项【分站下发报警、断电、复电值至传感器】,取默认值0");
                }
            }
            else
            {
                LogHelper.Error("未找到配置项【分站下发报警、断电、复电值至传感器】,取默认值0");
            }
            protocol.SensorParaControl = SensorParaControl;
            //瓦电3分强制解锁标记
            protocol.GasThreeUnlockContro = def.GasThreeUnlockContro;
            //清除分站历史数据标记
            protocol.ClearHistoryData = def.StationHisDataClear;
            #endregion
            masProtocol.Protocol = protocol;
            return masProtocol;
        }

        /// <summary>
        /// 生成初始化命令
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private MasProtocol GetInitializeRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.InitializeRequest);
            InitializeRequest protocol = new InitializeRequest();
            protocol.DeviceCode = def.Point;

            List<SettingInfo> settingItems = SafetyHelper.GetAllSetting();
            SettingInfo settingItem;
            byte faultBlockTime = 10;
            byte feedThreshold = 5;
            byte impowerJoinUp = 0;

            if (settingItems != null && settingItems.Count > 0)
            {
                #region ----取配置信息----
                try
                {
                    //分站主通讯故障闭锁输出延时
                    settingItem = settingItems.FirstOrDefault(a => a.StrKey == "FeedResponseTime");
                    if (settingItem != null)
                    {
                        faultBlockTime = Convert.ToByte(settingItem.StrValue);
                    }
                    else
                    {
                        LogHelper.Error("未从配置中获取到【分站主通讯故障闭锁输出延时】");
                    }
                    //分站的馈电识别阈值
                    settingItem = settingItems.FirstOrDefault(a => a.StrKey == "SubstationCommuFaultTime");
                    if (settingItem != null)
                    {
                        feedThreshold = Convert.ToByte(settingItem.StrValue);
                    }
                    else
                    {
                        LogHelper.Error("未从配置中获取到【分站的馈电识别阈值】");
                    }
                    //是否允许接入第三方的传感器
                    settingItem = settingItems.FirstOrDefault(a => a.StrKey == "SationImpowerJoinUp");
                    if (settingItem != null)
                    {
                        impowerJoinUp = Convert.ToByte(settingItem.StrValue);
                    }
                    else
                    {
                        LogHelper.Error("未从配置中获取到【否允许接入第三方的传感器】");
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("配置项【分站主通讯故障闭锁输出延时】、【分站的馈电识别阈值】、【否允许接入第三方的传感器】值异常，" + ex.Message + ex.StackTrace);
                }
                #endregion
            }
            else
            {
                LogHelper.Error("未从配置中获取到【分站主通讯故障闭锁输出延时】、【分站的馈电识别阈值】、【否允许接入第三方的传感器】");
            }

            //设置分站主通讯故障闭锁输出延时，单位：秒
            protocol.FaultBlockTime = faultBlockTime;
            // 设置分站的馈电识别阈值，秒
            protocol.FeedThreshold = feedThreshold;
            // 瓦电3分强制解锁标记=1表示解除，=0表示不解除
            protocol.GasThreeUnlockContro = def.GasThreeUnlockContro;
            // 表示是否允许接入第三方的传感器，=1表示允许，=0表示不允许
            protocol.ImpowerJoinUp = impowerJoinUp;

            //赋值控制列表
            protocol.ControlChanels = new List<DeviceControlItem>();
            DeviceControlItem deviceControlItem;
            if (def.DeviceControlItems != null)
            {
                foreach (ControlItem item in def.DeviceControlItems)
                {
                    deviceControlItem = new DeviceControlItem();
                    deviceControlItem.Channel = item.Channel;
                    deviceControlItem.ControlType = item.ControlType;
                    protocol.ControlChanels.Add(deviceControlItem);
                }
            }

            masProtocol.Protocol = protocol;
            return masProtocol;
        }
        /// <summary>
        /// 生成获取电源箱实时数据命令
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private MasProtocol GetQueryBatteryRealDataRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.QueryBatteryRealDataRequest);
            QueryBatteryRealDataRequest protocol = new QueryBatteryRealDataRequest();
            protocol.DeviceCode = def.Point;
            protocol.DevProperty = ItemDevProperty.Substation;
            //protocol.BatteryControl = (byte)(def.BDisCharge ? 1 : 0);
            protocol.BatteryControl = def.BDisCharge;
            try
            {
                SettingInfo settingItem = SafetyHelper.GetSettingByKeyStr("DevicePowerPercentum");
                if (settingItem != null)
                {
                    protocol.PowerPercentum = Convert.ToByte(settingItem.StrValue);
                }
                else
                {
                    protocol.PowerPercentum = 20;
                    LogHelper.Error("未找到配置项【放电时，切换的百分比】,取默认值20%");
                }
            }
            catch (Exception ex)
            {
                protocol.PowerPercentum = 20;
                LogHelper.Error("配置项【放电时，切换的百分比】值错误,取默认值20%;" + ex.Message + ex.StackTrace);
            }


            masProtocol.Protocol = protocol;
            return masProtocol;
        }
        /// <summary>
        /// 生成获取设备的详细信息
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private MasProtocol GetQueryDeviceInfoRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.QueryDeviceInfoRequest);
            GetDeviceInformationRequest protocol = new GetDeviceInformationRequest();
            protocol.DeviceCode = def.Point;
            protocol.GetAddressLst = def.GetDeviceDetailDtataAddressLst;
            masProtocol.Protocol = protocol;
            return masProtocol;
        }
        /// <summary>
        /// 生成取数命令
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private MasProtocol GetQueryRealDataRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.QueryRealDataRequest);
            QueryRealDataRequest protocol = new QueryRealDataRequest();
            protocol.DeviceCode = def.Point;

            protocol.LastAcceptFlag = (byte)def.LastAcceptFlag;//赋值

            #region 结构体赋值
            //赋值控制列表
            protocol.ControlChanels = new List<DeviceControlItem>();
            DeviceControlItem deviceControlItem;
            if (def.DeviceControlItems != null)
            {
                foreach (ControlItem item in def.DeviceControlItems)
                {
                    deviceControlItem = new DeviceControlItem();
                    deviceControlItem.Channel = item.Channel;
                    deviceControlItem.ControlType = item.ControlType;
                    protocol.ControlChanels.Add(deviceControlItem);
                }
            }
            //赋值唯一编码确认列表
            protocol.SoleCodingChanels = new List<DeviceControlItem>();
            if (def.SoleCodingChanels != null)
            {
                foreach (ControlItem item in def.SoleCodingChanels)
                {
                    deviceControlItem = new DeviceControlItem();
                    deviceControlItem.Channel = item.Channel;
                    deviceControlItem.ControlType = item.ControlType;
                    protocol.SoleCodingChanels.Add(deviceControlItem);
                }
            }
            // 表示控制分站下发报警、断电、复电值至传感器 (暂不启用，默认0)
            byte SensorParaControl = 0;
            SettingInfo settingItem = SafetyHelper.GetSettingByKeyStr("SensorParaControl");
            if (settingItem != null)
            {
                if (!byte.TryParse(settingItem.StrValue, out SensorParaControl))
                {
                    LogHelper.Error("未找到配置项【分站下发报警、断电、复电值至传感器】,取默认值0");
                }
            }
            else
            {
                LogHelper.Error("未找到配置项【分站下发报警、断电、复电值至传感器】,取默认值0");
            }
            protocol.SensorParaControl = SensorParaControl;
            //瓦电3分强制解锁标记
            protocol.GasThreeUnlockContro = def.GasThreeUnlockContro;
            //电源箱充放电
            protocol.BDisCharge = def.BDisCharge;
            //获取唯一编码标记 2018.3.26 by
            protocol.GetDeviceInfoCoding = def.GetDeviceSoleCoding;

            //清除分站历史数据标记
            //protocol.ClearHistoryData = def.StationHisDataClear;
            #endregion

            masProtocol.Protocol = protocol;

            def.IsSendFCommand = true;
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            updateItems.Add("IsSendFCommand", def.IsSendFCommand);

            //赋值下发F命令标记  20181123
            def.ClsCommObj.NCommandbz |= (ushort)(CommunicationCommandValue.Comm_QueryRealDataRequest);
            def.ClsCommObj.BInit = true;
            updateItems.Add("ClsCommObj", def.ClsCommObj);

            SafetyHelper.UpdatePointDefineInfoByProperties(def.PointID, updateItems);

            //LogHelper.Info("【FCommand-" + def.Point + "】(" + def.GetDeviceSoleCoding + ")下发F命令，下发标记置为True。");

            return masProtocol;
        }
        /// <summary>
        /// 生成复位命令
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private MasProtocol GetResetDeviceCommandRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.ResetDeviceCommandRequest);
            QueryRealDataRequest protocol = new QueryRealDataRequest();
            protocol.DeviceCode = def.Point;
            masProtocol.Protocol = protocol;
            return masProtocol;
        }
        /// <summary>
        /// 获取分站历史数据
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private MasProtocol GetQueryHistoryControlRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.QueryHistoryControlRequest);
            QueryHistoryControlRequest protocol = new QueryHistoryControlRequest();
            protocol.DeviceCode = def.Point;
            masProtocol.Protocol = protocol;
            return masProtocol;
        }
        /// <summary>
        /// 生成修改设备地址号结构体
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private MasProtocol GetModificationDeviceAddressRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.ModificationDeviceAdressRequest);
            ModificationDeviceAdressRequest protocol = new ModificationDeviceAdressRequest();
            protocol.ModificationItems = new List<EditDeviceAdressItem>();
            EditDeviceAdressItem editDeviceAdressItem;
            protocol.DeviceCode = def.Point;
            if (def.ModificationItems.Count > 0)
            {
                //默认每次下发链表中的第一个
                protocol.RandomCode = def.ModificationItems[0].RandomCode;
                foreach (DeviceAddressItem item in def.ModificationItems[0].DeviceAddressItems)
                {
                    editDeviceAdressItem = new EditDeviceAdressItem();
                    editDeviceAdressItem.SoleCoding = item.SoleCoding;
                    editDeviceAdressItem.AfterModification = item.AfterModification;
                    editDeviceAdressItem.BeforeModification = item.BeforeModification;
                    editDeviceAdressItem.DeviceType = item.DeviceType;
                    protocol.ModificationItems.Add(editDeviceAdressItem);
                }
            }
            masProtocol.Protocol = protocol;
            return masProtocol;
        }

        /// <summary>
        /// 获取历史5分钟数据
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        private MasProtocol GetQueryHistoryRealDataRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.QueryHistoryRealDataRequest);
            QueryHistoryRealDataRequest protocol = new QueryHistoryRealDataRequest();
            protocol.DeviceCode = def.Point;
            protocol.SerialNumber = def.SerialNumber;
            masProtocol.Protocol = protocol;
            return masProtocol;
        }

        /// <summary>
        /// 分级报警的控制命令 2018.2.2 by
        /// </summary>
        /// <param name="def"></param>
        /// <param name="controlItems"></param>
        /// <returns></returns>
        private MasProtocol SetSensorGradingAlarmRequest(Jc_DefInfo def)
        {
            MasProtocol masProtocol = new MasProtocol(SystemType.Security, DirectionType.Down, ProtocolType.SetSensorGradingAlarmRequest);

            SetSensorGradingAlarmRequest protocol = new SetSensorGradingAlarmRequest();
            protocol.DeviceCode = def.Point;
            List<SensorGradingAlarmItem> items = new List<SensorGradingAlarmItem>();
            SensorGradingAlarmItem item;
            foreach (GradingAlarmItem aItem in def.GradingAlarmItems)
            {
                item = new SensorGradingAlarmItem();
                item.Channel = (aItem.kh - 1).ToString();
                if (aItem.grade == 0)
                {
                    item.AlarmStep = (byte)aItem.grade;
                }
                else
                {
                    item.AlarmStep = (byte)(5 - aItem.grade);
                }
                items.Add(item);
            }
            if (items.Count == 0)
            {
                return null;
            }
            protocol.GradingAlarmItems = items;
            masProtocol.Protocol = protocol;
            LogHelper.Info("【" + def.Point + "】SetSensorGradingAlarmRequest");
            return masProtocol;
        }
        #endregion

        #region ----私有方法----
        /// <summary>
        /// 更新分站下属测点定义状态标记
        /// </summary>
        /// <param name="station"></param>
        private void UpdatePointEditState(Jc_DefInfo station)
        {
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            List<Jc_DefInfo> defItems = SafetyHelper.GetPointDefinesByStationID(station.Fzh);
            defItems.ForEach(item =>
            {
                updateItems = new Dictionary<string, object>();
                updateItems.Add("PointEditState", 0);
                SafetyHelper.UpdatePointDefineInfoByProperties(item.PointID, updateItems);//hdw:20170719-批量更新
            });
            defItems = SafetyHelper.GetPointDefinesByStationID(station.Fzh);//hdw:20170719:去掉
        }

        /// <summary>
        /// 处理历史五分钟数据
        /// </summary>
        /// <param name="station"></param>
        /// <param name="_deviceHistoryRealDataItems"></param>
        /// <param name="time"></param>
        private void DoDeviceHistoryRealData(Jc_DefInfo station, List<Jc_DefInfo> defItems, List<DeviceHistoryRealDataItem> _deviceHistoryRealDataItems, DateTime time)
        {
            List<DeviceHistoryRealDataItem> deviceHistoryRealDataItems = _deviceHistoryRealDataItems;

            List<Jc_DefInfo> multi_ParameterDevice;

            string point = "";
            foreach (DeviceHistoryRealDataItem item in deviceHistoryRealDataItems)
            {
                //数据处理
                if (item.DeviceProperty == ItemDevProperty.Substation)
                {
                    //若为分站，则说明此通道是设备中断，后续不带设备信息
                    multi_ParameterDevice = defItems.Where(a => a.Fzh == station.Fzh && a.Kh.ToString() == item.Channel).ToList();
                    if (multi_ParameterDevice.Count == 0)
                    {
                        //状态为断线，定义中无数据，默认中间类型为？
                        point = station.Fzh.ToString().PadLeft(3, '0') + "?" + item.Channel.ToString().PadLeft(2, '0') + item.Address == null ? "0" : item.Address; //GetPointStrByInfo(station.Fzh.ToString(), item.Channel, item.Address, item.DeviceProperty);
                        //updateStaionHistoryDataItems.Add(FiveMinBusiness.GetStaionHistoryData(station.Fzh, point, item, time));
                        updateStaionHistoryDataItems.Enqueue(FiveMinBusiness.GetStaionHistoryData(station.Fzh, point, item, time));
                    }
                    else
                    {
                        foreach (Jc_DefInfo def in multi_ParameterDevice)
                        {
                            //状态为断线，定义中有数据，以定义数据为准
                            //updateStaionHistoryDataItems.Add(FiveMinBusiness.GetStaionHistoryData(station.Fzh, def.Point, item, time));
                            updateStaionHistoryDataItems.Enqueue(FiveMinBusiness.GetStaionHistoryData(station.Fzh, def.Point, item, time));
                        }
                    }
                }
                else
                {
                    point = GetPointStrByInfo(station.Fzh.ToString(), item.Channel, item.Address, item.DeviceProperty);
                    //updateStaionHistoryDataItems.Add(FiveMinBusiness.GetStaionHistoryData(station.Fzh, point, item, time));
                    updateStaionHistoryDataItems.Enqueue(FiveMinBusiness.GetStaionHistoryData(station.Fzh, point, item, time));
                }
            }
            //批量入库
            //FiveMinBusiness.StaionHistoryDataToDB(updateStaionHistoryDataItems);
        }
        /// <summary>
        /// 处理历史控制数据
        /// </summary>
        /// <param name="station"></param>
        /// <param name="_deviceHistoryControlItem"></param>
        /// <param name="time"></param>
        private void DoDeviceHistoryControlData(Jc_DefInfo station, List<DeviceHistoryControlItem> _deviceHistoryControlItem, DateTime time)
        {
            List<DeviceHistoryControlItem> deviceHistoryControlItem = _deviceHistoryControlItem;
            List<StaionControlHistoryDataInfo> updateStaionControlHistoryDataItems = new List<StaionControlHistoryDataInfo>();
            string point = "";
            foreach (DeviceHistoryControlItem item in deviceHistoryControlItem)
            {
                // 数据处理
                point = GetPointStrByInfo(station.Fzh.ToString(), item.Channel, item.Address, item.DeviceProperty);
                updateStaionControlHistoryDataItems.Add(ControlBus.GetStaionControlHistoryData(station.Fzh.ToString(), point, item, time));
            }
            //批量入库
            ControlBus.StaionControlHistoryDataToDB(updateStaionControlHistoryDataItems);
        }

        /// <summary>
        /// 根据fzh,kh,dzh,设备类型生成point
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="kh"></param>
        /// <param name="dzh"></param>
        /// <param name="devProperty"></param>
        /// <returns></returns>
        private string GetPointStrByInfo(string fzh, string kh, string dzh, ItemDevProperty devProperty)
        {
            string pointStr = "";
            string mark = "0";
            switch (devProperty)
            {
                case ItemDevProperty.Analog:
                    mark = "A";
                    break;
                case ItemDevProperty.Derail:
                    mark = "D";
                    break;
                case ItemDevProperty.Control:
                    mark = "C";
                    break;
                case ItemDevProperty.Substation:
                    mark = "0";
                    kh = "";
                    break;
            }
            pointStr = fzh.PadLeft(3, '0') + mark + kh.PadLeft(2, '0') + (dzh == null ? "0" : dzh);
            return pointStr;
        }

        /// <summary>
        /// 创建日表（昨天、今天、明天;当判断到网关回发的时间或抽放时间与当前天日期不相等，直接按照网关或抽放时间建表）
        /// </summary>
        /// <param name="DataTime"></param>
        private void CreatDayTable(DateTime DataTime)
        {
            try
            {
                IPointDefineRepository pointDefineRepository = ServiceFactory.Create<IPointDefineRepository>();
                //跨天建表修改,由于存在月表,需要创建上个月及下个月的月表  20170703
                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.AddMonths(-1).ToString("yyyyMM") + "01");//默认创建一次上个月一号的表,通过此方法来创建上月月表
                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.AddMonths(1).ToString("yyyyMM") + "01");//默认创建一次下个月一号的表,通过此方法来创建下月月表

                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.AddDays(-1).ToString("yyyyMMdd"));
                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.ToString("yyyyMMdd"));
                pointDefineRepository.ExecuteNonQuery("CreateDateTable", DataTime.AddDays(+1).ToString("yyyyMMdd"));
            }
            catch (Exception ex)
            {
                LogHelper.Error("CreatDayTable Error:" + ex.Message + ex.StackTrace);
            }
        }

        private bool IsRightBranchNumber(int kh, int branchNumber)
        {
            if (kh == 0)
            {
                return true;//分站直接返回true
            }
            if ((kh - 1) / 4 + 1 == branchNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 模拟量峰值过滤
        /// </summary>
        /// <param name="defInfo"></param>
        /// <param name="dataItem"></param>
        private void AnalogPeakFilter(Jc_DefInfo defInfo, List<Jc_DevInfo> devItems, ref RealDataItem dataItem, int myAbnormalCount)
        {
            if (defInfo.DataState != (short)DeviceDataState.EquipmentStateUnknow)
            {
                double ssz, realData;
                double xs = 0.05; //量程系数
                if (defInfo.Ssz.Trim() != dataItem.RealData.Trim())
                {
                    if (double.TryParse(defInfo.Ssz, out ssz) && double.TryParse(dataItem.RealData, out realData))
                    {
                        Jc_DevInfo devItem = devItems.FirstOrDefault(a => a.Devid == defInfo.Devid);
                        if (devItem != null)
                        {
                            /* 变化系统说明：
                             *  0-500        0.05
                             *  500-1000     0.04
                             *  1000+        0.02
                             * 
                             * 判断条件：
                             *  当前值-上次值>=量程*变化系数  且  当前值-上次值>= 上次值*0.5   同时 已过滤次数<过滤最大次数
                             */
                            //2017.10.13 by
                            if (devItem.LC >= 1000)
                            {
                                xs = 0.02;
                            }
                            else if (devItem.LC >= 500)
                            {
                                xs = 0.04;
                            }
                            if (((realData - ssz) >= devItem.LC * xs) && ((realData - ssz) >= ssz * 0.5f))
                            {
                                if (defInfo.AbnormalCount < myAbnormalCount)
                                {
                                    defInfo.AbnormalCount++;
                                    OperateLogHelper.InsertOperateLog(63, "数据第" + defInfo.AbnormalCount + "次异常：测点【" + defInfo.Point + "】，类型【" + defInfo.DevName + "】，位置【" + defInfo.Wz + "】，实时值【" + realData + "】，替换值【" + ssz + "】，时间【" + DateTime.Now + "】", "");
                                    dataItem.RealData = defInfo.Ssz;
                                }
                            }
                            else
                            {
                                defInfo.AbnormalCount = 0;
                            }
                        }
                        else
                        {
                            LogHelper.Error("PeakFilter 未找到设备 Devid = " + defInfo.Devid);
                        }
                    }
                    else
                    {
                        defInfo.AbnormalCount = 0;
                    }
                }
                else
                {
                    defInfo.AbnormalCount = 0;
                }
            }
        }
        /// <summary>
        /// 欠压报警处理
        /// </summary>
        /// <param name="defInfo"></param>
        /// <param name="dataItem"></param>
        private void UnderVoltageAlarm(Jc_DefInfo defInfo, ref RealDataItem dataItem)
        {
            float voltage = 0;
            float.TryParse(dataItem.Voltage, out voltage);
            if (defInfo.Bz5 > 0 && voltage > 0)
            {
                if (voltage < defInfo.Bz5)
                {
                    dataItem.State = ItemState.UnderVoltageAlarm;
                }
            }
        }
        private void SensorChange(Jc_DefInfo defInfo, ref RealDataItem dataItem)
        {
            if (dataItem.ChangeSenior == 1)
            {
                dataItem.State = ItemState.SensorChangeing;
            }           
        }
        /// <summary>
        /// 线性突变
        /// </summary>
        /// <param name="defInfo"></param>
        /// <param name="devItems"></param>
        /// <param name="dataItem"></param>
        /// <param name="myAbnormalCount"></param>
        private void AnalogMutation(Jc_DefInfo defInfo, List<Jc_DevInfo> devItems, ref RealDataItem dataItem, int myAbnormalCount)
        {
            if (defInfo.DataState != (short)DeviceDataState.EquipmentStateUnknow
                && defInfo.State != (short)DeviceRunState.EquipmentStart
                && defInfo.DataState != (short)DeviceDataState.EquipmentStart)
            {
                if (dataItem.State != ItemState.EquipmentAdjusting)
                {
                    double ssz, realData;
                    double xs = 0.05; //量程系数  RangeCoefficient1
                    SettingInfo settingItem = SafetyHelper.GetSettingByKeyStr("RangeCoefficient1");
                    if (settingItem != null)
                    {
                        xs = Convert.ToDouble(settingItem.StrValue);
                    }
                    else
                    {
                        LogHelper.Error("AnalogMutation Error:缺少配置项 RangeCoefficient1");
                    }
                    if (defInfo == null || dataItem == null || defInfo.Ssz == null || dataItem.RealData == null)
                    {
                        return;
                    }
                    if (defInfo.Ssz.Trim() != dataItem.RealData.Trim())
                    {
                        if (double.TryParse(defInfo.Ssz, out ssz) && double.TryParse(dataItem.RealData, out realData))
                        {
                            Jc_DevInfo devItem = devItems.FirstOrDefault(a => a.Devid == defInfo.Devid);
                            if (devItem != null)
                            {
                                /* 变化系统说明：
                                 *  0-500        0.05
                                 *  500-1000     0.04
                                 *  1000+        0.02
                                 * 
                                 * 判断条件：
                                 *  当前值-上次值>=量程*变化系数  且  当前值-上次值>= 上次值*0.5   同时 已过滤次数<过滤最大次数
                                 */
                                //2017.10.13 by
                                if (devItem.LC >= 1000)
                                {
                                    xs = 0.02;
                                    settingItem = SafetyHelper.GetSettingByKeyStr("RangeCoefficient2");
                                    if (settingItem != null)
                                    {
                                        xs = Convert.ToDouble(settingItem.StrValue);
                                    }
                                    else
                                    {
                                        LogHelper.Error("AnalogMutation Error:缺少配置项 RangeCoefficient2");
                                    }
                                }
                                else if (devItem.LC >= 500)
                                {
                                    xs = 0.04;
                                    settingItem = SafetyHelper.GetSettingByKeyStr("RangeCoefficient3");
                                    if (settingItem != null)
                                    {
                                        xs = Convert.ToDouble(settingItem.StrValue);
                                    }
                                    else
                                    {
                                        LogHelper.Error("AnalogMutation Error:缺少配置项 RangeCoefficient3");
                                    }
                                }
                                if (((realData - ssz) >= devItem.LC * xs) && ((realData - ssz) >= ssz * 0.5f))
                                {
                                    dataItem.State = ItemState.EquipmentChange;
                                }
                            }
                            else
                            {
                                LogHelper.Error("PeakFilter 未找到设备 Devid = " + defInfo.Devid);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 开关量量峰值过滤
        /// </summary>
        /// <param name="defInfo"></param>
        /// <param name="dataItem"></param>
        /// <param name="myAbnormalCount"></param>
        private void DerailPeakFilter(Jc_DefInfo defInfo, ref RealDataItem dataItem, int myAbnormalCount)
        {
            if (defInfo.DataState != (short)DeviceDataState.EquipmentStateUnknow)
            {
                DeviceDataState datastate = DeviceDataState.EquipmentStateUnknow;
                if (dataItem.RealData == "1")
                {
                    datastate = DeviceDataState.DataDerailState1;
                }
                else if (dataItem.RealData == "2")
                {
                    datastate = DeviceDataState.DataDerailState2;
                }
                else
                {
                    datastate = DeviceDataState.DataDerailState0;
                }

                if (defInfo.DataState != (short)datastate)
                {
                    if (defInfo.AbnormalCount < myAbnormalCount)
                    {
                        defInfo.AbnormalCount++;
                        OperateLogHelper.InsertOperateLog(63, "数据第" + defInfo.AbnormalCount + "次异常：测点【" + defInfo.Point + "】，类型【" + defInfo.DevName + "】，位置【" + defInfo.Wz + "】，实时值【" + (short)datastate + "】，替换值【" + defInfo.DataState + "】，时间【" + DateTime.Now + "】", "");

                        if ((DeviceDataState)defInfo.DataState == DeviceDataState.DataDerailState1) { dataItem.RealData = "1"; }
                        else if ((DeviceDataState)defInfo.DataState == DeviceDataState.DataDerailState2) { dataItem.RealData = "2"; }
                        else { dataItem.RealData = "0"; }
                    }
                }
                else
                {
                    defInfo.AbnormalCount = 0;
                }
            }
        }

        /// <summary>
        /// 获取本分站的峰值过滤容错次数
        /// </summary>
        /// <param name="fzh"></param>
        /// <returns></returns>
        private int GetMyAbnormalCount(Jc_DefInfo station)
        {
            int myAbnormalCount = 2;
            try
            {
                Jc_MacInfo macItem = SafetyHelper.GetMacItemByMac(station.Jckz1);
                int stationCount = 0;
                if (macItem != null)
                {
                    SettingInfo settingItem = SafetyHelper.GetSettingByKeyStr("PorcDataTime");
                    if (settingItem != null)
                    {
                        myAbnormalCount = Convert.ToInt32(settingItem.StrValue);
                        string[] fzhItems = macItem.Bz1.Split('|');

                        foreach (string fzh in fzhItems)
                        {
                            if (fzh.Trim() != "" && fzh.Trim() != "0")
                            {
                                stationCount++;
                            }
                        }

                        if ((8 % stationCount) > 0)
                        {
                            myAbnormalCount *= (8 / stationCount) + 1;
                        }
                        else
                        {
                            myAbnormalCount *= (8 / stationCount);
                        }
                    }
                    else
                    {
                        LogHelper.Error("数据库缺少峰值过滤次数配置项PorcDataTime，取默认值2！");
                    }
                }
                else
                {
                    LogHelper.Error("未找到mac信息(fzh=" + station.Fzh + ";mac=" + station.Jckz1 + ")，取默认值2！");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetMyAbnormalCount" + ex.Message + ex.StackTrace);
            }

            return myAbnormalCount;
        }
        /// <summary>
        /// 没有唯一编码的设备生其唯一编码（年以3开头）
        /// </summary>
        /// <param name="fzh"></param>
        /// <param name="branch"></param>
        /// <param name="kh"></param>
        /// <returns></returns>
        private string CreateSoleCoding(string fzh, string branch, string kh)
        {
            //3 + 分站号3 + 01 + 01 + 分支号2 + 口号2
            return "3" + fzh.PadLeft(3, '0') + "01" + "01" + branch.PadLeft(2, '0') + kh.PadLeft(2, '0');
        }
        #endregion

        /// <summary>
        /// 数据状态测试
        /// </summary>
        /// <param name="item"></param>
        /// <param name="defInfo"></param>
        private void Test(ref RealDataItem item, Jc_DefInfo defInfo)
        {
            LogHelper.Info("【测试代码已开启，正常使用时请关闭测试代码】");
            if (defInfo.Point == "002A010")
            {
                //item.State = ItemState.EquipmentStart;
                double value = 0;
                double.TryParse(item.RealData, out value);

                if (value > 2)
                {
                    item.State = ItemState.EquipmentTypeError;
                }
                else
                {
                    item.State = ItemState.EquipmentCommOK;
                }
            }
        }
    }
}
