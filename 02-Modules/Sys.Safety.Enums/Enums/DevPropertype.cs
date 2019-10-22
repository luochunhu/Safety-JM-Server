using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Sys.Safety.Enums.Enums
{
    /// <summary>
    /// 设备性质枚举
    /// </summary>
    [Description("设备性质枚举")]
    public enum DevPropertype
    {
        /// <summary>
        /// 分站/基站
        /// </summary>
        [Description("分站/基站")]
        Substation = 0,
        /// <summary>
        /// 模拟量
        /// </summary>
        [Description("模拟量")]
        Analog = 1,
        /// <summary>
        /// 开关量
        /// </summary>
        [Description("开关量")]
        Derail = 2,
        /// <summary>
        /// 控制量
        /// </summary>
        [Description("控制量")]
        Control = 3,
        /// <summary>
        /// 累积量
        /// </summary>
        [Description("累积量")]
        Accumulation = 4,
        /// <summary>
        /// 导出量
        /// </summary>
        [Description("导出量")]
        Export = 5,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("分站/基站")]
        Other = 6,
        /// <summary>
        /// 路灯
        /// </summary>
        [Description("路灯")]
        RoadLamp = 7,
        /// <summary>
        /// 区域
        /// </summary>
        [Description("区域")]
        Area = 9,
        /// <summary>
        /// 字符串
        /// </summary>
        [Description("字符串")]
        Strings = 12,
        /// <summary>
        /// 统计量
        /// </summary>
        [Description("统计量")]
        Statistics = 13,
        /// <summary>
        /// 馈电量
        /// </summary>
        [Description("馈电量")]
        Statiskd = 14,
        /// <summary>
        /// 电源箱
        /// </summary>
        [Description("电源箱")]
        PowerStation = 15,
        /// <summary>
        /// 交换机
        /// </summary>
        [Description("交换机")]
        Switches = 16,
        /// <summary>
        /// 智能量
        /// </summary>
        [Description("智能量")]
        Intelligence = 17
    }
}
