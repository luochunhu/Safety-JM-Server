using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class RolerightInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 角色权限ID
        /// </summary>
        public string RoleRightID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleID
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


