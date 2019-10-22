using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_ListDataLayount")]
    public partial class ListdatalayountModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int ListDataLayoutID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int ListDataID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string StrDate
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string StrFileName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string StrConTextCondition
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string StrCondition
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string StrConditionCHS
        {
           get;
           set;
        }
            }
}

