using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_AreaLimitPersonnel")]
    public partial class R_ArearestrictedpersonModel
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
                public int Type
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string AreaId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Yid
        {
           get;
           set;
        }
            }
}

