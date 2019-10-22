using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_UndefinedDevice")]
    public partial class R_UndefinedDefModel
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
                public string PointId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Fzh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Kh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Dzh
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Point
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime CreateUpdateTime
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
                public string State
        {
           get;
           set;
        }
            }
}

