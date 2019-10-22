using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class EmergencyLinkageMasterDevTypeAssInfo
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
        /// 主控设备类型关联ID
        /// </summary>
        public string MasterDevTypeAssId
        {
           get;
           set;
        }
         	    /// <summary>
        /// 设备类型ID
        /// </summary>
        public string DevId
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


