using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class Jc_DefwbInfo : Basic.Framework.Web.BasicInfo
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
        /// 测点ID
        /// </summary>
        public string Pointid
        {
           get;
           set;
        }
         	    /// <summary>
        /// 当前用户
        /// </summary>
        public string User
        {
           get;
           set;
        }
         	    /// <summary>
        /// 维保记录
        /// </summary>
        public string Remerk
        {
           get;
           set;
        }
         	    /// <summary>
        /// 时间
        /// </summary>
        public DateTime Timer
        {
           get;
           set;
        }
            }
}


