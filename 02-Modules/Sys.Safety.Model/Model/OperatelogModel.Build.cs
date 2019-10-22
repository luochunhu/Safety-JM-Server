using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("BFT_OperateLog")]
    public partial class OperatelogModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public string OperateLogID
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string UserName
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string LoginIP
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
                public string OperationContent
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
                public string Remark
        {
           get;
           set;
        }
            }
}

