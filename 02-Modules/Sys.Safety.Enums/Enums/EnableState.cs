using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums.Enums
{
    /// <summary>
    /// 是否启用
    /// </summary>
    [Description("是否启用")]
    public enum EnableState
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Yes = 1,
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        No = 2
    }
}
