using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("ra_broadcastplanpointlist")]
    public partial class B_BroadcastplanpointlistModel
    {
        	    /// <summary>
        /// 
        /// </summary>
                [Key]
                public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string PlanId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string PointId
        {
           get;
           set;
        }
            }
}

