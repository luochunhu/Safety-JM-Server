using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Sys.Safety.DataContract;
using Basic.Framework.Logging;
using Sys.Safety.ServiceContract;
using Basic.Framework.Service;
using Sys.Safety.Request.AlarmHandle;
using Sys.Safety.Request.JC_Largedataanalysisconfig;
using Sys.Safety.Request.AlarmNotificationPersonnelConfig;
using Sys.Safety.Request.AlarmNotificationPersonnel;
using Sys.Safety.DataContract.UserRoleAuthorize;
using Sys.Safety.ServiceContract.Cache;
using Sys.Safety.Request.Cache;
using Sys.Safety.Request.Position;
using Sys.Safety.Request.PointDefine;
using Sys.Safety.Request.Area;
using Sys.Safety.Request.PersonCache;
using Basic.Framework.Web;
using Sys.Safety.Request.Powerboxchargehistory;
using Sys.Safety.Request.Setting;
using Sys.Safety.Request.RealMessage;


namespace Sys.Safety.Client.Alarm
{
    public partial class frmAlarmBgd : Form
    {
        private object _locker = new object();

        // 20180203
        private IGasContentService _gasContentService = ServiceFactory.Create<IGasContentService>();

        private IAlarmHandleService alarmHandleService = ServiceFactory.Create<IAlarmHandleService>();
        private ILargedataAnalysisConfigService largedataAnalysisConfigService = ServiceFactory.Create<ILargedataAnalysisConfigService>();
        private IAlarmNotificationPersonnelConfigService alarmNotificationPersonnelConfigService = ServiceFactory.Create<IAlarmNotificationPersonnelConfigService>();
        private IAlarmNotificationPersonnelService alarmNotificationPersonnelService = ServiceFactory.Create<IAlarmNotificationPersonnelService>();
        private IRatioAlarmCacheService ratioAlarmCacheService = ServiceFactory.Create<IRatioAlarmCacheService>();
        private IPositionService positionService = ServiceFactory.Create<IPositionService>();
        private INetworkModuleService networkModuleService = ServiceFactory.Create<INetworkModuleService>();
        private IPointDefineService pointDefineService = ServiceFactory.Create<IPointDefineService>();
        private IAreaRuleService areaRuleService = ServiceFactory.Create<IAreaRuleService>();
        private IAreaService areaService = ServiceFactory.Create<IAreaService>();
        private IDeviceDefineService deviceDefineService = ServiceFactory.Create<IDeviceDefineService>();
        private IPowerboxchargehistoryService powerboxchargehistoryService = ServiceFactory.Create<IPowerboxchargehistoryService>();
        private ISettingService settingService = ServiceFactory.Create<ISettingService>();

        private IR_PrealService r_PrealService = ServiceFactory.Create<IR_PrealService>();
        private IR_PersoninfService r_PersoninfService = ServiceFactory.Create<IR_PersoninfService>();
        private IR_PhjService r_PhjService = ServiceFactory.Create<IR_PhjService>();

        private IConfigService _configService = ServiceFactory.Create<IConfigService>();
        IRemoteStateService _RemoteStateService = ServiceFactory.Create<IRemoteStateService>();
        private IRealMessageService realMessageService = ServiceFactory.Create<IRealMessageService>();

        IKJ_AddresstyperuleService addresstyperuleService = ServiceFactory.Create<IKJ_AddresstyperuleService>();

        public frmAlarmBgd()
        {
            InitializeComponent();
        }

        #region ======================变量=========================
        /// <summary>
        /// 容器：存放报警记录
        /// </summary>
        public static List<ShowDataInfo> listsd = new List<ShowDataInfo>();
        /// <summary>
        /// ID 标记 上次获取的报警记录的最大ID
        /// </summary>
        private long id = 0;

        /// <summary>
        /// 获取最大报警时间(大数据分析)
        /// </summary>
        private string maxStartTime = "";
        /// <summary>
        /// 本地报警配置更新时间
        /// </summary>
        private DateTime dtmAlarmConfig = DateTime.Now;
        /// <summary>
        /// 语音窗体
        /// </summary>
        private frmSound _frmSound;
        /// <summary>
        /// 声光报警器窗体
        /// </summary>
        private frmSoundLight _frmSoundLight;
        /// <summary>
        /// 图文窗体
        /// </summary>
        private frmGraph _frmGraph;

        /// <summary>
        /// 配置改变时间标记
        /// </summary>
        private DateTime dtmLast = DateTime.Now;

        /// <summary>
        /// 图文弹窗是否已经打开
        /// </summary>
        public bool bIsOpenGraphDlg = false;

        public Thread alarmth;//1028 txy
        /// <summary>
        /// 蜂鸣器报警线程
        /// </summary>
        public Thread alarmbuzzer;
        /// <summary>
        /// 定义异常报警线程
        /// </summary>
        public Thread alarmDefineErrorThread;

        private frmPopupAlert _frmPopupAlert = new frmPopupAlert();

        private bool _isRun = false;

        List<EnumcodeInfo> listState = new List<EnumcodeInfo>();
        /// <summary>
        /// 倍数报警最后报警数据的时间
        /// </summary>
        private DateTime LastMultipleAlarmTime = DateTime.Parse("1900-01-01");
        /// <summary>
        /// 风机局扇报警记录报警数据变量
        /// </summary>
        private List<ShowDataInfo> FanAlarmShowDataList = new List<ShowDataInfo>();
        /// <summary>
        /// 传感器定义异常数据变量
        /// </summary>
        private List<ShowDataInfo> DefineErrorShowDataList = new List<ShowDataInfo>();
        /// <summary>
        /// 人员报警缓存变量
        /// </summary>
        private List<ShowDataInfo> PersonAlarmShowDataList = new List<ShowDataInfo>();
        /// <summary>
        /// 人员呼叫缓存变量
        /// </summary>
        private List<ShowDataInfo> PersonCallShowDataList = new List<ShowDataInfo>();
        /// <summary>
        /// 电源箱充放电到期提醒
        /// </summary>
        private List<ShowDataInfo> PowerBoxChargeCycleList = new List<ShowDataInfo>();
        /// <summary>
        /// 传感器电量过低报警提醒
        /// </summary>
        private List<ShowDataInfo> SensorPowerAlarmList = new List<ShowDataInfo>();
        /// <summary>
        /// 传感器不匹配报警
        /// </summary>
        private List<ShowDataInfo> SensorDefineNotMatchList = new List<ShowDataInfo>();
        /// <summary>
        /// 传感器分级报警提醒
        /// </summary>
        private List<ShowDataInfo> GradingAlarmLevelList = new List<ShowDataInfo>();
        /// <summary>
        /// 双机异常报警
        /// </summary>
        private List<ShowDataInfo> ServerBackUpErrorList = new List<ShowDataInfo>();
        /// <summary>
        /// 电压报警列表
        /// </summary>
        private List<ShowDataInfo> VoltageAlarmShowDataList = new List<ShowDataInfo>();


        /// <summary>
        /// 传感器报警电量
        /// </summary>
        float SensorPowerAlarmValue = 0;//放电时间间隔(小时)      

        // 20180203
        /// <summary>瓦斯含量报警信息
        /// 
        /// </summary>
        private readonly List<ShowDataInfo> _gasContentAlarmInfo = new List<ShowDataInfo>();

        /// <summary>
        /// 大数据分析当前登录用户编码
        /// </summary>
        private string BigDataLoginUserIdNow = "";
        /// <summary>
        /// 风机局扇开停临时缓存
        /// </summary>
        List<Jc_DefInfo> switchList = new List<Jc_DefInfo>();
        /// <summary>
        /// 当前设备类型定义缓存
        /// </summary>
        List<Jc_DevInfo> deviceDefineList = new List<Jc_DevInfo>();
        /// <summary>
        /// 测点定义缓存
        /// </summary>
        List<Jc_DefInfo> pointDefineList = new List<Jc_DefInfo>();
        /// <summary>
        /// 是否第一次启动动 
        /// </summary>
        bool isFristStart = true;
        /// <summary>
        /// 当前应急联动Id
        /// </summary>
        int realLinkageRecordId = 0;
        /// <summary>
        /// 数据异常次数
        /// </summary>
        int dbStateErrCount = 0;
        /// <summary>
        /// 实时数据列表
        /// </summary>
        List<RealDataDataInfo> realDataList = new List<RealDataDataInfo>();
        /// <summary>
        /// 连接服务端失败次数
        /// </summary>
        int LoseCount = 0;
        #endregion

        private void frmGraphBgd_Load(object sender, EventArgs e)
        {
            //更新报警配置的时间 与 上下文的定义变化时间做比较
            string dtm = ClientAlarmServer.GetDevDefineChangeDatetime();
            //加载本地配置
            ClientAlarmConfig.LoadConfigToCache();

            _frmSound = new frmSound();
            _frmSound.Show();
            _frmSoundLight = new frmSoundLight();
            _frmSoundLight.Show();

            //注册主控的关闭事件
            Sys.Safety.ClientFramework.CBFCommon.RequestUtil.OnMainFormCloseEvent += new Sys.Safety.ClientFramework.CBFCommon.RequestUtil.OnMainFormClose(FrmAlarmBgd_OnMainFormCloseEvent);

            //this.timer2.Enabled = true;

            _isRun = true;
            alarmth = new Thread(new ThreadStart(alarmthread));//1028 txy            
            alarmth.IsBackground = true;//设置成后台进程  20170502
            alarmth.Start();

            alarmDefineErrorThread = new Thread(new ThreadStart(getAbnormalRemind));//1028 txy            
            alarmDefineErrorThread.IsBackground = true;//设置成后台进程  20170502
            alarmDefineErrorThread.Start();

            alarmbuzzer = new Thread(new ThreadStart(alarmalarmbuzzerthread));//1028 txy            
            alarmbuzzer.IsBackground = true;//设置成后台进程  20170502
            alarmbuzzer.Start();

            this.Visible = false;

            //id = 635775263915937500;//测试
            //SensorDefineError sensorDefineError = new SensorDefineError();
            //sensorDefineError.Show();
        }

