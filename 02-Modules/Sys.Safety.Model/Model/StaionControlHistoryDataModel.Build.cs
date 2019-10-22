using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_StaionControlHistoryData")]
    public partial class StaionControlHistoryDataModel
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
                public int State
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Value
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public int ControlDevice
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime SaveTime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public DateTime DataTime
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Remark1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Remark2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Remark3
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Remark4
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
                public string Remark5
        {
           get;
           set;
        }
            }
}

