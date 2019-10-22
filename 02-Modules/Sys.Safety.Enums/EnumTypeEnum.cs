using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    /// <summary>
    /// 作者:
    /// 时间:2017-05-24
    /// 描述:枚举类型枚举
    /// 修改记录
    /// 2017-05-24
    /// </summary>
    public enum EnumTypeEnum
    {
        /// <summary>
        /// 设备性质
        /// </summary>
        [Description("设备性质")]
        DeviceProperty=1,

        /// <summary>
        /// 设备种类
        /// </summary>
        [Description("设备种类")]
        DeviceClass =2,

        /// <summary>
        /// 设备型号
        /// </summary>
        [Description("设备型号")]
        DeviceType=3,

        /// <summary>
        /// 数据状态
        /// </summary>
        [Description("数据状态")]
        DataState =4,

        /// <summary>
        /// 操作日志
        /// </summary>
        [Description("操作日志")]
        OperateLog =5,

        /// <summary>
        /// 配置类型
        /// </summary>
        [Description("配置类型")]
        ConfigType =6,

        /// <summary>
        /// 驱动类型
        /// </summary>
        [Description("驱动类型")]
        DriverType =7,

        /// <summary>
        /// 设备状态
        /// </summary>
        [Description("设备状态")]
        DeviceState =8,

        /// <summary>
        /// 职务
        /// </summary>
        [Description("职务")]
        Title=20,

        /// <summary>
        /// 工种
        /// </summary>
        [Description("工种")]
        WorkType=25
    }
}
