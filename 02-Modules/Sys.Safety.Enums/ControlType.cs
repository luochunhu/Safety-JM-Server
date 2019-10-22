using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Sys.Safety.Enums
{
    /// <summary>
    /// 控制类型（0~50表示对应的控制记录要加载到分站设备的控制链表中进行控制；51~100表示特殊控制命令，记录加载到对应的不同字段中）
    /// </summary>
    public enum ControlType
    {
        //------------------------------------------------------常规控制命令---------------------------------



        /// <summary>
        /// 不控制，ControlType的默认值
        /// </summary>
        [EnumMember]
        [Description("不进行控制")]
        NoControl = -1,
        /// <summary>
        /// 手动控制
        /// </summary>
        [EnumMember]
        [Description("手动控制")]
        LocalControl= 0,       
        /// <summary>
        /// 交叉控制
        /// </summary>
        [EnumMember]
        [Description("交叉控制")]
        RemoteControl = 1,
        /// <summary>
        /// 逻辑控制
        /// </summary>
        [EnumMember]
        [Description("逻辑控制")]
        LogicControl = 2,
        /// <summary>
        /// 0态控制
        /// </summary>
        [EnumMember]
        [Description("0态控制")]
        ControlState0 = 3,
        /// <summary>
        /// 1态控制
        /// </summary>
        [EnumMember]
        [Description("1态控制")]
        ControlState1 = 4,
        /// <summary>
        /// 2态控制
        /// </summary>
        [EnumMember]
        [Description("2态控制")]
        ControlState2 = 5,
        /// <summary>
        /// 
        /// </summary>
        [EnumMember]
        [Description("")]
        Controlhand = 6,
        /// <summary>
        /// 模拟量：断电控制
        /// </summary>
        [EnumMember]
        [Description("断电控制")]
        ControlPowerOff = 7,
        /// <summary>
        /// 模拟量：断线控制
        /// </summary>
        [EnumMember]
        [Description("断线控制")]
        ControlLineDown = 8,
        /// <summary>
        /// 模拟量：故障控制
        /// </summary>
        [EnumMember]
        [Description("故障控制")]
        ControlErro = 9,
        /// <summary>
        /// 大数据分析区域断电  20170613
        /// </summary>
        [EnumMember]
        [Description("大数据分析区域断电")]
        LargeDataAnalyticsAreaPowerOff = 11,
        /// <summary>
        /// 大数据分析区域断电  20170613
        /// </summary>
        [EnumMember]
        [Description("大数据分析传感器分级报警控制")]
        LargeDataAnalyticsSensorAlarmControl = 12,
        /// <summary>
        /// 强制解控
        /// </summary>
        [EnumMember]
        [Description("强制解控")]
        RemoveLocalControl = 13,
        //------------------------------------------------------特殊控制命令---------------------------------
        /// <summary>
        /// 分站放电 xuzp20151015
        /// </summary>
        [EnumMember]
        [Description("分站放电")]
        ControlDisCharge = 51,
        /// <summary>
        /// 瓦电3分强制解锁
        /// </summary>
        [EnumMember]
        [Description("瓦电3分强制解锁")]
        GasThreeUnlockControl = 52,
        /// <summary>
        /// 分站历史数据清除
        /// </summary>
        [EnumMember]
        [Description("分站历史数据清除")]
        StationHisDataClear = 53,
        ///// <summary>
        ///// 212指令设置指令hdw
        ///// </summary>
        //[EnumMember]
        //HJT212Set = 5555,
        ///// <summary>
        ///// 212指令控制指令hdw
        ///// </summary>
        //[EnumMember]
        //HJT212Control = 6666,
        ///// <summary>
        ///// 路灯监控控制指令xuzp20160503
        //[EnumMember]
        //LightControl = 7777,
    }
}
