using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class RightInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 权限ID
        /// </summary>
        public string RightID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 权限编号
        /// </summary>
        public string RightCode
        {
           get;
           set;
        }
         	    /// <summary>
        /// 权限名称
        /// </summary>
        public string RightName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 权限描述
        /// </summary>
        public string RightDescription
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
         	    /// <summary>
        /// 权限类型
        /// </summary>
        public string RightType
        {
           get;
           set;
        }
            }
}


