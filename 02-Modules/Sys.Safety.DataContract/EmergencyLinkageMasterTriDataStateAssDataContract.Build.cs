using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class EmergencyLinkageMasterTriDataStateAssInfo
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
        /// 主控触发数据状态关联ID
        /// </summary>
        public string MasterTriDataStateAssId
        {
           get;
           set;
        }
         	    /// <summary>
        /// 数据状态ID
        /// </summary>
        public string DataStateId
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


