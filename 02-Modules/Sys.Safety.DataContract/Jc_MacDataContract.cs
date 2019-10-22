using Sys.Safety.DataContract.CommunicateExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract
{
    public partial class Jc_MacInfo
    {
        #region Member Variables

        protected string _Wz;

        protected SpecialOrder _Order = SpecialOrder.None;

        //protected byte _PowerPackState;

        //protected byte _PowerPackVOL;

        //protected float _PowerPackMA;

        //protected float[] _BatteryVOL;

        //protected DateTime _PowerDateTime = new DateTime(1900, 1, 1, 0, 0, 0);

        protected List<BatteryItem> batteryItems;

        protected DateTime dttBridgeReceiveTime;

        protected bool _IsMemoryData;

        /// 
        #endregion

        #region Public Properties

        public virtual DateTime DttBridgeReceiveTime
        {
            get { return dttBridgeReceiveTime; }
            set { dttBridgeReceiveTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Wz
        {
            get { return _Wz; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("此属性的值长度过长 Wz", value, value.ToString());
                _Wz = value;
            }
        }


        /// <summary>
        /// 分站特殊命令
        /// </summary>
        public virtual SpecialOrder Order
        {
            get { return _Order; }
            set
            {
                _Order = value;
            }
        }
        /// <summary>
        /// 交换机电源箱数据
        /// </summary>
        public virtual List<BatteryItem> BatteryItems
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

        ///// <summary>
        ///// 电源箱状态
        ///// </summary>
        //public virtual byte PowerPackState
        //{
        //    get { return _PowerPackState; }
        //    set
        //    {
        //        _PowerPackState = value;
        //    }
        //}

        ///// <summary>
        ///// 电源箱电量
        ///// </summary>
        //public virtual byte PowerPackVOL
        //{
        //    get { return _PowerPackVOL; }
        //    set
        //    {
        //        _PowerPackVOL = value;
        //    }
        //}

        ///// <summary>
        ///// 电源箱负载电流
        ///// </summary>
        //public virtual float PowerPackMA
        //{
        //    get { return _PowerPackMA; }
        //    set
        //    {
        //        _PowerPackMA = value;
        //    }
        //}

        ///// <summary>
        ///// 电源箱电池电压
        ///// </summary>
        //public virtual float[] BatteryVOL
        //{
        //    get { return _BatteryVOL; }
        //    set
        //    {
        //        _BatteryVOL = value;
        //    }
        //}

        ///// <summary>
        ///// 获取电源箱电压时间
        ///// </summary>
        //public virtual DateTime PowerDateTime
        //{
        //    get { return _PowerDateTime; }
        //    set
        //    {
        //        _PowerDateTime = value;
        //    }
        //}

        private byte fdstate;

        /// <summary>
        /// 放电状态
        /// </summary>
        public virtual byte Fdstate
        {
            get { return fdstate; }
            set { fdstate = value; }
        }
        /// <summary>
        /// 内存数据标记
        /// </summary>
        public virtual bool IsMemoryData
        {
            get { return _IsMemoryData; }
            set { _IsMemoryData = value; }
        }
        #endregion

        #region 重写Equals 和 操作符号
        /// <summary>
        /// 重写Equals方法 实时值 状态 不做判断 NetID state / ID 不做判断
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Jc_MacInfo temp = obj as Jc_MacInfo;
            if (temp == null)
            {
                return false;
            }
            if (temp.MAC != MAC)
            {
                return false;
            }
            if (temp.IP != IP)
            {
                return false;
            }
            if (temp.Wzid != Wzid)
            {
                return false;
            }
            if (temp.Istmcs != Istmcs)
            {
                return false;
            }
            if (temp.Type != Type)
            {
                return false;
            }

            if (temp.Bz1 != Bz1)
            {
                return false;
            }
            if (temp.Bz2 != Bz2)
            {
                return false;
            }
            if (temp.Bz3 != Bz3)
            {
                return false;
            }
            if (temp.Bz4 != Bz4)
            {
                return false;
            }
            if (temp.Bz5 != Bz5)
            {
                return false;
            }
            if (temp.Bz6 != Bz6)
            {
                return false;
            }
            if (temp.Upflag != Upflag)
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
        public static bool operator ==(Jc_MacInfo m1, Jc_MacInfo m2)
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
        public static bool operator !=(Jc_MacInfo m1, Jc_MacInfo m2)
        {
            return !(m1 == m2);
        }
        #endregion

        #region OrderEnum
        public enum SpecialOrder
        {
            None,
            GetPowerPackInf,
            GetHistroyAlarmInf
        }
        #endregion

        #region ----原NetBridge内容----

        /// <summary>
        /// 时间同步命令是否需要下发
        /// </summary>
        private bool m_timeSynchronization = false;
        /// <summary>
        /// 时间同步命令是否需要下发
        /// </summary>
        public bool TimeSynchronization
        {
            get { return m_timeSynchronization; }
            set { m_timeSynchronization = value; }
        }
        /// <summary>
        /// 时间同步命令下发次数
        /// </summary>
        private int m_timeSynchronizationcount = 3;
        /// <summary>
        /// 时间同步命令下发次数
        /// </summary>
        public int TimeSynchronizationcount
        {
            get { return m_timeSynchronizationcount; }
            set { m_timeSynchronizationcount = value; }
        }

        /// <summary>
        /// 是否要下发分站序列 
        /// True-是 
        /// False-否
        /// </summary>
        private bool m_bBridgeInitStationQueen;
        /// <summary>
        /// 是否要下发分站序列 
        /// True-是 
        /// False-否
        /// </summary>
        public bool BBridgeInitStationQueen
        {
            get { return m_bBridgeInitStationQueen; }
            set { m_bBridgeInitStationQueen = value; }
        }

        /// <summary>
        /// 上一帧接收标志 1为正确
        /// </summary>
        private bool m_bBridgeRevMark;
        /// <summary>
        /// 上一帧接收标志 1为正确
        /// </summary>
        public bool BBridgeRevMark
        {
            get { return m_bBridgeRevMark; }
            set { m_bBridgeRevMark = value; }
        }
        /// <summary>
        /// 连接是否关闭
        /// </summary>
        private bool m_bBridgeClosed;
        /// <summary>
        /// 连接是否关闭
        /// </summary>
        public bool BBridgeClosed
        {
            get { return m_bBridgeClosed; }
            set { m_bBridgeClosed = value; }
        }
        /// <summary>
        /// 连接关闭时间
        /// </summary>
        private DateTime m_bBridgeClosedTime;
        /// <summary>
        /// 连接关闭时间
        /// </summary>
        public DateTime BBridgeClosedTime
        {
            get { return m_bBridgeClosedTime; }
            set { m_bBridgeClosedTime = value; }
        }


        private bool sendU;
        /// <summary>
        /// 发送u命令  广播 tanxingyan 20161124
        /// </summary>
        public bool SendU
        {
            get { return sendU; }
            set { sendU = value; }
        }
        #endregion


        private byte batteryControl;
        /// <summary>
        /// 表示对交换机电源箱的远程操作，=0表示无动作，=1表示远程放电。
        /// </summary>
        public byte BatteryControl
        {
            get { return batteryControl; }
            set { batteryControl = value; }
        }

        private byte powerPercentum { get; set; }
        /// <summary>
        /// 放电时，切换的百分比
        /// </summary>
        public byte PowerPercentum
        {
            get { return powerPercentum; }
            set { powerPercentum = value; }
        }
        private DateTime sendDtime;

        public DateTime SendDtime
        {
            get { return sendDtime; }
            set { sendDtime = value; }
        }
        private int nCommandbz;

        /// <summary>
        /// 命令下发标记（0x01 下发获取电源箱数据命令,0x02 下发模拟报警）
        /// </summary>
        public int NCommandbz
        {
            get { return nCommandbz; }
            set { nCommandbz = value; }
        }

        private int sendBatteryControlCount;
        /// <summary>
        /// 需要下发的QueryBatteryRealDataRequest次数
        /// </summary>
        public int SendBatteryControlCount
        {
            get { return sendBatteryControlCount; }
            set { sendBatteryControlCount = value; }
        }

        /// <summary>
        /// 最后一次接收到网络模块数据的时间（2017、8.22 by）
        /// </summary>
        public DateTime LastGetTime
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已向网络模块下发复位命令
        /// </summary>
        public bool isSendReset
        {
            get;
            set;
        }

        /// <summary>
        /// 测试报警命令下发次数
        /// </summary>
        public int sendAlarmCount
        {
            get;
            set;
        }
        /// <summary>
        /// 报警测试（1 报警、0 解除）
        /// </summary>
        public int testAlarmFlag
        {
            get;
            set;
        }

        #region 新增交换机实时状态缓存  20180525
        /// <summary>
        /// 电源箱电池控制状态（0不放电，1放电）
        /// </summary>
        public byte BatteryControlState { get; set; }
        /// <summary>
        ///电源箱状态  1表示直流供电，=0表示交流供电
        /// </summary>
        public byte BatteryState { get; set; }
        /// <summary>
        /// 电源箱电池容量，取值[0,100]；单位：%，如有电池箱是汇总电池箱后的容量百分比
        /// </summary>
        public byte BatteryCapacity { get; set; }
        /// <summary>
        /// （串口服务器-供电电源）：
        ///    0：供电故障；1：供电正常
        /// </summary>
        public byte SerialPortBatteryState { get; set; }
        /// <summary>
        /// （串口服务器-运行状态）：
        ///    0：运行故障；1：运行正常
        /// </summary>
        public byte SerialPortRunState { get; set; }
        /// <summary>
        /// （交换机-供电电源）：
        ///    0：供电故障；1：供电正常
        /// </summary>
        public byte SwitchBatteryState { get; set; }
        /// <summary>
        /// （交换机-运行状态）：
        ///    0：运行故障；1：运行正常
        /// </summary>
        public byte SwitchRunState { get; set; }
        /// <summary>
        /// 数组长度为3
        /// 【0】=(千兆光口1状态):
        ///0：通信故障；1：通信正常
        /// 【1】=(千兆光口2状态):
        ///0：通信故障；1：通信正常
        /// 【2】=(千兆光口3状态):
        ///0：通信故障；1：通信正常
        /// </summary>
        //public byte[] Switch1000State { get; set; }
        public string Switch1000State { get; set; }
        /// <summary>
        /// 数组长度为7
        /// 【0】=(百兆接口1状态):
        ///0：通信故障；1：通信正常
        /// 【1】=(百兆接口2状态):
        ///0：通信故障；1：通信正常
        /// 【2】=(百兆接口3状态):
        ///0：通信故障；1：通信正常
        /// 【3】=(百兆接口4状态):
        ///0：通信故障；1：通信正常
        /// 【4】=(百兆接口5状态):
        ///0：通信故障；1：通信正常
        /// 【5】=(百兆接口6状态):
        ///0：通信故障；1：通信正常
        /// 【6】=(百兆接口7状态):
        ///0：通信故障；1：通信正常
        /// </summary>
        //public byte[] Switch100State { get; set; }
        public string Switch100State { get; set; }
        /// <summary>
        /// 百兆电口状态
        /// </summary>
        //public byte[] Switch100RJ45State { get; set; }
        public string Switch100RJ45State { get; set; }
        #endregion

        /// <summary>
        /// 模块绑定的分站地址，如果是交换机则为0 
        /// </summary>
        public int BindStatinNumber
        {
            get;
            set;
        }        
        /// <summary>
        /// 网关 
        /// </summary>
        public string GatewayIp
        {
            get;
            set;
        }
        /// <summary>
        /// 子网掩码 
        /// </summary>
        public string SubMask
        {
            get;
            set;
        }
    }
}
