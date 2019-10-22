using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class R_ArearestrictedpersonInfo
    {        
         	    /// <summary>
        /// 
        /// </summary>
        public string Id
        {
           get;
           set;
        }
         	    /// <summary>
        /// （0-限制进入，1-禁止进入）
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


