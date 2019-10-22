using System.Runtime.Serialization;
using System.ComponentModel;

namespace Sys.Safety.Enums
{
    /// <summary>
    /// 设备数据状态
    /// </summary>
    public enum DeviceDataState
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
        /// 上限预警
        /// </summary>
        [EnumMember]
        [Description("上限预警")]
        DataHighPreAlarm = 8,
        /// <summary>
        /// 上限预警解除
        /// </summary>
        [EnumMember]
        [Description("上限预警解除")]
        DataHighPreAlarmRemove = 9,
        /// <summary>
        /// 上限报警
        /// </summary>
        [EnumMember]
        [Description("上限报警")]
        DataHighAlarm = 10,
        /// <summary>
        /// 上限报警解除
        /// </summary>
        [EnumMember]
        [Description("上限报警解除")]
        DataHighAlarmRemove = 11,
        /// <summary>
        /// 上限断电
        /// </summary>
        [EnumMember]
        [Description("上限断电")]
        DataHighAlarmPowerOFF = 12,
        /// <summary>
        /// 上限断电解除
        /// </summary>
        [EnumMember]
        [Description("上限断电解除")]
        DataHighAlarmPowerOFFRemove = 13,
        /// <summary>
        /// 下限预警
        /// </summary>
        [EnumMember]
        [Description("下限预警")]
        DataLowPreAlarm = 14,
        /// <summary>
        /// 下限预警解除
        /// </summary>
        [EnumMember]
        [Description("下限预警解除")]
        DataLowPreAlarmRemove = 15,
        /// <summary>
        /// 下限报警
        /// </summary>
        [EnumMember]
        [Description("下限报警")]
        DataLowAlarm = 16,
        /// <summary>
        /// 下限报警解除
        /// </summary>
        [EnumMember]
        [Description("下限报警解除")]
        DataLowAlarmRemove = 17,
        /// <summary>
        /// 下限断电
        /// </summary>
        [EnumMember]
        [Description("下限断电")]
        DataLowPower = 18,
        /// <summary>
        /// 下限断电解除
        /// </summary>
        [EnumMember]
        [Description("下限断电解除")]
        DataLowPowerRemove = 19,
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
        [Description("0态")]
        DataDerailState0 = 25,
        /// <summary>
        /// 开关量1态
        /// </summary>
        [EnumMember]
        [Description("1态")]
        DataDerailState1 = 26,
        /// <summary>
        /// 开关量2态
        /// </summary>
        [EnumMember]
        [Description("2态")]
        DataDerailState2 = 27,
        /// <summary>
        /// 开机
        /// </summary>
        [EnumMember]
        [Description("开机")]
        EquipmentStart = 28,
        /// <summary>
        /// 复电成功
        /// </summary>
        [EnumMember]
        [Description("复电成功")]
        DataPowerOnSuc = 29,
        /// <summary>
        /// 复电失败
        /// </summary>
        [EnumMember]
        [Description("复电失败")]
        DataPowerOnFail = 30,
        /// <summary>
        /// 断电成功
        /// </summary>
        [EnumMember]
        [Description("断电成功")]
        DataPowerOffSuc = 31,
        /// <summary>
        /// 断电失败
        /// </summary>
        [EnumMember]
        [Description("断电失败")]
        DataPowerOffFail = 32,
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
        /// 满足条件
        /// </summary>
        [EnumMember]
        [Description("满足")]
        DataSatisfyCon = 40,
        /// <summary>
        /// 不满足条件
        /// </summary>
        [EnumMember]
        [Description("不满足")]
        DataMissCon = 41,
        /// <summary>
        /// 线性突变
        /// </summary>
        [EnumMember]
        [Description("线性突变")]
        EquipmentChange = 42,
        /// <summary>
        ///控制量0态
        /// </summary>
        [EnumMember]
        [Description("0态")]
        DataControlState0 = 43,
        /// <summary>
        /// 控制量1态
        /// </summary>
        [EnumMember]
        [Description("1态")]
        DataControlState1 = 44,
        /// <summary>
        /// 控制量断线
        /// </summary>
        [EnumMember]
        [Description("断线")]
        EquipmentControlDown = 45,
        /// <summary>
        /// 设备状态未知(历史表中不会出现该状态，用于实时显示部分表示对应分站通讯中断后传感器的状态)
        /// </summary>
        [EnumMember]
        [Description("未知")]
        EquipmentStateUnknow = 46,
        /// <summary>
        /// 人员识别器-报警状态（用于图形显示）
        /// </summary>
        [EnumMember]
        [Description("识别器报警状态")]
        RecognizerAlarm = 47,
        /// <summary>
        /// 广播电话-通话中
        /// </summary>
        [EnumMember]
        [Description("通话中")]
        BroadCastInCall = 49,
        /// <summary>
        ///广播电话-广播中
        /// </summary>
        [EnumMember]
        [Description("广播中")]
        BroadCastInRadio = 50,
        /// <summary>
        ///广播电话-音乐播放中
        /// </summary>
        [EnumMember]
        [Description("音乐播放中")]
        BroadCastMusicPlay = 51,
        /// <summary>
        ///广播电话-空闲
        /// </summary>
        [EnumMember]
        [Description("空闲")]
        BroadCastFree = 52,
        /// <summary>
        ///广播电话-摘机
        /// </summary>
        [EnumMember]
        [Description("摘机")]
        BroadCastPicking = 53,
        /// <summary>
        ///广播电话-呼叫
        /// </summary>
        [EnumMember]
        [Description("呼叫")]
        BroadCastCalling = 54,
        /// <summary>
        ///广播电话-振铃
        /// </summary>
        [EnumMember]
        [Description("振铃")]
        BroadCastRinging = 55,
        /// <summary>
        ///广播电话-回铃
        /// </summary>
        [EnumMember]
        [Description("回铃")]
        BroadCastBackBell = 56,
        /// <summary>
        ///广播电话-通话保持
        /// </summary>
        [EnumMember]
        [Description("通话保持")]
        BroadCastKeep = 57,
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
        /// 与服务器连接中断
        /// </summary>
        [EnumMember]
        [Description("与服务器连接中断")]
        OutSevice = 119
    }
}
