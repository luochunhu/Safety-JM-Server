using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Safety.Model
{
    [Table("KJ_EmergencyLinkageHistoryMasterPointAss")]
    public partial class EmergencyLinkageHistoryMasterPointAssModel
    {
        	    /// <summary>
        /// 主键
        /// </summary>
                [Key]
                public string Id
        {
           get;
           set;
        }
        	    /// <summary>
        /// 应急联动历史记录id
        /// </summary>
                public string EmergencyLinkHistoryId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 测点id
        /// </summary>
                public string PointId
        {
           get;
           set;
        }
        	    /// <summary>
        /// 数据状态
        /// </summary>
                public int DataState
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注1
        /// </summary>
                public string Bz1
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注2
        /// </summary>
                public string Bz2
        {
           get;
           set;
        }
        	    /// <summary>
        /// 备注3
        /// </summary>
                public string Bz3
        {
           get;
           set;
        }
            }
}

