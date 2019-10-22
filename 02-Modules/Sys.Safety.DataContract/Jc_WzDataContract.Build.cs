using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class Jc_WzInfo : Basic.Framework.Web.BasicInfo
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
        /// 安装位置ID
        /// </summary>
        public string WzID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 安装位置，最多30个汉字
        /// </summary>
        public string Wz
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
        /// 上传标志0-未传1-已传
        /// </summary>
        public string Upflag
        {
           get;
           set;
        }
            }
}


