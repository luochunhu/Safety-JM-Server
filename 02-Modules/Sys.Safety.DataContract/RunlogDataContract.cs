using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class RunlogInfo
    {
        /// <summary>
        /// 系统运行日志等级枚举
        /// </summary>
        public enum EnumLogLevel
        {
            /// <summary>
            /// 所有等级
            /// </summary>
            All = 0x0,
            /// <summary>
            /// 警告等级
            /// </summary>
            WARN = 0x01,
            /// <summary>
            /// 错误等级
            /// </summary>
            ERROR = 0x02,
            /// <summary>
            /// 风险等级
            /// </summary>
            INFO = 0x03,
            /// <summary>
            /// 调试等级
            /// </summary>
            DEBUG = 0x04

        }
    }
}


