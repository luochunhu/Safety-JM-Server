using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_Measures")]
    public partial class Jc_CsModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public string ID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string CsID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Cs
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime CreateTime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Upflag
        {
           get;
           set;
        }
            }
}

