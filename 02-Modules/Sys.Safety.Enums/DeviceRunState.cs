using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sys.Safety.Enums
{
    /// <summary>
    /// 设备运行状态
    /// </summary>
    [Serializable, DataContract]
    public enum DeviceRunState
    {
        /// <summary>
        /// 通讯中断
        /// </summary>
        [EnumMember]
        [Description("通讯中断")]
        EquipmentInterrupted = 0,
        /// <summary>
        /// 通讯误码
        /// </summary>
        [EnumMember]
        [Description("通讯误码")]
        EquipmentBiterror = 1,
        /// <summary>
        /// 初始化中
        /// </summary>
        [EnumMember]
        [Description("初始化中")]
        EquipmentIniting = 2,
        /// <summary>
        /// 交流正常
        /// </summary>
        [EnumMember]
        [Description("交流正常")]
        EquipmentAC = 3,
        /// <summary>
        /// 直流正常
        /// </summary>
        [EnumMember]
        [Description("直流正常")]
        EquipmentDC = 4,
        /// <summary>
        /// 红外遥控
        /// </summary>
        [EnumMember]
        [Description("红外遥控")]
        EquipmentInfrareding = 5,
        /// <summary>
        /// 休眠
        /// </summary>
        [EnumMember]
        [Description("休眠")]
        EquipmentSleep = 6,
        /// <summary>
        /// 设备检修
        /// </summary>
        [EnumMember]
        [Description("检修")]
        EquipmentDebugging = 7,
        /// <summary>
        /// 断线
        /// </summary>
        [EnumMember]
        [Description("断线")]
        EquipmentDown = 20,
        /// <summary>
        /// 设备正常
        /// </summary>
        [EnumMember]
        [Description("正常")]
        EquipmentCommOK = 21,
        /// <summary>
        /// 上溢
        /// </summary>
        [EnumMember]
        [Description("上溢")]
        EquipmentOverrange = 22,
        /// <summary>
        /// 负漂
        /// </summary>
        [EnumMember]
        [Description("负漂")]
        EquipmentUnderrange = 23,
        /// <summary>
        /// 标校
        /// </summary>
        [EnumMember]
        [Description("标校")]
        EquipmentAdjusting = 24,
        /// <summary>
        /// 开关量0态
        /// </summary>
        [EnumMember]
        [Description("开关量断线")]
        DataDerailState0 = 25,
        /// <summary>
        /// 开机
        /// </summary>
        [EnumMember]
        [Description("开机")]
        EquipmentStart = 28,
        /// <summary>
        /// 头子断线
        /// </summary>
        [EnumMember]
        [Description("头子断线")]
        EquipmentHeadDown = 33,
        /// <summary>
        /// 类型有误
        /// </summary>
        [EnumMember]
        [Description("类型有误")]
        EquipmentTypeError = 34,
        /// <summary>
        /// 系统退出
        /// </summary>
        [EnumMember]
        [Description("系统退出")]
        SystemExsit = 35,
        /// <summary>
        /// 系统启动
        /// </summary>
        [EnumMember]
        [Description("系统启动")]
        SystemStart = 36,
        /// <summary>
        /// 非法退出
        /// </summary>
        [EnumMember]
        [Description("非法退出")]
        SystemExsitFF = 37,
        /// <summary>
        /// 过滤数据
        /// </summary>
        [EnumMember]
        [Description("过滤数据")]
        DataFilter = 38,
        /// <summary>
        /// 热备日志
        /// </summary>
        [EnumMember]
        [Description("热备日志")]
        DataCurLog = 39,
        /// <summary>
        /// 线性突变
        /// </summary>
        [EnumMember]
        [Description("线性突变")]
        EquipmentChange = 42,
        /// <summary>
        /// 控制量断线
        /// </summary>
        [EnumMember]
        [Description("控制量断线")]
        EquipmentControlDown = 45,
        /// <summary>
        /// 设备状态未知(历史表中不会出现该状态，用于实时显示部分表示对应分站通讯中断后传感器的状态)
        /// </summary>
        [EnumMember]
        [Description("未知")]
        EquipmentStateUnknow = 46,

        /// <summary>
        ///传感器电量过低
        /// </summary>
        [EnumMember]
        [Description("传感器电量过低")]
        SensorPowerAlarm = 58,
        /// <summary>
        ///传感器电压过低
        /// </summary>
        [EnumMember]
        [Description("传感器电压过低")]
        UnderVoltageAlarm = 59,
        /// <summary>
        ///传感器更换中
        /// </summary>
        [EnumMember]
        [Description("传感器更换中")]
        SensorChangeing = 60,
        /// <summary>
        /// 删除  
        /// </summary>
        [EnumMember]
        [Description("删除")]
        Deleted = 50
    }
}
