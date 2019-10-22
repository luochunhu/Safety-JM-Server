using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.DataContract.CommunicateExtend
{
    public class ControlItem
    {
        /// <summary>
        /// 控制口号
        /// </summary>
        public int Channel;
        /// <summary>
        /// 控制类型（0解控   1控制  2强制解控）
        /// </summary>
        public int ControlType;
    }
}
