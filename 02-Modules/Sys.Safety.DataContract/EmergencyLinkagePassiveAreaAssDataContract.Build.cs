using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class EmergencyLinkagePassiveAreaAssInfo
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
        /// 被控区域关联ID
        /// </summary>
        public string PassiveAreaAssId
        {
           get;
           set;
        }
         	    /// <summary>
        /// 区域ID
        /// </summary>
        public string AreaId
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


