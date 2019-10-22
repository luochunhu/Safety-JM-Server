using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class EmergencyLinkageMasterPointAssInfo
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
        /// 主控测点关联ID
        /// </summary>
        public string MasterPointAssId
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


