using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("ra_broadcastplan")]
    public partial class B_BroadcastplanModel
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
                public string PlanName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int Volume
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string PlayMode
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string PlayStyle
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime StartTime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime EndTime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string WeekDays
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

