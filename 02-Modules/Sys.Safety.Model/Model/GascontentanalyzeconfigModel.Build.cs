using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_GasAnalysisConfig")]
    public partial class GascontentanalyzeconfigModel
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
        		        public string Pointid
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Height
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Width
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Thickness
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Speed
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Length
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Acreage
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Percent
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Wind
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Comparevalue
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Spare1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Spare2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 
        /// </summary>
        		        public string Spare3
        {
           get;
           set;
        }
            }
}

