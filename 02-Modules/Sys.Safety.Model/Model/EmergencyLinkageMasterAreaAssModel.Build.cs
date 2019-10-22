using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_EmergencyLinkageMasterAreaAss")]
    public partial class EmergencyLinkageMasterAreaAssModel
    {
        	    /// <summary>
        /// 主键ID
        /// </summary>
                [Key]
                public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 主控区域关联ID
        /// </summary>
                public string MasterAreaAssId
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

