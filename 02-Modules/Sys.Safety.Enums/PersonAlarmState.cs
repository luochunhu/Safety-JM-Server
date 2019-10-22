using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    public enum PersonAlarmState
    {
        /// <summary>
        /// 不报警
        /// </summary>
        [EnumMember]
        [Description("不报警")]
        nomal = 0,
        /// <summary>
        /// 定点超时
        /// </summary>
        [EnumMember]
        [Description("定点超时")]
        PointTimeout = 1,
        /// <summary>
        /// 入井超时
        /// </summary>
        [EnumMember]
        [Description("入井超时")]
        PersonInWellTimeout = 2,
        /// <summary>
        /// 禁止进入
        /// </summary>
        [EnumMember]
        [Description("禁止进入")]
        PersonUnAccess = 3,
        /// <summary>
        /// 限制进入
        /// </summary>
        [EnumMember]
        [Description("限制进入")]
        PersonBanAccess = 4,
        /// <summary>
        /// 定点超员
        /// </summary>
        [EnumMember]
        [Description("定点超员")]
        PointOverCount = 5,
        /// <summary>
        /// 入井超员
        /// </summary>
        [EnumMember]
        [Description("入井超员")]
        InWellOverCount = 6,
        /// <summary>
        /// 工作异常
        /// </summary>
        [EnumMember]
        [Description("工作异常")]
        WorkAbnormal = 7,
        /// <summary>
        /// 报警解除
        /// </summary>
        [EnumMember]
        [Description("报警解除")]
        AlarmRemove = 8,
        /// <summary>
        /// 电池欠压
        /// </summary>
        [EnumMember]
        [Description("电池欠压")]
        PowerUnderVoltage = 9,
        /// <summary>
        /// 区域超时
        /// </summary>
        [EnumMember]
        [Description("区域超时")]
        AreaTimeout = 10,
        /// <summary>
        /// 区域超员
        /// </summary>
        [EnumMember]
        [Description("区域超员")]
        AreaOverCount = 11,
        /// <summary>
        /// 区域限制进入
        /// </summary>
        [EnumMember]
        [Description("区域限制进入")]
        AreaBanAccess = 12,
        /// <summary>
        /// 区域禁止进入
        /// </summary>
        [EnumMember]
        [Description("区域禁止进入")]
        AreaUnAccess = 13,
        /// <summary>
        /// 区域工作异常
        /// </summary>
        [EnumMember]
        [Description("区域工作异常")]
        AreaWorkAbnormal = 14,
        /// <summary>
        /// 井下呼叫
        /// </summary>
        [EnumMember]
        [Description("井下呼叫")]
        PersonCall = 15,
        /// <summary>
        /// 人员脱岗报警
        /// </summary>
        [EnumMember]
        [Description("人员脱岗报警")]
        PersonOutJob = 16,
        /// <summary>
        /// 未下井脱岗报警
        /// </summary>
        [EnumMember]
        [Description("未下井脱岗报警")]
        PersonNotInWellOutJob = 17
    }
}
