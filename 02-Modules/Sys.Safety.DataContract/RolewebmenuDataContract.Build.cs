using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class RolewebmenuInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 角色模块权限ID
        /// </summary>
        public string ID
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
        /// 模块ID
        /// </summary>
        public string ModuleID
        {
           get;
           set;
        }
            }
}


