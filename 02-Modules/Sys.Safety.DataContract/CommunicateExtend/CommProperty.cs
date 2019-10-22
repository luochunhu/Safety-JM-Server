using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    public class CommProperty
    {
        public CommProperty(uint _StationID)
        {
            m_bInit = true;//默认初始标记为true,不下发初始化
            StationID = _StationID;
        }
        private uint StationID;
        private uint m_nControlMark;
        private bool m_bSendControlCommand;
        private bool m_bSendCommand;
        private byte m_nCommand = 0;
        private ushort m_nCommandbz = 0;
        //private byte m_nCommand_Type = 0x02;
        private byte m_nCommand_Flen = 0;
        private int m_nCommand_DelayTime = 0;
        private uint m_nMaxErrCount = 0; 
        private uint m_nErrCount = 0;
        private DateTime m_dttStateTime = DateTime.Now;
        private bool m_bCommuOK = false;
        private ushort m_nCommCount = 0;
        private bool m_bInit = false;  
        private bool m_bNet_Change = false;
        private bool m_bCommTest;
        private uint m_nCommTest_ErrCount;
        private uint m_nCommTest_TotalCount;

        /// <summary>
        /// 设备当前的通迅命令
        /// 默认为空,分站时使用
        /// </summary>
        public byte NCommand
        {
            get { return m_nCommand; }
            set { m_nCommand = value; }
        }
        /// <summary>
        /// 可用于存储驱动下发命令标志 与 命令队列一起使用  [0x01:I 0x02:D 0x04:T 0x08:R 0x10:S 0x20:L 0x40:F ]
        /// </summary>
        public ushort NCommandbz
        {
            get { return m_nCommandbz; }
            set { m_nCommandbz = value; }
        }
        ///// <summary>
        ///// 设备通讯协议类型 0x01-老协议分站（累加和） 0x02-新协-议分站（CRC） 0x03-人员定位分站（累加和）0x04-KJ78A分站0x05-KJ78B分站新增
        ///// </summary>
        //public byte NCommand_Type
        //{
        //    get { return m_nCommand_Type; }
        //    set { m_nCommand_Type = value; }
        //}
        /// <summary>
        /// 设备的F命令回发长度  100交换机协议用
        /// </summary>
        public byte NCommand_Flen
        {
            get { return m_nCommand_Flen; }
            set { m_nCommand_Flen = value; }
        }

        /// <summary>
        /// 设备中断几次算中断
        /// </summary>
        public uint NMaxErrCount //xuzp20160503
        {
            get { return m_nMaxErrCount; }
            set { m_nMaxErrCount = value; }
        }
        /// <summary>
        /// 设备当前中断次数
        /// </summary>
        //public uint NErrCount //xuzp20160503 hdw:20170718,和外部的变量重复，统一使用外部变量
        //{
        //    get { return m_nErrCount; }
        //    set { m_nErrCount = value; }
        //}
        /// <summary>
        /// 最近一次状态时间
        /// </summary>
        //public DateTime DttStateTime
        //{
        //    get { return m_dttStateTime; }
        //    set { m_dttStateTime = value; }
        //}
        /// <summary>
        /// 设备上一次通迅正常标志
        /// True正确通信;False通迅异常
        /// </summary>
        public bool BCommuOK
        {
            get { return m_bCommuOK; }
            set { m_bCommuOK = value; }
        }

        /// <summary>
        /// 是否初始化  分站用
        /// </summary>
        public bool BInit
        {
            get { return m_bInit; }
            set { m_bInit = value; }
        }
        /// <summary>
        /// 网络模块绑定分站队列改变 或 分站通讯中断，下发F命令
        /// </summary>
        public bool BNet_Change
        {
            get { return m_bNet_Change; }
            set { m_bNet_Change = value; }
        }
        /// <summary>
        /// 设备通讯接收次数  测试时用
        /// </summary>
        public ushort NCommCount
        {
            get { return m_nCommCount; }
            set { m_nCommCount = value; }
        }

        /// <summary>
        /// 测试下发次数
        /// </summary>
        public uint NComTest_TotalCount
        {
            get { return m_nCommTest_TotalCount; }
            set { m_nCommTest_TotalCount = value; }
        }

        /// <summary>
        /// 当前设备是否需要下发命令
        /// </summary>
        public bool BSendCommand
        {
            get { return m_bSendCommand; }
            set { m_bSendCommand = value; }
        }

        /// <summary>
        /// 是否发送控制命令
        /// </summary>
        public bool BSendControlCommand
        {
            get { return m_bSendControlCommand; }
            set { m_bSendControlCommand = value; }
        }

        /// <summary>
        /// 手动控制标记
        /// </summary>
        public uint NControlMark
        {
            get { return m_nControlMark; }
            set { m_nControlMark = value; }
        }
        /// <summary>
        /// 是否处于通讯测试中
        /// </summary>
        public bool BCommTest
        {
            get { return m_bCommTest; }
            set { m_bCommTest = value; }
        }

        private int cfzfy;
        /// <summary>
        /// 抽放正负压
        /// </summary>
        public int Cfzfy
        {
            get { return cfzfy; }
            set { cfzfy = value; }
        }
        /// <summary>
        /// 本地大气压
        /// </summary>
        private decimal bddqy;
        /// <summary>
        /// 本地大气压
        /// </summary>
        public decimal Bddqy
        {
            get { return bddqy; }
            set { bddqy = value; }
        }
    }
}
