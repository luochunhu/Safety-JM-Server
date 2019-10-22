using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class AreaRuleInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string RuleID
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Areaid
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Devid
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int DeviceCount
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public float MaxValue
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public float MinValue
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


