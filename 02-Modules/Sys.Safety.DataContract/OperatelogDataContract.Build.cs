using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class OperatelogInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 
        /// </summary>
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


