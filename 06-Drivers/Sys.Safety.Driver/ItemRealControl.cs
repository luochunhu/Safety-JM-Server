using System;
using Sys.Safety.Enums;

namespace Sys.Safety.Driver
{
    /// <summary>
    /// 存储手动操作
    /// </summary>
    public class ItemRealControl
    {
        /// <summary>
        /// 主控制设备标签
        /// </summary>       
        public string m_strzEquipmentLabel;
        /// <summary>
        /// 主控制设备上级设备号
        /// </summary>      
        public decimal m_nzStationID;
        /// <summary>
        /// 主控制设备通道号
        /// </summary>     
        public decimal m_nzChannelID;
        /// <summary>
        /// 主控制设备地址号
        /// </summary>      
        public decimal m_nzAdrID;
        /// <summary>
        /// 被控制设备标签
        /// </summary>       
        public string m_strEquipmentLabel;
        /// <summary>
        /// 被控制设备上级设备号
        /// </summary>       
        public decimal m_nStationID;
        /// <summary>
        /// 被控制设备通道号
        /// </summary>    
        public decimal m_nChannelID;
        /// <summary>
        /// 被控设备地址号
        /// </summary>   
        public decimal m_nAdrID;
        /// <summary>
        /// 本次操作控制口的状态
        /// </summary>    
        public DeviceDataState ControlState;
        /// <summary>
        /// 控制类型
        /// </summary>
        public ControlType ControlType;
        /// <summary>
        /// 下发控制信息的时间
        /// </summary>    
        public DateTime m_dttControlTime;
        /// <summary>
        /// 下发控制信息的人员名称
        /// </summary>  
        public string m_strControlOperator;
        /// <summary>
        /// 表示数据库的ID hdw
        /// </summary>
        public string ID;
    }
}
