using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class WebmenuauthInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// ID
        /// </summary>
        public string ID
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
         	    /// <summary>
        /// 权限类型ID关联Mas_WEBAuthority表
        /// </summary>
        public string AuthID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用1
        /// </summary>
        public string By1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用2
        /// </summary>
        public string By2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用3
        /// </summary>
        public string By3
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用4
        /// </summary>
        public string By4
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用5
        /// </summary>
        public string By5
        {
           get;
           set;
        }
            }
}


