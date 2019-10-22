using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sys.Safety.DataContract
{
    public partial class DataexchangesettingInfo : Basic.Framework.Web.BasicInfo
    {        
         	    /// <summary>
        /// 主键ID
        /// </summary>
        public int DataExchangeSettingID
        {
           get;
           set;
        }
         	    /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
           get;
           set;
        }
         	    /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
           get;
           set;
        }
         	    /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime DatLastExportTime
        {
           get;
           set;
        }
            }
}


