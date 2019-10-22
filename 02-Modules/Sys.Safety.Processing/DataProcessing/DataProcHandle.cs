using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Basic.Framework.Rpc;
using System.Threading;
using Sys.DataCollection.Common.Protocols;
using Sys.Safety.Processing.Rpc;
using Sys.Safety.ServiceContract.Cache;
using Basic.Framework.Service;
using Sys.Safety.Request.Cache;
using Sys.Safety.DataContract;
using Sys.DataCollection.Common.Rpc;
using Basic.Framework.Logging;
using Sys.Safety.Interface;
using Sys.Safety.DataContract.CommunicateExtend;
using Sys.Safety.ServiceContract.DataToDb;
using Sys.Safety.Request.DataToDb;

using Sys.Safety.Model;
using Sys.Safety.ServiceContract;
using Sys.Safety.Request.Jc_B;
using Sys.Safety.Request.RemoteState;
using Sys.Safety.Enums;
using Sys.Safety.Request.Config;
using Basic.Framework.Web;
using Sys.DataCollection.Common.Protocols.Devices;
using Basic.Framework.Common;
using Sys.Safety.ServiceContract.Driver;
using Sys.Safety.Request.Driver;
using Sys.Safety.ServiceContract.KJ237Cache;
using Sys.Safety.Request.PersonCache;
using Sys.Safety.Request.PointDefine;
using System.Windows.Forms;
using System.Data;
using Sys.Safety.Request.NetworkModule;
using System.Net.NetworkInformation;


