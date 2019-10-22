using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    /// <summary>
    /// 未使用
    /// </summary>
    [Table("dataexchangesetting")]
    public partial class DataexchangesettingModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
         [Key]
        public string DataExchangeSettingID
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

