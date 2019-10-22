using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class Jc_CsInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 
        /// </summary>
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


