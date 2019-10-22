using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_EmergencyLinkagePassivePersonAss")]
    public partial class EmergencyLinkagePassivePersonAssModel
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
        /// 被控人员关联ID
        /// </summary>
                public string PassivePersonAssId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 人员ID
        /// </summary>
                public string PersonId
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

