using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums.Enums
{
    public enum VideoVendorType
    {
        /// <summary>
        /// 海康
        /// </summary>
        [Description("海康")]
        HK = 0,
        /// <summary>
        /// 大华
        /// </summary>
        [Description("大华")]
        DH = 1,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 2
    }
}