namespace Sys.Safety.Processing.DataProcessing
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-25
    /// 描述:数据处理模块
    /// 修改记录
    /// 2017-05-25
    /// </summary>
    public class DataProcHandle
    {
        /// <summary>
        /// 数据处理线程是否已退出 2017.9.10 by  与数据处理线程开启数对应
        /// </summary>
        bool[] isExit = { false, false, false };
        /// <summary>
        /// 多线程创建单例锁
        /// </summary>
        protected static readonly object obj = new object();
        private static volatile DataProcHandle getInstance;
        IRemoteStateService remoteStateService = null;

        /// <summary>
        /// 设备型号单例
        /// </summary>
        public static DataProcHandle Instance
        {
            get
            {
                LogHelper.Info("DataProcHandle Instance start");
                if (getInstance == null)
                {
                    lock (obj)
                    {
                        if (getInstance == null)
                        {
                            getInstance = new DataProcHandle();
                        }
                    }
                }
                LogHelper.Info("DataProcHandle Instance end");
                return getInstance;
            }
        }

        #region ----私有变量----
        /// <summary>
        /// 远程RPC服务器IP
        /// </summary>
        static string _rpcRemoteIp = "127.0.0.1";
        /// <summary>
        /// 远程RPC服务器端口号
        /// </summary>
        static int _rpcRemotePort = 10004;
        /// <summary>
        /// 自己做为RPC服务器的IP
        /// </summary>
        static string _rpcLocalIp = "127.0.0.1";
        /// <summary>
        /// 自己做为RPC服务器的端口号
        /// </summary>
        static int _rpcLocalPort = 10003;
        /// <summary>
        /// 心跳中断时间（单位：秒）
        /// </summary>
        static int Heartbeat = 60;
        /// <summary>
        /// 心跳时间
        /// </summary>
        DateTime gatewayTime = DateTime.Now;
        /// <summary>
        /// 网关当前状态
        /// </summary>
        bool gatewayState = false;
        /// <summary>
        /// 数据接收队列
        /// </summary>
        public static List<Queue<MasProtocol>> ArriveDataList = new List<Queue<MasProtocol>>();
        /// <summary>
        /// 数据处理线程
        /// </summary>
        List<Thread> DataProcThread = new List<Thread>();
        /// <summary>
        /// 数据处理线程数量
        /// </summary>
        int DataProcThreadCount = 1;
        /// <summary>
        /// 数据下发线程
        /// </summary>
        Thread DataSendThread = null;
        Thread SwitchesDataSendThread = null;
        Thread StationQueryDataSendThread = null;
        Thread ClearUnDefineThread = null;
        Thread SwitchQueryDataSendThread = null;
        Thread SwitchStatePingGetThread = null;//20191007
        /// <summary>
        /// 分站中断处理线程  20180822
        /// </summary>
        Thread StaionInterruptThread = null;

        /// <summary>
        /// ArriveData读写锁
        /// </summary>
        private static List<object> _lockObject = new List<object>();
        /// <summary>
        /// 单路网络模块最大挂接分站数
        /// </summary>
        static int netStaionListCount = 1;
        /// <summary>
        /// 是否停止线程工作
        /// </summary>
        bool isStop = false;
        /// <summary>
        /// 跨天、五分钟数据处理线程
        /// </summary>
        CrossDayAndFiveMiniteHandle crossDayAndFiveMiniteHandle = null;

        /// <summary>
        /// 接收数据包问题
        /// </summary>
        long _receivedDataCount = 0;

        /// <summary>
        /// 系统正常运行时处理一些基本事务线程 2018.2.27 by
        /// </summary>
        Thread SysRunThread = null;
        private bool sysrunFlag = true;
        /// <summary>
        /// 控制电源箱线程  201.3.12 by
        /// </summary>
        Thread PowerBoxControlThread = null;

        private Thread syncThread;
        private DateTime personLastSyncTime;
        private DateTime broadcastLastSyncTime;
        private bool personSyncTimeOutFlag = false;
        private bool broadcastSyncTimeOutFlag = false;

        private IRealMessageService realMessageService;
        private INetworkModuleService _NetworkModuleService;
        private INetworkModuleCacheService _NetworkModuleCacheService;
        #endregion
        #region ----数据入库模块----
        IInsertToDbService<Jc_Ll_DInfo> Jc_Ll_DToDbService = null;
        IInsertToDbService<Jc_Ll_HInfo> Jc_Ll_HToDbService = null;
        IInsertToDbService<Jc_Ll_MInfo> Jc_Ll_MToDbService = null;
        IInsertToDbService<Jc_Ll_YInfo> Jc_Ll_YToDbService = null;
        IInsertToDbService<Jc_BInfo> Jc_BInfoToDbService = null;
        IInsertToDbService<Jc_KdInfo> Jc_KdInfoToDbService = null;
        IInsertToDbService<Jc_MInfo> Jc_MInfoToDbService = null;
        IInsertToDbService<Jc_McInfo> Jc_McInfoToDbService = null;
        IInsertToDbService<Jc_RInfo> Jc_RInfoToDbService = null;
        IInsertToDbService<R_PbInfo> R_PBInfoToDbService = null;
        IInsertToDbService<R_PhistoryInfo> R_PHistoryToDbService = null;
        IInsertToDbService<R_PhjInfo> R_PHJToDbService = null;

        IInsertToDbService<R_BInfo> R_BInfoToDbService = null;
        IInsertToDbService<R_RInfo> R_RInfoToDbService = null;
        IRPointDefineCacheService _rPointDefineCacheService = null;
        IB_DefCacheService _b_DefCacheService = null;
        /// <summary>
        /// 获取抽放天数据入库内存积压情况  20170717
        /// </summary>
        /// <returns></returns>
        public int GetJc_Ll_DToDbListCount()
        {
            return Jc_Ll_DToDbService.GetQueueBacklog().Data;
        }
        /// <summary>
        /// 获取抽放小时数据入库内存积压情况  20170717
        /// </summary>
        /// <returns></returns>
        public int GetJc_Ll_HToDbListCount()
        {
            return Jc_Ll_HToDbService.GetQueueBacklog().Data;
        }
        /// <summary>
        /// 获取抽放月数据入库内存积压情况  20170717
        /// </summary>
        /// <returns></returns>
        public int GetJc_Ll_MToDbListCount()
        {
            return Jc_Ll_MToDbService.GetQueueBacklog().Data;
        }
        /// <summary>
        /// 获取抽放年数据入库内存积压情况  20170717
        /// </summary>
        /// <returns></returns>
        public int GetJc_Ll_YToDbListCount()
        {
            return Jc_Ll_YToDbService.GetQueueBacklog().Data;
        }
        /// <summary>
        /// 获取报警数据入库内存积压情况  20170717
        /// </summary>
        /// <returns></returns>
        public int GetJc_BInfoToDbListCount()
        {
            return Jc_BInfoToDbService.GetQueueBacklog().Data;
        }
        /// <summary>
        /// 获取馈电数据入库内存积压情况  20170717
        /// </summary>
        /// <returns></returns>
        public int GetJc_KdInfoToDbListCount()
        {
            return Jc_KdInfoToDbService.GetQueueBacklog().Data;
        }
        /// <summary>
        /// 获取5分钟数据入库内存积压情况  20170717
        /// </summary>
        /// <returns></returns>
        public int GetJc_MInfoToDbListCount()
        {
            return Jc_MInfoToDbService.GetQueueBacklog().Data;
        }
        /// <summary>
        /// 获取密采数据入库内存积压情况  20170717
        /// </summary>
        /// <returns></returns>
        public int GetJc_McInfoToDbListCount()
        {
            return Jc_McInfoToDbService.GetQueueBacklog().Data;
        }
        /// <summary>
        /// 获取运行记录数据入库内存积压情况  20170717
        /// </summary>
        /// <returns></returns>
        public int GetJc_RInfoToDbListCount()
        {
            return Jc_RInfoToDbService.GetQueueBacklog().Data;
        }
        #endregion
        #region ----对外接口----
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataProcHandle()
        {
            try
            {
                LogHelper.Info(" DataProcHandle 1");
                InitConfig();
                LogHelper.Info(" DataProcHandle 2");
                //实例化数据入库模块
                Jc_Ll_DToDbService = ServiceFactory.Create<IInsertToDbService<Jc_Ll_DInfo>>();
                Jc_Ll_HToDbService = ServiceFactory.Create<IInsertToDbService<Jc_Ll_HInfo>>();
                Jc_Ll_MToDbService = ServiceFactory.Create<IInsertToDbService<Jc_Ll_MInfo>>();
                Jc_Ll_YToDbService = ServiceFactory.Create<IInsertToDbService<Jc_Ll_YInfo>>();
                Jc_BInfoToDbService = ServiceFactory.Create<IInsertToDbService<Jc_BInfo>>();
                Jc_KdInfoToDbService = ServiceFactory.Create<IInsertToDbService<Jc_KdInfo>>();
                Jc_MInfoToDbService = ServiceFactory.Create<IInsertToDbService<Jc_MInfo>>();
                Jc_McInfoToDbService = ServiceFactory.Create<IInsertToDbService<Jc_McInfo>>();
                Jc_RInfoToDbService = ServiceFactory.Create<IInsertToDbService<Jc_RInfo>>();
                R_PBInfoToDbService = ServiceFactory.Create<IInsertToDbService<R_PbInfo>>();
                R_PHistoryToDbService = ServiceFactory.Create<IInsertToDbService<R_PhistoryInfo>>();
                R_PHJToDbService = ServiceFactory.Create<IInsertToDbService<R_PhjInfo>>();

                R_BInfoToDbService = ServiceFactory.Create<IInsertToDbService<R_BInfo>>();
                R_RInfoToDbService = ServiceFactory.Create<IInsertToDbService<R_RInfo>>();

                remoteStateService = ServiceFactory.Create<IRemoteStateService>();

                _rPointDefineCacheService = ServiceFactory.Create<IRPointDefineCacheService>();
                _b_DefCacheService = ServiceFactory.Create<IB_DefCacheService>();

                realMessageService = ServiceFactory.Create<IRealMessageService>();
                _NetworkModuleService = ServiceFactory.Create<INetworkModuleService>();
                _NetworkModuleCacheService = ServiceFactory.Create<INetworkModuleCacheService>();

                LogHelper.Info(" DataProcHandle 3");
                //实例化跨天、五分钟数据处理线程
                crossDayAndFiveMiniteHandle = new CrossDayAndFiveMiniteHandle();
                LogHelper.Info(" DataProcHandle 4");
                //补写JC_B结束记录系统启动时将之前未处理的报警写记录结束时间
                AddOverTime();  //监控系统补录                
                LogHelper.Info(" DataProcHandle 5");
                //实例化数据处理线程及相关对象 
                isExit = new bool[DataProcThreadCount]; //2017.9.10 by AO
                for (int i = 0; i < DataProcThreadCount; i++)
                {
                    DataProcThread.Add(new Thread(new ParameterizedThreadStart(DataProc)));
                    ArriveDataList.Add(new Queue<MasProtocol>());
                    _lockObject.Add(new object());
                    isExit[i] = false;  //2017.9.10 by AO
                }
                LogHelper.Info(" DataProcHandle 6");
                //实例化数据下发线程
                DataSendThread = new Thread(DataSend);
                SwitchesDataSendThread = new Thread(SwitchesDataSend);
                SwitchStatePingGetThread = new Thread(SwitchStatePingGet);//20191007
                StationQueryDataSendThread = new Thread(StationQueryDataSend);
                SwitchQueryDataSendThread = new Thread(SwitchQueryDataSend);
                ClearUnDefineThread = new Thread(ClearUnDefineItems);
                //实例化分站中断处理线程
                StaionInterruptThread = new Thread(StaionInterruptClass);
                LogHelper.Info(" DataProcHandle 7");
                //实例化数据处理驱动
                GlobleStaticVariable.driverHandle = new DriverHandle();
                LogHelper.Info(" DataProcHandle 8");
                //注册驱动加载成功事件
                GlobleStaticVariable.driverHandle.OnLoadDriverSuccessEvent += driverHandle_OnLoadDriverSuccessEvent;
                LogHelper.Info(" DataProcHandle 9");
                //驱动加载
                GlobleStaticVariable.driverHandle.LoadLocalDrivers();
                LogHelper.Info(" DataProcHandle 10");
                //注册RPC数据接收事件
                RpcService.OnDeviceMessageArrived += RpcService_OnDeviceMessageArrived;
                LogHelper.Info(" DataProcHandle 11");
                //PowerBoxControlThread = new Thread(PowerBoxControl);

                ////处理人员定位，广播数据同步超时  20180414
                //personLastSyncTime = DateTime.Now;
                //broadcastLastSyncTime = DateTime.Now;
                //syncThread = new Thread(SyncTimeOut);
                //syncThread.IsBackground = true;
                //syncThread.Start();
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void InitConfig()
        {
            _rpcRemoteIp = Basic.Framework.Configuration.ConfigurationManager.FileConfiguration.GetString("RpcRemoteIp", "127.0.0.1");
            _rpcRemotePort = Basic.Framework.Configuration.ConfigurationManager.FileConfiguration.GetInt("RpcRemotePort", 37000);

            _rpcLocalIp = Basic.Framework.Configuration.ConfigurationManager.FileConfiguration.GetString("RpcLocalIp", "127.0.0.1");
            _rpcLocalPort = Basic.Framework.Configuration.ConfigurationManager.FileConfiguration.GetInt("RpcLocalPort", 36000);
        }

        /// <summary>
        /// 启动数据处理模块
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                LogHelper.Info("开始启动数据处理模块");
                #region 启动历史数据存储
                if (Jc_Ll_DToDbService != null)
                {
                    Jc_Ll_DToDbService.Start(new DataToDbStartRequest());
                }
                if (Jc_Ll_HToDbService != null)
                {
                    Jc_Ll_HToDbService.Start(new DataToDbStartRequest());
                }
                if (Jc_Ll_MToDbService != null)
                {
                    Jc_Ll_MToDbService.Start(new DataToDbStartRequest());
                }
                if (Jc_Ll_YToDbService != null)
                {
                    Jc_Ll_YToDbService.Start(new DataToDbStartRequest());
                }
                if (Jc_BInfoToDbService != null)
                {
                    Jc_BInfoToDbService.Start(new DataToDbStartRequest());
                }
                if (Jc_KdInfoToDbService != null)
                {
                    Jc_KdInfoToDbService.Start(new DataToDbStartRequest());
                }
                if (Jc_MInfoToDbService != null)
                {
                    Jc_MInfoToDbService.Start(new DataToDbStartRequest());
                }
                if (Jc_McInfoToDbService != null)
                {
                    Jc_McInfoToDbService.Start(new DataToDbStartRequest());
                }
                if (Jc_RInfoToDbService != null)
                {
                    Jc_RInfoToDbService.Start(new DataToDbStartRequest());
                }
                //增加人员入库接口  20171204
                if (R_PHistoryToDbService != null)
                {
                    R_PHistoryToDbService.Start(new DataToDbStartRequest());
                }
                if (R_PBInfoToDbService != null)
                {
                    R_PBInfoToDbService.Start(new DataToDbStartRequest());
                }
                if (R_PHJToDbService != null)
                {
                    R_PHJToDbService.Start(new DataToDbStartRequest());
                }
                if (R_BInfoToDbService != null)
                {
                    R_BInfoToDbService.Start(new DataToDbStartRequest());
                }
                if (R_RInfoToDbService != null)
                {
                    R_RInfoToDbService.Start(new DataToDbStartRequest());
                }
                #endregion
                LogHelper.Info("启动数据存储模块完成");
                isStop = false;
                if (DataProcThread != null)
                {
                    for (int i = 0; i < DataProcThread.Count; i++)
                    {
                        if (DataProcThread[i].ThreadState != ThreadState.Running)
                        {
                            DataProcThread[i].Start(i);
                        }
                    }
                }
                if (DataSendThread.ThreadState != ThreadState.Running)
                {
                    DataSendThread.Start();
                }
                if (SwitchesDataSendThread.ThreadState != ThreadState.Running)
                {
                    SwitchesDataSendThread.Start();
                }
                if (SwitchStatePingGetThread.ThreadState != ThreadState.Running)
                {
                    SwitchStatePingGetThread.Start();//20191007
                }
                if (StationQueryDataSendThread.ThreadState != ThreadState.Running)
                {
                    StationQueryDataSendThread.Start();
                }
                if (ClearUnDefineThread.ThreadState != ThreadState.Running)
                {
                    ClearUnDefineThread.Start();
                }
                if (StaionInterruptThread.ThreadState != ThreadState.Running)
                {
                    StaionInterruptThread.Start();
                }
                if (SwitchQueryDataSendThread.ThreadState != ThreadState.Running)
                {
                    SwitchQueryDataSendThread.Start();
                }
                LogHelper.Info("启动数据处理核心线程完成");

                RpcService.StartRpcServer(_rpcRemoteIp, _rpcRemotePort, _rpcLocalIp, _rpcLocalPort);
                LogHelper.Info("启动与采集程序通讯模块完成");

                //启动跨天和五分钟处理线程
                if (crossDayAndFiveMiniteHandle != null)
                {
                    crossDayAndFiveMiniteHandle.Start();
                }
                LogHelper.Info("启动跨天处理线程完成");
                if (PowerBoxControlThread != null)
                {
                    PowerBoxControlThread.Start();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DataProcHandle start Error:" + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 关闭数据处理模块
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {

            WriteLog("停止数据处理模块【开始】！");
            //加入系统退出处理
            SystemOutProc();
            WriteLog("停止数据处理模块【驱动退出处理完成】！");
            isStop = true;

            RpcService.StopRpcServer();

            if (crossDayAndFiveMiniteHandle != null)
            {
                crossDayAndFiveMiniteHandle.Stop();
            }
            sysrunFlag = false;
            bool breakFlag = true;
            while (true)
            {
                breakFlag = true;
                foreach (bool flag in isExit)
                {
                    breakFlag &= flag;
                }
                if (breakFlag)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 停止数据入库线程
        /// </summary>
        private void StopDataToDBService()
        {
            if (Jc_Ll_DToDbService != null)
            {
                Jc_Ll_DToDbService.Stop(new DataToDbStopRequest());
            }
            if (Jc_Ll_HToDbService != null)
            {
                Jc_Ll_HToDbService.Stop(new DataToDbStopRequest());
            }
            if (Jc_Ll_MToDbService != null)
            {
                Jc_Ll_MToDbService.Stop(new DataToDbStopRequest());
            }
            if (Jc_Ll_YToDbService != null)
            {
                Jc_Ll_YToDbService.Stop(new DataToDbStopRequest());
            }
            if (Jc_BInfoToDbService != null)
            {
                Jc_BInfoToDbService.Stop(new DataToDbStopRequest());
            }
            if (Jc_KdInfoToDbService != null)
            {
                Jc_KdInfoToDbService.Stop(new DataToDbStopRequest());
            }
            if (Jc_MInfoToDbService != null)
            {
                Jc_MInfoToDbService.Stop(new DataToDbStopRequest());
            }
            if (Jc_McInfoToDbService != null)
            {
                Jc_McInfoToDbService.Stop(new DataToDbStopRequest());
            }
            if (Jc_RInfoToDbService != null)
            {
                Jc_RInfoToDbService.Stop(new DataToDbStopRequest());
            }
            //增加人员入库接口  20171204
            if (R_PHistoryToDbService != null)
            {
                R_PHistoryToDbService.Stop(new DataToDbStopRequest());
            }
            if (R_PBInfoToDbService != null)
            {
                R_PBInfoToDbService.Stop(new DataToDbStopRequest());
            }
            if (R_PHistoryToDbService != null)
            {
                R_PHistoryToDbService.Stop(new DataToDbStopRequest());
            }
            if (R_BInfoToDbService != null)
            {
                R_BInfoToDbService.Stop(new DataToDbStopRequest());
            }
            if (R_RInfoToDbService != null)
            {
                R_RInfoToDbService.Stop(new DataToDbStopRequest());
            }
            if (R_PHJToDbService != null)
            {
                R_PHJToDbService.Stop(new DataToDbStopRequest());
            }
        }
        #endregion
        #region ----数据接收、下发处理、数据清理----
        /// <summary>
        /// 回发数据处理
        /// </summary>
        private void DataProc(object threadId)
        {
            int OneTimeProcCount = 100;//单次最大处理数据量
            int dataCount = 0;
            int i = 0;
            MasProtocol data = null;//通讯基础对象
            int delayTime = 50;//单次处理完成休息时间，毫秒
            for (; ; )
            {
                try
                {
                    //PersonTestData();

                    dataCount = ArriveDataList[(int)(threadId)].Count;
                    if (isStop == true)
                    {
                        bool ListContainsData = false;
                        foreach (Queue<MasProtocol> queue in ArriveDataList)
                        {
                            if (queue.Count > 0)
                            {
                                ListContainsData = true;
                            }
                        }
                        if ((int)(threadId) == 0 && !ListContainsData)
                        {
                            isExit[(int)(threadId)] = true;
                            StopDataToDBService(); //stop线程，待数据处理完成后再退出线程
                            WriteLog("停止数据处理模块【回发数据处理完成】！,线程号：" + (int)(threadId));
                            break;
                        }
                        else if (!ListContainsData)
                        {
                            isExit[(int)(threadId)] = true;
                            WriteLog("停止数据处理模块【回发数据处理完成】！,线程号：" + (int)(threadId));
                            break;
                        }
                    }
                    if (dataCount > OneTimeProcCount)
                    {
                        dataCount = OneTimeProcCount;
                    }
                    if (dataCount > 0)
                    {
                        for (i = 0; i < dataCount; i++)
                        {
                            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                            sw.Start();
                            if (isStop)
                            {
                                delayTime = 0;  //系统即将退出，数据处理不加延时
                            }
                            data = DataDnqueue((int)(threadId));
                            if (data == null)
                            {
                                LogHelper.Error("DataProc:数据出队没有数据");
                                continue;
                            }
                            try
                            {
                                switch (data.ProtocolType)
                                {
                                    case ProtocolType.DeviceControlResponse:
                                        #region ----控制命令回复----
                                        CallDriverDataProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.DeviceInitializeRequest:
                                        #region ----设备请求初始化----
                                        CallDriverDataProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.InitializeResponse:
                                        #region ----初始化回发----
                                        CallDriverDataProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.HeartbeatRequest:
                                        #region ----网关心跳包----
                                        //gatewayTime = HeartbeatRequestProc(data);
                                        gatewayTime = DateTime.Now; //todo 后续分布式部署要考虑到时间同步机制                             
                                        WriteLog("HeartbeatRequest：" + gatewayTime);
                                        #endregion
                                        break;
                                    case ProtocolType.NetworkDeviceDataRequest:
                                        #region ----网络模块实时数据----
                                        NetworkDeviceDataRequestProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.QueryBatteryRealDataResponse:
                                        #region ----电源箱实时数据----
                                        QueryBatteryRealDataResponse queryBatteryRealDataResponse = data.Deserialize<QueryBatteryRealDataResponse>();
                                        if (queryBatteryRealDataResponse.DeviceProperty == ItemDevProperty.Substation)
                                        {
                                            //分站电源箱
                                            CallDriverDataProc(data);
                                        }
                                        else
                                        {
                                            //交换机电源箱
                                            QueryBatteryRealDataResponseProc(data);
                                        }
                                        #endregion
                                        break;
                                    case ProtocolType.QueryDeviceInfoResponse:
                                        #region ----设备唯一编码----
                                        CallDriverDataProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.QueryRealDataResponse:
                                        #region ----实时数据回发----
                                        sw.Restart();
                                        CallDriverDataProc(data);
                                        sw.Stop();
                                        if (sw.ElapsedMilliseconds > 30)
                                        {
                                            LogHelper.Debug("CallDriverDataProc TimeOut:" + data.DeviceNumber + "  -  " + sw.ElapsedMilliseconds);
                                        }
                                        #endregion
                                        break;
                                    case ProtocolType.ResetDeviceCommandResponse:
                                        #region ----复位命令回复----
                                        CallDriverDataProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.QueryHistoryRealDataResponse:
                                        #region ----历史五分钟数据----
                                        CallDriverDataProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.QueryHistoryControlResponse:
                                        #region ----历史控制数据----
                                        CallDriverDataProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.ModificationDeviceAdressResponse:
                                        #region ----修改地址号----
                                        CallDriverDataProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.SetSensorGradingAlarmResponse:
                                        #region ----分级报警---
                                        CallDriverDataProc(data);
                                        #endregion
                                        break;
                                    case ProtocolType.GetStationUpdateStateResponse://获取分站的工作状态(设备->上位机)----                                              
                                    case ProtocolType.InspectionResponse:   //巡检单台分站的文件接收情况回复(设备->上位机)
                                    case ProtocolType.ReductionResponse:    //远程还原最近一次备份程序(设备->上位机)
                                    case ProtocolType.RestartResponse:      //通知分站进行重启升级回复(设备->上位机)
                                    case ProtocolType.StationUpdateResponse:    //设备请求升级回复(设备->上位机)
                                    case ProtocolType.UpdateCancleResponse:     //异常中止升级流程(设备->上位机)
                                        CallDriverDataProc(data);
                                        break;
                                    case ProtocolType.GetSwitchboardParamCommResponse:
                                        GetSwitchboardParamCommResponse getSwitchboardParamCommResponse = data.Deserialize<GetSwitchboardParamCommResponse>();
                                        QuerySwitchRealDataResponseProc(getSwitchboardParamCommResponse);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error("DataProc Error:" + data.ProtocolType + "  -  " + ex.Message);
                            }
                            sw.Stop();
                            //LogHelper.Warn("DataProc:" + sw.ElapsedMilliseconds + "-" + data.DeviceNumber);
                        }
                    }
                    Thread.Sleep(delayTime);
                }
                catch (Exception ex)
                {
                    LogHelper.Error("DataProc Error:" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 分站数据下发线程
        /// </summary>
        private void DataSend()
        {
            try
            {
                int i = 0, j = 0;
                string strLog;
                MasProtocol masProtocol = null;
                int[] SendTime = new int[256];
                int curSendTime = 0;
                List<MasProtocol> masProtocolByMacList = new List<MasProtocol>();//网络模块其它命令集

                IDriver driverObj = null;

                List<Jc_MacInfo> macItems = null; //CacheDataHelper.GetAllMacItems();
                List<Jc_MacInfo> allMacItems = null; //CacheDataHelper.GetAllMacItems();                
                List<Jc_DefInfo> defItems = null;// CacheDataHelper.GetAllPointDefineItems();
                List<Jc_DevInfo> devItems = null;// CacheDataHelper.GetAllDevItems();

                Jc_DefInfo defItem = null;
                Jc_DevInfo devItem = null;

                NetworkModuleCacheUpdateRequest networkModuleCacheUpdateRequest = new NetworkModuleCacheUpdateRequest();
                List<short> stationList = new List<short>();    //网络模块挂接的分站队列
                short stationID = 0;
                List<short> communicationStation = new List<short>();   //挂接在网络模块上的分站
                Dictionary<string, object> updateItems;

                DateTime timeNow;
                Dictionary<string, object> defUpdateItems = new Dictionary<string, object>();
                int indexFz = 0;
                int maxStationCount = 0; //模块实际最大挂接分站数量

                int timeSynchronizationcount = 0;//时间同步待下发次数
                bool isSendTimeSynchronization = false; //是否下发时间同步
                DateTime sendTimeSynchronizationDate = DateTime.Now.AddDays(-1);//上次下发时间同步的时间

                SettingInfo setting = CacheDataHelper.GetSettingByKeyStr("TimeSynchronization");
                bool isAutoTimeSynchronization = false;
                if (setting != null)
                {
                    if (setting.StrValue == "1")
                    {
                        isAutoTimeSynchronization = true;
                    }
                }

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch inspectionTimer = new System.Diagnostics.Stopwatch();
                for (; ; )
                {

                    try
                    {
                        inspectionTimer.Restart();
                        timeNow = DateTime.Now;

                        #region ----判断是否下发时间同步----

                        if (isAutoTimeSynchronization)
                        {
                            if (timeNow.Date != sendTimeSynchronizationDate.Date)
                            {
                                if (timeNow.Hour == 0 && timeNow.Minute >= 5)
                                {
                                    sendTimeSynchronizationDate = timeNow;
                                    isSendTimeSynchronization = true;
                                    timeSynchronizationcount = 2;//修改为2次。
                                }
                            }
                            else
                            {
                                isSendTimeSynchronization = false;
                            }
                        }
                        else
                        {
                            isSendTimeSynchronization = false;
                        }
                        #endregion

                        #region ----网关状态判断----

                        if ((DateTime.Now - gatewayTime).TotalSeconds > Heartbeat)
                        {
                            if (gatewayState != false)
                            {
                                //网关断线
                                SetGatewayState(false);
                                gatewayState = GetGatewayState();
                                //置网格模块及网络模块下的分站中断
                                //GateWayOutLine();
                            }
                        }
                        else
                        {
                            if (gatewayState != true)
                            {
                                SetGatewayState(true);
                                gatewayState = GetGatewayState();
                            }
                        }
                        #endregion

                        allMacItems = CacheDataHelper.GetAllMacItems();//只加载分站模块 
                        macItems = allMacItems.FindAll(a => a.Upflag == "0");
                        devItems = CacheDataHelper.GetAllDevItems();
                        defItems = CacheDataHelper.GetAllSystemPointDefineItems();

                        if (isSendTimeSynchronization)  //2017.11.10 by
                        {
                            macItems.ForEach(a =>
                            {
                                a.TimeSynchronizationcount = timeSynchronizationcount;
                                a.TimeSynchronization = true;
                            });
                        }


                        stopwatch.Restart();

                        communicationStation.Clear();
                        if (isStop)
                        {
                            WriteLog("停止数据处理模块【数据下发线程处理完成】！");
                            break;
                        }

                        maxStationCount = 0;

                        for (i = 0; i < netStaionListCount; i++)//按网络模块最大挂接数循环，每次下发所有模块的1路
                        {
                            if (isStop)
                            {
                                WriteLog("停止数据处理模块【数据下发线程处理完成】！");
                                break;
                            }
                            //for (j = 0; j < SendTime.Length; j++)
                            //{
                            //    SendTime[j] = 500;
                            //}

                            #region 下发所有分站命令
                            for (j = 0; j < macItems.Count; j++)//循环所有模块
                            {
                                if (isStop)
                                {
                                    WriteLog("停止数据处理模块【数据下发线程处理完成】！");
                                    break;
                                }

                                //判断当前网络模块是否使用，才继续往下执行  20171105
                                if (string.IsNullOrEmpty(macItems[j].Bz1)) { continue; }

                                ////增加循环网络模块端口  20180504
                                //string[] subComItems = macItems[j].Bz1.Split(';');
                                //下发网络模块命令  20171213
                                inspectionTimer.Stop();
                                if (macItems[j].TimeSynchronization)//|| isSendTimeSynchronization)
                                {
                                    #region ----下发网络模块的时间同步----
                                    //for (indexFz = 0; indexFz < subComItems.Length; indexFz++)
                                    //{
                                    //if (!IsHaveDeviceForCommInfo(subComItems[indexFz])) continue;
                                    //获取时间同步命令结构体
                                    masProtocol = TimeSynchronization(macItems[j].Bz1, indexFz + 1);
                                    //下发时间同步命令
                                    DLLObj_OnDriverSendDataEventHandler(masProtocol);
                                    //}
                                    //待下发次数-1
                                    macItems[j].TimeSynchronizationcount--;
                                    if (macItems[j].TimeSynchronizationcount <= 0)
                                    {
                                        macItems[j].TimeSynchronization = false;
                                    }
                                    else
                                    {
                                        macItems[j].TimeSynchronization = true;
                                    }
                                    //更新网络模块内存
                                    updateItems = new Dictionary<string, object>();
                                    updateItems.Add("TimeSynchronizationcount", macItems[j].TimeSynchronizationcount);
                                    updateItems.Add("TimeSynchronization", macItems[j].TimeSynchronization);
                                    CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(macItems[j].MAC, updateItems);
                                    LogHelper.Info(macItems[j].MAC + "下发时间同步，待下发" + macItems[j].TimeSynchronizationcount + "次");
                                    //Thread.Sleep(300);//下发网络模块命令后，需要加延时，否则会导致交换机处理异常
                                    //20190129--当分站挂多了，此处加迟时，会造成中断，每个分站300秒10个就是3秒，修改为发时间同步命令此次不下发其它命令。
                                    continue;
                                    #endregion
                                }
                                else
                                {
                                    #region ----下发网络模块的其他命令----

                                    masProtocolByMacList = CacheDataHelper.GetMacSendData(macItems[j].MAC, macItems);
                                    if (masProtocolByMacList.Count > 0)
                                    {
                                        foreach (MasProtocol protocol in masProtocolByMacList)
                                        {
                                            LogHelper.Info("【" + macItems[j].MAC + "】" + protocol.ProtocolType.ToString());
                                            DLLObj_OnDriverSendDataEventHandler(protocol);
                                            Thread.Sleep(300);//下发网络模块命令后，需要加延时，否则会导致交换机处理异常
                                        }
                                    }


                                    #endregion
                                }
                                inspectionTimer.Start();


                                //foreach (string tempComItem in subComItems)
                                //{
                                ////下发分站命令判断，如果当前路绑定了分站才继续执行 
                                //if (tempComItem == "") continue;
                                //getStationList(tempComItem, ref  stationList);
                                //if (maxStationCount < stationList.Count)
                                //{
                                //    maxStationCount = stationList.Count;
                                //}
                                //if (stationList.Count > 0)
                                //{
                                //    indexFz = i % stationList.Count;
                                //}
                                //if (indexFz > stationList.Count) indexFz = 0;
                                //if (stationList.Count > 0)
                                //{
                                //    stationID = stationList[indexFz];//todo 提高效率，当空转时进行队列分站查找
                                //}
                                //else
                                //{
                                //    continue;
                                //}
                                //if (stationID <= 0)
                                //{
                                //    continue;
                                //}
                                short.TryParse(macItems[j].Bz1, out stationID);
                                communicationStation.Add(stationID);

                                #region ----下发分站命令----

                                if (defItems == null || devItems == null)
                                {
                                    LogHelper.Error("分站或设备类型信息为空");
                                    continue;
                                }
                                defItem = defItems.FirstOrDefault(a => a.Fzh == stationID && a.DevPropertyID == (int)ItemDevProperty.Substation);

                                if (defItem == null)
                                {
                                    //LogHelper.Error(macItems[j].IP + "网络模块绑定有分站" + stationID + "信息，但未找到此分站！");
                                    continue;
                                }
                                if ((defItem.Bz4 & 0x02) != 0x02 && defItem.State != ((short)ItemState.Deleted))    //设备休眠或设备已删除，不作处理
                                {
                                    devItem = devItems.FirstOrDefault(a => a.Devid == defItem.Devid);
                                    if (devItem == null)
                                    {
                                        LogHelper.Error("分站" + stationID + "设备类型未找到，devid = " + defItem.Devid);
                                        continue;
                                    }

                                    if (GlobleStaticVariable.driverHandle.DriverItems.ContainsKey(devItem.Sysid))
                                    {
                                        driverObj = GlobleStaticVariable.driverHandle.DriverItems[devItem.Sysid].DLLObj;

                                        #region 进入驱动处理
                                        try
                                        {
                                            //获取下发命令
                                            //curSendTime = 500;
                                            DriverTransferInterface.GetSendData(driverObj, defItem.Point, devItem, ref curSendTime);
                                            //if (SendTime[i] < curSendTime) SendTime[i] = curSendTime;
                                        }
                                        catch (Exception ex)
                                        {
                                            strLog = ex.Message + " SendProc";
                                            LogHelper.Error(strLog);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 写驱动不存在异常
                                        strLog = string.Format("SendProc-驱动({0}不存在)", devItem.Sysid);
                                        LogHelper.Error(strLog);
                                        #endregion
                                    }
                                }
                                #endregion
                            }

                            #endregion
                        }
                        #region 人员定位呼叫命令次数处理  20171212
                        //List<R_CallInfo> callItems = KJ237CacheHelper.GetCallItems().Where(a => a.CallType != 2).OrderBy(a => a.CallTime).ToList();
                        //if (callItems.Count == 0)//如果不存在呼叫
                        //{
                        //    #region //呼叫解除命令次数处理
                        //    callItems = KJ237CacheHelper.GetCallItems().Where(a => a.CallType == 2).OrderBy(a => a.CallTime).ToList();
                        //    if (callItems.Count > 0)
                        //    {
                        //        //解除呼叫时，下发3次命令  20171212
                        //        Dictionary<string, Dictionary<string, object>> updateCallItems = new Dictionary<string, Dictionary<string, object>>();
                        //        foreach (R_CallInfo tempCall in callItems)
                        //        {
                        //            if (tempCall.SendCount - 1 < 0)
                        //            {
                        //                //下发完毕，直接清除缓存
                        //                //KJ237CacheHelper.DeleteCallItem(tempCall.Id);
                        //            }
                        //            tempCall.SendCount -= 1;//下发次数减1次
                        //            Dictionary<string, object> updateCallItem = new Dictionary<string, object>();
                        //            updateCallItem.Add("SendCount", tempCall.SendCount);
                        //            updateCallItems.Add(tempCall.Id, updateCallItem);
                        //        }
                        //        KJ237CacheHelper.UpdateCallItems(updateCallItems);
                        //    }
                        //    #endregion
                        //}
                        //else//置其它命令的下发次数
                        //{
                        //    Dictionary<string, Dictionary<string, object>> updateCallItems = new Dictionary<string, Dictionary<string, object>>();
                        //    foreach (R_CallInfo tempCall in callItems)
                        //    {
                        //        if (tempCall.CallType == 0)//一般呼叫只下发3次，每次下发减1
                        //        {
                        //            if (tempCall.SendCount - 1 < 0)
                        //            {
                        //                //下发完毕，直接清除缓存
                        //                //KJ237CacheHelper.DeleteCallItem(tempCall.Id);
                        //            }
                        //            tempCall.SendCount -= 1;//下发次数减1次
                        //            Dictionary<string, object> updateCallItem = new Dictionary<string, object>();
                        //            updateCallItem.Add("SendCount", tempCall.SendCount);
                        //            updateCallItems.Add(tempCall.Id, updateCallItem);
                        //        }
                        //    }
                        //    KJ237CacheHelper.UpdateCallItems(updateCallItems);
                        //}
                        #endregion

                        curSendTime = 2000;
                        //for (j = 0; j < SendTime.Length; j++)
                        //{
                        //    if (SendTime[j] > curSendTime) curSendTime = SendTime[j];
                        //}

                        Thread.Sleep(curSendTime);
                        //}
                        ClearNoCommunicationStation(communicationStation, defItems, devItems);

                        stopwatch.Stop();
                        if (stopwatch.ElapsedMilliseconds >= 10 * 1000)
                        {
                            LogHelper.Info("DataSend 时间过长：" + stopwatch.ElapsedMilliseconds + "毫秒");
                        }
                        inspectionTimer.Stop();

                        if (maxStationCount > 0)
                        {
                            //初始化命令延时过长，降低客户端显示巡检周期
                            CacheDataHelper.UpdateInspectionTime(inspectionTimer.ElapsedMilliseconds / (netStaionListCount / maxStationCount));
                        }
                        else
                        {
                            CacheDataHelper.UpdateInspectionTime(inspectionTimer.ElapsedMilliseconds);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("DataSend Error:" + ex.Message + ex.StackTrace);
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DataSend Thread Error:" + ex.Message + ex.StackTrace);
            }

        }
        /*********************/        
        public class NetPingClass
        {
            public string Mac;
            public string IP;
            public string Id;
            public string wzid;
            public int pCount = 0;
            public DateTime stime;
            public DateTime etime;
            public bool Upflag = false;
            public bool writerunflag = false;//表示是否已经写运行记录
            public string alarmid;
            public Jc_BInfo alarminfo;
        }
        /*单独生成一个缓存来存储判断的次数20191007
         */
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败 
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名</param>
        /// <returns></returns>
        private bool PingIP(string DoNameOrIP)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "FData";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 10;
                PingReply objPinReply = objPingSender.Send(DoNameOrIP, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 通过Ping的方式获取交换机的实时状态，并生成运行记录和报警记录  20191007
        /// </summary>
        private void SwitchStatePingGet()
        {
            try
            {
                int j = 0;
                List<Jc_MacInfo> allMacItems = null; //CacheDataHelper.GetAllMacItems();
                List<Jc_MacInfo> switchesMacItems = null; //CacheDataHelper.GetAllMacItems();
                List<MasProtocol> masProtocolByMacList = new List<MasProtocol>();//网络模块其它命令集
                List<NetPingClass> lstNet = new List<NetPingClass>();//表示当前的交换机状态通过Ping判断的缓存列表。
                NetPingClass objNet;
                Jc_MacInfo objMac;
                Jc_BInfo alarmInfo;
                IAlarmCacheService _AlarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
                IInsertToDbService<Jc_BInfo> alarmTodbService =  ServiceFactory.Create<IInsertToDbService<Jc_BInfo>>();
                Dictionary<string, object> updateItems = new Dictionary<string, object>();

                int index=0;
                for (; ; )
                {
                    try
                    {
                        allMacItems = CacheDataHelper.GetAllMacItems();//                        
                        switchesMacItems = allMacItems.FindAll(a => a.Upflag == "1");
                        if (isStop)
                        {
                            WriteLog("停止数据处理模块【交换机Ping状态线程处理完成】！");
                            break;
                        }
                        for (j = 0; j < lstNet.Count; j++)
                        {
                            lstNet[j].Upflag = false;
                        }
                        for (j = 0; j < switchesMacItems.Count; j++)//循环所有交换机
                        {
                            index=lstNet.FindIndex(p => p.Mac == switchesMacItems[j].MAC);
                            if (index < 0)
                            {
                                objNet = new NetPingClass();
                                objNet.Id = switchesMacItems[j].ID;
                                objNet.Mac = switchesMacItems[j].MAC;
                                objNet.wzid = switchesMacItems[j].Wzid;
                                objNet.IP = switchesMacItems[j].IP;
                                objNet.pCount = 0;
                                objNet.alarmid = "0";
                                objNet.alarminfo = null;
                                objNet.stime = DateTime.Now;
                                objNet.etime = objNet.stime;
                                objNet.Upflag = true;
                                lstNet.Add(objNet);
                            }
                            else
                            {
                                lstNet[index].Upflag = true;
                            }
                        }
                        for (j = 0; j < lstNet.Count; j++)
                        {
                            if (!lstNet[j].Upflag)
                            {
                                lstNet.RemoveAt(j);
                                j--;
                            }
                        }
                        #region 数据处理
                        for (j = 0; j < lstNet.Count; j++)//循环所有交换机
                        {
                            objMac = switchesMacItems.Find(p => p.MAC == lstNet[j].Mac);
                            if (objMac == null) continue;
                            if (PingIP(lstNet[j].IP))
                            {//如果Ping得通说明交换机是通的。
                                lstNet[j].pCount = 0;
                                lstNet[j].alarminfo = null;
                                lstNet[j].alarmid = "0";
                                //如果写了中断记录的此时，需要写一条交换机运行正常记录到运行记录中去。
                                if (lstNet[j].writerunflag)
                                {
                                    CreateMacRunLogInfo(objMac, DateTime.Now, 3, lstNet[j].IP);//写交流正常记录
                                }
                                lstNet[j].writerunflag = false;
                                //更新实时缓存
                                updateItems.Clear();
                                updateItems.Add("IP", objMac.IP);
                                updateItems.Add("MAC", objMac.MAC);
                                updateItems.Add("State", "3");//交流正常
                                CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(objMac.MAC, updateItems);
                            }
                            else
                            {
                                lstNet[j].pCount++;
                                if (lstNet[j].pCount == 1)
                                {
                                    lstNet[j].stime = DateTime.Now;
                                }
                            }
                            if (lstNet[j].pCount > 3)
                            {//如果连续Ping3次都不通，此次写相应的报警记录和运行记录
                                lstNet[j].pCount = 100;

                                #region 写运行记录
                                if (!lstNet[j].writerunflag)//没有写运行记录
                                {
                                    CreateMacRunLogInfo(objMac, lstNet[j].stime, 0, objMac.IP);//写中断记录
                                }
                                #endregion

                                #region 写报警记录
                                if (!lstNet[j].writerunflag)//没有写运行记录
                                {
                                    alarmInfo = new Jc_BInfo();
                                    alarmInfo.Cs = "";
                                    alarmInfo.ID = IdHelper.CreateLongId().ToString();
                                    alarmInfo.PointID = "0";
                                    alarmInfo.Devid = "0";
                                    alarmInfo.Fzh =0;
                                    alarmInfo.Kh = 0;
                                    alarmInfo.Dzh = 0;
                                    alarmInfo.Kzk = "";
                                    alarmInfo.Point = objMac.MAC;
                                    alarmInfo.Ssz = "通讯中断";
                                    alarmInfo.Stime = lstNet[j].stime;
                                    alarmInfo.Etime = DateTime.Now;
                                    alarmInfo.Isalarm = 1;
                                    alarmInfo.Type = 0;
                                    alarmInfo.State = 0;
                                    alarmInfo.Kdid = "";
                                    alarmInfo.Upflag = "0";
                                    alarmInfo.Wzid = objMac.Wzid;
                                    alarmInfo.Remark = objMac.IP;
                                    alarmInfo.InfoState = Basic.Framework.Web.InfoState.AddNew;
                                    //添加记录入缓存
                                    alarmInfo.Zdzs = new DateTime(1900, 1, 1, 0, 0, 0);
                                    AlarmCacheAddRequest addRequest = new AlarmCacheAddRequest();
                                    addRequest.AlarmInfo = alarmInfo;
                                    _AlarmCacheService.AddAlarmCache(addRequest);

                                    //添加报警信息至数据库
                                    DataToDbAddRequest<Jc_BInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_BInfo>();
                                    dataToDbAddRequest.Item = alarmInfo;
                                    alarmTodbService.AddItem(dataToDbAddRequest);
                                    lstNet[j].alarmid = alarmInfo.ID;
                                    lstNet[j].alarminfo = alarmInfo;
                                }
                                else
                                {//只是更新结束时间
                                    updateItems.Clear();
                                    updateItems.Add("Etime", DateTime.Now.ToString());
                                    //更新到缓存
                                    AlarmCacheUpdatePropertiesRequest alarmCacheUpdatePropertiesRequest = new AlarmCacheUpdatePropertiesRequest();
                                    alarmCacheUpdatePropertiesRequest.AlarmKey = lstNet[j].alarmid;
                                    alarmCacheUpdatePropertiesRequest.UpdateItems = updateItems;
                                    _AlarmCacheService.UpdateAlarmInfoProperties(alarmCacheUpdatePropertiesRequest);

                                    //更新到数据库    
                                    DataToDbAddRequest<Jc_BInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_BInfo>();
                                    lstNet[j].alarminfo.Etime = DateTime.Now;
                                    lstNet[j].alarminfo.InfoState = Basic.Framework.Web.InfoState.Modified;
                                    dataToDbAddRequest.Item = lstNet[j].alarminfo;

                                    alarmTodbService.AddItem(dataToDbAddRequest);
                                }
                                #endregion
                                lstNet[j].writerunflag = true;

                                //更新实时缓存
                                updateItems.Clear();
                                updateItems.Add("IP", objMac.IP);
                                updateItems.Add("MAC", objMac.MAC);
                                updateItems.Add("State", "0");//通讯中断
                                CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(objMac.MAC, updateItems);
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("SwitchesDataSend Error:" + ex.Message + ex.StackTrace);
                    }
                    Thread.Sleep(10000);//10秒判断一次
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("SwitchStatePingGet Thread Error:" + ex.Message + ex.StackTrace);
            }
        }
        /******************END ******************/
        /// <summary>
        /// 交换机命令发送线程
        /// </summary>
        private void SwitchesDataSend()
        {
            try
            {
                int j = 0;
                List<Jc_MacInfo> allMacItems = null; //CacheDataHelper.GetAllMacItems();
                List<Jc_MacInfo> switchesMacItems = null; //CacheDataHelper.GetAllMacItems();
                List<MasProtocol> masProtocolByMacList = new List<MasProtocol>();//网络模块其它命令集

                for (; ; )
                {
                    try
                    {
                        allMacItems = CacheDataHelper.GetAllMacItems();//只加载分站模块                        
                        switchesMacItems = allMacItems.FindAll(a => a.Upflag == "1");
                        if (isStop)
                        {
                            WriteLog("停止数据处理模块【交换机数据下发线程处理完成】！");
                            break;
                        }
                        #region 下发交换机命令
                        for (j = 0; j < switchesMacItems.Count; j++)//循环所有交换机
                        {
                            #region ----下发交换机命令----

                            masProtocolByMacList = CacheDataHelper.GetMacSendData(switchesMacItems[j].MAC, switchesMacItems);
                            if (masProtocolByMacList.Count > 0)
                            {
                                foreach (MasProtocol protocol in masProtocolByMacList)
                                {
                                    LogHelper.Info("【" + switchesMacItems[j].MAC + "】" + protocol.ProtocolType.ToString());
                                    DLLObj_OnDriverSendDataEventHandler(protocol);
                                    //Thread.Sleep(300);//下发网络模块命令后，需要加延时，否则会导致交换机处理异常
                                }
                            }


                            #endregion
                        }
                        #endregion



                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("SwitchesDataSend Error:" + ex.Message + ex.StackTrace);
                    }
                    Thread.Sleep(3000);//3秒获取一次
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("SwitchesDataSend Thread Error:" + ex.Message + ex.StackTrace);
            }

        }
        //private void GetSwitchState(string ip, string mac)
        //{            
        //    webBrowser1.Navigate("http://" + ip + "/view.html?next_file=view.html");

        //    string str = "";
        //    string Switch100StateString = "";
        //    string[] textArray1 = new string[] { str, "1   ", webBrowser1.Document.GetElementById("link_0").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("port_state_0").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("speed_0").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("media_0").InnerText, "\r\n" };
        //    str = string.Concat(textArray1);
        //    string[] textArray2 = new string[] { str, "2   ", webBrowser1.Document.GetElementById("link_1").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("port_state_1").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("speed_1").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("media_1").InnerText, "\r\n" };
        //    str = string.Concat(textArray2);
        //    string[] textArray3 = new string[] { str, "3   ", webBrowser1.Document.GetElementById("link_2").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("port_state_2").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("speed_2").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("media_2").InnerText, "\r\n" };
        //    str = string.Concat(textArray3);
        //    string[] textArray4 = new string[] { str, "4   ", webBrowser1.Document.GetElementById("link_3").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("port_state_3").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("speed_3").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("media_3").InnerText, "\r\n" };
        //    Switch100StateString = str;

        //    string Switch100RJ45StateString = "";
        //    str = "";
        //    str = string.Concat(textArray4);
        //    string[] textArray5 = new string[] { str, "5   ", webBrowser1.Document.GetElementById("link_4").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("port_state_4").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("speed_4").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("media_4").InnerText, "\r\n" };
        //    str = string.Concat(textArray5);
        //    string[] textArray6 = new string[] { str, "6   ", webBrowser1.Document.GetElementById("link_5").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("port_state_5").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("speed_5").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("media_5").InnerText, "\r\n" };
        //    str = string.Concat(textArray6);
        //    string[] textArray7 = new string[] { str, "7   ", webBrowser1.Document.GetElementById("link_6").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("port_state_6").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("speed_6").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("media_6").InnerText, "\r\n" };
        //    str = string.Concat(textArray7);
        //    Switch100RJ45StateString = str;

        //    string Switch1000StateString = "";
        //    str = "";
        //    string[] textArray8 = new string[] { str, "G1  ", webBrowser1.Document.GetElementById("link_7").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("port_state_7").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("speed_7").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("media_7").InnerText, "\r\n" };
        //    str = string.Concat(textArray8);
        //    string[] textArray9 = new string[] { str, "G2  ", webBrowser1.Document.GetElementById("link_8").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("port_state_8").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("speed_8").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("media_8").InnerText, "\r\n" };
        //    str = string.Concat(textArray9);
        //    string[] textArray10 = new string[] { str, "G3  ", webBrowser1.Document.GetElementById("link_9").InnerText,
        //        "  ", webBrowser1.Document.GetElementById("port_state_9").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("speed_9").InnerText, 
        //        "  ", webBrowser1.Document.GetElementById("media_9").InnerText, "\r\n" };
        //    str = string.Concat(textArray10);
        //    Switch1000StateString = str;

        //    List<Jc_MacInfo> macItems = CacheDataHelper.GetAllMacItems(); //todo 查单个MAC
        //    Jc_MacInfo macItem = macItems.FirstOrDefault(a => a.MAC == mac);

        //    Dictionary<string, object> updateItems = new Dictionary<string, object>();
        //    if (macItem != null)
        //    {

        //        updateItems.Add("Switch1000State", Switch1000StateString);
        //        updateItems.Add("Switch100State", Switch100StateString);
        //        updateItems.Add("Switch100RJ45State", Switch100RJ45StateString);

        //        CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(macItem.MAC, updateItems);


        //        //WriteLog("QuerySwitchRealDataResponseProc:" + macItem.IP);
        //    }
        //}
        /// <summary>
        /// 定期下发获取所有分站传感器完整信息命令，用于传感器自动挂接
        /// </summary>
        private void StationQueryDataSend()
        {
            //try
            //{
            //    int j;
            //    List<Jc_DefInfo> defItems = null; //CacheDataHelper.GetAllMacItems();   
            //    List<Jc_DefInfo> stationItems = null;
            //    for (; ; )
            //    {

            //        try
            //        {
            //            defItems = CacheDataHelper.GetAllSystemPointDefineItems();//只加载分站模块                  
            //            stationItems = defItems.FindAll(a => a.DevPropertyID == 0);
            //            if (isStop)
            //            {
            //                WriteLog("停止数据处理模块【获取分站传感器完整信息命令下发线程处理完成】！");
            //                break;
            //            }
            //            #region 下发分站获取传感器完整信息命令
            //            List<DeviceInfoRequestItem> deviceInfoRequestItems = new List<DeviceInfoRequestItem>();
            //            for (j = 0; j < stationItems.Count; j++)//循环所有分站
            //            {
            //                DeviceInfoRequestItem deviceInfoRequestItem = new DeviceInfoRequestItem();
            //                deviceInfoRequestItem.Fzh = (ushort)stationItems[j].Fzh;
            //                deviceInfoRequestItem.controlType = 1;
            //                deviceInfoRequestItem.GetAddressLst = new List<int>();
            //                List<Jc_DefInfo> sensorItems = defItems.FindAll(a => a.Fzh == stationItems[j].Fzh && a.Kh > 0 && (a.Dzh == 0 || a.Dzh == 1));
            //                foreach (Jc_DefInfo sensor in sensorItems)
            //                {
            //                    deviceInfoRequestItem.GetAddressLst.Add(sensor.Kh);
            //                }
            //                deviceInfoRequestItems.Add(deviceInfoRequestItem);
            //            }
            //            CacheDataHelper.QueryDeviceInfoRequest(deviceInfoRequestItems);
            //            #endregion
            //        }
            //        catch (Exception ex)
            //        {
            //            LogHelper.Error("StationQueryDataSend Error:" + ex.Message + ex.StackTrace);
            //        }
            //        Thread.Sleep(60000);//60秒获取一次
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error("StationQueryDataSend Thread Error:" + ex.Message + ex.StackTrace);
            //}
        }
        /// <summary>
        /// 获取交换机状态线程
        /// </summary>
        private void SwitchQueryDataSend()
        {
            while (true)
            {
                try
                {
                    DataTable dt = GetRealMac();
                    List<Jc_MacInfo> allMacList = _NetworkModuleCacheService.GetAllNetworkModuleCache(new NetworkModuleCacheGetAllRequest()).Data.FindAll(a => a.Upflag == "1");
                    //MethodInvoker In1 = new MethodInvoker(() => show());
                    //this.BeginInvoke(In1);
                    List<Jc_MacInfo> updateMacs = new List<Jc_MacInfo>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Jc_MacInfo tempMac = allMacList.Find(a => a.MAC == dt.Rows[i]["mac"].ToString());

                        NetDeviceSettingInfo tempNetInfo = null;
                        try
                        {
                            tempNetInfo = _NetworkModuleService.GetNetworkModuletParameters(new NetworkModuletParametersGetRequest()
                            {
                                Mac = dt.Rows[i]["mac"].ToString()
                            }).Data;
                        }
                        catch
                        { }

                        if (tempNetInfo == null)
                        {
                            tempMac.Switch1000State = "-";
                            tempMac.Switch100State = "-";
                            tempMac.Switch100RJ45State = "-";
                            updateMacs.Add(tempMac);
                            continue;
                        }

                        string switch1000StateString = "";
                        if (tempNetInfo.NetSetting != null && tempNetInfo.NetSetting.Switch1000JkState != null)
                        {
                            for (int j = 0; j < tempNetInfo.NetSetting.Switch1000JkState.Length; j++)
                            {
                                switch1000StateString += "千兆光口" + (j + 1).ToString() + ":" + (tempNetInfo.NetSetting.Switch1000JkState[j] == 0 ? "断开" : "正常") + "\r\n";
                            }
                        }
                        tempMac.Switch1000State = switch1000StateString == "" ? "-" : switch1000StateString;
                        string switch100StateString = "";
                        if (tempNetInfo.NetSetting != null && tempNetInfo.NetSetting.Switch100JkState != null)
                        {
                            for (int j = 0; j < tempNetInfo.NetSetting.Switch100JkState.Length; j++)
                            {
                                switch100StateString += "百兆光口" + (j + 1).ToString() + ":" + (tempNetInfo.NetSetting.Switch100JkState[j] == 0 ? "断开" : "正常") + "\r\n";
                            }
                        }
                        tempMac.Switch100State = switch100StateString == "" ? "-" : switch100StateString;
                        string switch100RJ45State = "";
                        if (tempNetInfo.NetSetting != null && tempNetInfo.NetSetting.Switch100RJ45State != null)
                        {
                            for (int j = 0; j < tempNetInfo.NetSetting.Switch100RJ45State.Length; j++)
                            {
                                switch100RJ45State += "百兆电口" + (j + 1).ToString() + ":" + (tempNetInfo.NetSetting.Switch100RJ45State[j] == 0 ? "断开" : "正常") + "\r\n";
                            }
                        }
                        tempMac.Switch100RJ45State = switch100RJ45State == "" ? "-" : switch100RJ45State;

                        updateMacs.Add(tempMac);
                    }

                    NetworkModuleCacheBatchUpdateRequest request = new NetworkModuleCacheBatchUpdateRequest();
                    request.NetworkModuleInfos = updateMacs;
                    _NetworkModuleCacheService.BatchUpdateNetworkModuleCache(request);
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(30000);
            }
        }
        public DataTable GetRealMac()
        {
            DataTable dt = new DataTable();
            try
            {

                var response = realMessageService.GetRealMac();
                if (response.Data != null)
                {
                    dt = response.Data;
                }

            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
            return dt;
        }
        /// <summary>
        /// 分站中断处理线程  20180822
        /// </summary>
        private void StaionInterruptClass()
        {
            try
            {
                IDriver driverObj = null;

                while (!isStop)
                {

                    try
                    {
                        if (!CrossDayAndFiveMiniteHandle.isCrossDay)//跨天处理时，不进行中断判断
                        {
                            List<Jc_DefInfo> defItems = CacheDataHelper.GetStationAllSystemPointDefineItems();
                            List<Jc_DevInfo> devItems = CacheDataHelper.GetAllDevItems();

                            for (int i = 0; i < defItems.Count; i++)
                            {
                                Jc_DefInfo defItem = defItems[i];
                                short stationID = defItem.Fzh;
                                if ((defItem.Bz4 & 0x02) != 0x02 && defItem.State != ((short)ItemState.Deleted))//设备休眠或设备已删除，不作处理
                                {
                                    Jc_DevInfo devItem = devItems.FirstOrDefault(a => a.Devid == defItem.Devid);
                                    if (devItem == null)
                                    {
                                        LogHelper.Error("分站" + defItem.Point + "设备类型未找到，devid = " + defItem.Devid);
                                        continue;
                                    }

                                    if (GlobleStaticVariable.driverHandle.DriverItems.ContainsKey(devItem.Sysid))
                                    {
                                        driverObj = GlobleStaticVariable.driverHandle.DriverItems[devItem.Sysid].DLLObj;

                                        #region 进入驱动处理
                                        try
                                        {
                                            // 置中断
                                            if (defItem.State != (int)ItemState.EquipmentInterrupted)
                                            {
                                                DriverTransferInterface.Drv_InterruptPro(driverObj, stationID);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LogHelper.Error(ex);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 写驱动不存在异常
                                        string strLog = string.Format("StaionInterruptClass-驱动({0}不存在)", devItem.Sysid);
                                        LogHelper.Error(strLog);
                                        #endregion
                                    }
                                }
                            }
                        }
                        Thread.Sleep(4000);//间隔4秒判断一次，
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error("StaionInterruptClass Error:" + ex.Message + ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("StaionInterruptClass Thread Error:" + ex.Message + ex.StackTrace);
            }

        }

        /// <summary>
        /// 判断网关是否处于保持连接，但无数据回发状态
        /// </summary>
        /// <param name="macItem"></param>
        /// <returns></returns>
        private bool IsNetWorkDown(Jc_MacInfo macItem, DateTime time)
        {
            bool flag = false;
            try
            {
                if (macItem.NetID > 0 && (time - macItem.LastGetTime).TotalSeconds > 20)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DataProcHandle-IsNetWorkDown:" + ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// 下发网络模块复位命令
        /// </summary>
        /// <param name="macItem"></param>
        private void SendNetDeviceReset(Jc_MacInfo macItem)
        {
            try
            {
                if (macItem.isSendReset == false)
                {
                    MasProtocol masProtocol = ResetNetWorkDevice(macItem.MAC);
                    //DLLObj_OnDriverSendDataEventHandler(masProtocol); 2017.8.24 by ,下发由网关判断
                    macItem.isSendReset = true;

                    Dictionary<string, object> updateItems = new Dictionary<string, object>();
                    updateItems.Add("isSendReset", macItem.isSendReset);
                    CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(macItem.MAC, updateItems);

                    LogHelper.Info(macItem.MAC + "传输数据超时下发复位命令，当前时间：" + DateTime.Now + ",最后一次接收时间：" + macItem.LastGetTime);
                }
                else
                {
                    //LogHelper.Info(macItem.MAC + "传输数据超时已下发过复位命令，当前时间：" + DateTime.Now + ",最后一次接收时间：" + macItem.LastGetTime);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DataProcHandle-SendNetDeviceReset:" + ex.Message);
            }
        }

        /// <summary>
        /// 清理当前已定义，之前未定义的信息
        /// </summary>
        private void ClearUnDefineItems()
        {

            List<AutomaticArticulatedDeviceInfo> removeItem = new List<AutomaticArticulatedDeviceInfo>();
            List<AutomaticArticulatedDeviceInfo> unDefineItems;

            List<Jc_DefInfo> defItems;
            Jc_DefInfo defItem;

            DateTime timeNow = DateTime.Now;
            int timeSpan = 5;//默认5分钟
            SettingInfo settingInfo;
            for (; ; )
            {
                try
                {
                    //2017.10.17 by 增加超时自动清除功能
                    settingInfo = CacheDataHelper.GetSettingByKeyStr("ClearUnDefineTime");
                    if (settingInfo != null)
                    {
                        if (!int.TryParse(settingInfo.StrValue, out timeSpan))
                        {
                            timeSpan = 5;
                            LogHelper.Error("缺少配置项 ClearUnDefineTime ，取默认值 5 分钟");
                        }
                    }
                    else
                    {
                        LogHelper.Error("缺少配置项 ClearUnDefineTime ，取默认值 5 分钟");
                    }
                    timeNow = DateTime.Now;

                    if (isStop)
                    {
                        break;
                    }
                    removeItem.Clear();
                    //获取当前缓存中所有未定义的测点信息
                    unDefineItems = CacheDataHelper.GetAllUnDefinePoint();
                    if (unDefineItems.Count > 0)
                    {
                        defItems = CacheDataHelper.GetKJPointDefineItems();
                        foreach (AutomaticArticulatedDeviceInfo item in unDefineItems)
                        {
                            if ((timeNow - item.ReciveTime).TotalMinutes > timeSpan)
                            {
                                removeItem.Add(item);
                                LogHelper.Info(item.StationNumber.ToString().PadLeft(3, '0') + item.ChanelNumber.ToString().PadLeft(2, '0') + item.AddressNumber + "未定义设备超时删除！");
                            }
                            else
                            {
                                //2017.9.9 by
                                //if (((item.ChanelNumber - 1) / 4) + 1 != item.BranchNumber)
                                //{
                                //    continue;
                                //}

                                //defItem = defItems.FirstOrDefault(a => a.Fzh == item.StationNumber && a.Kh == item.ChanelNumber && a.DevModelID == item.DeviceModel);
                                defItem = defItems.FirstOrDefault(a => a.Fzh == item.StationNumber
                                            && a.Kh == item.ChanelNumber
                                        && ((a.Dzh >= 0 && a.DevPropertyID != (int)ItemDevProperty.Control) //除控制量外的所有传感器
                                        || (a.Dzh > 0 && a.DevPropertyID == (int)ItemDevProperty.Control))); //智能断电器
                                if (defItem != null)
                                {
                                    //从未定义链表中移除
                                    removeItem.Add(item);
                                    LogHelper.Info(item.StationNumber.ToString().PadLeft(3, '0') + item.ChanelNumber.ToString().PadLeft(2, '0') + item.AddressNumber + "从未定义链表中移除！");
                                }
                            }
                        }
                        if (removeItem.Count > 0)
                        {
                            CacheDataHelper.RemoveUnDefineItems(removeItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("ClearUnDefineItems Thread Error:" + ex.Message + ex.StackTrace);
                }
                Thread.Sleep(1000);
            }

        }

        #region ----清除不在通讯队列的设备----
        /// <summary>
        /// 清除未挂接在网络模块上的分站为中断
        /// </summary>
        /// <param name="communicationStation">已挂接在网络模块上的分站队列</param>
        /// <param name="defItems">所有分站信息队列</param>
        private void ClearNoCommunicationStation(List<short> communicationStation, List<Jc_DefInfo> defItems, List<Jc_DevInfo> devItems)
        {
            try
            {
                IDriver driverObj = null;
                Jc_DevInfo devItem;
                List<Jc_DefInfo> stationItems = defItems.Where(a => a.DevPropertyID == (int)DeviceProperty.Substation).ToList();
                foreach (Jc_DefInfo station in stationItems)
                {
                    if (communicationStation.Contains(station.Fzh))
                    {
                        continue;
                    }

                    if ((station.Bz4 & 0x2) != 0x2 && station.State != (short)DeviceRunState.Deleted)
                    {
                        devItem = devItems.FirstOrDefault(a => a.Devid == station.Devid);
                        if (devItem == null)
                        {
                            LogHelper.Debug("分站" + station.Fzh + "设备类型未找到，devid = " + station.Devid);
                            continue;
                        }
                        if (GlobleStaticVariable.driverHandle.DriverItems.ContainsKey(devItem.Sysid))
                        {
                            driverObj = GlobleStaticVariable.driverHandle.DriverItems[devItem.Sysid].DLLObj;
                            // 置分站通讯中断
                            if (station.State != (int)ItemState.EquipmentInterrupted)
                            {
                                LogHelper.Info("分站" + station.Fzh + "未挂接到任何网络模块上，置分站中断：" + station.DttStateTime.ToString() + "-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//2017.10.13 by
                                DriverTransferInterface.Drv_InterruptPro(driverObj, station.Fzh);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("ClearNoCommunicationStation ERROR:" + ex.Message);
            }
        }
        /// <summary>
        /// 网关断线处理（所有网络不置中断）
        /// </summary>
        private void GateWayOutLine()
        {
            try
            {
                //List<Jc_MacInfo> macItems = CacheDataHelper.GetAllMacItems();
                //Dictionary<string, object> updateItems;
                //foreach (Jc_MacInfo mac in macItems)
                //{
                //    mac.NetID = 0;
                //    mac.State = (short)ItemState.EquipmentInterrupted;
                //    //CacheDataHelper.UpdateNetworkModuleCahce(mac);
                //    updateItems = new Dictionary<string, object>();
                //    updateItems.Add("NetID", mac.NetID);
                //    updateItems.Add("State", mac.State);
                //    CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(mac.MAC, updateItems);
                //    StationOutLineByMac(mac);
                //    WriteLog("GateWayOutLine IP:" + mac.IP + "  MAC:" + mac.MAC);
                //}
            }
            catch (Exception ex)
            {
                LogHelper.Error("GateWayOutLine ERROR:" + ex.Message);
            }
        }
        /// <summary>
        /// 强制中断分站
        /// </summary>
        private void StationOutLineByMac(Jc_MacInfo mac)
        {
            //Dictionary<string, object> updateItems;
            //List<Jc_DefInfo> stationItems = GetStationByMac(mac);
            //foreach (Jc_DefInfo item in stationItems)
            //{
            //    item.NErrCount = item.K4 - 2;   //保留两个周期的容错 2017.7.7 by
            //    updateItems = new Dictionary<string, object>();
            //    updateItems.Add("NErrCount", item.NErrCount);
            //    CacheDataHelper.UpdatePointDefineInfoByProperties(item.PointID, updateItems);
            //}
        }

        /// <summary>
        /// 获取Mac下的所有分站
        /// </summary>
        /// <param name="mac"></param>
        /// <returns></returns>
        private List<Jc_DefInfo> GetStationByMac(Jc_MacInfo mac)
        {
            List<Jc_DefInfo> stationItems = new List<Jc_DefInfo>();
            stationItems = CacheDataHelper.GetKJPointDefineItems();
            if (stationItems != null)
            {
                stationItems = stationItems.FindAll(a => a.Jckz1 == mac.MAC && a.Jckz2 == mac.IP);

            }
            return stationItems;
        }

        /// <summary>
        /// 系统退出处理
        /// </summary>
        private void SystemOutProc()
        {
            List<Jc_DefInfo> pointDefineItems = CacheDataHelper.GetKJPointDefineItems();
            pointDefineItems = pointDefineItems.Where(a => a.DevPropertyID == (int)DeviceProperty.Substation).ToList();
            List<Jc_DevInfo> pointDevItems = CacheDataHelper.GetAllDevItems();
            Jc_DevInfo pointDevItem;
            IDriver driverObj;
            DateTime time = DateTime.Now;
            foreach (Jc_DefInfo station in pointDefineItems)
            {
                pointDevItem = pointDevItems.FirstOrDefault(a => a.Devid == station.Devid);
                if (pointDevItem != null)
                {
                    if (GlobleStaticVariable.driverHandle.DriverItems.ContainsKey(pointDevItem.Sysid))
                    {
                        driverObj = GlobleStaticVariable.driverHandle.DriverItems[pointDevItem.Sysid].DLLObj;
                        DriverTransferInterface.Drv_SystemExistPro(driverObj, station.Fzh, time);
                    }
                }
            }
            //人员定位报警表退出处理  20171206
            KJ237CacheHelper.Drv_SystemExistPro(time);

            //写系统退出记录，并更新退出时间到Config表  20170703
            SystemExistRecord();
            SystemExistMcRecord();
        }
        /// <summary>
        /// 写系统退出记录，并更新系统退出时间
        /// </summary>
        /// <returns></returns>
        public static bool SystemExistRecord()
        {
            DateTime SystemExistTime = DateTime.Now;
            bool rvalue = false;
            try
            {
                //IInsertToDbService<Jc_RInfo> jcRTodbService = ServiceFactory.Create<IInsertToDbService<Jc_RInfo>>();
                IConfigService configService = ServiceFactory.Create<IConfigService>();
                ////写系统退出记录
                //Jc_RInfo runlogInfo = new Jc_RInfo();
                //runlogInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                //runlogInfo.PointID = runlogInfo.ID;
                //runlogInfo.Devid = "0";
                //runlogInfo.Fzh = 0;
                //runlogInfo.Kh = 0;
                //runlogInfo.Point = "000000";
                //runlogInfo.Timer = SystemExistTime;
                //runlogInfo.Type = 35;//系统退出
                //runlogInfo.State = 35;//系统退出
                //runlogInfo.Upflag = "0";
                //runlogInfo.Val = "系统退出";
                //runlogInfo.Wzid = "0";
                //runlogInfo.InfoState = InfoState.AddNew;
                ////添加运行记录至数据库          
                //DataToDbAddRequest<Jc_RInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_RInfo>();
                //dataToDbAddRequest.Item = runlogInfo;
                //jcRTodbService.AddItem(dataToDbAddRequest);
                //更新系统退出时间
                ConfigInfo configInfo = new ConfigInfo();
                ConfigGetByNameRequest configGetByNameRequest = new ConfigGetByNameRequest();
                configGetByNameRequest.Name = "SystemExistTime";
                var result = configService.GetConfigByName(configGetByNameRequest);
                if (result != null && result.Data != null)
                {
                    ConfigInfo updateConfigInfo = result.Data;
                    updateConfigInfo.Text = SystemExistTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ConfigUpdateRequest configUpdateRequest = new ConfigUpdateRequest();
                    configUpdateRequest.ConfigInfo = updateConfigInfo;
                    configService.UpdateConfig(configUpdateRequest);
                }
                else
                {
                    ConfigInfo addConfigInfo = new ConfigInfo();
                    addConfigInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId().ToString();
                    addConfigInfo.Name = "SystemExistTime";
                    addConfigInfo.Text = SystemExistTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ConfigAddRequest configAddRequest = new ConfigAddRequest();
                    configAddRequest.ConfigInfo = addConfigInfo;
                    configService.AddConfig(configAddRequest);
                }
                rvalue = true;
            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
                rvalue = false;
            }
            return rvalue;
        }

        public static bool SystemExistMcRecord()
        {
            bool rvalue = false;
            //try
            //{
            //    IInsertToDbService<Jc_McInfo> mcDataInsertToDbService = ServiceFactory.Create<IInsertToDbService<Jc_McInfo>>();
            //    Jc_McInfo densityColl = new Jc_McInfo();
            //    densityColl.PointID = "0";
            //    densityColl.ID = IdHelper.CreateLongId().ToString();
            //    densityColl.Point = "0";
            //    densityColl.Devid = "0";
            //    densityColl.Fzh = 0;
            //    densityColl.Kh = 0;
            //    densityColl.Dzh = 0;
            //    densityColl.Timer = DateTime.Now;
            //    densityColl.Type = 35;
            //    densityColl.State = 35;
            //    densityColl.Ssz = 0;
            //    densityColl.Upflag = "0";
            //    densityColl.Wzid = "0";
            //    densityColl.InfoState = InfoState.AddNew;
            //    //密采记录入库
            //    DataToDbAddRequest<Jc_McInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_McInfo>();
            //    dataToDbAddRequest.Item = densityColl;
            //    mcDataInsertToDbService.AddItem(dataToDbAddRequest);
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Error("SystemExistMcRecord Error【系统退出】" + ex.Message);
            //    rvalue = false;
            //}
            return rvalue;
        }

        #endregion

        /// <summary>
        /// 获取网络模块时间同步下发结构体
        /// </summary>
        /// <param name="_mac"></param>
        /// <returns></returns>
        private MasProtocol TimeSynchronization(string _mac, int _commPort)
        {
            MasProtocol masProtocol = new MasProtocol();

            masProtocol.GenerateNewSeriesNumber();
            masProtocol.Direction = DirectionType.Down;
            masProtocol.SystemType = SystemType.Security;
            masProtocol.ProtocolType = ProtocolType.TimeSynchronizationRequest;
            masProtocol.Version = 1;    //无用
            masProtocol.Flag = 2;

            TimeSynchronizationRequest timeSynchronizationRequest = new TimeSynchronizationRequest();
            timeSynchronizationRequest.DeviceCode = _mac;
            timeSynchronizationRequest.SyncTime = DateTime.Now;
            timeSynchronizationRequest.CommPort = _commPort;
            masProtocol.Protocol = timeSynchronizationRequest;

            return masProtocol;
        }

        private MasProtocol ResetNetWorkDevice(string _mac)
        {
            MasProtocol masProtocol = new MasProtocol();
            masProtocol.GenerateNewSeriesNumber();
            masProtocol.Direction = DirectionType.Down;
            masProtocol.SystemType = SystemType.Security;
            masProtocol.ProtocolType = ProtocolType.ResetNetWorkDeviceRequest;
            masProtocol.Version = 1;    //无用
            masProtocol.Flag = 2;
            //ResetNetWorkDeviceRequest resetNetWorkDeviceRequest = new ResetNetWorkDeviceRequest();
            //resetNetWorkDeviceRequest.Mac = _mac;
            //masProtocol.Protocol = resetNetWorkDeviceRequest;//todo:hdw

            return masProtocol;
        }

        /// <summary>
        /// 取出网络模块绑定的分站队列
        /// </summary>
        /// <param name="_netStationList">JC_MAC.bz1</param>
        /// <param name="stationList"></param>
        private void getStationList(string _netStationList, ref List<short> stationList)
        {
            try
            {
                stationList.Clear();
                foreach (string fzh in _netStationList.Split('|'))
                {
                    if (fzh != "" && fzh != "0")
                    {
                        stationList.Add(Convert.ToInt16(fzh));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug("getStationList_Error:_netStationList = " + _netStationList + " - " + ex.Message);
            }
        }
        /// <summary>
        /// 判断是否有挂接分站
        /// </summary>
        /// <param name="_netStationList"></param>
        /// <param name="stationList"></param>
        private bool IsHaveDeviceForCommInfo(string _netStationList)
        {
            bool ret = false;
            try
            {
                string[] strValue = _netStationList.Split('|');
                if (strValue != null)
                {
                    for (int i = 0; i < strValue.Length; i++)
                    {
                        if (Convert.ToInt32(strValue[i]) > 0)
                        {
                            ret = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug("IsHaveDeviceForCommInfo: = " + _netStationList + " - " + ex.Message);
            }
            return ret;
        }
        /// <summary>
        /// 老F16H分站自动取消电源箱放电 2018.3.12 by
        /// </summary>
        private void PowerBoxControl()
        {
            List<Jc_DefInfo> defItems;
            List<Jc_DevInfo> devItems;
            Jc_DevInfo devItem;
            List<ushort> fzhs;
            List<byte> controls;
            SettingInfo setting = CacheDataHelper.GetSettingByKeyStr("DevicePowerPercentum");
            byte DevicePowerPercentum = 20;
            if (setting != null)
            {
                DevicePowerPercentum = Convert.ToByte(setting.StrValue);
            }

            while (sysrunFlag)
            {
                try
                {
                    fzhs = new List<ushort>();
                    controls = new List<byte>();
                    defItems = CacheDataHelper.GetKJPointDefineItems();
                    devItems = CacheDataHelper.GetAllDevItems();
                    //temp.Bz3 |= 0x8;
                    fzhs = new List<ushort>();
                    controls = new List<byte>();
                    foreach (Jc_DefInfo def in defItems)
                    {
                        if ((def.Bz3 & 0x08) == 0x08)
                        {
                            devItem = devItems.FirstOrDefault(a => a.Devid == def.Devid);

                            if (devItem == null) { continue; }
                            //if (devItem.LC2 != 15) { continue; }    //只处理F16H老分站

                            if ((DateTime.Now - def.PowerDateTime).TotalMinutes > 5)    //五分钟获取一次电源箱数据
                            {
                                //下发D命令获取电源箱数据
                                fzhs.Add((ushort)def.Fzh);
                                controls.Add(0);
                            }
                            else
                            {
                                if (def.State != (short)ItemState.EquipmentDC) { continue; }//只有分站为直流供电时要判断是否需要取消放电

                                List<BatteryItem> BatteryItems = def.BatteryItems;
                                if (BatteryItems == null)
                                {
                                    //下发D命令获取电源箱数据
                                    fzhs.Add((ushort)def.Fzh);
                                    controls.Add(0);
                                }
                                else
                                {
                                    foreach (BatteryItem item in BatteryItems)
                                    {
                                        if (item.PowerPackVOL <= DevicePowerPercentum)
                                        {
                                            //下发D命令取消放电
                                            fzhs.Add((ushort)def.Fzh);
                                            controls.Add(1);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (fzhs.Count > 0 && controls.Count > 0)
                    {
                        CacheDataHelper.SendPowerControl(fzhs, controls);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error("PowerBoxControl Error:" + ex.Message);
                }
                Thread.Sleep(30000);    //两分钟执行一次循环
            }
        }
        #endregion
        #region ----接收数据缓存处理----

        /// <summary>
        /// 出队操作
        /// </summary>
        /// <returns></returns>
        public MasProtocol DataDnqueue(int threadId)
        {
            MasProtocol Rvalue = null;
            lock (_lockObject[threadId])
            {
                Rvalue = ArriveDataList[threadId].Dequeue();
            }
            return Rvalue;
        }
        /// <summary>
        /// 入队操作
        /// </summary>
        /// <param name="Data"></param>
        public void MsgEnqueue(MasProtocol Data)
        {
            //线程启动后再接收数据，线程关闭后不再接收数据
            if (!isStop)
            {
                _receivedDataCount++;
                int tempThreadId = Data.DeviceNumber % DataProcThreadCount;
                lock (_lockObject[tempThreadId])
                {
                    ArriveDataList[tempThreadId].Enqueue(Data);
                }
            }
        }
        /// <summary>
        /// 获取当前队列中的数据总数
        /// </summary>
        /// <returns></returns>
        public int GetArriveDataCount()
        {
            int ArriveDataCount = 0;
            foreach (Queue<MasProtocol> ArriveData in ArriveDataList)
            {
                ArriveDataCount += ArriveData.Count;
            }
            return ArriveDataCount;
        }
        /// <summary>
        /// 获取接收数据包总数
        /// </summary>
        /// <returns></returns>
        public long GetReceivedDataCount()
        {
            return _receivedDataCount;
        }


        long _DataHandlerHandedCount = 0;
        long _Jc_Mc_HandedCount = 0;
        long _Jc_M_HandedCount = 0;
        long _Jc_R_HandedCount = 0;
        long _Jc_B_HandedCount = 0;
        long _Jc_K_HandedCount = 0;
        long _Jc_LL_H_HandedCount = 0;
        long _Jc_LL_D_HandedCount = 0;
        long _Jc_LL_M_HandedCount = 0;
        long _Jc_LL_Y_HandedCount = 0;


        public void OutputStaticLog()
        {
            long totalCount = 0;
            long unHandleCount = 0;
            long handledCount = 0;
            long cycelHandledCount = 0;

            totalCount = GetReceivedDataCount();
            unHandleCount = GetArriveDataCount();
            handledCount = totalCount - unHandleCount;//获取总的已处理量；
            cycelHandledCount = handledCount - _DataHandlerHandedCount;//获取本周期处理量
            _DataHandlerHandedCount = handledCount;//上次总处理量
            //数据处理队列积压情况
            Basic.Framework.Logging.LogHelper.SystemInfo("数据处理积压数量：" + unHandleCount + "  接收数据数量：" + GetReceivedDataCount() + " 本次处理量：" + cycelHandledCount + "");


            //    totalCount = Jc_McInfoToDbService.GetTotalCount().Data;
            //    unHandleCount = GetJc_McInfoToDbListCount();
            //    handledCount = totalCount - unHandleCount;//获取总的已处理量；
            //    cycelHandledCount = handledCount - _Jc_Mc_HandedCount;//获取本周期处理量
            //    _Jc_Mc_HandedCount = handledCount;//上次总处理量
            //    //密采入库队列
            //    Basic.Framework.Logging.LogHelper.SystemInfo("密采入库队列中积压数量：" + unHandleCount + "  总数据量：" + totalCount + " 本周期处理量：" + cycelHandledCount);




            //    //5分钟入库队列
            //    Basic.Framework.Logging.LogHelper.SystemInfo("5分钟入库队列中积压数量：" + GetJc_MInfoToDbListCount() + "  总数据量：" + Jc_MInfoToDbService.GetTotalCount().Data);
            //    //运行记录入库队列
            //    Basic.Framework.Logging.LogHelper.SystemInfo("运行记录入库队列中积压数量：" + GetJc_RInfoToDbListCount() + "  总数据量：" + Jc_RInfoToDbService.GetTotalCount().Data);





            //    totalCount = Jc_BInfoToDbService.GetTotalCount().Data;
            //    unHandleCount = GetJc_BInfoToDbListCount();
            //    handledCount = totalCount - unHandleCount;//获取总的已处理量；
            //    cycelHandledCount = handledCount - _Jc_B_HandedCount;//获取本周期处理量
            //    _Jc_B_HandedCount = handledCount;//上次总处理量
            //    //报警记录入库队列
            //    Basic.Framework.Logging.LogHelper.SystemInfo("报警记录入库队列中积压数量：" + unHandleCount + "  总数据量：" + totalCount + " 本周期处理量：" + cycelHandledCount);



            //    //馈电异常记录入库队列
            //    Basic.Framework.Logging.LogHelper.SystemInfo("馈电异常记录入库队列中积压数量：" + GetJc_KdInfoToDbListCount() + "  总数据量：" + Jc_KdInfoToDbService.GetTotalCount().Data);
            //    //抽放小时记录入库队列
            //    Basic.Framework.Logging.LogHelper.SystemInfo("抽放小时记录入库队列中积压数量：" + GetJc_Ll_HToDbListCount() + "  总数据量：" + Jc_Ll_HToDbService.GetTotalCount().Data);
            //    //抽放日记录入库队列
            //    Basic.Framework.Logging.LogHelper.SystemInfo("抽放日记录入库队列中积压数量：" + GetJc_Ll_DToDbListCount() + "  总数据量：" + Jc_Ll_DToDbService.GetTotalCount().Data);
            //    //抽放月记录入库队列
            //    Basic.Framework.Logging.LogHelper.SystemInfo("抽放月记录入库队列中积压数量：" + GetJc_Ll_MToDbListCount() + "  总数据量：" + Jc_Ll_MToDbService.GetTotalCount().Data);
            //    //抽放年记录入库队列
            //    Basic.Framework.Logging.LogHelper.SystemInfo("&&&&&&&&&&&&&&&抽放年记录入库队列中积压数量：" + GetJc_Ll_YToDbListCount() + "  总数据量：" + Jc_Ll_YToDbService.GetTotalCount().Data + "&&&&&&&&&&&&&&&");
            //
        }


        #endregion
        #region ----数据处理方法----

        /// <summary>
        /// 调用驱动接口处理数据
        /// </summary>
        /// <param name="masProtocol"></param>
        private void CallDriverDataProc(MasProtocol masProtocol)
        {
            if (masProtocol.SystemType == SystemType.Security)
            {
                //安全监控系统
                //IDriver dDeviceObj = GlobleStaticVariable.driverHandle.DriverItems[0].DLLObj;
                IDriver dDeviceObj = GlobleStaticVariable.driverHandle.DriverItems.FirstOrDefault(a => a.Key == (int)SystemEnum.Security).Value.DLLObj;
                if (dDeviceObj != null)
                {
                    DriverTransferInterface.DataProc(dDeviceObj, masProtocol);
                }
            }
            else if (masProtocol.SystemType == SystemType.Personnel)
            {
                //人员定位系统
                IDriver dDeviceObj = GlobleStaticVariable.driverHandle.DriverItems.FirstOrDefault(a => a.Key == (int)SystemEnum.Personnel).Value.DLLObj;
                if (dDeviceObj != null)
                {
                    DriverTransferInterface.DataProc(dDeviceObj, masProtocol);
                }
            }
            else
            {
                LogHelper.Debug("CallDriverDataProc not find    --   " + masProtocol.SystemType);
            }
        }

        /// <summary>
        /// 网络模块状态数据回发 5秒一包
        /// </summary>
        /// <param name="masProtocol"></param>
        private void NetworkDeviceDataRequestProc(MasProtocol masProtocol)
        {
            NetworkDeviceDataRequest networkDeviceDataRequest = masProtocol.Deserialize<NetworkDeviceDataRequest>();
            List<Jc_MacInfo> macItems = CacheDataHelper.GetAllMacItems();
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            if (macItems != null)
            {
                Jc_MacInfo mac = macItems.FirstOrDefault(a => a.IP == networkDeviceDataRequest.DeviceCode);
                if (mac != null)
                {
                    if (mac.IP != networkDeviceDataRequest.Address || mac.State != (short)networkDeviceDataRequest.State)
                    {
                        CreateMacRunLogInfo(mac, DateTime.Now, (short)networkDeviceDataRequest.State, networkDeviceDataRequest.Address);
                    }
                    mac.IP = networkDeviceDataRequest.Address;
                    mac.MAC = networkDeviceDataRequest.Channel;
                    mac.NetID = Convert.ToInt32(networkDeviceDataRequest.RealData);
                    mac.State = (short)networkDeviceDataRequest.State;
                    mac.LastGetTime = DateTime.Now;
                    mac.isSendReset = false;
                    //CacheDataHelper.UpdateNetworkModuleCahce(mac);
                    updateItems.Add("IP", mac.IP);
                    updateItems.Add("MAC", mac.MAC);
                    updateItems.Add("NetID", mac.NetID);
                    updateItems.Add("State", mac.State);
                    updateItems.Add("LastGetTime", mac.LastGetTime);
                    updateItems.Add("isSendReset", mac.isSendReset);
                    CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(mac.MAC, updateItems);
                    //屏蔽  20170718
                    //if (mac.NetID == 0)
                    //{
                    //    StationOutLineByMac(mac);
                    //}
                    WriteLog("NetworkDeviceDataRequestProc:" + mac.IP + " , netID:" + mac.NetID);

                }
            }
        }

        public static IRunLogCacheService runLogCacheService = null;
        public static IInsertToDbService<Jc_RInfo> jcRTodbService = null;
        /// <summary>
        /// 生成运行记录 并保存到数据库及缓存 2018.3.21 by
        /// </summary>
        /// <param name="def">测点信息</param>
        /// <param name="time">数据采集时间</param>
        /// <param name="dataState">数据状态</param>
        /// <param name="runState">设备状态</param>
        /// <param name="realValue">实时值</param>
        public static void CreateMacRunLogInfo(Jc_MacInfo macInfo, DateTime time, short state, string ip)
        {
            try
            {
                if (runLogCacheService == null)
                {
                    runLogCacheService = ServiceFactory.Create<IRunLogCacheService>();
                }
                if (jcRTodbService == null)
                {
                    jcRTodbService = ServiceFactory.Create<IInsertToDbService<Jc_RInfo>>();
                }
                Jc_RInfo runlogInfo = new Jc_RInfo();
                runlogInfo.ID = IdHelper.CreateLongId().ToString();
                runlogInfo.Devid = "0";
                runlogInfo.PointID = "0";
                runlogInfo.Fzh = 0;
                runlogInfo.Kh = 0;
                runlogInfo.Dzh = 0;
                runlogInfo.Point = macInfo.MAC;
                runlogInfo.Upflag = "0";
                runlogInfo.Wzid = macInfo.Wzid;

                runlogInfo.Val = state == 0 ? "通讯中断" : "交流正常";
                runlogInfo.Timer = time;
                runlogInfo.Type = state;
                runlogInfo.State = state;
                runlogInfo.Bz1 = ip;

                runlogInfo.InfoState = InfoState.AddNew;
                //添加运行记录至缓存
                RunLogCacheAddRequest runLogCacheAddRequest = new RunLogCacheAddRequest();
                runLogCacheAddRequest.RunLogInfo = runlogInfo;
                runLogCacheService.AddRunLogCache(runLogCacheAddRequest);
                //添加运行记录至数据库
                DataToDbAddRequest<Jc_RInfo> dataToDbAddRequest = new DataToDbAddRequest<Jc_RInfo>();
                dataToDbAddRequest.Item = runlogInfo;
                jcRTodbService.AddItem(dataToDbAddRequest);
            }
            catch (Exception ex)
            {
                LogHelper.Error("CreateMacRunLogInfo Error【" + macInfo.MAC + "】" + ex.Message);
            }
        }

        /// <summary>
        /// 处理交换机电源箱数据
        /// </summary>
        /// <param name="masProtocol"></param>
        private void QueryBatteryRealDataResponseProc(MasProtocol masProtocol)
        {

            QueryBatteryRealDataResponse queryBatteryRealDataResponse = masProtocol.Deserialize<QueryBatteryRealDataResponse>();
            List<BatteryRealDataItem> batteryRealDataItems = queryBatteryRealDataResponse.BatteryRealDataItems;
            List<Jc_MacInfo> macItems = CacheDataHelper.GetAllMacItems(); //todo 查单个MAC
            Jc_MacInfo macItem = macItems.FirstOrDefault(a => a.IP == queryBatteryRealDataResponse.DeviceCode);
            BatteryItem batteryItem;
            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            if (macItem != null && batteryRealDataItems != null)
            {
                macItem.BatteryItems = new List<BatteryItem>();
                foreach (BatteryRealDataItem item in batteryRealDataItems)
                {
                    batteryItem = new BatteryItem();
                    batteryItem.Channel = item.Channel;
                    batteryItem.BatteryAddress = item.Address;
                    //batteryItem.BatteryState = item.BatteryState;
                    //batteryItem.BatteryTooHot = item.BatteryTooHot;
                    batteryItem.BatteryACDC = item.BatteryACDC;
                    //batteryItem.BatteryOverCharge = item.BatteryOverCharge;
                    //batteryItem.BatteryUndervoltage = item.BatteryUndervoltage;
                    //batteryItem.BatteryPackStateCd = item.BatteryPackStateCd;
                    //batteryItem.BatteryPackStateJh = item.BatteryPackStateJh;
                    //batteryItem.BatteryPackStateFd = item.BatteryPackStateFd;
                    batteryItem.BatteryVOL = item.BatteryVOL;
                    //batteryItem.PowerPackMA = item.BatteryPackMA;
                    //batteryItem.PowerPackVOL = item.BatteryPackVOL;
                    batteryItem.DeviceTemperature1 = item.DeviceTemperature1;
                    batteryItem.DeviceTemperature2 = item.DeviceTemperature2;
                    batteryItem.TotalVoltage = item.TotalVoltage;
                    macItem.BatteryItems.Add(batteryItem);
                }
                macItem.PowerDateTime = queryBatteryRealDataResponse.BatteryDateTime;
                macItem.SendBatteryControlCount--;

                if (macItem.SendBatteryControlCount > 1)
                {
                    macItem.SendBatteryControlCount = 1;
                }
                else
                {
                    macItem.NCommandbz &= 0xFFFE;
                }

                if (macItem.SendBatteryControlCount < 0)
                {
                    //避免下发少，回发多的情况造成之后的命令少下发了的问题
                    macItem.SendBatteryControlCount = 0;
                }
                updateItems.Add("BatteryItems", macItem.BatteryItems);
                updateItems.Add("PowerDateTime", macItem.PowerDateTime);
                updateItems.Add("SendBatteryControlCount", macItem.SendBatteryControlCount);
                updateItems.Add("NCommandbz", macItem.NCommandbz);
                CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(macItem.MAC, updateItems);
                //CacheDataHelper.UpdateNetworkModuleCahce(macItem);

                WriteLog("QueryBatteryRealDataResponseProc:" + macItem.IP);
            }
        }

        /// <summary>
        /// 处理交换机相关状态数据 
        /// </summary>
        /// <param name="masProtocol"></param>
        private void QuerySwitchRealDataResponseProc(GetSwitchboardParamCommResponse getSwitchboardParamCommResponse)
        {


            List<Jc_MacInfo> macItems = CacheDataHelper.GetAllMacItems(); //todo 查单个MAC
            Jc_MacInfo macItem = macItems.FirstOrDefault(a => a.MAC == getSwitchboardParamCommResponse.DeviceCode);

            Dictionary<string, object> updateItems = new Dictionary<string, object>();
            if (macItem != null)
            {

                updateItems.Add("BatteryControlState", getSwitchboardParamCommResponse.BatteryControlState);
                updateItems.Add("BatteryState", getSwitchboardParamCommResponse.BatteryState);
                updateItems.Add("BatteryCapacity", getSwitchboardParamCommResponse.BatteryCapacity);
                updateItems.Add("SerialPortBatteryState", getSwitchboardParamCommResponse.SerialPortBatteryState);
                updateItems.Add("SerialPortRunState", getSwitchboardParamCommResponse.SerialPortRunState);
                updateItems.Add("SwitchBatteryState", getSwitchboardParamCommResponse.SwitchBatteryState);
                updateItems.Add("SwitchRunState", getSwitchboardParamCommResponse.SwitchRunState);
                updateItems.Add("Switch1000State", getSwitchboardParamCommResponse.Switch1000State);
                updateItems.Add("Switch100State", getSwitchboardParamCommResponse.Switch100State);

                CacheDataHelper.UpdateNetworkModeuleCacheByPropertys(macItem.MAC, updateItems);


                //WriteLog("QuerySwitchRealDataResponseProc:" + macItem.IP);
            }
        }

        private void SetGatewayState(bool setGatewayState)
        {
            IRemoteStateService remoteStateService = ServiceFactory.Create<IRemoteStateService>();
            RemoteStateRequest remoteStateRequest = new RemoteStateRequest();
            remoteStateRequest.State = setGatewayState;
            remoteStateService.SetGatewayState(remoteStateRequest);
        }

        private bool GetGatewayState()
        {
            bool state = false;
            //IRemoteStateService remoteStateService = ServiceFactory.Create<IRemoteStateService>();
            var response = remoteStateService.GetGatewayState();
            if (response.IsSuccess)
            {
                state = response.Data;
            }
            return state;
        }

        private void SetGatewayTime(DateTime time)
        {
            RemoteStateRequest remoteStateRequest = new RemoteStateRequest();
            remoteStateRequest.LastReviceTime = time;
            remoteStateService.SetLastReciveTime(remoteStateRequest);
        }

        #endregion
        #region ----数据补录方法----
        /// <summary>
        /// 补写JC_B结束记录系统启动时将之前未处理的报警写记录结束时间
        /// </summary>
        public void AddOverTime()
        {
            try
            {
                string key = "SysRunTime";
                DateTime time = DateTime.Now;
                ////读取系统退出时间，做为数据补录的结束时间  20170703
                //IConfigService configService = ServiceFactory.Create<IConfigService>();
                //ConfigGetByNameRequest configGetByNameRequest = new ConfigGetByNameRequest();
                //configGetByNameRequest.Name = "SystemExistTime";

                //var resultConfig = configService.GetConfigByName(configGetByNameRequest);

                //if (resultConfig != null && resultConfig.Data != null)
                //{
                //    DateTime tempTime = time;
                //    DateTime.TryParse(resultConfig.Data.Text, out tempTime);

                //    if (tempTime.ToShortDateString() == time.AddDays(-1).ToShortDateString())//昨天退出，今天才启动的情况下补昨天的23:59:59
                //    {
                //        tempTime = DateTime.Parse(tempTime.ToString("yyyy-MM-dd") + " 23:59:59");
                //    }
                //    else//其它情况，补当前启动时间
                //    {
                //        tempTime = time;
                //    }
                //    time = tempTime;//2017.11.28 by
                //}

                //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //if (config.AppSettings.Settings[key] != null)
                //{
                //    time = Convert.ToDateTime(config.AppSettings.Settings[key].Value);
                //}

                double tempValue = 0;

                IAlarmRecordService alarmRecordService = ServiceFactory.Create<IAlarmRecordService>();

                #region 处理监控设备报警数据  20171208
                var response = alarmRecordService.GetAlarmedDataList();
                string myIp = GetMyIp();
                if (response.IsSuccess && response.Data != null)
                {

                    List<Jc_BInfo> alarmItems = response.Data;
                    LogHelper.Info("处理之前未结束的监控设备报警数据:" + alarmItems.Count);
                    alarmItems.ForEach(item =>
                    {
                        if (item.Stime.Date == time.Date)
                        {
                            if (item.Stime > time)  //2018.3.23 by  避免数据补写时，开始时间大于结束时间
                            {
                                time = item.Stime;
                            }
                            item.Etime = time;
                        }
                        else
                        {
                            //item.Etime = new DateTime(time.AddDays(-1).Year, time.AddDays(-1).Month, time.AddDays(-1).Day, 23, 59, 59);
                            item.Etime = new DateTime(item.Stime.Year, item.Stime.Month, item.Stime.Day, 23, 59, 59);
                        }

                        //2017.6.25 by
                        if (item.Zdzs < new DateTime(2000, 1, 1))
                        {
                            if (double.TryParse(item.Ssz, out tempValue))
                            {
                                item.Zdz = tempValue;
                                item.Pjz = tempValue;
                                item.Bz4 = tempValue.ToString("F2");
                            }
                            else
                            {
                                item.Zdz = 0;
                                item.Pjz = 0;
                                item.Bz4 = "0";
                            }
                            item.Zdzs = item.Stime;
                            item.Bz5 = item.Stime.ToString("yyyy-MM-dd HH:mm:ss");
                            item.Bz3 = myIp;
                        }

                    });
                    //批量更新报警记录
                    AlarmRecordBatchUpateRequesst alarmRecordBatchUpateRequesst = new AlarmRecordBatchUpateRequesst();
                    alarmRecordBatchUpateRequesst.AlarmInfos = alarmItems;
                    alarmRecordService.BacthUpdateAlarmRecord(alarmRecordBatchUpateRequesst);
                    LogHelper.Info("处理之前未结束的监控设备报警数据完成");
                }
                #endregion

                #region 处理人员定位设备报警数据  20171208
                //response = alarmRecordService.GetR_AlarmedDataList();
                //if (response.IsSuccess && response.Data != null)
                //{

                //    List<Jc_BInfo> alarmItems = response.Data;
                //    LogHelper.Info("处理之前未结束的人员设备报警数据:" + alarmItems.Count);
                //    alarmItems.ForEach(item =>
                //    {
                //        item.Etime = time;

                //        //2017.6.25 by
                //        if (item.Zdzs < new DateTime(2000, 1, 1))
                //        {
                //            if (double.TryParse(item.Ssz, out tempValue))
                //            {
                //                item.Zdz = tempValue;
                //                item.Pjz = tempValue;
                //            }
                //            else
                //            {
                //                item.Zdz = 0;
                //                item.Pjz = 0;
                //            }
                //            item.Zdzs = item.Stime;
                //            item.Bz3 = myIp + " " + time.ToString("yyyy-MM-dd HH:mm:ss");
                //        }

                //    });
                //    //批量更新报警记录
                //    AlarmRecordBatchUpateRequesst alarmRecordBatchUpateRequesst = new AlarmRecordBatchUpateRequesst();
                //    alarmRecordBatchUpateRequesst.AlarmInfos = alarmItems;
                //    alarmRecordService.BacthUpdateR_AlarmRecord(alarmRecordBatchUpateRequesst);
                //    LogHelper.Info("处理之前未结束的人员设备报警数据完成");
                //}
                #endregion

                //重新加载报警记录(处理，将人员定位和监控的报警数据加载到同一缓存)  20171208
                IAlarmCacheService _AlarmCacheService = ServiceFactory.Create<IAlarmCacheService>();
                _AlarmCacheService.LoadAlarmCache(new AlarmCacheLoadRequest());

            }
            catch (Exception ex)
            {
                Basic.Framework.Logging.LogHelper.Error(ex);
            }
        }

        public string GetMyIp()
        {
            string myIp = "127.0.0.1";
            try
            {
                string name = System.Net.Dns.GetHostName();
                System.Net.IPAddress[] ipadrlist = System.Net.Dns.GetHostAddresses(name);
                foreach (System.Net.IPAddress ipa in ipadrlist)
                {
                    if (ipa.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        myIp = ipa.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("GetMyIp Error:" + ex);
                myIp = "Error";
            }
            return myIp;
        }
        #endregion
        #region ----注册事件----
        /// <summary>
        /// 驱动数据下发（通过注册驱动的数据下发事件完成）
        /// </summary>
        /// <param name="masProtocol"></param>
        void DLLObj_OnDriverSendDataEventHandler(MasProtocol masProtocol)
        {
            try
            {
                if (gatewayState)
                {
                    MasProtocol sendResult = RpcService.Send<MasProtocol>(masProtocol, RequestType.DeviceRequest);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    LogHelper.Debug("RpcService.Send<MasProtocol> Error:" + Basic.Framework.Common.JSONHelper.ToJSONString(masProtocol) + ex.Message + ex.StackTrace);
                }
                catch { }
            }
        }
        /// <summary>
        /// 驱动加载成功，注册驱动下发数据事件
        /// </summary>
        void driverHandle_OnLoadDriverSuccessEvent()
        {
            Dictionary<decimal, DriverItem> DriverItems = GlobleStaticVariable.driverHandle.DriverItems;
            foreach (DriverItem driver in DriverItems.Values)
            {
                driver.DLLObj.OnDriverSendDataEventHandler += DLLObj_OnDriverSendDataEventHandler;
            }
        }
        void RpcService_OnDeviceMessageArrived(MasProtocol masProtocol)
        {
            if (!CrossDayAndFiveMiniteHandle.isCrossDay)
            {
                SetGatewayTime(DateTime.Now);
                MsgEnqueue(masProtocol);
            }
        }
        #endregion

        private void WriteLog(string _log)
        {
            LogHelper.Info(_log);
        }

        private void SysRun()
        {
            string key = "SysRunTime";
            string value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            while (sysrunFlag)
            {
                try
                {
                    value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    if (config.AppSettings.Settings[key] != null)
                    {
                        config.AppSettings.Settings[key].Value = value;
                    }
                    else
                    {
                        config.AppSettings.Settings.Add(key, value);
                    }
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");

                }
                catch (Exception ex)
                {
                    LogHelper.Error("SysRun Error:" + ex);
                }
                Thread.Sleep(2000);
            }
        }

        #region ---test----
        private static DateTime testData = DateTime.Now;
        private void PersonTestData()
        {
            if ((DateTime.Now - testData).TotalSeconds > 10) //10秒生成一条数据
            {
                testData = DateTime.Now;

                MasProtocol masProtocol = new MasProtocol(SystemType.Personnel, DirectionType.Up, ProtocolType.QueryRealDataResponse);
                masProtocol.ProtocolType = ProtocolType.QueryRealDataResponse;
                masProtocol.Protocol = JSONHelper.ToJSONString(GetResponse());

                MsgEnqueue(masProtocol);
            }
        }

        private QueryRealDataResponse GetResponse()
        {
            QueryRealDataResponse data = new QueryRealDataResponse();

            data.DeviceCode = "0010000";
            data.RealDataItems = GetRealDataItem();

            return data;
        }

        private List<RealDataItem> GetRealDataItem()
        {
            List<RealDataItem> items = new List<RealDataItem>();

            RealDataItem item;
            string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            #region ----人员标识卡----

            item = new RealDataItem();
            ///// <summary>
            ///// 表示设备的通道号（=0表示采集设备本身，=1表示此采集设备下的1号设备….）;人员定位：kh
            ///// </summary>
            item.Channel = "1";
            ///// <summary>
            ///// 表示设备的地址号（单参数传感器此值为0，多参数传感器此值为其地址号） 人员定位：bh
            ///// </summary>
            item.Address = "1001";
            ///// <summary>
            ///// 实时值（表示监测设备的实时值，如果此分站为人员定位系统下的识别器就表示为卡号；）；人员定位：接收时间
            ///// </summary>
            item.RealData = timeNow;
            ///// <summary>
            ///// 数据状态描述
            ///// </summary>
            //public ItemState State { get; set; }
            ///// <summary>
            ///// 电压值描述（传感器时跟的是电压等级，分站如果挂接的智能电源箱跟的是电源箱的电压值）；人员定位：系统类型标志:0—人员,1—机车
            ///// <summary>
            item.Voltage = "0";
            ///// 回控状态（0无电 1无电）;  人员定位：是否呼叫
            ///// </summary>
            item.FeedBackState = "1";
            ///// <summary>
            ///// 馈电状态（1馈电成功 2 馈电失败 3 复电成功 4 负电失败） ；  人员定位：是否为补传
            ///// </summary>
            item.FeedState = "0";
            ///// <summary>
            ///// 表示设备的唯一编码；人员定位：欠压标记
            ///// </summary>
            item.SoleCoding = "0";
            item.DeviceProperty = ItemDevProperty.CardInfo;
            items.Add(item);
            #endregion

            #region ----人员标识卡----

            item = new RealDataItem();
            ///// <summary>
            ///// 表示设备的通道号（=0表示采集设备本身，=1表示此采集设备下的1号设备….）;人员定位：kh
            ///// </summary>
            item.Channel = "2";
            ///// <summary>
            ///// 表示设备的地址号（单参数传感器此值为0，多参数传感器此值为其地址号） 人员定位：bh
            ///// </summary>
            item.Address = "1002";
            ///// <summary>
            ///// 实时值（表示监测设备的实时值，如果此分站为人员定位系统下的识别器就表示为卡号；）；人员定位：接收时间
            ///// </summary>
            item.RealData = timeNow;
            ///// <summary>
            ///// 数据状态描述
            ///// </summary>
            //public ItemState State { get; set; }
            ///// <summary>
            ///// 电压值描述（传感器时跟的是电压等级，分站如果挂接的智能电源箱跟的是电源箱的电压值）；人员定位：系统类型标志:0—人员,1—机车
            ///// <summary>
            item.Voltage = "0";
            ///// 回控状态（0无电 1无电）;  人员定位：是否呼叫
            ///// </summary>
            item.FeedBackState = "1";
            ///// <summary>
            ///// 馈电状态（1馈电成功 2 馈电失败 3 复电成功 4 负电失败） ；  人员定位：是否为补传
            ///// </summary>
            item.FeedState = "0";
            ///// <summary>
            ///// 表示设备的唯一编码；人员定位：欠压标记
            ///// </summary>
            item.SoleCoding = "0";
            item.DeviceProperty = ItemDevProperty.CardInfo;
            items.Add(item);
            #endregion

            #region ----分站----

            item = new RealDataItem();
            ///// <summary>
            ///// 表示设备的通道号（=0表示采集设备本身，=1表示此采集设备下的1号设备….）;人员定位：kh
            ///// </summary>
            item.Channel = "0";
            ///// <summary>
            ///// 表示设备的地址号（单参数传感器此值为0，多参数传感器此值为其地址号） 人员定位：bh
            ///// </summary>
            item.Address = "0";
            ///// <summary>
            ///// 实时值（表示监测设备的实时值，如果此分站为人员定位系统下的识别器就表示为卡号；）；人员定位：接收时间
            ///// </summary>
            item.RealData = "交流正常";
            ///// <summary>
            ///// 数据状态描述
            ///// </summary>
            item.State = ItemState.EquipmentDC;
            ///// <summary>
            ///// 电压值描述（传感器时跟的是电压等级，分站如果挂接的智能电源箱跟的是电源箱的电压值）；人员定位：系统类型标志:0—人员,1—机车
            ///// <summary>
            item.Voltage = "0";
            ///// 回控状态（0无电 1无电）;  人员定位：是否呼叫
            ///// </summary>
            item.FeedBackState = "0";
            ///// <summary>
            ///// 馈电状态（1馈电成功 2 馈电失败 3 复电成功 4 负电失败） ；  人员定位：是否为补传
            ///// </summary>
            item.FeedState = "0";
            ///// <summary>
            ///// 表示设备的唯一编码；人员定位：欠压标记
            ///// </summary>
            item.SoleCoding = "0";
            item.DeviceProperty = ItemDevProperty.Substation;
            items.Add(item);
            #endregion

            #region ----识别器----

            item = new RealDataItem();
            ///// <summary>
            ///// 表示设备的通道号（=0表示采集设备本身，=1表示此采集设备下的1号设备….）;人员定位：kh
            ///// </summary>
            item.Channel = "1";
            ///// <summary>
            ///// 表示设备的地址号（单参数传感器此值为0，多参数传感器此值为其地址号） 人员定位：bh
            ///// </summary>
            item.Address = "0";
            ///// <summary>
            ///// 实时值（表示监测设备的实时值，如果此分站为人员定位系统下的识别器就表示为卡号；）；人员定位：接收时间
            ///// </summary>
            item.RealData = "正常";
            ///// <summary>
            ///// 数据状态描述
            ///// </summary>
            item.State = ItemState.EquipmentCommOK;
            ///// <summary>
            ///// 电压值描述（传感器时跟的是电压等级，分站如果挂接的智能电源箱跟的是电源箱的电压值）；人员定位：系统类型标志:0—人员,1—机车
            ///// <summary>
            item.Voltage = "0";
            ///// 回控状态（0无电 1无电）;  人员定位：是否呼叫
            ///// </summary>
            item.FeedBackState = "0";
            ///// <summary>
            ///// 馈电状态（1馈电成功 2 馈电失败 3 复电成功 4 负电失败） ；  人员定位：是否为补传
            ///// </summary>
            item.FeedState = "0";
            ///// <summary>
            ///// 表示设备的唯一编码；人员定位：欠压标记
            ///// </summary>
            item.SoleCoding = "0";
            item.DeviceProperty = ItemDevProperty.Recognizer;
            items.Add(item);
            #endregion

            #region ----识别器----

            item = new RealDataItem();
            ///// <summary>
            ///// 表示设备的通道号（=0表示采集设备本身，=1表示此采集设备下的1号设备….）;人员定位：kh
            ///// </summary>
            item.Channel = "2";
            ///// <summary>
            ///// 表示设备的地址号（单参数传感器此值为0，多参数传感器此值为其地址号） 人员定位：bh
            ///// </summary>
            item.Address = "0";
            ///// <summary>
            ///// 实时值（表示监测设备的实时值，如果此分站为人员定位系统下的识别器就表示为卡号；）；人员定位：接收时间
            ///// </summary>
            item.RealData = "正常";
            ///// <summary>
            ///// 数据状态描述
            ///// </summary>
            item.State = ItemState.EquipmentCommOK;
            ///// <summary>
            ///// 电压值描述（传感器时跟的是电压等级，分站如果挂接的智能电源箱跟的是电源箱的电压值）；人员定位：系统类型标志:0—人员,1—机车
            ///// <summary>
            item.Voltage = "0";
            ///// 回控状态（0无电 1无电）;  人员定位：是否呼叫
            ///// </summary>
            item.FeedBackState = "0";
            ///// <summary>
            ///// 馈电状态（1馈电成功 2 馈电失败 3 复电成功 4 负电失败） ；  人员定位：是否为补传
            ///// </summary>
            item.FeedState = "0";
            ///// <summary>
            ///// 表示设备的唯一编码；人员定位：欠压标记
            ///// </summary>
            item.SoleCoding = "0";
            item.DeviceProperty = ItemDevProperty.Recognizer;
            items.Add(item);
            #endregion
            return items;
        }
        #endregion

        /// <summary>
        /// 同步超时检测
        /// </summary>
        private void SyncTimeOut()
        {
            while (!isStop)
            {
                try
                {
                    RPointDefineCacheGetAllRequest pointDefineCacheRequest = new RPointDefineCacheGetAllRequest();
                    List<Jc_DefInfo> allPersonPoint = _rPointDefineCacheService.GetAllRPointDefineCache(pointDefineCacheRequest).Data;
                    foreach (Jc_DefInfo point in allPersonPoint)
                    {
                        if (point.DttStateTime > personLastSyncTime)
                        {
                            personLastSyncTime = point.DttStateTime;
                        }
                    }
                    TimeSpan ts = DateTime.Now - personLastSyncTime;
                    if (ts.TotalSeconds > 300 && !personSyncTimeOutFlag)//5分钟如果人员定位系统都还没有调用接口的话，将所有人员定位设备置成通讯中断
                    {
                        //输出日志
                        Basic.Framework.Logging.LogHelper.Debug("人员定位系统超过5分钟未上传数据，置所有分站中断！！！");
                        Dictionary<string, Dictionary<string, object>> updatePointItems = new Dictionary<string, Dictionary<string, object>>();
                        foreach (Jc_DefInfo point in allPersonPoint)
                        {
                            Dictionary<string, object> updatePointItem = new Dictionary<string, object>();
                            if (point.DevPropertyID == 0)
                            {
                                updatePointItem.Add("State", 0);
                                updatePointItem.Add("DataState", 0);
                                updatePointItem.Add("DttStateTime", DateTime.Now);
                                updatePointItem.Add("Ssz", EnumHelper.GetEnumDescription((DeviceDataState)0));
                            }
                            else
                            {
                                updatePointItem.Add("State", 20);
                                updatePointItem.Add("DataState", 20);
                                updatePointItem.Add("DttStateTime", DateTime.Now);
                                updatePointItem.Add("Ssz", EnumHelper.GetEnumDescription((DeviceDataState)20));
                            }
                            updatePointItems.Add(point.PointID, updatePointItem);
                        }
                        if (updatePointItems.Count > 0)
                        {
                            RDefineCacheBatchUpdatePropertiesRequest rDefUpdateCacheRequest = new RDefineCacheBatchUpdatePropertiesRequest();
                            rDefUpdateCacheRequest.PointItems = updatePointItems;
                            _rPointDefineCacheService.BatchUpdatePointDefineInfo(rDefUpdateCacheRequest);
                            personSyncTimeOutFlag = true;
                        }
                    }
                    else
                    {
                        personSyncTimeOutFlag = false;
                    }

                    B_DefCacheGetAllRequest bDefCacheRequest = new B_DefCacheGetAllRequest();
                    List<Jc_DefInfo> allBroadCastPoint = _b_DefCacheService.GetAll(bDefCacheRequest).Data;

                    foreach (Jc_DefInfo point in allBroadCastPoint)
                    {
                        if (point.DttStateTime > broadcastLastSyncTime)
                        {
                            broadcastLastSyncTime = point.DttStateTime;
                        }
                    }
                    ts = DateTime.Now - broadcastLastSyncTime;
                    if (ts.TotalSeconds > 300 && !broadcastSyncTimeOutFlag)//5分钟如果广播系统都还没有调用接口的话，将所有广播设备置成通讯中断
                    {
                        //输出日志
                        Basic.Framework.Logging.LogHelper.Debug("广播系统超过5分钟未上传数据，置所有分站中断！！！");
                        Dictionary<string, Dictionary<string, object>> updatePointItems = new Dictionary<string, Dictionary<string, object>>();
                        foreach (Jc_DefInfo point in allBroadCastPoint)
                        {
                            Dictionary<string, object> updatePointItem = new Dictionary<string, object>();
                            updatePointItem.Add("State", 0);
                            updatePointItem.Add("DataState", 0);
                            updatePointItem.Add("DttStateTime", DateTime.Now);
                            updatePointItem.Add("Ssz", EnumHelper.GetEnumDescription((DeviceDataState)0));
                            updatePointItems.Add(point.PointID, updatePointItem);
                        }
                        if (updatePointItems.Count > 0)
                        {
                            BatchUpdatePropertiesRequest bDefUpdateCacheRequest = new BatchUpdatePropertiesRequest();
                            bDefUpdateCacheRequest.PointItems = updatePointItems;
                            _b_DefCacheService.BatchUpdateInfo(bDefUpdateCacheRequest);
                            broadcastSyncTimeOutFlag = true;
                        }
                    }
                    else
                    {
                        broadcastSyncTimeOutFlag = false;
                    }

                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(10000);
            }
        }
    }
}