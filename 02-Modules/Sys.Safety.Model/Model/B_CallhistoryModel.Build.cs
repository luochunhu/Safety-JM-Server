using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("ra_callhistory")]
    public partial class B_CallhistoryModel
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
                public string MasterId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string CallName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int CallType
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int RadioType
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Message
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string CallUserId
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

