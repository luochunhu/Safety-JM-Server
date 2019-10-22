using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class EmergencyLinkHistoryInfo
    {
        /// <summary>
        /// 唯一编码
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 应急联动配置Id
        /// </summary>
        public string SysEmergencyLinkageId
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 是否强制结束（0-不是，1-是）
        /// </summary>
        public int IsForceEnd
        {
            get;
            set;
        }
        /// <summary>
        /// 强制结束人Id
        /// </summary>
        public string EndPerson
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By3
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By4
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string By5
        {
            get;
            set;
        }
    }
}


