using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class UserrightInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 用户权限ID
        /// </summary>
        public string UserRightID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 权限ID
        /// </summary>
        public string RightID
        {
           get;
           set;
        }
            }
}


