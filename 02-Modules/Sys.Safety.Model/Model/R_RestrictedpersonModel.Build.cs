using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("PE_Restrictedperson")]
    public partial class R_RestrictedpersonModel
    {
        	    /// <summary>
        /// 唯一编码
        /// </summary>
                [Key]
                public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 限制类型（0-限制进入，1-禁止进入）
        /// </summary>
                public int Type
        {
           get;
           set;
        }
        	    /// <summary>
        /// 设备Id
        /// </summary>
                public string PointId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 人员编号
        /// </summary>
                public string Yid
        {
           get;
           set;
        }
            }
}

