using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class Jc_BxInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// ID编号
        /// </summary>
        public string ID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 测点号
        /// </summary>
        public string Point
        {
           get;
           set;
        }
         	    /// <summary>
        /// 日期
        /// </summary>
        public DateTime Timer
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用
        /// </summary>
        public string Bz1
        {
           get;
           set;
        }
            }
}