        /// <summary>
        /// 获取报警及报警显示加锁对象
        /// </summary>
        private bool clover = true;
        /// <summary>
        /// 是否启动报警配置
        /// </summary>
        private bool clflg = true;
        /// <summary>
        /// 高频率报警分析线程
        /// </summary>
        private void alarmthread()
        {
            while (_isRun)
            {
                try
                {
                    if (ClientAlarmConfigCache.IsUseAlarmConfig)
                    {
                        clflg = true;
                        if (clover)
                        {
                            //大数据分析报警
                            GetBigDataAnalysisAlarm();
                            //设备报警
                            getmsg();
                            //倍数报警
                            GetMultipleAlarm();
                            //风机、局扇、主扇报警
                            GetFanAlarm();

                            GetGasContentAlarm();

                            GetGradingAlarmLevelList();

                            //获取实时应急联动报警信息，并弹窗
                            GetRealLinkageList();

                            //双机监测异常报警
                            getServerBackUpError();

                            //传感器欠压报警
                            GetSensorVoltageAlarm();

                            MethodInvoker In = new MethodInvoker(showmsg);
                            if (!this.IsDisposed)
                            {
                                this.BeginInvoke(In);
                            }
                        }
                    }
                    else
                    {
                        if (clflg)
                        {
                            MethodInvoker In = new MethodInvoker(showmsg);
                            if (!this.IsDisposed)
                            {
                                this.BeginInvoke(In);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                Thread.Sleep(5000);
            }
        }
        /// <summary>
        /// 低频率报警分析线程
        /// </summary>
        private void getAbnormalRemind()
        {
            while (_isRun)
            {
                try
                {
                    if (ClientAlarmConfigCache.IsUseAlarmConfig)
                    {
                        clflg = true;
                        if (clover)
                        {
                            //传感器定义异常报警
                            GetPointDefineError();
                            //传感器标效、到期提醒,电源箱放电到期提醒                            
                            //if ((DateTime.Now.Minute % 10 == 0) || isFristStart)//10分钟执行一次  20180124
                            //{
                            GetCalibrationDue();
                            GetPowerBoxChargeCycleLifeList();
                            isFristStart = false;
                            //}

                            //List<R_PrealInfo> R_PrealAlarmList = r_PrealService.GetAllPrealCacheList(new RPrealCacheGetAllRequest()).Data;
                            //List<R_PersoninfInfo> R_PersoninfInfoList = r_PersoninfService.GetAllDefinedPersonInfoCache(new BasicRequest()).Data;
                            ////人员定位报警
                            //GetPersonAlarm(R_PrealAlarmList, R_PersoninfInfoList);
                            ////人员定位呼叫
                            //GetPersonCall(R_PersoninfInfoList);

                            GetSensorPowerAlarmList();


                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex);
                }
                Thread.Sleep(60000);
            }
        }
        /// <summary>
        /// 双机异常报警
        /// </summary>
        private void getServerBackUpError()
        {
            try
            {
                //网关的状态
                bool DataCollectorState = _RemoteStateService.GetGatewayState().Data;

                bool dbState = _configService.GetDbState().Data;

                RunningInfo runinfo = _configService.GetRunningInfo().Data;

                LoseCount = 0;
                var tempServerBackUpError = ServerBackUpErrorList.Find(a => a.ID == 3);
                if (tempServerBackUpError != null)
                {
                    ServerBackUpErrorList.Remove(tempServerBackUpError);
                }
                //获取当前服务器是在主机运行还是在备机运行  20171109
                if (runinfo != null)
                {
                    if (!runinfo.IsUseHA)
                    {
                        runinfo.BackUpWorkState = -2;//将-2用于表示双机热备未启用  
                    }
                    //双机热备状态  20180703                   
                    switch (runinfo.BackUpWorkState)
                    {
                        case -2:
                        case -1:
                        case 1:
                        case 2:
                        case 7:
                        case 8:
                            var exist = ServerBackUpErrorList.Any(a => a.ID == 1);
                            //var exist = listsd.Any(a => a.ID.ToString() == item.Id);

                            if (!exist)
                            {
                                var newItem = new ShowDataInfo
                                {
                                    ID = 1,
                                    Point = "",
                                    Wz = "",
                                    Fzh = 0,
                                    Ssz = "双机热备状态异常",
                                    State = 21,
                                    Timer = DateTime.Now,
                                    Type = 112,
                                    TypeDisplay = "双机热备状态异常",
                                    Alarm = "1,2,3",
                                    Property = 6
                                };
                                lock (_locker)
                                {
                                    listsd.Add(newItem);
                                }
                                ServerBackUpErrorList.Add(newItem);
                            }
                            break;
                        default:
                            var temp = ServerBackUpErrorList.Find(a => a.ID == 1);
                            if (temp != null)
                            {
                                ServerBackUpErrorList.Remove(temp);
                            }
                            break;
                    }
                }

                //数据库状态                   
                if (!dbState)//中断
                {
                    dbStateErrCount++;
                    if (dbStateErrCount > 2)
                    {
                        var exist = ServerBackUpErrorList.Any(a => a.ID == 2);
                        //var exist = listsd.Any(a => a.ID.ToString() == item.Id);

                        if (!exist)
                        {
                            var newItem = new ShowDataInfo
                            {
                                ID = 2,
                                Point = "",
                                Wz = "",
                                Fzh = 0,
                                Ssz = "数据库异常",
                                State = 21,
                                Timer = DateTime.Now,
                                Type = 112,
                                TypeDisplay = "数据库异常",
                                Alarm = "1,2,3",
                                Property = 6
                            };
                            lock (_locker)
                            {
                                listsd.Add(newItem);
                            }
                            ServerBackUpErrorList.Add(newItem);
                        }
                    }
                }
                else
                {
                    dbStateErrCount = 0;
                    var temp = ServerBackUpErrorList.Find(a => a.ID == 2);
                    if (temp != null)
                    {
                        ServerBackUpErrorList.Remove(temp);
                    }
                }
                if (!DataCollectorState)//采集程序中断
                {
                    var exist = ServerBackUpErrorList.Any(a => a.ID == 4);
                    //var exist = listsd.Any(a => a.ID.ToString() == item.Id);

                    if (!exist)
                    {
                        var newItem = new ShowDataInfo
                        {
                            ID = 4,
                            Point = "",
                            Wz = "",
                            Fzh = 0,
                            Ssz = "采集端异常",
                            State = 21,
                            Timer = DateTime.Now,
                            Type = 112,
                            TypeDisplay = "采集端异常",
                            Alarm = "1,2,3",
                            Property = 6
                        };
                        lock (_locker)
                        {
                            listsd.Add(newItem);
                        }
                        ServerBackUpErrorList.Add(newItem);
                    }
                }
                else
                {
                    var temp = ServerBackUpErrorList.Find(a => a.ID == 4);
                    if (temp != null)
                    {
                        ServerBackUpErrorList.Remove(temp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                LoseCount++;
                if (LoseCount > 5)//服务端5次连接不上，认为中断
                {
                    var exist = ServerBackUpErrorList.Any(a => a.ID == 3);
                    //var exist = listsd.Any(a => a.ID.ToString() == item.Id);

                    if (!exist)
                    {
                        var newItem = new ShowDataInfo
                        {
                            ID = 3,
                            Point = "",
                            Wz = "",
                            Fzh = 0,
                            Ssz = "服务端异常",
                            State = 21,
                            Timer = DateTime.Now,
                            Type = 112,
                            TypeDisplay = "服务端异常",
                            Alarm = "1,2,3",
                            Property = 6
                        };
                        lock (_locker)
                        {
                            listsd.Add(newItem);
                        }
                        ServerBackUpErrorList.Add(newItem);
                    }
                }
            }
        }
        /// <summary>
        /// 获取人员报警实时数据
        /// </summary>
        private void GetPersonAlarm(List<R_PrealInfo> R_PrealAlarmList, List<R_PersoninfInfo> R_PersoninfInfoList)
        {
            if (R_PrealAlarmList != null && R_PrealAlarmList.Count > 0)
            {
                foreach (R_PrealInfo alarmPreal in R_PrealAlarmList)
                {
                    ShowDataInfo AlarmedPreal = PersonAlarmShowDataList.Find(a => a.Point == alarmPreal.Bh);
                    if (alarmPreal.Bjtype > 0)//有报警
                    {
                        if (AlarmedPreal == null)
                        {
                            var showDataInfo = new ShowDataInfo();
                            showDataInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId();
                            showDataInfo.Fzh = 0;
                            showDataInfo.Ssz = alarmPreal.BjtypeDesc;
                            showDataInfo.State = -1;
                            showDataInfo.Timer = DateTime.Now;
                            showDataInfo.Type = 201;
                            showDataInfo.TypeDisplay = "人员报警";
                            showDataInfo.Alarm = "1,2,3";
                            R_PersoninfInfo tempPerson = R_PersoninfInfoList.Find(a => a.Yid == alarmPreal.Yid);
                            if (tempPerson != null)
                            {
                                showDataInfo.Wz = tempPerson.Name;
                            }
                            showDataInfo.Point = alarmPreal.Bh;
                            showDataInfo.Property = 6;

                            PersonAlarmShowDataList.Add(showDataInfo);

                            lock (_locker)
                            {
                                listsd.Add(showDataInfo);
                            }
                        }
                        else if (AlarmedPreal.Ssz != alarmPreal.BjtypeDesc)//如果报警类型改变，则重新报警
                        {
                            var showDataInfo = new ShowDataInfo();
                            showDataInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId();
                            showDataInfo.Fzh = 0;
                            showDataInfo.Ssz = alarmPreal.BjtypeDesc;
                            showDataInfo.Timer = DateTime.Now;
                            showDataInfo.Type = 201;
                            showDataInfo.State = 21;
                            showDataInfo.TypeDisplay = "人员报警";
                            showDataInfo.Alarm = "1,2,3";
                            R_PersoninfInfo tempPerson = R_PersoninfInfoList.Find(a => a.Yid == alarmPreal.Yid);
                            if (tempPerson != null)
                            {
                                showDataInfo.Wz = tempPerson.Name;
                            }
                            showDataInfo.Point = alarmPreal.Bh;
                            showDataInfo.Property = 6;

                            PersonAlarmShowDataList.Remove(AlarmedPreal);//移除之前的报警，重新添加新的报警

                            PersonAlarmShowDataList.Add(showDataInfo);

                            lock (_locker)
                            {
                                listsd.Add(showDataInfo);
                            }
                        }
                    }
                    else//报警解除
                    {
                        if (AlarmedPreal != null)
                        {
                            PersonAlarmShowDataList.Remove(AlarmedPreal);//报警解除后，移除之前的报警
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取井下人员呼叫信息
        /// </summary>
        private void GetPersonCall(List<R_PersoninfInfo> R_PersoninfInfoList)
        {
            List<string> PersonCallList = r_PhjService.GetPhjAlarmedList().Data;
            if (PersonCallList.Count > 0)
            {
                foreach (string Bh in PersonCallList)
                {
                    ShowDataInfo AlarmedPreal = PersonCallShowDataList.Find(a => a.Point == Bh);

                    if (AlarmedPreal == null)
                    {
                        var showDataInfo = new ShowDataInfo();
                        showDataInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId();
                        showDataInfo.Fzh = 0;
                        showDataInfo.Ssz = "";
                        showDataInfo.Timer = DateTime.Now;
                        showDataInfo.Type = 202;
                        showDataInfo.State = -1;
                        showDataInfo.TypeDisplay = "井下呼叫";
                        showDataInfo.Alarm = "1,2,3";
                        R_PersoninfInfo tempPerson = R_PersoninfInfoList.Find(a => a.Bh == Bh);
                        if (tempPerson != null)
                        {
                            showDataInfo.Wz = tempPerson.Name;
                        }
                        showDataInfo.Point = Bh;
                        showDataInfo.Property = 6;

                        PersonCallShowDataList.Add(showDataInfo);

                        lock (_locker)
                        {
                            listsd.Add(showDataInfo);
                        }
                    }
                }
                //移除未在当前呼叫队列的人员
                foreach (ShowDataInfo tempshow in PersonCallShowDataList)
                {
                    if (!PersonCallList.Contains(tempshow.Point))
                    {
                        PersonCallShowDataList.Remove(tempshow);
                    }
                }
            }
        }
        /// <summary>
        /// 获取传感器定义异常报警信息
        /// </summary>
        private void GetPointDefineError()
        {
            try
            {
                //传感器定义异常报警
                AreaGetListRequest arearequest = new AreaGetListRequest();
                List<AreaInfo> areaList = areaService.GetAllAreaList(arearequest).Data;
                AreaRuleGetListRequest areaRuleRequest = new AreaRuleGetListRequest();
                areaRuleRequest.PagerInfo = new Basic.Framework.Web.PagerInfo();
                areaRuleRequest.PagerInfo.PageIndex = 0;
                areaRuleRequest.PagerInfo.PageSize = Int16.MaxValue;
                List<AreaRuleInfo> areaRuleList = areaRuleService.GetAreaRuleList(areaRuleRequest).Data;
                List<KJ_AddresstyperuleInfo> addresstyperuleInfoList = addresstyperuleService.GetKJ_AddresstyperuleList(new Request.KJ_Addresstyperule.KJ_AddresstyperuleGetListRequest()).Data;

                if (deviceDefineList.Count == 0 || bIsDevChange())
                {
                    deviceDefineList = deviceDefineService.GetAllDeviceDefineCache().Data;
                    pointDefineList = pointDefineService.GetAllPointDefineCache().Data;
                }
                #region 区域测点缺失报警
                foreach (AreaRuleInfo areaRule in areaRuleList)
                {
                    if (areaRule.DeviceCount > 0)
                    {
                        PointDefineGetByDevIDRequest PointDefineRequest = new PointDefineGetByDevIDRequest();
                        PointDefineRequest.DevID = areaRule.Devid;
                        Jc_DevInfo tempdev = deviceDefineList.Find(a => a.Devid == areaRule.Devid);
                        string devName = "";
                        if (tempdev != null)
                        {
                            devName = tempdev.Name;
                        }
                        List<Jc_DefInfo> pointDefineCount = pointDefineList.FindAll(a => a.Areaid == areaRule.Areaid && a.Devid == areaRule.Devid);
                        if (pointDefineCount.Count < areaRule.DeviceCount)
                        {//传感实际定义与区域中规定的不一致

                            var showDataInfo = new ShowDataInfo();
                            showDataInfo.ID = long.Parse(areaRule.RuleID);
                            showDataInfo.Fzh = 0;
                            showDataInfo.Ssz = "规定" + devName + "可定义" + areaRule.DeviceCount + "个，实际定义" + pointDefineCount.Count + "个";
                            showDataInfo.State = -1;
                            showDataInfo.Timer = DateTime.Now;
                            showDataInfo.Type = 104;
                            showDataInfo.TypeDisplay = "定义异常报警";
                            showDataInfo.Alarm = "1,2,3";
                            AreaInfo temparea = areaList.Find(a => a.Areaid == areaRule.Areaid);
                            if (temparea != null)
                            {
                                showDataInfo.Wz = temparea.Areaname;
                            }
                            else
                            {
                                showDataInfo.Wz = "";
                            }
                            showDataInfo.Point = "";
                            showDataInfo.Property = 6;

                            //if (DefineErrorShowDataList.FindAll(a => a.ID == long.Parse(areaRule.RuleID)).Count < 1)
                            //{
                            lock (_locker)
                            {
                                listsd.Add(showDataInfo);
                            }
                            //    DefineErrorShowDataList.Add(showDataInfo);
                            //}
                        }
                        //else//如果定义正常，则清除DefineErrorShowDataList缓存
                        //{
                        //    for (int i = DefineErrorShowDataList.Count - 1; i >= 0; i--)
                        //    {
                        //        if (DefineErrorShowDataList[i].ID == long.Parse(areaRule.RuleID))
                        //        {
                        //            DefineErrorShowDataList.RemoveAt(i);
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                }
                #endregion
                #region 测点门限定义异常报警(在定义时已判断，此处不会生效)
                foreach (KJ_AddresstyperuleInfo addresstyperule in addresstyperuleInfoList)
                {
                    if (addresstyperule.LowAlarmHighValue > 0 || addresstyperule.LowAlarmLowValue > 0 || addresstyperule.LowPoweroffHighValue > 0 || addresstyperule.LowPoweroffLowValue > 0
                        || addresstyperule.UpAlarmHighValue > 0 || addresstyperule.UpAlarmLowValue > 0 || addresstyperule.UpPoweroffHighValue > 0 || addresstyperule.UpPoweroffLowValue > 0)
                    {
                        List<Jc_DefInfo> defList = pointDefineList.FindAll(a => a.Devid == addresstyperule.Devid && a.Addresstypeid == addresstyperule.Addresstypeid);
                        foreach (Jc_DefInfo def in defList)
                        {
                            if (def.Z2 < addresstyperule.UpAlarmLowValue || def.Z2 > addresstyperule.UpAlarmHighValue)
                            {
                                var showDataInfo = new ShowDataInfo();
                                showDataInfo.ID = long.Parse(def.PointID);
                                showDataInfo.Fzh = def.Fzh;
                                showDataInfo.Ssz = "传感器上限报警门限定义异常";
                                showDataInfo.State = def.State;
                                showDataInfo.Timer = DateTime.Now;
                                showDataInfo.Type = 104;
                                showDataInfo.TypeDisplay = "定义异常报警";
                                showDataInfo.Alarm = "1,2,3";
                                showDataInfo.Wz = def.Wz;
                                showDataInfo.Point = def.Point;
                                showDataInfo.Property = 6;
                                lock (_locker)
                                {
                                    listsd.Add(showDataInfo);
                                }
                            }
                            if (def.Z3 < addresstyperule.UpPoweroffLowValue || def.Z3 > addresstyperule.UpPoweroffHighValue)
                            {
                                var showDataInfo = new ShowDataInfo();
                                showDataInfo.ID = long.Parse(def.PointID);
                                showDataInfo.Fzh = def.Fzh;
                                showDataInfo.Ssz = "传感器上限断电门限定义异常";
                                showDataInfo.State = def.State;
                                showDataInfo.Timer = DateTime.Now;
                                showDataInfo.Type = 104;
                                showDataInfo.TypeDisplay = "定义异常报警";
                                showDataInfo.Alarm = "1,2,3";
                                showDataInfo.Wz = def.Wz;
                                showDataInfo.Point = def.Point;
                                showDataInfo.Property = 6;
                                lock (_locker)
                                {
                                    listsd.Add(showDataInfo);
                                }
                            }
                            if (def.Z6 > addresstyperule.LowAlarmLowValue || def.Z6 < addresstyperule.LowAlarmHighValue)
                            {
                                var showDataInfo = new ShowDataInfo();
                                showDataInfo.ID = long.Parse(def.PointID);
                                showDataInfo.Fzh = def.Fzh;
                                showDataInfo.Ssz = "传感器下限报警门限定义异常";
                                showDataInfo.State = def.State;
                                showDataInfo.Timer = DateTime.Now;
                                showDataInfo.Type = 104;
                                showDataInfo.TypeDisplay = "定义异常报警";
                                showDataInfo.Alarm = "1,2,3";
                                showDataInfo.Wz = def.Wz;
                                showDataInfo.Point = def.Point;
                                showDataInfo.Property = 6;
                                lock (_locker)
                                {
                                    listsd.Add(showDataInfo);
                                }
                            }
                            if (def.Z7 > addresstyperule.LowPoweroffLowValue || def.Z7 < addresstyperule.LowPoweroffHighValue)
                            {
                                var showDataInfo = new ShowDataInfo();
                                showDataInfo.ID = long.Parse(def.PointID);
                                showDataInfo.Fzh = def.Fzh;
                                showDataInfo.Ssz = "传感器下限断电门限定义异常";
                                showDataInfo.State = def.State;
                                showDataInfo.Timer = DateTime.Now;
                                showDataInfo.Type = 104;
                                showDataInfo.TypeDisplay = "定义异常报警";
                                showDataInfo.Alarm = "1,2,3";
                                showDataInfo.Wz = def.Wz;
                                showDataInfo.Point = def.Point;
                                showDataInfo.Property = 6;
                                lock (_locker)
                                {
                                    listsd.Add(showDataInfo);
                                }
                            }
                        }
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 传感器标效到期提醒
        /// </summary>
        private void GetCalibrationDue()
        {
            try
            {
                bool sensorCalibrationFlag = false;//是否有新的标校报警
                bool overtermServiceFlag = false;//是否有新的超期服役
                //获取设备类型及测点定义基础数据
                if (deviceDefineList.Count == 0 || bIsDevChange())
                {
                    deviceDefineList = deviceDefineService.GetAllDeviceDefineCache().Data;
                    pointDefineList = pointDefineService.GetAllPointDefineCache().Data;
                }
                //查询近90天内的标效记录(统计前一天，及之前90天的记录)
                DateTime startTime = DateTime.Parse(DateTime.Now.AddDays(-91).ToShortDateString());
                DateTime endTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");
                DataTable calibrationRecord = ClientAlarmServer.GetCalibrationRecord(startTime, endTime);
                //按设备类型循环获取未标校、到期的传感器数量
                int NoCalibrationCount = 0;
                int DueCount = 0;
                foreach (Jc_DevInfo dev in deviceDefineList)
                {
                    int tempCalibrationTime = dev.Pl4;
                    int tempDueTime = dev.Bz5;
                    List<Jc_DefInfo> pointList = pointDefineList.FindAll(a => a.Devid == dev.Devid);
                    if (tempCalibrationTime > 0)
                    {
                        #region //计算当前设备类型下面的传感器是否到标效期
                        foreach (Jc_DefInfo def in pointList)
                        {
                            DataRow[] dr = calibrationRecord.Select("pointid='" + def.PointID + "' and stime<='" + endTime + "'", "stime desc");
                            if (dr.Length > 0)
                            {
                                TimeSpan ts = endTime - DateTime.Parse(dr[0]["stime"].ToString());
                                //if (dr[0]["bxzt"].ToString() == "2" || dr[0]["bxzt"].ToString() == "3")//2017.12.18 by 
                                //{
                                //    int index = ClientAlarmServer.sensorCalibrationInfoList.FindIndex(a => a.Point == def.Point);
                                //    if (index >= 0)
                                //    {
                                //        ClientAlarmServer.sensorCalibrationInfoList.RemoveAt(index);
                                //        sensorCalibrationFlag = true;
                                //    }
                                //}
                                //else
                                //{
                                if ((int)ts.TotalDays > tempCalibrationTime)//如果上一次标校记录时间超过了设置的标校时间周期，则记入未标校数量
                                {
                                    NoCalibrationCount++;

                                    SensorCalibrationInfo tempSensorCalibrationInfo = new SensorCalibrationInfo();
                                    tempSensorCalibrationInfo.Point = def.Point;
                                    tempSensorCalibrationInfo.Position = def.Wz;
                                    tempSensorCalibrationInfo.DevName = def.DevName;
                                    tempSensorCalibrationInfo.SetCalibrationTime = tempCalibrationTime.ToString();
                                    tempSensorCalibrationInfo.LastCalibrationTime = dr[0]["stime"].ToString();
                                    tempSensorCalibrationInfo.CalibrationDays = ((int)(ts.TotalDays)).ToString();
                                    tempSensorCalibrationInfo.id = dr[0]["id"].ToString();
                                    tempSensorCalibrationInfo.pointid = def.PointID;
                                    if (ClientAlarmServer.sensorCalibrationInfoList.Find(a => a.Point == tempSensorCalibrationInfo.Point) == null)
                                    {
                                        ClientAlarmServer.sensorCalibrationInfoList.Add(tempSensorCalibrationInfo);
                                        sensorCalibrationFlag = true;
                                    }
                                }
                                else
                                {
                                    int index = ClientAlarmServer.sensorCalibrationInfoList.FindIndex(a => a.Point == def.Point);
                                    if (index >= 0)
                                    {
                                        ClientAlarmServer.sensorCalibrationInfoList.RemoveAt(index);
                                        sensorCalibrationFlag = true;
                                    }
                                }
                                //}

                            }
                            else//未找到标校记录，则直接记入未标校
                            {
                                //未找到标校记录，根据定义时间进行判断
                                TimeSpan ts = endTime - def.CreateUpdateTime;
                                if ((int)ts.TotalDays > tempCalibrationTime)
                                {
                                    NoCalibrationCount++;

                                    SensorCalibrationInfo tempSensorCalibrationInfo = new SensorCalibrationInfo();
                                    tempSensorCalibrationInfo.Point = def.Point;
                                    tempSensorCalibrationInfo.Position = def.Wz;
                                    tempSensorCalibrationInfo.DevName = def.DevName;
                                    tempSensorCalibrationInfo.SetCalibrationTime = tempCalibrationTime.ToString();
                                    tempSensorCalibrationInfo.LastCalibrationTime = "未记录";
                                    tempSensorCalibrationInfo.CalibrationDays = "-";
                                    tempSensorCalibrationInfo.pointid = def.PointID;
                                    //tempSensorCalibrationInfo.id = dr[0]["id"].ToString();
                                    if (ClientAlarmServer.sensorCalibrationInfoList.Find(a => a.Point == tempSensorCalibrationInfo.Point) == null)
                                    {
                                        ClientAlarmServer.sensorCalibrationInfoList.Add(tempSensorCalibrationInfo);
                                        sensorCalibrationFlag = true;
                                    }
                                }
                                else
                                {
                                    int index = ClientAlarmServer.sensorCalibrationInfoList.FindIndex(a => a.Point == def.Point);
                                    if (index >= 0)
                                    {
                                        ClientAlarmServer.sensorCalibrationInfoList.RemoveAt(index);
                                        sensorCalibrationFlag = true;
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    if (tempDueTime > 0)
                    {
                        #region  //计算当前设备类型下面的传感器是否过有效期
                        foreach (Jc_DefInfo def in pointList)
                        {
                            DateTime pointDueTime = new DateTime();

                            DateTime.TryParse(def.Bz15, out pointDueTime);

                            if (pointDueTime >= DateTime.Parse("2000-01-01"))
                            {
                                TimeSpan ts = endTime - pointDueTime;
                                if (ts.TotalDays / 365 > tempDueTime)
                                {
                                    DueCount++;
                                    OvertermServiceInfo tempOvertermServiceInfo = new OvertermServiceInfo();
                                    tempOvertermServiceInfo.Point = def.Point;
                                    tempOvertermServiceInfo.Position = def.Wz;
                                    tempOvertermServiceInfo.DevName = def.DevName;
                                    tempOvertermServiceInfo.UseYear = tempDueTime.ToString();
                                    tempOvertermServiceInfo.ManufactureDate = def.Bz15;
                                    tempOvertermServiceInfo.UsedTime = (int)(ts.TotalDays / 365) + "年" + (int)(ts.TotalDays % 365) + "天";
                                    if (ClientAlarmServer.sensorOvertermServiceInfoList.Find(a => a.Point == tempOvertermServiceInfo.Point) == null)
                                    {
                                        ClientAlarmServer.sensorOvertermServiceInfoList.Add(tempOvertermServiceInfo);
                                        overtermServiceFlag = true;
                                    }
                                }
                                else
                                {
                                    int index = ClientAlarmServer.sensorOvertermServiceInfoList.FindIndex(a => a.Point == def.Point);
                                    if (index >= 0)
                                    {
                                        ClientAlarmServer.sensorOvertermServiceInfoList.RemoveAt(index);
                                        overtermServiceFlag = true;
                                    }
                                }
                            }
                            else
                            {
                                int index = ClientAlarmServer.sensorOvertermServiceInfoList.FindIndex(a => a.Point == def.Point);
                                if (index >= 0)
                                {
                                    ClientAlarmServer.sensorOvertermServiceInfoList.RemoveAt(index);
                                    overtermServiceFlag = true;
                                }
                            }
                        }
                        #endregion
                    }

                }
                //清除已删除的测点
                foreach (SensorCalibrationInfo tempSensorCalibrationInfo in ClientAlarmServer.sensorCalibrationInfoList)
                {
                    if (pointDefineList.FindAll(a => a.Point == tempSensorCalibrationInfo.Point).Count == 0)//如果测点定义缓存中不存在测点，则清除未标校集合中的记录
                    {
                        ClientAlarmServer.sensorCalibrationInfoList.Remove(tempSensorCalibrationInfo);
                        overtermServiceFlag = true;
                    }
                }
                foreach (OvertermServiceInfo tempSensorOvertermServiceInfo in ClientAlarmServer.sensorOvertermServiceInfoList)
                {
                    if (pointDefineList.FindAll(a => a.Point == tempSensorOvertermServiceInfo.Point).Count == 0)//如果测点定义缓存中不存在测点，则清除未标校集合中的记录
                    {
                        ClientAlarmServer.sensorOvertermServiceInfoList.Remove(tempSensorOvertermServiceInfo);
                        overtermServiceFlag = true;
                    }
                }
                //输出到报警界面
                //if (NoCalibrationCount > 0)
                if (sensorCalibrationFlag && NoCalibrationCount > 0)
                {
                    var showDataInfo = new ShowDataInfo();
                    showDataInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId();
                    showDataInfo.Fzh = 0;
                    showDataInfo.Ssz = "存在" + NoCalibrationCount + "个";
                    showDataInfo.State = -1;
                    showDataInfo.Timer = DateTime.Now;
                    showDataInfo.Type = 105;
                    showDataInfo.TypeDisplay = "传感器未标校";
                    showDataInfo.Alarm = "1,2,3";
                    showDataInfo.Wz = "";
                    showDataInfo.Point = "";
                    showDataInfo.Property = 6;

                    lock (_locker)
                    {
                        listsd.Add(showDataInfo);
                    }
                }
                //if (DueCount > 0)
                if (overtermServiceFlag && DueCount > 0)
                {
                    var showDataInfo = new ShowDataInfo();
                    showDataInfo.ID = Basic.Framework.Common.IdHelper.CreateLongId();
                    showDataInfo.Fzh = 0;
                    showDataInfo.Ssz = "存在" + DueCount + "个";
                    showDataInfo.State = -1;
                    showDataInfo.Timer = DateTime.Now;
                    showDataInfo.Type = 106;
                    showDataInfo.TypeDisplay = "传感器超期服役";
                    showDataInfo.Alarm = "1,2,3";
                    showDataInfo.Wz = "";
                    showDataInfo.Point = "";
                    showDataInfo.Property = 6;

                    lock (_locker)
                    {
                        listsd.Add(showDataInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
        }

        private void GetSensorVoltageAlarm()
        {
            //GetRealDataRequest request = new GetRealDataRequest();
            //realDataList = realMessageService.GetRealData(request).Data;

            List<Jc_DefInfo> pointVoltageAlarmList = pointDefineService.GetPointDefineCacheByUnderVoltageAlarmValue(new PointDefineGetByUnderVoltageAlarmValueRequest()).Data;

            #region 传感器电压报警
            foreach (Jc_DefInfo def in pointVoltageAlarmList)
            {
                //bool isAlarm = false;
                //RealDataDataInfo realData = realDataList.Find(a => a.PointID == def.PointID);
                //if (realData != null)
                //{
                //    if (realData.Voltage > 0 && def.Bz5 > 0)
                //    {
                //        if (realData.Voltage < def.Bz5)
                //        {
                //            isAlarm = true;
                //        }
                //    }
                //}
                //if (isAlarm)
                //{
                var showDataInfo = new ShowDataInfo();
                showDataInfo.ID = long.Parse(def.PointID);
                showDataInfo.Fzh = def.Fzh;
                showDataInfo.Point = def.Point;
                showDataInfo.Ssz = "传感器电压过低,定义值：" + def.Bz5 + ",实时值：" + def.Voltage;
                showDataInfo.State = def.State;
                showDataInfo.Timer = DateTime.Now;
                showDataInfo.Type = 113;
                showDataInfo.TypeDisplay = "传感器欠压报警";
                showDataInfo.Alarm = "1,2,3";
                showDataInfo.Wz = def.Wz;
                showDataInfo.Property = 6;

                if (VoltageAlarmShowDataList.FindIndex(a => a.ID == long.Parse(def.PointID)) < 0)
                {//如果欠压进行报警提醒，不重复提醒
                    lock (_locker)
                    {
                        listsd.Add(showDataInfo);
                    }
                    VoltageAlarmShowDataList.Add(showDataInfo);
                }
                //}
                //else
                //{//正常之后，移除报警列表

                //}
            }
            //移除已经解除报警的记录
            for (int i = VoltageAlarmShowDataList.Count - 1; i >= 0; i--)
            {
                int tempIndex = pointVoltageAlarmList.FindIndex(a => a.PointID == VoltageAlarmShowDataList[i].ID.ToString());
                if (tempIndex < 0)
                {
                    VoltageAlarmShowDataList.RemoveAt(tempIndex);
                }
            }
            #endregion
        }

        private void GetPowerBoxChargeCycleLifeList()
        {
            int powerBoxChargeCycle = 120;//放电时间间隔(小时)           
            var request = new GetSettingByKeyRequest() { StrKey = "PowerBoxChargeCycle" };
            var response = settingService.GetSettingByKey(request);
            if (response.Data != null)
            {
                powerBoxChargeCycle = int.Parse(response.Data.StrValue);
            }

            #region //查找所有交换机电源箱信息
            List<Jc_MacInfo> macList = networkModuleService.GetAllNetworkModuleCache().Data.FindAll(a => a.Bz4 == "1");//所有挂接了智能电源箱的设备
            foreach (Jc_MacInfo mac in macList)
            {
                PowerboxchargehistoryGetByFzhOrMacRequest powerboxchargehistoryRequest = new PowerboxchargehistoryGetByFzhOrMacRequest();
                powerboxchargehistoryRequest.Mac = mac.MAC;
                powerboxchargehistoryRequest.Fzh = "0";
                List<PowerboxchargehistoryInfo> powerboxchargehistoryList = powerboxchargehistoryService.GetPowerboxchargehistoryByFzhOrMac(powerboxchargehistoryRequest).Data;
                if (powerboxchargehistoryList.Count > 0)
                {
                    DateTime lastChargeTime = powerboxchargehistoryList.OrderByDescending(a => a.Stime).ToList()[0].Stime;
                    TimeSpan ts = DateTime.Now - lastChargeTime;
                    if (ts.TotalHours > powerBoxChargeCycle)//超过了设置的放电时间间隔，则进行报警提示
                    {
                        var showDataInfo = new ShowDataInfo();
                        showDataInfo.ID = 1;
                        showDataInfo.Point = mac.MAC;
                        showDataInfo.Wz = "";
                        showDataInfo.Fzh = 0;
                        showDataInfo.Ssz = "交换机电源箱超过放电周期未进行放电";
                        showDataInfo.State = 21;
                        showDataInfo.Timer = DateTime.Now;
                        showDataInfo.Type = 107;
                        showDataInfo.TypeDisplay = "电源箱放电提醒";
                        showDataInfo.Alarm = "1,2,3";
                        showDataInfo.Property = 6;

                        if (PowerBoxChargeCycleList.FindAll(a => a.Point == mac.MAC).Count < 1)
                        {
                            lock (_locker)
                            {
                                listsd.Add(showDataInfo);
                            }
                            PowerBoxChargeCycleList.Add(showDataInfo);
                        }
                    }
                    else
                    {
                        //如果没有超过设置的放电时间间隔，则清除本地缓存
                        for (int i = PowerBoxChargeCycleList.Count - 1; i >= 0; i--)
                        {
                            if (PowerBoxChargeCycleList[i].Point == mac.MAC)
                            {
                                PowerBoxChargeCycleList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                else
                { //如果没有放电记录，则直接提示需进行放电
                    var showDataInfo = new ShowDataInfo();
                    showDataInfo.ID = 1;
                    showDataInfo.Point = mac.MAC;
                    showDataInfo.Wz = "";
                    showDataInfo.Fzh = 0;
                    showDataInfo.Ssz = "交换机电源箱超过放电周期未进行放电";
                    showDataInfo.State = 21;
                    showDataInfo.Timer = DateTime.Now;
                    showDataInfo.Type = 107;
                    showDataInfo.TypeDisplay = "电源箱放电提醒";
                    showDataInfo.Alarm = "1,2,3";
                    showDataInfo.Property = 6;

                    if (PowerBoxChargeCycleList.FindAll(a => a.Point == mac.MAC).Count < 1)
                    {
                        lock (_locker)
                        {
                            listsd.Add(showDataInfo);
                        }
                        PowerBoxChargeCycleList.Add(showDataInfo);
                    }
                }

            }
            #endregion

            #region //查找所有分站电源箱信息
            List<Jc_DefInfo> stationList = pointDefineList.FindAll(a => a.DevPropertyID == 0 && (a.Bz3 & 0x8) == 0x8);//所有挂接了智能电源箱的设备
            foreach (Jc_DefInfo station in stationList)
            {
                PowerboxchargehistoryGetByFzhOrMacRequest powerboxchargehistoryRequest = new PowerboxchargehistoryGetByFzhOrMacRequest();
                powerboxchargehistoryRequest.Fzh = station.Fzh.ToString();
                powerboxchargehistoryRequest.Mac = "0";
                List<PowerboxchargehistoryInfo> powerboxchargehistoryList = powerboxchargehistoryService.GetPowerboxchargehistoryByFzhOrMac(powerboxchargehistoryRequest).Data;
                if (powerboxchargehistoryList.Count > 0)
                {
                    DateTime lastChargeTime = powerboxchargehistoryList.OrderByDescending(a => a.Stime).ToList()[0].Stime;
                    TimeSpan ts = DateTime.Now - lastChargeTime;
                    if (ts.TotalHours > powerBoxChargeCycle)//超过了设置的放电时间间隔，则进行报警提示
                    {
                        var showDataInfo = new ShowDataInfo();
                        showDataInfo.ID = 1;
                        showDataInfo.Point = station.Point.ToString();
                        showDataInfo.Wz = station.Wz;
                        showDataInfo.Fzh = 0;
                        showDataInfo.Ssz = "分站电源箱超过放电周期未进行放电";
                        showDataInfo.State = 21;
                        showDataInfo.Timer = DateTime.Now;
                        showDataInfo.Type = 107;
                        showDataInfo.TypeDisplay = "电源箱放电提醒";
                        showDataInfo.Alarm = "1,2,3";
                        showDataInfo.Property = 6;

                        if (PowerBoxChargeCycleList.FindAll(a => a.Point == station.Point.ToString()).Count < 1)
                        {
                            lock (_locker)
                            {
                                listsd.Add(showDataInfo);
                            }
                            PowerBoxChargeCycleList.Add(showDataInfo);
                        }
                    }
                    else
                    {
                        //如果没有超过设置的放电时间间隔，则清除本地缓存
                        for (int i = PowerBoxChargeCycleList.Count - 1; i >= 0; i--)
                        {
                            if (PowerBoxChargeCycleList[i].Point == station.Point)
                            {
                                PowerBoxChargeCycleList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    DateTime lastChargeTime = station.CreateUpdateTime;
                    TimeSpan ts = DateTime.Now - lastChargeTime;
                    if (ts.TotalHours > powerBoxChargeCycle)//超过了设置的放电时间间隔，则进行报警提示
                    {
                        //如果没有放电记录，则直接提示需进行放电
                        var showDataInfo = new ShowDataInfo();
                        showDataInfo.ID = 1;
                        showDataInfo.Point = station.Point.ToString();
                        showDataInfo.Wz = station.Wz;
                        showDataInfo.Fzh = 0;
                        showDataInfo.Ssz = "电源箱超过放电周期未进行放电";
                        showDataInfo.State = 21;
                        showDataInfo.Timer = DateTime.Now;
                        showDataInfo.Type = 107;
                        showDataInfo.TypeDisplay = "电源箱放电提醒";
                        showDataInfo.Alarm = "1,2,3";
                        showDataInfo.Property = 6;

                        if (PowerBoxChargeCycleList.FindAll(a => a.Point == station.Point.ToString()).Count < 1)
                        {
                            lock (_locker)
                            {
                                listsd.Add(showDataInfo);
                            }
                            PowerBoxChargeCycleList.Add(showDataInfo);
                        }
                    }
                }

            }
            #endregion
        }
        /// <summary>
        /// 传感器电量过低报警
        /// </summary>
        private void GetSensorPowerAlarmList()
        {
            if (SensorPowerAlarmValue == 0)
            {
                var request = new GetSettingByKeyRequest() { StrKey = "SensorPowerAlarmValue" };
                var response = settingService.GetSettingByKey(request);
                if (response.Data != null)
                {
                    SensorPowerAlarmValue = float.Parse(response.Data.StrValue);
                }
            }

            #region //查找所有传感器电量过低的记录，并进行报警提示
            PointDefineGetBySensorPowerAlarmValueRequest PointDefineRequest = new PointDefineGetBySensorPowerAlarmValueRequest();
            PointDefineRequest.SensorPowerAlarmValue = SensorPowerAlarmValue;
            List<Jc_DefInfo> defList = pointDefineService.GetPointDefineCacheBySensorPowerAlarmValue(PointDefineRequest).Data;
            //删除已结束的记录
            for (int i = SensorPowerAlarmList.Count - 1; i >= 0; i--)
            {
                var exist = defList.Any(a => a.Point == SensorPowerAlarmList[i].Point);
                if (!exist)
                {
                    SensorPowerAlarmList.RemoveAt(i);
                }
            }
            //添加新的报警
            foreach (Jc_DefInfo def in defList)
            {
                //如果没有放电记录，则直接提示需进行放电
                var showDataInfo = new ShowDataInfo();
                showDataInfo.ID = long.Parse(def.PointID);
                showDataInfo.Point = def.Point;
                showDataInfo.Wz = def.Wz;
                showDataInfo.Fzh = def.Fzh;
                showDataInfo.Ssz = "传感器电量过低";
                showDataInfo.State = 21;
                showDataInfo.Timer = DateTime.Now;
                showDataInfo.Type = 109;
                showDataInfo.TypeDisplay = "传感器电量过低";
                showDataInfo.Alarm = "1,2,3";
                showDataInfo.Property = 6;

                if (SensorPowerAlarmList.FindAll(a => a.Point == def.Point).Count < 1)
                {
                    lock (_locker)
                    {
                        listsd.Add(showDataInfo);
                    }
                    SensorPowerAlarmList.Add(showDataInfo);
                }
            }
            #endregion


        }
        /// <summary>
        /// 传感器分级报警
        /// </summary>
        private void GetGradingAlarmLevelList()
        {

            #region //查找所有传感器分级报警的记录，并进行报警提示
            PointDefineGetByGradingAlarmLevelRequest PointDefineRequest = new PointDefineGetByGradingAlarmLevelRequest();
            List<Jc_DefInfo> defList = pointDefineService.GetPointDefineCacheByGradingAlarmLevel(PointDefineRequest).Data;
            //删除已结束的记录
            for (int i = GradingAlarmLevelList.Count - 1; i >= 0; i--)
            {
                var exist = defList.Any(a => a.Point == GradingAlarmLevelList[i].Point && (a.GradingAlarmLevel.ToString() + "级报警") == GradingAlarmLevelList[i].Ssz);
                if (!exist)
                {
                    GradingAlarmLevelList.RemoveAt(i);
                }
            }
            //添加新的报警
            foreach (Jc_DefInfo def in defList)
            {
                //如果没有放电记录，则直接提示需进行放电
                var showDataInfo = new ShowDataInfo();
                showDataInfo.ID = long.Parse(def.PointID);
                showDataInfo.Point = def.Point;
                showDataInfo.Wz = def.Wz;
                showDataInfo.Fzh = def.Fzh;
                showDataInfo.Ssz = def.GradingAlarmLevel.ToString() + "级报警";
                showDataInfo.State = def.State;
                showDataInfo.Timer = DateTime.Now;
                showDataInfo.Type = 110;
                showDataInfo.TypeDisplay = "传感器分级报警";
                showDataInfo.Alarm = "1,2,3";
                showDataInfo.Property = 6;
                switch (def.GradingAlarmLevel)
                {
                    case 4:
                        showDataInfo.AlarmColor = "-16748352";//蓝色
                        break;
                    case 3:
                        showDataInfo.AlarmColor = "-16384";//黄色
                        break;
                    case 2:
                        showDataInfo.AlarmColor = "-1872887";//橙色
                        break;
                    case 1:
                        showDataInfo.AlarmColor = "-1048576";//红色
                        break;
                }

                if (GradingAlarmLevelList.FindAll(a => a.Point == def.Point && (def.GradingAlarmLevel.ToString() + "级报警") == a.Ssz).Count < 1)
                {
                    lock (_locker)
                    {
                        listsd.Add(showDataInfo);
                    }
                    GradingAlarmLevelList.Add(showDataInfo);
                }
            }
            #endregion


        }
        /// <summary>
        /// 获取应急联动记录，并弹出应急联动展示窗口
        /// </summary>
        private void GetRealLinkageList()
        {
            GetRealLinkageInfoRequest request = new GetRealLinkageInfoRequest();
            request.recordId = realLinkageRecordId;
            int reRecordId = pointDefineService.QueryRealLinkageInfoFromMonitor(request).Data;
            if (reRecordId > realLinkageRecordId)
            {
                realLinkageRecordId = reRecordId;
                //调用并弹出应急联动报警窗体                
                MethodInvoker In = new MethodInvoker(() => showLinkages(reRecordId));
                this.BeginInvoke(In);
            }
        }
        private void showLinkages(int reRecordId)
        {
            WebDisFromMonitor webDisFromMonitor = new WebDisFromMonitor(reRecordId.ToString());
            webDisFromMonitor.Show();
        }
        /// <summary>
        /// 传感器与中心站定义不匹配报警
        /// </summary>
        private void GetSensorDefineNotMatchList()
        {
            #region //是否有传感器不匹配报警
            PointDefineGetByDevpropertIDRequest PointDefineRequest = new PointDefineGetByDevpropertIDRequest();
            PointDefineRequest.DevpropertID = 1;
            List<Jc_DefInfo> defList = pointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest).Data;
            //defList = defList.FindAll(a => a.Z2 != a.DeviceInfoItem.UpAarmValue || a.Z3 != a.DeviceInfoItem.UpDdValue || a.Z4 != a.DeviceInfoItem.UpHfValue
            //    || a.Z6 != a.DeviceInfoItem.DownAarmValue || a.Z7 != a.DeviceInfoItem.DownDdValue || a.Z8 != a.DeviceInfoItem.DownHfValue);
            List<Jc_DefInfo> allSensorDefineNotMatchList = new List<Jc_DefInfo>();
            foreach (Jc_DefInfo def in defList)
            {
                if (def.Z2 != def.DeviceInfoItem.UpAarmValue)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (def.Z3 != def.DeviceInfoItem.UpDdValue)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (def.Z4 != def.DeviceInfoItem.UpHfValue)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (def.Z6 != def.DeviceInfoItem.DownAarmValue)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (def.Z7 != def.DeviceInfoItem.DownDdValue)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (def.Z8 != def.DeviceInfoItem.DownHfValue)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                string[] GradingAlarmLevel = def.Bz8.Split(',');
                if (GradingAlarmLevel.Length < 4)
                {
                    GradingAlarmLevel = new string[4] { "0", "0", "0", "0" };
                }
                if (float.Parse(GradingAlarmLevel[0]) != def.DeviceInfoItem.SeniorGradeAlarmValue1)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (float.Parse(GradingAlarmLevel[1]) != def.DeviceInfoItem.SeniorGradeAlarmValue2)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (float.Parse(GradingAlarmLevel[2]) != def.DeviceInfoItem.SeniorGradeAlarmValue3)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (float.Parse(GradingAlarmLevel[3]) != def.DeviceInfoItem.SeniorGradeAlarmValue4)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                string[] GradingAlarmTime = def.Bz9.Split(',');
                if (GradingAlarmTime.Length < 4)
                {
                    GradingAlarmTime = new string[4] { "0", "0", "0", "0" };
                }
                if (float.Parse(GradingAlarmTime[0]) != def.DeviceInfoItem.SeniorGradeTimeValue1)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (float.Parse(GradingAlarmTime[1]) != def.DeviceInfoItem.SeniorGradeTimeValue2)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (float.Parse(GradingAlarmTime[2]) != def.DeviceInfoItem.SeniorGradeTimeValue3)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
                if (float.Parse(GradingAlarmTime[3]) != def.DeviceInfoItem.SeniorGradeTimeValue4)
                {
                    allSensorDefineNotMatchList.Add(def);
                }
            }
            //删除已结束的记录
            for (int i = SensorDefineNotMatchList.Count - 1; i >= 0; i--)
            {
                var exist = allSensorDefineNotMatchList.Any(a => a.Point == SensorDefineNotMatchList[i].Point);
                if (!exist)
                {
                    SensorDefineNotMatchList.RemoveAt(i);
                }
            }
            //添加新的报警
            foreach (Jc_DefInfo def in allSensorDefineNotMatchList)
            {
                //如果没有放电记录，则直接提示需进行放电
                var showDataInfo = new ShowDataInfo();
                showDataInfo.ID = long.Parse(def.PointID);
                showDataInfo.Point = def.Point;
                showDataInfo.Wz = def.Wz;
                showDataInfo.Fzh = def.Fzh;
                showDataInfo.Ssz = "传感器与中心站定义不一致报警";
                showDataInfo.State = def.State;
                showDataInfo.Timer = DateTime.Now;
                showDataInfo.Type = 111;
                showDataInfo.TypeDisplay = "传感器定义不匹配";
                showDataInfo.Alarm = "1,2,3";
                showDataInfo.Property = 6;

                if (SensorDefineNotMatchList.FindAll(a => a.Point == def.Point).Count > 0)
                {
                    lock (_locker)
                    {
                        listsd.Add(showDataInfo);
                    }
                    SensorDefineNotMatchList.Add(showDataInfo);
                }
            }
            #endregion
        }

        /// <summary>
        /// 获取系统设备报警数据
        /// </summary>
        private void getmsg()
        {
            try
            {
                List<ShowDataInfo> sds = new List<ShowDataInfo>();
                //id = 635774399911406250;//测试

                if (id < 1) { id = ClientAlarmModle.GetMaxID(); }
                sds = ClientAlarmModle.GetReleaseAlarmRecords(id);
                if (sds != null && sds.Count > 0)
                {
                    #region 重复的记录不重复添加到List
                    foreach (ShowDataInfo sd in sds)
                    {
                        lock (_locker)
                        {
                            if (listsd.Where(a => a.ID == sd.ID).ToList().Count > 0)
                            {
                                continue;
                            }
                            listsd.Add(sd);
                        }
                    }
                    #endregion

                    #region 记录下当前已获取的报警记录的最大ID，作为下次获取报警记录的起始标记
                    ShowDataInfo ha = new ShowDataInfo();
                    ha = sds.OrderBy(a => a.Counter).LastOrDefault();
                    id = ha.Counter;
                    #endregion
                }

                #region 设备定发生改变后，需要对报警配置进行处理
                if (bIsDevChange())
                {
                    deviceDefineList = deviceDefineService.GetAllDeviceDefineCache().Data;
                    pointDefineList = pointDefineService.GetAllPointDefineCache().Data;

                    List<Jc_DefInfo> listDef = new List<Jc_DefInfo>();
                    listDef = ClientAlarmModle.GetListDef();
                    //定义发生改变，则检查并更新报警配置，更新定义变更时间
                    for (int i = ClientAlarmConfigCache.listPoint.Count - 1; i >= 0; i--)
                    {
                        if (listDef.FindAll(a => a.Point == ClientAlarmConfigCache.listPoint[i].alarmCode).Count > 0) { continue; }
                        ClientAlarmConfigCache.listPoint.RemoveAt(i);
                    }
                    ClientAlarmConfig.SaveConfig(3, ClientAlarmConfigCache.listPoint);
                    ClientAlarmConfig.LoadConfigToCache();
                    //ClientAlarmConfig.SaveConfigToServer();//去掉，不保存到服务器，否则会导致本地的配置把服务器冲掉  20180911
                }
                #endregion

                //获取设备状态,放到非UI线程  20170713
                IList<EnumcodeInfo> templistState = ClientAlarmServer.GetListEnumState();
                if (templistState != null)
                {
                    listState = templistState.ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取报警记录 出现异常", ex);
            }
        }
        /// <summary>
        /// 大数据分析逻辑报警获取 
        /// </summary>
        private void GetBigDataAnalysisAlarm()
        {
            try
            {
                #region 获取大数据模块报警处理记录
                string currentUserId = "";
                ClientItem _ClientItem = new ClientItem();
                if (Basic.Framework.Data.PlatRuntime.Items.ContainsKey(KeyConst.ClientItemKey))
                {
                    _ClientItem = Basic.Framework.Data.PlatRuntime.Items[KeyConst.ClientItemKey] as ClientItem;
                }
                if (!string.IsNullOrWhiteSpace(_ClientItem.UserID))
                {
                    currentUserId = _ClientItem.UserID;
                }
                //增加登录判断
                if (BigDataLoginUserIdNow == "")
                {
                    BigDataLoginUserIdNow = currentUserId;
                }
                else if (BigDataLoginUserIdNow != currentUserId)//如果当前登录用户与之前登录用户发生变化时，结束报警，并重新判断  20170728
                {
                    CancleAlarmAndViewList();
                    maxStartTime = "";
                    lock (_locker)
                    {
                        listsd.Clear();
                    }
                    BigDataLoginUserIdNow = currentUserId;
                }

                var request = new AlarmHandleNoEndListByCondition();
                request.StartTime = maxStartTime;
                request.EndTime = new DateTime(1900, 01, 01, 00, 00, 00);
                request.PersonId = currentUserId;
                var response = alarmHandleService.GetAlarmHandleNoEndListByCondition(request);
                if (response != null && response.Data.Count > 0)
                {
                    //获取最大开始时间的记录
                    var maxStartTimeInfo = response.Data.OrderByDescending(c => c.StartTime).FirstOrDefault();
                    if (maxStartTimeInfo != null)
                    {
                        maxStartTime = maxStartTimeInfo.StartTime.ToString();
                    }
                    foreach (var alarmHandle in response.Data)
                    {
                        var showDataInfo = new ShowDataInfo();
                        showDataInfo.Ssz = alarmHandle.AnalysisResult;
                        showDataInfo.Timer = alarmHandle.StartTime;
                        showDataInfo.State = -1;
                        showDataInfo.Alarm = alarmHandle.AlarmType;
                        showDataInfo.TypeDisplay = "逻辑分析报警";
                        showDataInfo.AlarmColor = alarmHandle.AlarmColor;
                        showDataInfo.Wz = alarmHandle.Name;
                        lock (_locker)
                        {
                            listsd.Add(showDataInfo);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取大数据分析逻辑报警记录 出现异常", ex);
            }
        }

        // 20180203
        /// <summary>获取瓦斯含量分析报警
        /// 
        /// </summary>
        private void GetGasContentAlarm()
        {
            var alarmInfo = _gasContentService.GetAllGasContentAlarmCache().Data;

            for (int i = _gasContentAlarmInfo.Count - 1; i >= 0; i--)
            {
                var exist = alarmInfo.Any(a => a.Id == _gasContentAlarmInfo[i].ID.ToString());
                if (!exist)
                {
                    _gasContentAlarmInfo.RemoveAt(i);
                }
            }

            foreach (var item in alarmInfo)
            {
                var exist = _gasContentAlarmInfo.Any(a => a.ID.ToString() == item.Id);
                //var exist = listsd.Any(a => a.ID.ToString() == item.Id);

                if (!exist)
                {
                    var newItem = new ShowDataInfo
                    {
                        ID = Convert.ToInt64(item.Id),
                        Point = item.Point,
                        Wz = item.Location,
                        Fzh = Convert.ToInt32(item.Point.Substring(0, 3)),
                        Ssz = item.GasContent,
                        State = 21,
                        Timer = DateTime.Now,
                        Type = 108,
                        TypeDisplay = "瓦斯含量超标",
                        Alarm = "1,2,3",
                        Property = 6
                    };
                    lock (_locker)
                    {
                        listsd.Add(newItem);
                    }
                    _gasContentAlarmInfo.Add(newItem);
                }
            }
        }

        /// <summary>
        /// 获取倍数报警
        /// </summary>
        private void GetMultipleAlarm()
        {
            try
            {
                RatioAlarmCacheGetByStimeRequest alarmCacheRequest = new RatioAlarmCacheGetByStimeRequest();
                alarmCacheRequest.Stime = LastMultipleAlarmTime;
                List<JC_MbInfo> multipleAlarmList = ratioAlarmCacheService.GetAlarmCacheByStime(alarmCacheRequest).Data;
                if (multipleAlarmList != null && multipleAlarmList.Count > 0)
                {
                    //查询位置，以便于关联
                    //PositionGetByWzIDRequest positionRequest = new PositionGetByWzIDRequest();
                    //positionRequest.WzID = multipleAlarm.Wzid;
                    var resultWz = positionService.GetAllPositionCache().Data;

                    foreach (JC_MbInfo multipleAlarm in multipleAlarmList)
                    {
                        if (multipleAlarm.Stime > LastMultipleAlarmTime)//每次记录上次获取的最后报警记录时间，下次增量获取
                        {
                            LastMultipleAlarmTime = multipleAlarm.Stime;
                        }
                        var showDataInfo = new ShowDataInfo();
                        showDataInfo.ID = long.Parse(multipleAlarm.Id);
                        showDataInfo.Ssz = multipleAlarm.Ssz.ToString();
                        showDataInfo.Timer = multipleAlarm.Stime;
                        showDataInfo.Type = 101;//101默认是倍数报警的类型
                        showDataInfo.State = -1;
                        showDataInfo.TypeDisplay = "倍数报警";
                        showDataInfo.Property = 6;
                        //获取报警位置
                        string multipleAlarmWz = "";
                        //PositionGetByWzIDRequest positionRequest = new PositionGetByWzIDRequest();
                        //positionRequest.WzID = multipleAlarm.Wzid;
                        //var resultWz = positionService.GetPositionCacheByWzID(positionRequest).Data;
                        var tempresultWz = resultWz.Find(a => a.WzID == multipleAlarm.Wzid);
                        if (tempresultWz != null)
                        {
                            multipleAlarmWz = tempresultWz.Wz;
                        }
                        showDataInfo.Point = multipleAlarm.Point;
                        showDataInfo.Wz = multipleAlarmWz;

                        lock (_locker)
                        {
                            listsd.Add(showDataInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("获取倍数报警记录 出现异常", ex);
            }
        }
        /// <summary>
        /// 获取风机、局扇、主扇同时停报警（同一分站下，两个及以上的风机、局扇、主扇同时停之后，进行报警，通过安装位置进行判断）
        /// </summary>
        private void GetFanAlarm()
        {
            PointDefineGetByDevpropertIDRequest PointDefineRequest = new PointDefineGetByDevpropertIDRequest();
            PointDefineRequest.DevpropertID = 2;//查询所有开关量设备进行判断
            //if (switchList.Count == 0 || bIsDevChange())
            //{
                switchList = pointDefineService.GetPointDefineCacheByDevpropertID(PointDefineRequest).Data;
            //}
            if (switchList != null && switchList.Count > 0)
            {
                //按分站查找所有分站下面是否定义了风机、局扇、主扇
                for (int i = 1; i <= 250; i++)
                {
                    bool stationIsAlarm = false;
                    //#region 风机

                    //bool isAllStop = true;
                    //List<Jc_DefInfo> tempFanList = switchList.FindAll(a => a.Wz.Contains("风机") && a.Fzh == i);
                    //if (tempFanList.Count > 0)
                    //{

                    //    string Wz = "";
                    //    string Point = "";
                    //    foreach (Jc_DefInfo tempdef in tempFanList)
                    //    {
                    //        Wz += tempdef.Wz + ",";
                    //        Point += tempdef.Point + ",";
                    //        if (tempdef.Ssz == "开" || tempdef.Ssz == "断线" || tempdef.DataState == 46 || tempdef.State == 6)//未知和休眠状态不判断
                    //        {
                    //            isAllStop = false;
                    //        }
                    //    }
                    //    if (isAllStop)
                    //    {
                    //        stationIsAlarm = true;
                    //        var showDataInfo = new ShowDataInfo();
                    //        showDataInfo.ID = long.Parse(tempFanList[0].ID);
                    //        showDataInfo.Fzh = i;
                    //        showDataInfo.Ssz = "风机停";
                    //        showDataInfo.Timer = DateTime.Now;
                    //        showDataInfo.Type = 102;//101默认是倍数报警的类型
                    //        showDataInfo.State = 21;
                    //        showDataInfo.TypeDisplay = "风机局扇停报警";
                    //        showDataInfo.Wz = Wz;
                    //        showDataInfo.Point = Point;
                    //        showDataInfo.Property = 6;


                    //        if (FanAlarmShowDataList.FindAll(a => a.Fzh == i && a.Ssz == "风机停").Count < 1)
                    //        {
                    //            lock (_locker)
                    //            {
                    //                listsd.Add(showDataInfo);
                    //            }
                    //            FanAlarmShowDataList.Add(showDataInfo);
                    //        }
                    //    }
                    //}
                    //#endregion
                    //#region 局扇
                    //List<Jc_DefInfo> tempFanList1 = switchList.FindAll(a => a.Wz.Contains("局扇") && a.Fzh == i);
                    //if (tempFanList1.Count > 0)
                    //{
                    //    isAllStop = true;
                    //    string Wz = "";
                    //    string Point = "";
                    //    foreach (Jc_DefInfo tempdef in tempFanList1)
                    //    {
                    //        Wz += tempdef.Wz + ",";
                    //        Point += tempdef.Point + ",";
                    //        if (tempdef.Ssz == "开" || tempdef.Ssz == "断线" || tempdef.DataState == 46 || tempdef.State == 6)//未知和休眠状态不判断
                    //        {
                    //            isAllStop = false;
                    //        }
                    //    }
                    //    if (isAllStop)
                    //    {
                    //        stationIsAlarm = true;
                    //        var showDataInfo = new ShowDataInfo();
                    //        showDataInfo.ID = long.Parse(tempFanList1[0].ID);
                    //        showDataInfo.Fzh = i;
                    //        showDataInfo.Ssz = "局扇停";
                    //        showDataInfo.State = 21;
                    //        showDataInfo.Timer = DateTime.Now;
                    //        showDataInfo.Type = 102;//101默认是倍数报警的类型
                    //        showDataInfo.TypeDisplay = "风机局扇停报警";
                    //        showDataInfo.Wz = Wz;
                    //        showDataInfo.Point = Point;
                    //        showDataInfo.Property = 6;

                    //        if (FanAlarmShowDataList.FindAll(a => a.Fzh == i && a.Ssz == "局扇停").Count < 1)
                    //        {
                    //            lock (_locker)
                    //            {
                    //                listsd.Add(showDataInfo);
                    //            }
                    //            FanAlarmShowDataList.Add(showDataInfo);
                    //        }
                    //    }
                    //}
                    //#endregion
                    //#region 主扇
                    //List<Jc_DefInfo> tempFanList2 = switchList.FindAll(a => a.Wz.Contains("主扇") && a.Fzh == i);
                    //if (tempFanList2.Count > 0)
                    //{
                    //    isAllStop = true;
                    //    string Wz = "";
                    //    string Point = "";
                    //    foreach (Jc_DefInfo tempdef in tempFanList2)
                    //    {
                    //        Wz += tempdef.Wz + ",";
                    //        Point += tempdef.Point + ",";
                    //        if (tempdef.Ssz == "开" || tempdef.Ssz == "断线" || tempdef.DataState == 46 || tempdef.State == 6)//未知和休眠状态不判断
                    //        {
                    //            isAllStop = false;
                    //        }
                    //    }
                    //    if (isAllStop)
                    //    {
                    //        stationIsAlarm = true;
                    //        var showDataInfo = new ShowDataInfo();
                    //        showDataInfo.ID = long.Parse(tempFanList2[0].ID);
                    //        showDataInfo.Fzh = i;
                    //        showDataInfo.Ssz = "主扇停";
                    //        showDataInfo.State = 21;
                    //        showDataInfo.Timer = DateTime.Now;
                    //        showDataInfo.Type = 102;//101默认是倍数报警的类型
                    //        showDataInfo.TypeDisplay = "风机局扇停报警";
                    //        showDataInfo.Wz = Wz;
                    //        showDataInfo.Point = Point;
                    //        showDataInfo.Property = 6;

                    //        if (FanAlarmShowDataList.FindAll(a => a.Fzh == i && a.Ssz == "局扇停").Count < 1)
                    //        {
                    //            lock (_locker)
                    //            {
                    //                listsd.Add(showDataInfo);
                    //            }
                    //            FanAlarmShowDataList.Add(showDataInfo);
                    //        }
                    //    }
                    //}
                    //#endregion

                    #region 风门开报警
                    List<Jc_DefInfo> tempFanList2 = switchList.FindAll(a => a.Wz.Contains("风门") && a.Fzh == i && a.Dzh > 0);
                    if (tempFanList2.Count > 0)
                    {
                        for (int k = 1; k <= 16; k++)
                        {
                            List<Jc_DefInfo> tempFanList3 = tempFanList2.FindAll(a => a.Kh == k);
                            if (tempFanList3.Count > 1)
                            {
                                bool isAllStop = true;
                                string Wz = "";
                                string Point = "";
                                foreach (Jc_DefInfo tempdef in tempFanList3)
                                {
                                    Wz += tempdef.Wz + ",";
                                    Point += tempdef.Point + ",";
                                    if (tempdef.Ssz == "关" || tempdef.Ssz == "断线" || tempdef.DataState == 46 || tempdef.State == 6)//未知和休眠状态不判断
                                    {
                                        isAllStop = false;
                                    }
                                }
                                if (isAllStop)
                                {
                                    stationIsAlarm = true;
                                    var showDataInfo = new ShowDataInfo();
                                    showDataInfo.ID = long.Parse(tempFanList3[0].ID);
                                    showDataInfo.Fzh = i;
                                    showDataInfo.Kh = k;
                                    showDataInfo.Ssz = "风门开";
                                    showDataInfo.State = 21;
                                    showDataInfo.Timer = DateTime.Now;
                                    showDataInfo.Type = 102;//101默认是倍数报警的类型
                                    showDataInfo.TypeDisplay = "风门开报警";
                                    showDataInfo.Wz = Wz;
                                    showDataInfo.Point = Point;
                                    showDataInfo.Property = 6;

                                    if (FanAlarmShowDataList.FindAll(a => a.Fzh == i && a.Kh == k && a.Ssz == "风门开").Count < 1)
                                    {
                                        lock (_locker)
                                        {
                                            listsd.Add(showDataInfo);
                                        }
                                        FanAlarmShowDataList.Add(showDataInfo);
                                    }
                                }
                                else
                                {
                                    foreach (ShowDataInfo tempdata in FanAlarmShowDataList)
                                    {
                                        if (tempdata.Fzh == i && tempdata.Kh == k)
                                        {
                                            FanAlarmShowDataList.Remove(tempdata);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    //if (!stationIsAlarm)
                    //{
                    //    //删除当前分站的报警临时缓存
                    //    if (FanAlarmShowDataList != null)
                    //    {
                    //        foreach (ShowDataInfo tempdata in FanAlarmShowDataList)
                    //        {
                    //            if (tempdata.Fzh == i)
                    //            {
                    //                FanAlarmShowDataList.Remove(tempdata);
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }

        private void showmsg()
        {
            clover = false;
            try
            {
                //持续报警  20170716

                if (!ClientAlarmConfigCache.IsUseAlarmConfig)
                {
                    CancleAlarm();
                    clflg = false;
                }
                else
                {
                    lock (_locker)
                    {
                        if (listsd == null || listsd.Count < 1)
                        {
                            clover = true;
                            return;
                        }
                        listsd.OrderBy(a => a.ID);//按ID升序排列
                        foreach (ShowDataInfo showData in listsd)//从最早的运行记录开始进行处理
                        {
                            if (showData.ID > 0)
                            {
                                #region 在本地报警配置中找该测点的报警配置
                                List<ClientAlarmItems> list = new List<ClientAlarmItems>();
                                list = ClientAlarmConfigCache.listPoint.FindAll(a => a.code == showData.Point);
                                if (list == null || list.Count < 1)
                                {
                                    if (!string.IsNullOrEmpty(showData.Devid))
                                    {
                                        list = ClientAlarmConfigCache.listDev.FindAll(a => a.code == showData.Devid.ToString());
                                    }
                                }
                                if (list == null || list.Count < 1)
                                {
                                    list = ClientAlarmConfigCache.listClass.FindAll(a => a.code == showData.Class.ToString());
                                }
                                if (list == null || list.Count < 1)
                                {
                                    list = ClientAlarmConfigCache.listProperty.FindAll(a => a.code == showData.Property.ToString());
                                }
                                #endregion

                                #region 寻找该记录的报警设置，若找不到则跳过，若找到则继续处理
                                ClientAlarmItems cai = list.Find(a => a.alarmCode == showData.Type.ToString());
                                if (cai == null || string.IsNullOrEmpty(cai.alarmShow))
                                {
                                    continue;
                                }
                                #endregion

                                showData.TypeDisplay = cai.alarmType;//数据状态

                                #region 设备状态 设备状态state找不到对应的枚举记录时，则不报设备状态
                                if (listState != null && listState.Count > 0)
                                {
                                    listState = listState.Where(a => a.LngEnumValue == showData.State).ToList();
                                }
                                if (listState == null || listState.Count < 1)
                                {
                                    showData.StateDisplay = "";
                                }
                                else
                                {
                                    showData.StateDisplay = listState[0].StrEnumDisplay;//设备状态
                                }
                                #endregion

                                //报警展示
                                AlarmDisplay(showData, cai.alarmShow);
                            }
                            else
                            {
                                //报警展示(主要针对大数据分析报警展示)
                                AlarmDisplay(showData, showData.Alarm);
                            }
                        }

                        listsd.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("刷新报警记录 出现异常", ex);
            }
            clover = true;
        }

        private void AlarmDisplay(ShowDataInfo showData, string showType)
        {
            //是否启用右下角报警提示（201170119 ）
            if (ClientAlarmConfigCache.IsUsePopupAlarm)
            {
                _frmPopupAlert.ShowData(showData);
            }


            #region 解析报警方式，分派报警
            string[] alarmType = showType.Trim().Split(',');//注意空格
            //分派到对应的列表中去
            for (int i = 0; i < alarmType.Length; i++)
            {
                switch (alarmType[i].Trim())
                {
                    case "1"://语音
                        if (ClientAlarmConfigCache.showDataSound == null) { ClientAlarmConfigCache.showDataSound = new List<ShowDataInfo>(); }
                        ClientAlarmConfigCache.showDataSound.Add(showData);
                        break;
                    case "2"://声光
                        if (ClientAlarmConfigCache.showDataLight == null) { ClientAlarmConfigCache.showDataLight = new List<ShowDataInfo>(); }
                        ClientAlarmConfigCache.showDataLight.Add(showData);
                        break;
                    case "3"://图文
                    default:
                        if (ClientAlarmConfigCache.showDataGraph == null) { ClientAlarmConfigCache.showDataGraph = new List<ShowDataInfo>(); }
                        ClientAlarmConfigCache.showDataGraph.Add(showData);
                        if (ClientAlarmConfigCache.showDataGraph.Count > 0)
                        {
                            if (!bIsOpenGraphDlg)
                            {
                                bIsOpenGraphDlg = true;
                                _frmGraph = null;
                                _frmGraph = new frmGraph(this);
                                _frmGraph.Show(this);
                            }

                            //added by qy 20170118 由于偶然测试发现bIsOpenGraphDlg==true,但报警窗体关闭，所以在没有还原出问题的情况下，多加一个判断 ，做冗余处理
                            if (_frmGraph != null && _frmGraph.Visible == false && !_frmGraph.IsDisposed)
                            {
                                _frmGraph.Visible = true;
                                _frmGraph.Show();
                            }
                            //_frmGraph.TopMost = true;
                            _frmGraph.Focus();
                        }
                        break;
                    case "4"://蜂鸣器报警  20170716
                        if (ClientAlarmConfigCache.showDataBuzzer == null) { ClientAlarmConfigCache.showDataBuzzer = new List<ShowDataInfo>(); }
                        ClientAlarmConfigCache.showDataBuzzer.Add(showData);
                        break;
                }
            }
            #endregion
        }

        /// <summary>
        /// 服务端上下文中设备定义是否发生变化（通过对比客户端设备定义时间和服务端上下文设备定义时间来判断）
        /// </summary>
        /// <returns></returns>
        public bool bIsDevChange()
        {
            bool b = false;
            try
            {
                string TempTime = ClientAlarmServer.GetDevDefineChangeDatetime();
                if (!string.IsNullOrEmpty(TempTime))
                {
                    DateTime dtmTemp = dtmLast;
                    DateTime.TryParse(TempTime, out dtmTemp);

                    if (dtmLast != dtmTemp)//20151028 txy
                    {
                        b = true;
                        dtmLast = dtmTemp;
                    }
                }
                else
                {
                    b = false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("bIsDevChange-发生异常 " + ex.Message);
                b = false;
            }
            return b;
        }

        /// <summary>
        /// 取消当前所有报警
        /// </summary>
        public void CancleAlarm()
        {
            try
            {
                //清除蜂鸣器报警  20170716
                if (ClientAlarmConfigCache.showDataBuzzer != null)
                {
                    ClientAlarmConfigCache.showDataBuzzer.Clear();
                }
                if (_frmGraph != null)
                {
                    _frmGraph.CancelAlarm();
                }
                if (_frmSound != null)
                {
                    _frmSound.CancelAlarm();
                }
                if (_frmSoundLight != null)
                {
                    _frmSoundLight.CancelAlarm();
                }
                if (_frmPopupAlert != null)
                {
                    _frmPopupAlert.CancelAlarm();
                }
            }
            catch (Exception ex)
            {
                //写日志或抛出异常等
                LogHelper.Error("关闭所有报警时出现异常！", ex);
            }
        }
        public void OpenAlarm()
        {
            try
            {
                if (_frmPopupAlert != null)
                {
                    _frmPopupAlert.OpenAlarm();
                }
            }
            catch (Exception ex)
            {
                //写日志或抛出异常等
                LogHelper.Error("关闭所有报警时出现异常！", ex);
            }
        }

        /// <summary>
        /// 清除报警及报警日志列表
        /// </summary>
        public void CancleAlarmAndViewList()
        {
            try
            {
                //清除蜂鸣器报警  20170716
                if (ClientAlarmConfigCache.showDataBuzzer != null)
                {
                    ClientAlarmConfigCache.showDataBuzzer.Clear();
                }
                if (_frmGraph != null)
                {
                    _frmGraph.CancelAlarm();
                }
                if (_frmSound != null)
                {
                    _frmSound.CancelAlarm();
                }
                if (_frmSoundLight != null)
                {
                    _frmSoundLight.CancelAlarm();
                }
                if (_frmPopupAlert != null)
                {
                    _frmPopupAlert.ClearViewList();//清除右下角弹窗日志
                    _frmPopupAlert.CancelAlarm();
                }
            }
            catch (Exception ex)
            {
                //写日志或抛出异常等
                LogHelper.Error("关闭所有报警时出现异常！", ex);
            }
        }

        /// <summary>
        /// 退出事件
        /// </summary>
        private void FrmAlarmBgd_OnMainFormCloseEvent()
        {
            if (_frmGraph != null)
            {
                _frmGraph.Close();
            }
            if (_frmSound != null)
            {
                _frmSound.Close();
            }
            if (_frmSoundLight != null)
            {
                _frmSoundLight.Close();
            }
            try
            {
                //alarmth.Abort();//1028 txy //设置成后台进程,不需要强制退出  20170502
            }
            catch
            { }
            this.Close();
        }

        private void frmAlarmBgd_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isRun = false;
            FrmAlarmBgd_OnMainFormCloseEvent();
        }
        ///// <summary>
        ///// 蜂鸣器报警
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void timer3_Tick(object sender, EventArgs e)
        //{
        //    if (ClientAlarmConfigCache.showDataBuzzer != null)
        //    {
        //        if (ClientAlarmConfigCache.showDataBuzzer.Count > 0)
        //        {
        //            System.Console.Beep();
        //            Thread.Sleep(50);
        //            System.Console.Beep();
        //            Thread.Sleep(500);
        //            System.Console.Beep();
        //            Thread.Sleep(50);
        //            System.Console.Beep();
        //        }
        //    }
        //}
        private void alarmalarmbuzzerthread()
        {
            while (true)
            {
                try
                {
                    if (ClientAlarmConfigCache.showDataBuzzer != null)
                    {
                        if (ClientAlarmConfigCache.showDataBuzzer.Count > 0)
                        {
                            System.Console.Beep();
                            Thread.Sleep(50);
                            System.Console.Beep();
                            Thread.Sleep(500);
                            System.Console.Beep();
                            Thread.Sleep(50);
                            System.Console.Beep();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Basic.Framework.Logging.LogHelper.Error(ex);
                }
                Thread.Sleep(2000);
            }
        }
    }
}
