using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sys.Safety.Enums
{
    /// <summary>
    /// 设备性质枚举
    /// </summary>
    public enum DeviceProperty
    {
        /// <summary>
        /// 分站/基站
        /// </summary>
        [EnumMember]
        [Description("分站/基站")]
        Substation = 0,
        /// <summary>
        /// 模拟量
        /// </summary>
        [EnumMember]
        [Description("模拟量")]
        Analog = 1,
        /// <summary>
        /// 开关量
        /// </summary>
        [EnumMember]
        [Description("开关量")]
        Derail = 2,
        /// <summary>
        /// 控制量
        /// </summary>
        [EnumMember]
        [Description("控制量")]
        Control = 3,
        /// <summary>
        /// 累积量
        /// </summary>
        [EnumMember]
        [Description("累积量")]
        Accumulation = 4,
        /// <summary>
        /// 导出量
        /// </summary>
        [EnumMember]
        [Description("导出量")]
        Export = 5,
        /// <summary>
        /// 其他
        /// </summary>
        [EnumMember]
        [Description("其他")]
        Other = 6,
        /// <summary>
        /// 路灯
        /// </summary>
        [EnumMember]
        [Description("人员识别器")]
        Recognizer = 7,
        /// <summary>
        /// 区域
        /// </summary>
        [EnumMember]
        [Description("区域")]
        Area = 9,
        /// <summary>
        /// 字符串
        /// </summary>
        [EnumMember]
        [Description("字符串")]
        Strings = 12,
        /// <summary>
        /// 统计量
        /// </summary>
        [EnumMember]
        [Description("统计量")]
        Statistics = 13,
        /// <summary>
        /// 馈电量
        /// </summary>
        [EnumMember]
        [Description("馈电量")]
        Statiskd = 14,
        /// <summary>
        /// 电源箱
        /// </summary>
        [EnumMember]
        [Description("电源箱")]
        PowerStation = 15,
        /// <summary>
        /// 交换机
        /// </summary>
        [EnumMember]
        [Description("交换机")]
        Switches = 16,
        /// <summary>
        /// 智能量
        /// </summary>
        [EnumMember]
        [Description("智能量")]
        Intelligence = 17
    }

}
