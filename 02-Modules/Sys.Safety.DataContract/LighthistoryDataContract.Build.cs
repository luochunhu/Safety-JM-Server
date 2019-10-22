using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class LighthistoryInfo : Basic.Framework.Web.BasicInfo
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
        public double Ssz
        {
           get;
           set;
        }
         	    /// <summary>
        /// 
        /// </summary>
        public DateTime Timer
        {
           get;
           set;
        }
            }
}


