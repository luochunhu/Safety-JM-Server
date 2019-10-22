using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Safety.Enums
{
    /// <summary>
    /// 馈电异常状态
    /// </summary>
    public enum ControlState
    {
        /// <summary>
        /// 复电成功
        /// </summary>
        [EnumMember]
        DataPowerOnSuccess = 0,
        /// <summary>
        /// 复电失败
        /// </summary>
        [EnumMember]
        DataPowerOnFail = 30,
        /// <summary>
        /// 断电成功
        /// </summary>
        [EnumMember]
        DataPowerOffSuccess = 2,
        /// <summary>
        /// 断电失败
        /// </summary>
        [EnumMember]
        DataPowerOffFail = 32,
        /// <summary>
        /// 断电状态未知
        /// </summary>
        [EnumMember]
        DataPowerUnKnowm = 46
    }
}
