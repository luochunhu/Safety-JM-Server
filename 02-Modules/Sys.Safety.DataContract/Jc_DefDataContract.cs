using Sys.Safety.DataContract.CommunicateExtend;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sys.Safety.DataContract
{
    public partial class Jc_DefInfo
    {
        #region Public Properties
        protected string _Wz;
        /// <summary>
        /// 安装位置
        /// </summary>
        public string Wz
        {
            get { return _Wz; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 Wz", value, value.ToString());
                _Wz = value;
            }
        }

        protected string _DevName;
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DevName
        {
            get { return _DevName; }
            set
            {
                if (value != null && value.Length > 40)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 DevName", value, value.ToString());
                _DevName = value;
            }
        }

        protected int _DevPropertyID;
        /// <summary>
        /// 设备性质ID
        /// </summary>
        public int DevPropertyID
        {
            get { return _DevPropertyID; }
            set
            {
                _DevPropertyID = value;
            }
        }

        protected int _DevClassID;
        /// <summary>
        /// 设备种类ID
        /// </summary>
        public int DevClassID
        {
            get { return _DevClassID; }
            set
            {
                _DevClassID = value;
            }
        }

        protected int _DevModelID;
        /// <summary>
        /// 设备型号ID
        /// </summary>
        public int DevModelID
        {
            get { return _DevModelID; }
            set
            {
                _DevModelID = value;
            }
        }

        protected string _DevProperty;
        /// <summary>
        /// 设备性质名称
        /// </summary>
        public string DevProperty
        {
            get { return _DevProperty; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 DevProperty", value, value.ToString());
                _DevProperty = value;
            }
        }

        protected string _DevClass;
        /// <summary>
        /// 设备种类名称
        /// </summary>
        public string DevClass
        {
            get { return _DevClass; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 DevClass", value, value.ToString());
                _DevClass = value;
            }
        }

        protected string _DevModel;
        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string DevModel
        {
            get { return _DevModel; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 DevModel", value, value.ToString());
                _DevModel = value;
            }
        }

        protected string _unit;
        /// <summary>
        /// 单位 （Xs1）
        /// </summary>
        public string Unit
        {
            get { return _unit; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 Unit", value, value.ToString());
                _unit = value;
            }
        }


        protected DateTime _PowerDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
        /// <summary>
        /// 获取电源箱电压时间
        /// </summary>
        public DateTime PowerDateTime
        {
            get { return _PowerDateTime; }
            set
            {
                _PowerDateTime = value;
            }
        }

        protected List<BatteryItem> batteryItems;
        /// <summary>
        /// 电源箱（电池箱）信息
        /// </summary>
        public List<BatteryItem> BatteryItems
        {
            get { return batteryItems; }
            set
            {
                batteryItems = value;
            }
        }

        protected List<BatteryPowerConsumption> batteryPowerConsumptions;
        /// <summary>
        /// 电源箱5分钟耗电量（与电源箱一一对应，根据BatteryAddress）
        /// </summary>
        public List<BatteryPowerConsumption> BatteryPowerConsumptions
        {
            get { return batteryPowerConsumptions; }
            set
            {
                batteryPowerConsumptions = value;
            }
        }

        protected string _AreaName;
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName
        {
            get { return _AreaName; }
            set
            {
                _AreaName = value;
            }
        }

        protected string _areaLoc;
        /// <summary>
        /// 所属区域编码（Loc）
        /// </summary>
        public string AreaLoc
        {
            get { return _areaLoc; }
            set
            {
                _areaLoc = value;
            }
        }

        protected string _XCoordinate;
        /// <summary>
        /// 经度
        /// </summary>
        public string XCoordinate
        {
            get { return _XCoordinate; }
            set
            {
                _XCoordinate = value;
            }
        }

        protected string _YCoordinate;
        /// <summary>
        /// 纬度
        /// </summary>
        public string YCoordinate
        {
            get { return _YCoordinate; }
            set
            {
                _YCoordinate = value;
            }
        }

        protected bool _DefIsInit;
        /// <summary>
        /// 定义成功后是否需要下发初始化的标记
        /// </summary>
        public bool DefIsInit
        {
            get { return _DefIsInit; }
            set { _DefIsInit = value; }
        }

        private int _NErrCount;
        /// <summary>
        /// 当前通讯中断次数
        /// </summary>
        public int NErrCount
        {
            set { _NErrCount = value; }
            get { return _NErrCount; }
        }
        private DateTime _DttStateTime;
        /// <summary>
        /// 最近一次获取到数据的时间 
        /// </summary>
        public DateTime DttStateTime
        {
            set { _DttStateTime = value; }
            get { return _DttStateTime; }
        }

        private DateTime _DttSensorPowerAlarmTime;
        /// <summary>
        /// 最近一次写传感器电量过低运行记录日志的时间（用于判断，每小时写一次）
        /// </summary>
        public DateTime DttSensorPowerAlarmTime
        {
            set { _DttSensorPowerAlarmTime = value; }
            get { return _DttSensorPowerAlarmTime; }
        }

        private DateTime _DttCfUpTime;
        /// <summary>
        /// 最近一次更新抽放数据的时间 
        /// </summary>
        public DateTime DttCfUpTime
        {
            set { _DttCfUpTime = value; }
            get { return _DttCfUpTime; }
        }

        private byte _Fdstate;
        /// <summary>
        /// 放电状态（0不放电，1放电）
        /// </summary>
        public byte Fdstate
        {
            set { _Fdstate = value; }
            get { return _Fdstate; }
        }

        private byte _BDisCharge;
        /// <summary>
        /// 手动放电 0不进行操作，1取消维护性放电，2维护性放电
        /// </summary>
        public byte BDisCharge
        {
            get { return _BDisCharge; }
            set { _BDisCharge = value; }
        }

        private byte _LastOrderNum;
        /// <summary>
        /// 上次下发的命令
        /// </summary>
        public byte LastOrderNum
        {
            get { return _LastOrderNum; }
            set { _LastOrderNum = value; }
        }

        private DateTime _lastOrderSendTime;
        /// <summary>
        /// 上次下发命令的时间
        /// </summary>
        public DateTime lastOrderSendTime
        {
            get { return _lastOrderSendTime; }
            set { _lastOrderSendTime = value; }
        }

        private int _realControlCount;
        /// <summary>
        /// 需要下发多少次F命令
        /// </summary>
        public int realControlCount
        {
            get { return _realControlCount; }
            set { _realControlCount = value; }
        }

        private int _sendIniCount;
        /// <summary>
        /// 需要下发多少次i命令
        /// </summary>
        public int sendIniCount
        {
            get { return _sendIniCount; }
            set { _sendIniCount = value; }
        }

        private DateTime _sendDTime;
        /// <summary>
        /// D命令下发时间
        /// </summary>
        public DateTime sendDTime
        {
            get { return _sendDTime; }
            set { _sendDTime = value; }
        }

        private bool _kzchangeflag;
        /// <summary>
        /// 控制口变化标记
        /// </summary>
        public bool kzchangeflag
        {
            get { return _kzchangeflag; }
            set { _kzchangeflag = value; }
        }

        private bool _dormancyflag;
        /// <summary>
        /// 定义休眠标记
        /// </summary>
        public bool Dormancyflag
        {
            get { return _dormancyflag; }
            set { _dormancyflag = value; }
        }

        /// <summary>
        /// 处理措施
        /// </summary>
        private string m_strMeasure;
        /// <summary>
        /// 处理措施
        /// </summary>
        public string StrMeasure
        {
            get { return m_strMeasure; }
            set { m_strMeasure = value; }
        }

        private List<DateTime> m_ddyistarttime;
        /// <summary>
        /// 断电开始时间
        /// </summary>
        public List<DateTime> Ddyistarttime
        {
            get { return m_ddyistarttime; }
            set { m_ddyistarttime = value; }
        }

        private List<DateTime> m_kzstarttime;
        /// <summary>
        /// 控制执行开始时间
        /// </summary>
        public List<DateTime> Kzstarttime
        {
            get { return m_kzstarttime; }
            set { m_kzstarttime = value; }
        }

        private int m_nCtrlSate;
        /// <summary>
        /// 控制返回状态 复电成功/失败 断电成功/失败
        /// </summary>
        public int NCtrlSate
        {
            get { return m_nCtrlSate; }
            set { m_nCtrlSate = value; }
        }

        private DateTime m_dttkdStrtime;
        /// <summary>
        /// 馈电异常开始时间
        /// </summary>
        public DateTime DttkdStrtime
        {
            get { return m_dttkdStrtime; }
            set { m_dttkdStrtime = value; }
        }

        private long m_sckdid;
        /// <summary>
        /// 上次馈电异常的id号
        /// </summary>
        public long Sckdid
        {
            get { return m_sckdid; }
            set { m_sckdid = value; }
        }

        private DateTime m_kdStrtime;
        /// <summary>
        /// 馈电异常开始时间 用于复电失败更新记录
        /// </summary>
        public DateTime DkdStrtime
        {
            get { return m_kdStrtime; }
            set { m_kdStrtime = value; }
        }

        private DateTime m_dttRunStateTime;
        /// <summary>
        /// 对象运行状态时间
        /// </summary>
        public DateTime DttRunStateTime
        {
            get { return m_dttRunStateTime; }
            set { m_dttRunStateTime = value; }
        }

        private bool m_bEdit;
        /// <summary>
        /// 对象修改标记
        /// </summary>
        public bool BEdit
        {
            get { return m_bEdit; }
            set { m_bEdit = value; }
        }

        private CommProperty m_clsCommObj;
        /// <summary>
        /// 对象通讯类
        /// </summary>
        public CommProperty ClsCommObj
        {
            get { return m_clsCommObj; }
            set { m_clsCommObj = value; }
        }

        private AlarmProperty m_clsAlarmObj;
        /// <summary>
        /// 对象报警类
        /// </summary>
        public AlarmProperty ClsAlarmObj
        {
            get { return m_clsAlarmObj; }
            set { m_clsAlarmObj = value; }
        }

        private FiveMinData m_clsFiveMinObj;
        /// <summary>
        /// 5分钟数据处理类
        /// </summary>
        public FiveMinData ClsFiveMinObj
        {
            get { return m_clsFiveMinObj; }
            set { m_clsFiveMinObj = value; }
        }

        private List<ControlRemote> m_ClsCtrlObj;
        /// <summary>
        /// 未用
        /// </summary>
        public List<ControlRemote> ClsCtrlObj
        {
            get { return m_ClsCtrlObj; }
            set { m_ClsCtrlObj = value; }
        }

        private bool m_bCommDevTypeMatching = true;//默认是匹配的  20170817
        /// <summary>
        /// 设备匹配标记 true 匹配，false 不匹配
        /// </summary>
        public bool BCommDevTypeMatching
        {
            get { return m_bCommDevTypeMatching; }
            set { m_bCommDevTypeMatching = value; }
        }

        private List<ControlItem> deviceControlItems;
        /// <summary>
        /// 设备控制链表
        /// </summary>
        public List<ControlItem> DeviceControlItems
        {
            get { return deviceControlItems; }
            set { deviceControlItems = value; }
        }

        private List<ControlItem> soleCodingChanels;
        /// <summary>
        /// 设备唯一编码确认链表
        /// </summary>
        public List<ControlItem> SoleCodingChanels
        {
            get { return soleCodingChanels; }
            set { soleCodingChanels = value; }
        }

        private byte gasThreeUnlockContro;
        /// <summary>
        /// 瓦电3分强制解锁标记(0不解锁，1解锁)
        /// </summary>
        public byte GasThreeUnlockContro
        {
            get { return gasThreeUnlockContro; }
            set { gasThreeUnlockContro = value; }
        }

        private byte stationHisDataClear;
        /// <summary>
        /// 清除分站历史数据标记(0不清除，1清除)
        /// </summary>
        public byte StationHisDataClear
        {
            get { return stationHisDataClear; }
            set { stationHisDataClear = value; }
        }

        private int pointEditState;
        /// <summary>
        /// 测点定义状态（0 正常，1 已新增还未下发的测点，2 已删除还未下发的测点 ， 3 已修改还未下发的测点）
        /// </summary>
        public int PointEditState
        {
            get { return pointEditState; }
            set { pointEditState = value; }
        }

        //private int deviceInfoRequest;
        ///// <summary>
        ///// 获取设备信息字（第0-1位：=0 不获取唯一编码，=1 仅获取分站的唯一编码（含电量箱），=2 获取分站及下级设备全部的唯一编码；第2位：是否需要获取硬件版本号 =1表示要获取  =0表示不获取；第3位：是否需要获取软件版本号=1表示要获取  =0表示不获取）
        ///// </summary>
        //public int DeviceInfoRequest
        //{
        //    get { return deviceInfoRequest; }
        //    set { deviceInfoRequest = value; }
        //}

        private byte getSoftwareVersions;
        /// <summary>
        /// 是否需要获取软件版本号=1表示要获取  =0表示不获取
        /// </summary>
        public byte GetSoftwareVersions
        {
            get { return getSoftwareVersions; }
            set { getSoftwareVersions = value; }
        }

        private byte getHardwareVersions;
        /// <summary>
        /// 是否需要获取硬件版本号 =1表示要获取  =0表示不获取
        /// </summary>
        public byte GetHardwareVersions
        {
            get { return getHardwareVersions; }
            set { getHardwareVersions = value; }
        }

        private byte getDeviceSoleCoding;
        /// <summary>
        /// 获取唯一信息编码标记=0表法不获取，=1表示仅获取分站的唯一编码（含电量箱），=2表示获取分站及下级设备全部的唯一编码
        /// </summary>
        public byte GetDeviceSoleCoding
        {
            get { return getDeviceSoleCoding; }
            set { getDeviceSoleCoding = value; }
        }

        private DateTime getDeviceSoleCodingTime;
        /// <summary>
        /// 获取唯一编码时间 2018.3.29 by
        /// </summary>
        public DateTime GetDeviceSoleCodingTime
        {
            get { return getDeviceSoleCodingTime; }
            set { getDeviceSoleCodingTime = value; }
        }
        /// <summary>
        /// 获取设备详细信息传感器地址号列表
        /// </summary>
        public List<int> GetDeviceDetailDtataAddressLst { get; set; }
        ///// <summary>
        ///// 获取设备详细信息分站详细信息
        ///// </summary>
        //public List<StationInfo> DeviceDetailDtatalstStation { get; set; }
        ///// <summary>
        ///// 获取设备详细信息传感器详细信息
        ///// </summary>
        //public List<SensorInfo> DeviceDetailDtatalstSensor { get; set; }


        private List<EditDeviceAddressItem> modificationItems;
        /// <summary>
        /// 修改传感器地址列表，一次性仅能修改1个设备，链表是便于后续扩展
        /// </summary>
        public List<EditDeviceAddressItem> ModificationItems
        {
            get { return modificationItems; }
            set { modificationItems = value; }
        }

        public byte softwareVersions;
        /// <summary>
        /// 软件版本号,=0表示未获取
        /// </summary>
        public byte SoftwareVersions
        {
            get { return softwareVersions; }
            set { softwareVersions = value; }
        }

        public byte hardwareVersions;
        /// <summary>
        /// 硬件版本号,=0表示未获取
        /// </summary>
        public byte HardwareVersions
        {
            get { return hardwareVersions; }
            set { hardwareVersions = value; }
        }

        #endregion


        private bool doFiveMinData;
        /// <summary>
        /// 是否需要处理五分钟数据（true要处理，false 不处理） 2017.7.26 by  解决多线程同时操作五分钟类存在的冲突问题
        /// </summary>
        public bool DoFiveMinData
        {
            get { return doFiveMinData; }
            set { doFiveMinData = value; }
        }

        private bool isSendFCommand;
        /// <summary>
        /// 是否已发送F命令（标记为True才能清除F命令标记，为False不能清除F命令标记） 201.9.20 byAI  解决I命令下发后，可能不下发F命令问题
        /// </summary>
        public bool IsSendFCommand
        {
            get;
            set;
        }

        #region ----分站历史控制数据----
        private int historyControlState;
        /// <summary>
        /// 分站历史控制数据获取状态（0 未动作,1 已下发，未收到回复（通信中），2 已下发，设备已回复（通信正常），3 获取完成）
        /// </summary>
        public int HistoryControlState
        {
            get { return historyControlState; }
            set { historyControlState = value; }
        }
        private int historyControlLegacyCount;
        /// <summary>
        /// 分站历史控制数据剩余未获取数量（-1 表示已下发命令，但未获取到数据，不知道剩余条数）
        /// </summary>
        public int HistoryControlLegacyCount
        {
            get { return historyControlLegacyCount; }
            set { historyControlLegacyCount = value; }
        }

        private byte serialNumber;
        /// <summary>
        ///  生成获取历史五分钟数据命令时的顺序号，当切换QueryMinute时发生改变，且保证与上一次下发的顺序号不一致
        /// </summary>
        public byte SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }

        #endregion
        #region ----分站历史控制数据----
        private int historyRealDataState;
        /// <summary>
        /// 分站历史控制数据获取状态（0 未动作,1 已下发，未收到回复（通信中），2 已下发，设备已回复（通信正常），3 获取完成）
        /// </summary>
        public int HistoryRealDataState
        {
            get { return historyRealDataState; }
            set { historyRealDataState = value; }
        }
        private int historyRealDataLegacyCount;
        /// <summary>
        /// 分站历史控制数据剩余未获取数量（-1 表示已下发命令，但未获取到数据，不知道剩余条数）
        /// </summary>
        public int HistoryRealDataLegacyCount
        {
            get { return historyRealDataLegacyCount; }
            set { historyRealDataLegacyCount = value; }
        }

        private int reDoDeal;
        /// <summary>
        /// 是否要驱动重新对数据进行强制处理(2 要强制处理，1 待强制处理（定义了之后还没有保存巡检），0 不需要强制处理)
        /// </summary>
        public int ReDoDeal
        {
            get { return reDoDeal; }
            set { reDoDeal = value; }
        }

        private string realTypeInfo;
        /// <summary>
        /// 设备真实信息（当设备类型不匹配时，此属性存储设备的真实设备类型与实时值，格式：甲烷传感器【0.5】）
        /// </summary>
        public string RealTypeInfo
        {
            get { return realTypeInfo; }
            set { realTypeInfo = value; }
        }

        #endregion

        #region ----峰值过滤变量----

        private int abnormalCount;
        /// <summary>
        /// 监测值异常次数，用于过滤峰值
        /// </summary>
        public int AbnormalCount
        {
            get { return abnormalCount; }
            set { abnormalCount = value; }
        }

        #endregion

        private string calibrationNum;
        /// <summary>
        /// 数据批次标识（标校状态用，用于写入JC_MC表区分标识批次） 2017.10.13 by
        /// </summary>
        public string CalibrationNum
        {
            get { return calibrationNum; }
            set { calibrationNum = value; }
        }

        private int gradingAlarmCount;
        /// <summary>
        /// 分级报警待下发次数 2018.2.2 by
        /// </summary>
        public int GradingAlarmCount
        {
            get { return gradingAlarmCount; }
            set { gradingAlarmCount = value; }
        }

        private List<GradingAlarmItem> gradingAlarmItems;
        /// <summary>
        /// 分组报警列表  2018.2.2 by
        /// </summary>
        public List<GradingAlarmItem> GradingAlarmItems
        {
            get { return gradingAlarmItems; }
            set { gradingAlarmItems = value; }
        }
        private DateTime gradingAlarmTime;
        public DateTime GradingAlarmTime
        {
            get { return gradingAlarmTime; }
            set { gradingAlarmTime = value; }
        }



        private int gradingAlarmLevel;
        /// <summary>
        /// 当前分级报警等级
        /// </summary>
        public int GradingAlarmLevel
        {
            get { return gradingAlarmLevel; }
            set { gradingAlarmLevel = value; }
        }

        private int changeSenior;
        /// <summary>
        /// 传感器更换标志（0：未更换，1：更换中）
        /// </summary>
        public int ChangeSenior
        {
            get { return changeSenior; }
            set { changeSenior = value; }
        }
        /// <summary>
        /// 设备基本信息列表(用于对比传感器与定义差别，提示用户以下面定义为准，还是以中心站定义为准)
        /// </summary>
        public DeviceInfoMation DeviceInfoItem { get; set; }
        /// <summary>
        /// 表示上一次是否正常接受标记
        /// </summary>
        public int LastAcceptFlag { get; set; }
        /// <summary>
        /// 分站电源状态，=1表示分站电源箱和分站通讯故障。
        /// </summary>
        public byte StationDyType { get; set; }

        /// <summary>
        /// 分站起动时容错次数（默认20次，以免切换时分站通讯中断）
        /// </summary>
        public byte StationFaultCount { get; set; }

        #region 重写Equals / 操作符号 / 深度拷贝
        /// <summary>
        /// 重写Equals方法 实时值 状态 不做判断 ssz state alarm Voltage zts/ ID 不做判断
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Jc_DefInfo temp = obj as Jc_DefInfo;
            if (temp == null)
            {
                return false;
            }
            if (temp.Fzh != this.Fzh)
            {
                return false;
            }
            if (temp.Kh != this.Kh)
            {
                return false;
            }
            if (temp.Dzh != this.Dzh)
            {
                return false;
            }
            if (temp.Devid != this.Devid)
            {
                return false;
            }
            if (temp.Wzid != this.Wzid)
            {
                return false;
            }
            if (temp.Csid != this.Csid)
            {
                return false;
            }
            if (temp.Point != this.Point)
            {
                return false;
            }
            if (temp.Jckz1 != this.Jckz1)
            {
                return false;
            }
            if (temp.Jckz2 != this.Jckz2)
            {
                return false;
            }
            if (temp.Jckz3 != this.Jckz3)
            {
                return false;
            }
            if (temp.Z1 != this.Z1)
            {
                return false;
            }
            if (temp.Z2 != this.Z2)
            {
                return false;
            }
            if (temp.Z3 != this.Z3)
            {
                return false;
            }
            if (temp.Z4 != this.Z4)
            {
                return false;
            }
            if (temp.Z5 != this.Z5)
            {
                return false;
            }
            if (temp.Z6 != this.Z6)
            {
                return false;
            }
            if (temp.Z7 != this.Z7)
            {
                return false;
            }
            if (temp.Z8 != this.Z8)
            {
                return false;
            }
            if (temp.K1 != this.K1)
            {
                return false;
            }
            if (temp.K2 != this.K2)
            {
                return false;
            }
            if (temp.K3 != this.K3)
            {
                return false;
            }
            if (temp.K4 != this.K4)
            {
                return false;
            }
            if (temp.K5 != this.K5)
            {
                return false;
            }
            if (temp.K6 != this.K6)
            {
                return false;
            }
            if (temp.K7 != this.K7)
            {
                return false;
            }
            if (temp.K8 != this.K8)
            {
                return false;
            }
            if (temp.Bz1 != this.Bz1)
            {
                return false;
            }
            if (temp.Bz2 != this.Bz2)
            {
                return false;
            }
            if (temp.Bz3 != this.Bz3)
            {
                return false;
            }
            if (temp.Bz4 != this.Bz4)
            {
                return false;
            }
            if (temp.Bz5 != this.Bz5)
            {
                return false;
            }
            if (temp.Bz6 != this.Bz6)
            {
                return false;
            }
            if (temp.Bz7 != this.Bz7)
            {
                return false;
            }
            if (temp.Bz8 != this.Bz8)
            {
                return false;
            }
            if (temp.Bz9 != this.Bz9)
            {
                return false;
            }
            if (temp.Bz10 != this.Bz10)
            {
                return false;
            }
            if (temp.Bz11 != this.Bz11)
            {
                return false;
            }
            if (temp.Bz12 != this.Bz12)
            {
                return false;
            }
            if (temp.Bz13 != this.Bz13)
            {
                return false;
            }
            if (temp.Bz14 != this.Bz14)
            {
                return false;
            }
            if (temp.Bz15 != this.Bz15)
            {
                return false;
            }
            if (temp.Bz16 != this.Bz16)
            {
                return false;
            }
            if (temp.Bz17 != this.Bz17)
            {
                return false;
            }
            if (temp.Bz18 != this.Bz18)
            {
                return false;
            }
            if (temp.Bz19 != this.Bz19)
            {
                return false;
            }
            if (temp.Bz20 != this.Bz20)
            {
                return false;
            }
            if (temp.Upflag != this.Upflag)
            {
                return false;
            }
            if (temp.Remark != this.Remark)
            {
                return false;
            }
            if (temp.AreaName != this.AreaName)
            {
                return false;
            }
            if (temp.XCoordinate != this.XCoordinate)
            {
                return false;
            }
            if (temp.YCoordinate != this.YCoordinate)
            {
                return false;
            }
            if (temp.AreaLoc != this.AreaLoc)
            {
                return false;
            }
            if (temp.Areaid != this.Areaid)
            {
                return false;
            }
            if (temp.Addresstypeid != this.Addresstypeid)
            {
                return false;
            }
            //增加人员限制进入、禁止进入人员判断  20171123
            if (temp.RestrictedpersonInfoList != this.RestrictedpersonInfoList)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 重写==运算符 
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool operator ==(Jc_DefInfo m1, Jc_DefInfo m2)
        {
            if (null == (m1 as object)) { return null == (m2 as object); }
            return m1.Equals(m2);
        }
        /// <summary>
        /// 重写!=运算符
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool operator !=(Jc_DefInfo m1, Jc_DefInfo m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <returns></returns>
        public Jc_DefInfo Clone()
        {
            Jc_DefInfo CloneDef = Basic.Framework.Common.JSONHelper.ParseJSONString<Jc_DefInfo>(Basic.Framework.Common.JSONHelper.ToJSONString(this));
            //MemoryStream stream = new MemoryStream();
            //BinaryFormatter formatter = new BinaryFormatter();
            //formatter.Serialize(stream, this);
            //stream.Position = 0;
            return CloneDef;
        }

        #endregion
    }
}
