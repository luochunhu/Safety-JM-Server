using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class UserroleInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 用户角色ID
        /// </summary>
        public string UserRoleID
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
        /// 角色ID
        /// </summary>
        public string RoleID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime
        {
           get;
           set;
        }
         	    /// <summary>
        /// 创建人
        /// </summary>
        public string CreateName
        {
           get;
           set;
        }
            }
}


