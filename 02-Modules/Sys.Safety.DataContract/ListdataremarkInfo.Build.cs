using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class ListdataremarkInfo
    {        
         	    /// <summary>
        /// 主键id
        /// </summary>
        public string Listdataremarkid
        {
           get;
           set;
        }
         	    /// <summary>
        /// 方案id
        /// </summary>
        public string Listdataid
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备注时间
        /// </summary>
        public DateTime Time
        {
           get;
           set;
        }
         	    /// <summary>
        /// 备注内容
        /// </summary>
        public string Remark
        {
           get;
           set;
        }
         	    /// <summary>
        /// 预留1
        /// </summary>
        public string Reserve1
        {
           get;
           set;
        }
         	    /// <summary>
        /// 预留2
        /// </summary>
        public string Reserve2
        {
           get;
           set;
        }
         	    /// <summary>
        /// 预留3
        /// </summary>
        public string Reserve3
        {
           get;
           set;
        }
            }
}


