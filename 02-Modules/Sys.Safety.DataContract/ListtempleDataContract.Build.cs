using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class ListtempleInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 报表模板ID
        /// </summary>
        public int ListTempleID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 列表方案ID
        /// </summary>
        public int ListDataID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 文件名
        /// </summary>
        public string StrFileName
        {
           get;
           set;
        }
         	    /// <summary>
        /// 文件大小
        /// </summary>
        public int LngFileSize
        {
           get;
           set;
        }
         	    /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] BloImage
        {
           get;
           set;
        }
            }
}


