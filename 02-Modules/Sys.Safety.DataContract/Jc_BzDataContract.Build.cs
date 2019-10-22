using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class Jc_BzInfo : Basic.Framework.Web.BasicInfo
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
        /// 备注类型 1-模拟日报备注 2-模拟月报备注
        /// </summary>
        public string Type
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备注内容
        /// </summary>
        public string Inf
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备注日期
        /// </summary>
        public DateTime Timer
        {
           get;
           set;
        }
         	    /// <summary>
        /// 标志
        /// </summary>
        public string Upflag
        {
           get;
           set;
        }
            }
}


