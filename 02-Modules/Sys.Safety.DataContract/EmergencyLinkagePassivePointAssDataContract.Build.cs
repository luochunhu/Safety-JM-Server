using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class EmergencyLinkagePassivePointAssInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 被控测点关联ID
        /// </summary>
        public string PassivePointAssId
        {
            get;
            set;
        }
        /// <summary>
        /// 测点ID
        /// </summary>
        public string PointId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统编号
        /// </summary>
        public int Sysid
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Bz1
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz2
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Bz3
        {
            get;
            set;
        }
    }
}


