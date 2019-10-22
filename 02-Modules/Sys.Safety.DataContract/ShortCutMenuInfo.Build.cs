using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class ShortCutMenuInfo
    {        
         	    /// <summary>
        /// 唯一编码
        /// </summary>
        public string Id
        {
           get;
           set;
        }
         	    /// <summary>
        /// 用户编码
        /// </summary>
        public string UserId
        {
           get;
           set;
        }
         	    /// <summary>
        /// 菜单编码
        /// </summary>
        public string MenuId
        {
           get;
           set;
        }
         	    /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnable
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用
        /// </summary>
        public string B1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备用
        /// </summary>
        public string B2
        {
           get;
           set;
        }
            }
}


