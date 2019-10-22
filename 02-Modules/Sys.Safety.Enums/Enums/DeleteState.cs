using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Sys.Safety.Enums.Enums
{
    /// <summary>
    /// 是否删除
    /// </summary>
    [Description("是否删除")]
    public enum DeleteState
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 否
        /// </summary>
        [Description("否")]
        No = 1,
        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        Yes = 2,
    }
}
